# Cake.Powershell

Cake Addon that extends Cake with Powershell commands
[![Build status](https://ci.appveyor.com/api/projects/status/5g0u2757tix9se6f?svg=true)](https://ci.appveyor.com/project/PhillipSharpe/cake-powershell)



## Implemented functionality

This is a list of some the currently implemented functionality:

* Local / remote scripts
* Script files
* Download and run remote script file
* Script Parameters
* Outputing to the cake log



## Usage

```csharp
Task("Powershell-Script")
    .Description("Run an example powershell call with parameters")
    .Does(() =>
{
    PowershellScript("Write-Host", new PowershellSettings().WithArguments(args => { args.Append("Testing..."); });
});

Task("Powershell-File")
    .Description("Run an example powershell script file with parameters")
    .Does(() =>
{
    PowershellFile("../Scripts/Install.ps1", new PowershellSettings().WithArguments(args => 
	{ 
		args.Append("Username", "admin")
			.Append("Password", "pass1");
	}
});

Task("Powershell-Download")
    .Description("Run an example powershell script file after downloading its contents to a local directory")
    .Does(() =>
{
    PowershellDownload("https://chocolatey.org/install.ps1", "C:/Temp/install.ps1", new PowershellSettings());
});

RunTarget("Powershell-Script");
```



## TroubleShooting

A few pointers for correctly enabling powershell scripts can be found [here](https://github.com/SharpeRAD/Cake.Powershell/blob/master/TroubleShooting.md)