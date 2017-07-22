#region Using Statements
using System.IO;

using Cake.Core;

using NSubstitute;
#endregion



namespace Cake.Powershell.Tests
{
    internal static class CakeHelper
    {
        #region Methods (2)
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
