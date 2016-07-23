Param(
    [string]$Script = "build.cake",
    [string]$Tools,

    [string]$Target = "Default",
    [string]$Custom = "",

    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",

    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Verbose",

    [int]$BufferHeight = 5000,

    [switch]$Experimental,
    [switch]$WhatIf,
    [switch]$Mono,
    [switch]$SkipToolPackageRestore
)



[Reflection.Assembly]::LoadWithPartialName("System.Security") | Out-Null
function MD5HashFile([string] $filePath)
{
    if ([string]::IsNullOrEmpty($filePath) -or !(Test-Path $filePath -PathType Leaf))
    {
        return $null
    }

    [System.IO.Stream] $file = $null;
    [System.Security.Cryptography.MD5] $md5 = $null;
    try
    {
        $md5 = [System.Security.Cryptography.MD5]::Create()
        $file = [System.IO.File]::OpenRead($filePath)
        return [System.BitConverter]::ToString($md5.ComputeHash($file))
    }
    finally
    {
        if ($file -ne $null)
        {
            $file.Dispose()
        }
    }
}



Write-Host "Preparing to run build script..."



# Script Location
if(!$PSScriptRoot)
{
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}



# Find tools
if($Tools)
{
    # Parameter
    Write-Verbose -Message "Using tools parameter"
    $TOOLS_DIR = Join-Path $PSScriptRoot $Tools
}
elseif (Test-Path "C:/Tools/Cake/Cake.exe")
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
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR))
{
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}





# Define Paths
$ADDINS_DIR = Join-Path $TOOLS_DIR "/Addins"
$MODULES_DIR = Join-Path $TOOLS_DIR "/Modules"

Write-Verbose -Message $ADDINS_DIR

$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"

$NUGET_URL = "https://nuget.org/nuget.exe"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"

$PACKAGES_CONFIG = Join-Path $TOOLS_DIR "packages.config"
$PACKAGES_CONFIG_MD5 = Join-Path $TOOLS_DIR "packages.config.md5sum"



# Save paths to environment for use in child processes
$ENV:NUGET_EXE = $NUGET_EXE

$ENV:CAKE_PATHS_TOOLS = $TOOLS_DIR
$ENV:CAKE_PATHS_ADDINS = $ADDINS_DIR
$ENV:CAKE_PATHS_MODULES =$MODULES_DIR



# Increase the default buffer size
$PSHost = get-host
$PSWindow = $PSHost.ui.rawui

$BufferSize = $PSWindow.buffersize
$BufferSize.height = $BufferHeight
$BufferSize.width = 120
$PSWindow.buffersize = $BufferSize

$WindowSize = $PSWindow.windowsize
$WindowSize.height = 50
$WindowSize.width = 120
$PSWindow.windowsize = $WindowSize





# Should we use mono?
$UseMono = "";
if($Mono.IsPresent)
{
    Write-Verbose -Message "Using the Mono based scripting engine."
    $UseMono = "-mono"
}

# Should we use the new Roslyn?
$UseExperimental = "";
if($Experimental.IsPresent -and !($Mono.IsPresent))
{
    Write-Verbose -Message "Using experimental version of Roslyn."
    $UseExperimental = "-experimental"
}

# Is this a dry run?
$UseDryRun = "";
if($WhatIf.IsPresent)
{
    $UseDryRun = "-dryrun"
}



# Make sure that packages.config exist.
if (!(Test-Path $PACKAGES_CONFIG))
{
    Write-Verbose -Message "Downloading packages.config..."
    try
    {
        (New-Object System.Net.WebClient).DownloadFile("http://cakebuild.net/download/bootstrapper/packages", $PACKAGES_CONFIG)
    }
    catch
    {
        Throw "Could not download packages.config."
    }
}

# Try find NuGet.exe in path if not exists
if (!(Test-Path $NUGET_EXE))
{
    Write-Verbose -Message "Trying to find nuget.exe in PATH..."
    $existingPaths = $Env:Path -Split ';' | Where-Object { (![string]::IsNullOrEmpty($_)) -and (Test-Path $_) }
    $NUGET_EXE_IN_PATH = Get-ChildItem -Path $existingPaths -Filter "nuget.exe" | Select -First 1
    if ($NUGET_EXE_IN_PATH -ne $null -and (Test-Path $NUGET_EXE_IN_PATH.FullName))
    {
        Write-Verbose -Message "Found in PATH at $($NUGET_EXE_IN_PATH.FullName)."
        $NUGET_EXE = $NUGET_EXE_IN_PATH.FullName
    }
}

# Try download NuGet.exe if not exists
if (!(Test-Path $NUGET_EXE))
{
    Write-Verbose -Message "Downloading NuGet.exe..."
    try
    {
        (New-Object System.Net.WebClient).DownloadFile($NUGET_URL, $NUGET_EXE)
    }
    catch
    {
        Throw "Could not download NuGet.exe."
    }
}





# Restore tools from NuGet?
if(-Not $SkipToolPackageRestore.IsPresent)
{
    Push-Location
    Set-Location $TOOLS_DIR

    # Check for changes in packages.config and remove installed tools if true.
    [string] $md5Hash = MD5HashFile($PACKAGES_CONFIG)
    if((!(Test-Path $PACKAGES_CONFIG_MD5)) -Or ($md5Hash -ne (Get-Content $PACKAGES_CONFIG_MD5 )))
    {
        Write-Verbose -Message "Missing or changed package.config hash..."
        Remove-Item * -Recurse -Exclude packages.config,nuget.exe
    }

    Write-Verbose -Message "Restoring tools from NuGet..."
    $NuGetOutput = Invoke-Expression "&`"$NUGET_EXE`" install -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""

    if ($LASTEXITCODE -ne 0)
    {
        Throw "An error occured while restoring NuGet tools."
    }
    else
    {
        $md5Hash | Out-File $PACKAGES_CONFIG_MD5 -Encoding "ASCII"
    }
    Write-Verbose -Message ($NuGetOutput | Out-String)
    Pop-Location
}

# Make sure that Cake has been installed.
if (!(Test-Path $CAKE_EXE))
{
    Throw "Could not find Cake.exe at $CAKE_EXE"
}



# Start Cake
Invoke-Expression "$CAKE_EXE `"$Script`" -target=`"$Target`" -configuration=`"$Configuration`" -custom=`"$Custom`" -verbosity=`"$Verbosity`" $UseMono $UseDryRun $UseExperimental"
exit $LASTEXITCODE
