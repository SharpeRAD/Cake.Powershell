using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Powershell.Runner
{
    public class PwshSettings : ToolSettings
    {

    }

    /// <summary>
    /// Runs Pwsh scripts on the command line
    /// </summary>
    public class PwshScriptRunner : Tool<PwshSettings>
    {
        /// <summary>
        /// Constructs a PwshScriptRunner
        /// </summary>
        public PwshScriptRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>Gets the possible names of the tool executable.</summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "pwsh" };
        }

        /// <summary>Gets the name of the tool.</summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "pwsh";
        }

        /// <summary>
        /// Runs `pwsh` against the script and settings provided
        /// </summary>
        public void RunScript(string script, PowershellSettings settings)
        {
            var pwshSettings = new PwshSettings
            {
                WorkingDirectory = settings.WorkingDirectory,
                ToolTimeout = settings.Timeout == null
                    ? (TimeSpan?) null
                    : new TimeSpan(0, 0, settings.Timeout.Value)
            };
            settings.Arguments.Prepend(script);
            var args = GetArguments(script);
            Run(pwshSettings, args);
        }

        /// <summary>
        /// Builds the arguments for pwsh.
        /// </summary>
        /// <returns>Argument builder containing the arguments based on <paramref name="script"/>.</returns>
        private static ProcessArgumentBuilder GetArguments(string script)
        {
            var args = new ProcessArgumentBuilder();
            args.Append(script);
            return args;
        }
    }
}