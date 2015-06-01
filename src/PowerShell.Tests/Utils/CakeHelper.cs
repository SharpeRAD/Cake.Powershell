#region Using Statements
    using System;
    using System.IO;
    using System.Collections.Generic;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;
    using Cake.Powershell;

    using NSubstitute;
#endregion



namespace Cake.Powershell.Tests
{
    internal static class CakeHelper
    {
        #region Functions (2)
            public static ICakeEnvironment CreateEnvironment()
            {
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory = Directory.GetCurrentDirectory();

                return environment;
            }



            public static IPowershellRunner CreatePowershellRunner()
            {
                return new PowershellRunner(CakeHelper.CreateEnvironment(), new DebugLog());
            }
        #endregion
    }
}
