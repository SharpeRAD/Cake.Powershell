using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
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
        private readonly ICakeLog _log;

        /// <summary>
        /// Constructs a PwshScriptRunner
        /// </summary>
        public PwshScriptRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            _log = log;
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "pwsh" };
        }

        protected override string GetToolName()
        {
            return "pwsh";
        }

        public void RunScript(string script, PowershellSettings settings)
        {
            var pwshSettings = new PwshSettings
            {
                WorkingDirectory = settings.WorkingDirectory,
                ToolTimeout = settings.Timeout == null ? (TimeSpan?)null : new TimeSpan(0, 0, settings.Timeout.Value)
            };
            settings.Arguments.Prepend(script);
            var args = GetArguments(script, settings);
            Run(pwshSettings, args);
        }

        /// <summary>
        /// Builds the arguments for npm.
        /// </summary>
        /// <param name="settings">Settings used for building the arguments.</param>
        /// <returns>Argument builder containing the arguments based on <paramref name="settings"/>.</returns>
        protected ProcessArgumentBuilder GetArguments(string script, PowershellSettings settings)
        {

            var args = new ProcessArgumentBuilder();
            args.Append(script);

            foreach (var argument in settings.Arguments)
            {
                _log.Debug("Maybe append: " + argument);
            }

            _log.Verbose("pwsh arguments: {0}", args.RenderSafe());

            return args;
        }
    }
}