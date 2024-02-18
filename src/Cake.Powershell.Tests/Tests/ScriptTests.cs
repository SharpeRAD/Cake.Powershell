#region Using Statements

using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

using Xunit;

using Cake.Core;
using Cake.Core.IO;
using Cake.Powershell;
#endregion



namespace Cake.Powershell.Tests
{
    public class ScriptTests
    {
        #region Methods
        [Fact]
        public void Start_Write_Script()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host", 
                new PowershellSettings().WithArguments(args => args.Append("Testing...")));

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("Testing...")), "Output not written to the powershell host");
        }

        [Fact]
        public void Start_Service_Script()
        {
            if (OperatingSystem.IsWindows())
            {
                Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Get-Service -ErrorAction SilentlyContinue", 
                    new PowershellSettings());

                Assert.True((results != null) && (results.Count > 0), "Check Rights");
            }
            Assert.True(true);
        }



        [Fact]
        public void Working_Directory_With_Spaces_Should_Properly_Execute()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "test dir");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host",
                new PowershellSettings().WithArguments(args => args.Append("Testing..."))
                                        .UseWorkingDirectory(path));

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("Testing...")), "Output not written to the powershell host");
        }
        
        [Fact]
        public void Escapes_Curley_Brackets()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host '{ blah }'",
                new PowershellSettings());

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("{ blah }")), "Output not written to the powershell host");
        }
        #endregion
    }
}