#region Using Statements
    using System;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
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
    /// Responsible for starting processes.
    /// </summary>
    public sealed class PowershellRunner : IPowershellRunner
    {
        #region Fields (2)
            private readonly ICakeEnvironment _environment;
            private readonly ICakeLog _log;
        #endregion
        




        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="ProcessRunner" /> class.
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

                _environment = environment;
                _log = log;
            }
        #endregion





        #region Functions (6)
            /// <inheritdoc/>
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

                _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", this.AppendArguments(script, settings.Arguments, true));



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }

            /// <inheritdoc/>
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

                _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", this.AppendArguments(script, settings.Arguments, true));



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }

            /// <inheritdoc/>
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

                _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", this.AppendArguments(script, settings.Arguments, true));



                //Call
                script = this.AppendArguments(script, settings.Arguments, false);

                return this.Invoke(script, settings);
            }



            private void SetWorkingDirectory(PowershellSettings settings)
            {
                DirectoryPath workingDirectory = settings.WorkingDirectory ?? _environment.WorkingDirectory;

                settings.WorkingDirectory = workingDirectory.MakeAbsolute(_environment);
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
                        args = builder.Render();
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
                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_log));
                }
                else
                {
                    //Remote
                    WSManConnectionInfo connection = new WSManConnectionInfo();

                    if (!String.IsNullOrEmpty(settings.ComputerName))
                    {
                        connection.ComputerName = settings.ComputerName;
                    }

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

                    runspace = RunspaceFactory.CreateRunspace(new CakePSHost(_log), connection);
                }

                runspace.Open();



                //Create Pipline
                Collection<PSObject> results = null;

                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript("Set-Location -Path " + settings.WorkingDirectory.FullPath);

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
                }

                runspace.Close();
                runspace.Dispose();



                //Return Results
                return results;
            }
        #endregion
    }
}
