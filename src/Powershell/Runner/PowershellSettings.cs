#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core;
    using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Responsible for starting powershell processes.
    /// </summary>
    public class PowershellSettings
    {
        #region Constructor (1)
            public PowershellSettings()
            {
                this.FormatOutput = false;
            }
        #endregion





        #region Properties (1)
            /// <summary>
            /// Gets or sets the working directory for the process to be started.
            /// </summary>
            public DirectoryPath WorkingDirectory { get; set; }

            /// <summary>
            /// Gets or sets optional timeout for process execution
            /// </summary>
            public int? Timeout { get; set; }



            /// <summary>
            /// Gets or sets optional computer name to run the process on
            /// </summary>
            public string ComputerName { get; set; }

            /// <summary>
            /// Gets or sets the remote port to connect on
            /// </summary>
            public int Port { get; set; }



            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Password { get; set; }



            /// <summary>
            /// Gets or sets optional timeout for process execution
            /// </summary>
            public bool FormatOutput { get; set; }



            /// <summary>
            /// Gets or sets the modules to load into the initial state
            /// </summary>
            public IList<string> Modules { get; set; }

            /// <summary>
            /// Gets or sets the set of command-line arguments to use when starting the application.
            /// </summary>
            /// <value>The set of command-line arguments to use when starting the application.</value>
            public ProcessArgumentBuilder Arguments { get; set; }
        #endregion
    }
}
