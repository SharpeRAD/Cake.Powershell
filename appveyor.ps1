#-------------------------------
# Installation
#-------------------------------

$arguments = "/S /v/qn"

Write-Host "Downloading .NET SDK 8.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.415/dotnet-sdk-8.0.415-win-x64.exe','dotnet-sdk-8.0.415-win-x64.exe')

Write-Host "Installing .NET SDK 8.0 ..."
$installerPath = "./dotnet-sdk-8.0.415-win-x64.exe"
Start-Process -FilePath $installerPath -ArgumentList $arguments -Wait
Write-Host "Installation succeeded." -ForegroundColor Green

Write-Host "Downloading .NET SDK 9.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.306/dotnet-sdk-9.0.306-win-x64.exe','dotnet-sdk-9.0.306-win-x64.exe')

Write-Host "Installing .NET SDK 9.0 ..."
$installerPath = "./dotnet-sdk-9.0.306-win-x64.exe"
Start-Process -FilePath $installerPath -ArgumentList $arguments -Wait
Write-Host "Installation succeeded." -ForegroundColor Green

# Add the .NET SDK to the PATH environment variable
$dotnetPath = "C:\Program Files\dotnet"
if (Test-Path $dotnetPath) {
    $currentPath = [System.Environment]::GetEnvironmentVariable("PATH", [System.EnvironmentVariableTarget]::User)
    if ($currentPath -notlike "*$dotnetPath*") {
        [System.Environment]::SetEnvironmentVariable("PATH", $currentPath + ";$dotnetPath", [System.EnvironmentVariableTarget]::User)
        Write-Host "PATH updated for current user." -ForegroundColor Green
    } else {
        Write-Host "PATH already contains .NET SDK path." -ForegroundColor Yellow
    }
} else {
    Write-Host "Error: .NET SDK installation path not found." -ForegroundColor Red
    exit 1
}

Write-Host "Checking installed version"
Invoke-Command -ScriptBlock { dotnet --version }
