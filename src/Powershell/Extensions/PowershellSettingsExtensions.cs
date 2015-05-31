#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.IO.Arguments;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Contains extension methods for <see cref="PowershellSettings" />.
    /// </summary>
    public static class PowershellSettingsExtensions
    {
        /// <summary>
        /// Sets the arguments for the process
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments to append.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings WithArguments(this PowershellSettings settings, Action<ProcessArgumentBuilder> arguments)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (settings.Arguments == null)
            {
                settings.Arguments = new ProcessArgumentBuilder();
            }

            arguments(settings.Arguments);
            return settings;
        }

        /// <summary>
        /// Adds the specified module to load into the initial state
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="module">The module to load.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings WithModule(this PowershellSettings settings, string module)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (settings.Arguments == null)
            {
                settings.Arguments = new ProcessArgumentBuilder();
            }

            if (settings.Modules == null)
            {
                settings.Modules = new List<string>();
            }

            settings.Modules.Add(module);
            return settings;
        }



        /// <summary>
        /// Sets the working directory for the process to be started.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="path">The working directory for the process to be started.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings UseWorkingDirectory(this PowershellSettings settings, DirectoryPath path)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.WorkingDirectory = path;
            return settings;
        }

        /// <summary>
        /// Sets the optional timeout for process execution
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="timeout">The timeout duration</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings SetTimeout(this PowershellSettings settings, int timeout)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Timeout = timeout;
            return settings;
        }



        /// <summary>
        /// Sets the working directory for the process to be started.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="path">The working directory for the process to be started.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings UseComputerName(this PowershellSettings settings, string name)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.ComputerName = name;
            return settings;
        }

        /// <summary>
        /// Sets the remote port to connect on.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="path">The working directory for the process to be started.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings UsePort(this PowershellSettings settings, int port)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Port = port;
            return settings;
        }

        /// <summary>
        /// Gets or sets the credentials to use when connecting
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="username">The username to connect with.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings UseUsername(this PowershellSettings settings, string username)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Username = username;
            return settings;
        }

        /// <summary>
        /// Gets or sets the credentials to use when connecting
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="password">The password to connect with.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings UsePassword(this PowershellSettings settings, string password)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Password = password;
            return settings;
        }



        /// <summary>
        /// Sets a value indicating whether the output of an application should be formatted as text
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="format">true if output should be written to the cake console; otherwise, false. The default is false.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings SetFormatOutput(this PowershellSettings settings, bool format = true)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.FormatOutput = format;
            return settings;
        }

        /// <summary>
        /// Sets a value indicating whether the output of an application is written to the cake console
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="log">true if output should be written to the cake console; otherwise, false. The default is false.</param>
        /// <returns>The same <see cref="PowershellSettings"/> instance so that multiple calls can be chained.</returns>
        public static PowershellSettings SetLogOutput(this PowershellSettings settings, bool log = true)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.LogOutput = log;
            return settings;
        }
    }
}
