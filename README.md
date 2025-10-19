# Cake.Powershell
Cake-Build addin that extends Cake with Powershell commands

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake)



## Table of contents

1. [Implemented functionality](https://github.com/SharpeRAD/Cake.Powershell#implemented-functionality)
2. [Continuous integration](https://github.com/SharpeRAD/Cake.Powershell#continuous-integration)
2. [Referencing](https://github.com/SharpeRAD/Cake.Powershell#referencing)
3. [Usage](https://github.com/SharpeRAD/Cake.Powershell#usage)
4. [Example](https://github.com/SharpeRAD/Cake.Powershell#example)
5. [TroubleShooting](https://github.com/SharpeRAD/Cake.Powershell#troubleshooting)
6. [Plays well with](https://github.com/SharpeRAD/Cake.Powershell#plays-well-with)
7. [License](https://github.com/SharpeRAD/Cake.Powershell#license)
8. [Share the love](https://github.com/SharpeRAD/Cake.Powershell#share-the-love)



## Implemented functionality

* Local / remote scripts
* Script files
* Download and run remote script file
* Script parameters
* Outputting to the cake console



## Continuous integration

| Build server                | Platform      | Build status                                                                                                                                                        |                       
|-----------------------------|---------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows       | [![AppVeyor](https://ci.appveyor.com/api/projects/status/5g0u2757tix9se6f?svg=true)](https://ci.appveyor.com/project/SharpeRAD/cake-powershell)                     |
| Travis                      | Linux / MacOS | [![Travis](https://travis-ci.com/SharpeRAD/Cake.Powershell.svg?branch=master)](https://travis-ci.org/cake-build/cake)                                               |   



## Referencing

[![NuGet Version](http://img.shields.io/nuget/v/Cake.Powershell.svg?style=flat)](https://www.nuget.org/packages/Cake.Powershell/)
[![NuGet Downloads](http://img.shields.io/nuget/dt/Cake.Powershell.svg?style=flat)](https://www.nuget.org/packages/Cake.Powershell/)

Cake.Powershell is available as a nuget package from the package manager console:

```csharp
Install-Package Cake.Powershell
```

or directly in your build script via a cake addin directive:

```csharp
#addin nuget:?package=Cake.Powershell&loaddependencies=true
```



## Usage

```csharp
#addin "Cake.Powershell"

Task("Powershell-Script")
    .Description("Run an example powershell command with parameters")
    .Does(() =>
{
    StartPowershellScript("Write-Host", args =>
        {
            args.AppendQuoted("Testing...");
        });
});

Task("Powershell-Script-Settings")
    .Description("Run an example powershell command with settings and parameters")
    .Does(() =>
{
    StartPowershellScript("Get-Process", new PowershellSettings()
        .SetFormatOutput()
        .SetLogOutput()
        .WithArguments(args =>
        {
            args.AppendQuoted("svchost");
        }));
});


Task("Powershell-File")
    .Description("Run an example powershell script file with parameters")
    .Does(() =>
{
    StartPowershellFile("../Scripts/Install.ps1", args =>
        {
            args.Append("Username", "admin")
                .AppendSecret("Password", "pass1");
        });
});

Task("Powershell-File-Settings")
    .Description("Run an example powershell script file with settings and parameters")
    .Does(() =>
{
    StartPowershellFile("../Scripts/sql.ps1", new PowershellSettings()
        .WithModule("sqlps")
        .WithArguments(args =>
        {
            args.Append("Username", "admin")
                .AppendSecret("Password", "pass1");
        }));
});


Task("Powershell-Remote-Script")
    .Description("Run an example powershell command remotely")
    .Does(() =>
{
    StartPowershellScript("Get-Services", new PowershellSettings()
    {
        ComputerName = "remote-location",
        Username = "admin",
        Password = "pass1",

        FormatOutput = true,
        LogOutput = true
    });
});

Task("Powershell-Remote-File")
    .Description("Run an example powershell file remotely")
    .Does(() =>
{
    StartPowershellFile("C:/Scripts/sql.ps1", new PowershellSettings()
        {
            ComputerName = "remote-location",
            Username = "admin",
            Password = "pass1",

            FormatOutput = true,
            LogOutput = true
        }.WithArguments(args =>
        {
            args.Append("task", "do-what-i-say");
        }));
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

A complete Cake example can be found [here](https://github.com/SharpeRAD/Cake.Powershell/blob/master/test/build.cake).



## TroubleShooting

* Please be aware of the breaking changes that occurred with the release of [Cake v0.22.0](https://cakebuild.net/blog/2017/09/cake-v0.22.0-released), you will need to upgrade Cake in order to use Cake.Powershell v0.4.0 or above.

A few pointers for correctly enabling powershell scripting can be found [here](https://github.com/SharpeRAD/Cake.Powershell/blob/master/TroubleShooting.md).



## Plays well with

If your looking for a way to trigger cake tasks based on windows events or at scheduled intervals then check out [CakeBoss](https://github.com/SharpeRAD/CakeBoss).



## License

Copyright (c) 2015 - 2016 Phillip Sharpe

Cake.Powershell is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/SharpeRAD/Cake.Powershell/blob/master/LICENSE).



## Share the love

If this project helps you in anyway then please :star: the repository.
