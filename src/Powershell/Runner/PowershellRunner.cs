#region Using Statements
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Threading;

#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Responsible for executing powershell scripts.
    /// </summary>
    public sealed class PowershellRunner : IPowershellRunner
    {
        #region Fields (5)
            private readonly ICakeEnvironment _Environment;
            private readonly ICakeLog _Log;
            private Collection<PSObject> _pipelineResults = new Collection<PSObject>();
            private bool _complete;
            private bool _successful = true;

        #endregion
        




        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="PowershellRunner" /> class.
            /// </summary>
            /// <param name="environment">The environment.</param>
            /// <param name="log">The log.</param>
            public PowershellRunner(ICakeEnvironment environment, ICakeLog log)
            {
                if (environment == null)
                {
                    throw new ArgumentNullException("environment");
                }
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                _Environment = environment;
                _Log = log;
            }
        #endregion





        #region Functions (6)
            /// <summary>
            /// Starts a powershell script using the specified information.
            /// </summary>
            /// <param name="script">The powershell script to run.</param>
            /// <param name="settings">The information about the script to start.</param>
            /// <returns>Powershell objects.</returns>
            public Collection<PSObject> Start(string script, PowershellSettings settings)
            {
                if (String.IsNullOrEmpty(script))
                {
                    throw new ArgumentNullException("script");
                }
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }



                //Get Script
                this.SetWorkingDirectory(settings);

                LogExecutingCommand(settings, script);



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }

            /// <summary>
            /// Starts a powershell script using the specified information.
            /// </summary>
            /// <param name="path">The path of the script file to run.</param>
            /// <param name="settings">The information about the script to start.</param>
            /// <returns>Powershell objects.</returns>
            public Collection<PSObject> Start(FilePath path, PowershellSettings settings)
            {
                if (path == null)
                {
                    throw new ArgumentNullException("path");
                }
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }



                //Get Script
                this.SetWorkingDirectory(settings);
                string script = "&\"" + path.MakeAbsolute(settings.WorkingDirectory).FullPath + "\"";

                LogExecutingCommand(settings, script);



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }

            /// <summary>
            /// Starts a powershell script using the specified information.
            /// </summary>
            /// <param name="uri">The location of the script file to download and run.</param>
            /// <param name="path">The temporary path to download the file to.</param>
            /// <param name="settings">The information about the process to start.</param>
            /// <returns>Powershell objects.</returns>
            public Collection<PSObject> Start(Uri uri, FilePath path, PowershellSettings settings)
            {
                if (uri == null)
                {
                    throw new ArgumentNullException("uri");
                }
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }



                //Get Script
                this.SetWorkingDirectory(settings);

                string fullPath = path.MakeAbsolute(settings.WorkingDirectory).FullPath;
                string script = "&\"" + path.MakeAbsolute(settings.WorkingDirectory).FullPath + "\"";



                //Download Script
                WebClient client = new WebClient();
                client.DownloadFile(uri, fullPath);

                LogExecutingCommand(settings, script);



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }



            //Helpers
            private void SetWorkingDirectory(PowershellSettings settings)
            {
                if (String.IsNullOrEmpty(settings.ComputerName))
                {
                    DirectoryPath workingDirectory = settings.WorkingDirectory ?? _Environment.WorkingDirectory;

                    settings.WorkingDirectory = workingDirectory.MakeAbsolute(_Environment);
                }
                else if (settings.WorkingDirectory == null)
                {
                    settings.WorkingDirectory = new DirectoryPath("C:/");
                }
            }

            private string AppendArguments(string script, ProcessArgumentBuilder builder, bool safe)
            {
                if (builder != null)
                {
                    string args;

                    if (safe)
                    {
                        args = builder.RenderSafe().TrimEnd();
                    }
                    else
                    {
                        args = builder.Render().TrimEnd();
                    }

                    if (!String.IsNullOrEmpty(args))
                    {
                        script += " " + args;
                    }
                }

                return script;
            }
        


            private void LogExecutingCommand(PowershellSettings settings, string script, bool safe = true)
            {
                _Log.Debug(Verbosity.Normal, String.Format("Executing: {0}", this.AppendArguments(script, settings.Arguments, safe).EscapeCurleyBrackets()));
            }



            private Collection<PSObject> Invoke(string script, PowershellSettings settings)
            {
                //Create Runspace
                Runspace runspace = null;

                if (String.IsNullOrEmpty(settings.ComputerName))
                {
                    //Local
                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_Log, settings.OutputToAppConsole));
                }
                else
                {
                    //Remote
                    WSManConnectionInfo connection = new WSManConnectionInfo();

                    connection.ComputerName = settings.ComputerName;

                    if (settings.Port > 0)
                    {
                        connection.Port = settings.Port;
                    }
                    else
                    {
                        connection.Port = 5985;
                    }

                    if (!String.IsNullOrEmpty(settings.Username))
                    {
                        connection.Credential = new PSCredential(settings.Username, settings.Password.MakeSecure());
                    }

                    if (settings.Timeout.HasValue)
                    {
                        connection.OperationTimeout = settings.Timeout.Value;
                        connection.OpenTimeout = settings.Timeout.Value;
                    }

                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_Log, settings.OutputToAppConsole), connection);
                }

                runspace.Open();



                //Create Pipline
                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    //Invoke Command
                    if (settings.WorkingDirectory != null)
                    {
                        var path = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", settings.WorkingDirectory.FullPath);
                        pipeline.Commands.AddScript("Set-Location -Path " + path);
                    }

                    if ((settings.Modules != null) && (settings.Modules.Count > 0))
                    {
                        settings.Modules.ToList().ForEach(m => pipeline.Commands.AddScript("Import-Module " + m));
                    }

                    pipeline.Commands.AddScript(script);

                    if (settings.FormatOutput)
                    {
                        pipeline.Commands.Add("Out-Default");
                    }

                    pipeline.Output.DataReady += Output_DataReady;
                    pipeline.Error.DataReady += Error_DataReady;
                    pipeline.StateChanged += Pipeline_StateChanged;

                    pipeline.InvokeAsync();

                    while (!_complete)
                    {
                        Thread.Sleep(500);
                    }
                }

                runspace.Close();
                runspace.Dispose();



                return _pipelineResults;
            }

            private void Pipeline_StateChanged(object sender, PipelineStateEventArgs e)
            {
                if (e.PipelineStateInfo.State != PipelineState.Completed && e.PipelineStateInfo.State != PipelineState.Failed && e.PipelineStateInfo.State != PipelineState.Stopped) return;

                if (e.PipelineStateInfo.State == PipelineState.Failed)
                {
                    var failedPso = new PSObject(e.PipelineStateInfo.Reason);
                    _pipelineResults.Add(failedPso);
                    _successful = false;
                }

                var pso = new PSObject(_successful ? 0 : 1);
                _pipelineResults.Insert(0, pso);

                _complete = true;
            }

            private void Error_DataReady(object sender, EventArgs e)
            {
                var error = sender as PipelineReader<object>;

                if (error == null) return;

                if (_successful && error.Count > 0)
                {
                    _successful = false;
                }

                while (error.Count > 0)
                {
                    var errorItem = error.Read();
                    var pso = new PSObject(errorItem);
                    _pipelineResults.Add(pso);
                    _Log.Error(Verbosity.Normal, errorItem.ToString().EscapeCurleyBrackets());
                }
            }

            private void Output_DataReady(object sender, EventArgs e)
            {
                var output = sender as PipelineReader<PSObject>;

                if (output == null) return;

                while (output.Count > 0)
                {
                    var outputItem = output.Read();
                    _pipelineResults.Add(outputItem);
                    _Log.Debug(Verbosity.Normal, outputItem.ToString().EscapeCurleyBrackets());
                }
            }

        #endregion
    }
}
