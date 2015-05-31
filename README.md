# Cake.Powershell

Cake Addon that extends Cake with Powershell commands

[![Build status](https://ci.appveyor.com/api/projects/status/5g0u2757tix9se6f?svg=true)](https://ci.appveyor.com/project/PhillipSharpe/cake-powershell)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



## Implemented functionality

This is a list of some the currently implemented functionality:

* Local / remote scripts
* Script files
* Download and run remote script file
* Script parameters
* Outputing to the cake log



## Usage

#addin "Cake.Powershell"

```csharp
Task("Powershell-Script")
    .Description("Run an example powershell command with parameters")
    .Does(() =>
{
    StartPowershellScript("Write-Host", new PowershellSettings().WithArguments(args => 
	{ 
		args.AppendQuoted("Testing..."); 
	}));
});

Task("Powershell-File")
    .Description("Run an example powershell script file with parameters")
    .Does(() =>
{
    StartPowershellFile("../Scripts/Install.ps1", new PowershellSettings().WithArguments(args => 
	{ 
		args.Append("Username", "admin")
			.AppendSecret("Password", "pass1");
	}));
});

Task("Powershell-Remote")
    .Description("Run an example powershell command remotely")
    .Does(() =>
{
    StartPowershellScript("Get-Services", new PowershellSettings()
	{
		ComputerName = "remote-location",
		Username = "admin",
		Password = "pass1"
	});
});

Task("Powershell-Download")
    .Description("Run an example powershell script file after downloading its contents to a local directory")
    .Does(() =>
{
    StartPowershellDownload("https://chocolatey.org/install.ps1", "C:/Temp/install.ps1", new PowershellSettings());
});

RunTarget("Powershell-Script");
```



## Example

A complete cake build example can be found [here](https://github.com/SharpeRAD/Cake.Powershell/blob/master/test/build.cake)



## TroubleShooting

A few pointers for correctly enabling powershell scripting can be found [here](https://github.com/SharpeRAD/Cake.Powershell/blob/master/TroubleShooting.md)