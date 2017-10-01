#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;

using Cake.Core.Diagnostics;
#endregion



namespace Cake.Powershell.Tests
{
    /// <summary>
    /// A log that outputs messages to debug
    /// </summary>
    internal class DebugLog : ICakeLog
    {
        #region Properties
        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public static IList<string> Lines { get; set; }

        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity
        {
            get { return Verbosity.Diagnostic; }
            set { }
        }
        #endregion





        #region Methods
        /// <summary>
        /// Writes the text representation of the specified array of objects to the 
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            try
            {
                if (DebugLog.Lines == null)
                {
                    DebugLog.Lines = new List<string>();
                }

                format = String.Format(format, args);
                DebugLog.Lines.Add(format);

                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(format);
                }
                else
                {
                    Console.WriteLine(format);
                }
            }
            catch { }
        }
        #endregion
    }
}
