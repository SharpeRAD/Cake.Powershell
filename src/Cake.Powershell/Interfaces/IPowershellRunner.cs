#region Using Statements
using System;
using System.Collections.ObjectModel;

using System.Management.Automation;

using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Responsible for executing powershell scripts.
    /// </summary>
    public interface IPowershellRunner
    {
        #region Methods (3)
        /// <summary>
        /// Starts a powershell script using the specified information.
        /// </summary>
        /// <param name="script">The powershell script to run.</param>
        /// <param name="settings">The information about the script to start.</param>
        /// <returns>Powershell objects.</returns>
        Collection<PSObject> Start(string script, PowershellSettings settings);

        /// <summary>
        /// Starts a powershell script using the specified information.
        /// </summary>
        /// <param name="path">The path of the script file to run.</param>
        /// <param name="settings">The information about the script to start.</param>
        /// <returns>Powershell objects.</returns>
        Collection<PSObject> Start(FilePath path, PowershellSettings settings);

        /// <summary>
        /// Starts a powershell script using the specified information.
        /// </summary>
        /// <param name="uri">The location of the script file to download and run.</param>
        /// <param name="path">The temporary path to download the file to.</param>
        /// <param name="settings">The information about the process to start.</param>
        /// <returns>Powershell objects.</returns>
        Collection<PSObject> Start(Uri uri, FilePath path, PowershellSettings settings);
        #endregion
    }
}
