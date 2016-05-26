#region Using Statements
    using System;
    using System.Globalization;

    using System.Management.Automation;
    using System.Management.Automation.Host;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.Powershell
{
    internal class CakePSHost : PSHost
    {
        #region Fields (2)
            private Guid _ID = Guid.NewGuid();
            private CakePSHostInterface _Interface;
        #endregion





        #region Constructor (1)
            internal CakePSHost(ICakeLog log, bool outputToAppConsole)
            {
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                _Interface = new CakePSHostInterface(log, outputToAppConsole);
            }
        #endregion





        #region Properties (6)
            public override Guid InstanceId
            {
                get { return _ID; }
            }

            public override string Name
            {
                get { return "CakePSHost"; }
            }

            public override Version Version
            {
                get { return new Version(1, 0); }
            }

            public override CultureInfo CurrentCulture
            {
                get { return CultureInfo.CurrentCulture; }
            }

            public override CultureInfo CurrentUICulture
            {
                get { return CultureInfo.CurrentUICulture; }
            }



            public override PSHostUserInterface UI
            {
                get { return _Interface; }
            }
        #endregion





        #region Functions (5)
            public override void EnterNestedPrompt()
            {
                throw new NotImplementedException();
            }

            public override void ExitNestedPrompt()
            {
                throw new NotImplementedException();
            }

            public override void NotifyBeginApplication()
            {
                return;
            }

            public override void NotifyEndApplication()
            {
                return;
            }

            public override void SetShouldExit(int exitCode)
            {
                return;
            }
        #endregion
    }
}
