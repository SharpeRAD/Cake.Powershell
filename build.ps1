<#
.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.
.DESCRIPTION
This Powershell script will restore dotnet tools (including Cake)
and execute your Cake build script with the parameters you provide.
.LINK
https://cakebuild.net

.PARAMETER Script
The build script file to run.

.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.

.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER WhatIf
Performs a dry run of the build script.
No tasks will be executed.

.PARAMETER ScriptArgs
Remaining arguments are added here.
#>





###########################################################################
# Define Parameters
###########################################################################

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",

    [string]$Target = "Default",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
	
    [string]$Verbosity = "Verbose",
    [switch]$WhatIf,
	
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)


# Use TLS 1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12;





###########################################################################
# Define Paths
###########################################################################

# Get Script root
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent



###########################################################################
# INSTALL CAKE
###########################################################################

# Make sure Cake has been installed.

Invoke-Expression "dotnet tool restore" | Out-Null;

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

# Build the argument list.
$Arguments = @{
    target=$Target;
    configuration=$Configuration;
    verbosity=$Verbosity;
    dryrun=$WhatIf;
}.GetEnumerator() | %{"--{0}=`"{1}`"" -f $_.key, $_.value };

# Start Cake
Write-Host "Running build script..."
Invoke-Expression "dotnet cake `"$Script`" $Arguments $ScriptArgs"

exit $LASTEXITCODE