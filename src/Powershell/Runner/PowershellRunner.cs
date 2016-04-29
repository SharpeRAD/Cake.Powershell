#region Using Statements
    using System;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Principal;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.IO.Arguments;
    using Cake.Core.Diagnostics;

    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Responsible for executing powershell scripts.
    /// </summary>
    public sealed class PowershellRunner : IPowershellRunner
    {
        #region Fields (2)
            private readonly ICakeEnvironment _Environment;
            private readonly ICakeLog _Log;
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

                _Log.Debug(Verbosity.Normal, String.Format("Executing: {0}", this.AppendArguments(script, settings.Arguments, true)));



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

                _Log.Debug(Verbosity.Normal, String.Format("Executing: {0}", this.AppendArguments(script, settings.Arguments, true)));



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

                _Log.Debug(Verbosity.Normal, String.Format("Executing: {0}", this.AppendArguments(script, settings.Arguments, true)));



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

            private Collection<PSObject> Invoke(string script, PowershellSettings settings)
            {
                //Create Runspace
                Runspace runspace = null;

                if (String.IsNullOrEmpty(settings.ComputerName))
                {
                    //Local
                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_Log));
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

                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_Log), connection);
                }

                runspace.Open();



                //Create Pipline
                Collection<PSObject> results = null;

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
                        pipeline.Commands.AddScript("Import-Module " + settings.Modules);
                    }

                    pipeline.Commands.AddScript(script);

                    if (settings.FormatOutput)
                    {
                        pipeline.Commands.Add("Out-String");
                    }

                    results = pipeline.Invoke();



                    //Log Errors
                    if (pipeline.Error.Count > 0)
                    {
                        while (!pipeline.Error.EndOfPipeline)
                        {
                            PSObject value = (PSObject)pipeline.Error.Read();

                            if (value != null)
                            {
                                ErrorRecord record = (ErrorRecord)value.BaseObject;

                                if (record != null)
                                {
                                    _Log.Error(Verbosity.Normal, record.Exception.Message);
                                }
                            }
                        }
                    }
                }

                runspace.Close();
                runspace.Dispose();



                //Log Results
                if (settings.LogOutput)
                {
                    foreach (PSObject res in results)
                    {
                        _Log.Debug(Verbosity.Normal, res.ToString());
                    }
                }

                return results;
            }
        #endregion
    }
}
