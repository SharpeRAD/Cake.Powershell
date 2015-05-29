#region Using Statements
    using System;
    using System.Text;
    using System.Collections.Generic;

    using System.Management.Automation;
    using System.Management.Automation.Host;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.Powershell
{
    public class CakePSHostInterface : PSHostUserInterface
    {
        #region Fields (2)
            private readonly ICakeLog _Log;
            private readonly PSHostRawUserInterface _RawUI;
        #endregion





        #region Constructor (1)
            public CakePSHostInterface(ICakeLog log)
            {
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                _Log = log;
                _RawUI = new CakePSHostRawUserInterface(_Log);
            }
        #endregion





        #region Properties (1)
            public override PSHostRawUserInterface RawUI
            {
                get 
                { 
                    return _RawUI; 
                }
            }
        #endregion





        #region Functions (14)
            public override void Write(string value)
            {
                _Log.Write(Verbosity.Normal, LogLevel.Information, "", value);
            }

            public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
            {
                _Log.Write(Verbosity.Normal, LogLevel.Information, "", value);
            }



            public override void WriteLine()
            {

            }

            public override void WriteLine(string value)
            {
                _Log.Write(Verbosity.Normal, LogLevel.Information, "", value);
            }

            public override void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
            {
                _Log.Write(Verbosity.Normal, LogLevel.Information, "", value);
            }



            public override void WriteDebugLine(string value)
            {
                _Log.Write(Verbosity.Diagnostic, LogLevel.Debug, "", value);
            }

            public override void WriteErrorLine(string value)
            {
                _Log.Write(Verbosity.Verbose, LogLevel.Error, "", value);
            }

            public override void WriteVerboseLine(string value)
            {
                _Log.Write(Verbosity.Verbose, LogLevel.Verbose, "", value);
            }

            public override void WriteWarningLine(string value)
            {
                _Log.Write(Verbosity.Verbose, LogLevel.Warning, "", value);
            }



            public override void WriteProgress(long sourceId, ProgressRecord record)
            {
                return;
            }



            public override Dictionary<string, PSObject> Prompt(string caption, string message, System.Collections.ObjectModel.Collection<FieldDescription> descriptions)
            {
                throw new NotImplementedException();
            }

            public override int PromptForChoice(string caption, string message, System.Collections.ObjectModel.Collection<ChoiceDescription> choices, int defaultChoice)
            {
                throw new NotImplementedException();
            }

            public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
            {
                throw new NotImplementedException();
            }

            public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
            {
                throw new NotImplementedException();
            }

            public override string ReadLine()
            {
                throw new NotImplementedException();
            }

            public override System.Security.SecureString ReadLineAsSecureString()
            {
                throw new NotImplementedException();
            }
        #endregion
    }
}
