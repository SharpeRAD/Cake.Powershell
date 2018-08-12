<#
.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.
.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.
.LINK
https://cakebuild.net

.PARAMETER Script
The build script file to run.
.PARAMETER Tools
The tools directory to use.

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
    [string]$Tools,

    [string]$Target = "Default",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
	
    [string]$Verbosity = "Verbose",
    [switch]$WhatIf,
	
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)



$CakeVersion = "0.29.0"
$DotNetChannel = "preview";
$DotNetVersion = "2.1.2";
$DotNetInstallerUri = "https://dot.net/v1/dotnet-install.ps1";
$NugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# Temporarily skip verification and opt-in to new in-proc NuGet
$ENV:CAKE_SETTINGS_SKIPVERIFICATION='true'
$ENV:CAKE_NUGET_USEINPROCESSCLIENT='true'





###########################################################################
# Define Paths
###########################################################################

# Get Script root
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

# Find tools
if($Tools)
{
    # Parameter
    Write-Verbose -Message "Using tools parameter"
    $TOOLS_DIR = Join-Path $PSScriptRoot $Tools
}
elseif (Test-Path "C:/Tools/")
{
    # Shared location
    Write-Verbose -Message "Using shared tools"
    $TOOLS_DIR = "C:/Tools"
}
else
{
    # Local path
    Write-Verbose -Message "Using local tools"
    $TOOLS_DIR = Join-Path $PSScriptRoot "tools"
}

# Make sure tools folder exists
if (!(Test-Path $TOOLS_DIR)) 
{
    Write-Verbose "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}



# Define directories
$ADDINS_DIR = Join-Path $TOOLS_DIR "/Addins"
$MODULES_DIR = Join-Path $TOOLS_DIR "/Modules"

Write-Verbose -Message $ADDINS_DIR

# Save paths to environment for use in child processes
$ENV:CAKE_PATHS_TOOLS = $TOOLS_DIR
$ENV:CAKE_PATHS_ADDINS = $ADDINS_DIR
$ENV:CAKE_PATHS_MODULES = $MODULES_DIR





###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

Function Remove-PathVariable([string]$VariableToRemove)
{
    $path = [Environment]::GetEnvironmentVariable("PATH", "User")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
    }

    $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
    }
}



# Enforce TLS 1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12;



# Get .NET Core CLI path if installed.
$FoundDotNetCliVersion = $null;

if (Get-Command dotnet -ErrorAction SilentlyContinue) 
{
    $FoundDotNetCliVersion = dotnet --version;
}

if($FoundDotNetCliVersion -ne $DotNetVersion) 
{
    $InstallPath = Join-Path $TOOLS_DIR "DotNet"

    if (!(Test-Path $InstallPath)) 
    {
        mkdir -Force $InstallPath | Out-Null;
    }

    (New-Object System.Net.WebClient).DownloadFile($DotNetInstallerUri, "$InstallPath\dotnet-install.ps1");
    & $InstallPath\dotnet-install.ps1 -Channel $DotNetChannel -Version $DotNetVersion -InstallDir $InstallPath;

    Remove-PathVariable "$InstallPath"
    $env:PATH = "$InstallPath;$env:PATH"
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
    $env:DOTNET_CLI_TELEMETRY_OPTOUT=1
}





###########################################################################
# INSTALL NUGET
###########################################################################

# Make sure nuget.exe exists.
$NugetPath = Join-Path $TOOLS_DIR "nuget.exe"

if (!(Test-Path $NugetPath)) 
{
    Write-Host "Downloading NuGet.exe..."
    (New-Object System.Net.WebClient).DownloadFile($NugetUrl, $NugetPath);
}





###########################################################################
# INSTALL CAKE
###########################################################################

# Make sure Cake has been installed.
$CakePath = Join-Path $TOOLS_DIR "Cake.$CakeVersion/Cake.exe"

if (!(Test-Path $CakePath)) 
{
    Write-Host "Installing Cake..."
    Invoke-Expression "&`"$NugetPath`" install Cake -Version $CakeVersion -OutputDirectory `"$TOOLS_DIR`"" | Out-Null;

    if ($LASTEXITCODE -ne 0) 
    {
        Throw "An error occured while restoring Cake from NuGet."
    }
}




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
Invoke-Expression "& `"$CakePath`" `"$Script`" $Arguments $ScriptArgs"

exit $LASTEXITCODE