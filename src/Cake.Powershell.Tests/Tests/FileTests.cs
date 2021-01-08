﻿#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Management.Automation;

using Xunit;
using Cake.Core.IO;
#endregion



namespace Cake.Powershell.Tests
{
    public class FileTests
    {
        #region Methods
        [Fact]
        public void Start_Local_File()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("Scripts/Test.ps1"), 
                new PowershellSettings());

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }

        [Fact]
        public void Start_File_With_Parameters()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("Scripts/Test.ps1"),
                new PowershellSettings().WithArguments(args => args.Append("Service", "eventlog")));

            Assert.True((results != null) && (results.Count >= 1), "Check Rights");
        }

        [Fact]
        public void Start_File_With_ArrayParameters()
        {
            var array = new string[] {"A", "B", "C"};

            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("Scripts/ArrayTest.ps1"),
                new PowershellSettings().WithArguments(args => args.AppendArray("AnArray", array)));

            Assert.True((results != null) && (results.Count == array.Length + 1));
            Assert.Equal("", results[0].BaseObject.ToString());

            foreach (var item in array)
            {
                Assert.True(results.FirstOrDefault(r => r.BaseObject.ToString().Equals((item))) != null);
            }
        }

        [Fact]
        public void Start_File_With_HashTableParameters()
        {
            var dict = new Dictionary<string, string>
            {
                { "A", "1" },
                { "B", "2 "},
                { "C", "3" }
            };
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("Scripts/HashTableTest.ps1"),
                new PowershellSettings().WithArguments(args => args.AppendHashTable("AHashTable", dict)));

            Assert.True((results != null) && (results.Count == dict.Count + 1));
            Assert.Equal("", results[0].BaseObject.ToString());

            foreach (var item in dict.ToArray())
            {
                Assert.True(results.FirstOrDefault(r => r.BaseObject.ToString().Equals(($"{item.Key} = {item.Value}"))) != null);
            }
        }

        [Fact]
        public void Start_File_With_Dot_Sourcing()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("Scripts/Test.ps1"),
                new PowershellSettings().WithDotSourcing());

            Assert.True((results != null) && (results.Count >= 1), "Check Rights");
        }

        [Fact]
        public void Exception_Script_Should_Throw_Exception()
        {
            var runner = CakeHelper.CreatePowershellRunner();
            Assert.Throws<AggregateException>(() => runner.Start(new FilePath("Scripts/ErrorScript.ps1"),
                new PowershellSettings()));
        }

        [Fact]
        public void Returned_Error_Code_Should_Not_Throw_Exception_If_Option_Passed()
        {
            var results = CakeHelper.CreatePowershellRunner()
                .Start(new FilePath("Scripts/FailingScript.ps1"), new PowershellSettings() { ExceptionOnScriptError = false });

            Assert.True(results != null && results.Count >= 1, "Check Rights");
            Assert.True(results.Where(r => r.BaseObject.ToString().Contains("Cannot find path")) != null);
            Assert.Equal("1", results.Last().BaseObject.ToString());
        }

        [Fact]
        public void Returned_Error_Code_Should_Throw_Exception()
        {
            var runner = CakeHelper.CreatePowershellRunner();
            Assert.Throws<AggregateException>(() => runner.Start(new FilePath("Scripts/FailingScript.ps1"),
                new PowershellSettings() { ExceptionOnScriptError = true }));
        }

        /*
        [Fact]
        public void Start_Remote_File()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"), 
                new PowershellSettings()
                {
                    ComputerName = "remote-machine"
                });

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }
        */
        #endregion
    }
}
