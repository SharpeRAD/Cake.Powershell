#-------------------------------
# Installation
#-------------------------------

$arguments = "/S /v/qn"

Write-Host "Downloading .NET SDK 6.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/0814dade-52c0-4f97-83f4-21f784b03a2e/6f0d4b4dc596824a365b63882982031b/dotnet-sdk-6.0.423-win-x64.exe','dotnet-sdk-6.0.423-win-x64.exe')

Write-Host "Installing .NET SDK 6.0 ..."
$installerPath = "./dotnet-sdk-6.0.423-win-x64.exe"
Start-Process -FilePath $installerPath -ArgumentList $arguments -Wait
Write-Host "Installation succeeded." -ForegroundColor Green


Write-Host "Downloading .NET SDK 7.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/6f7abf5c-3f6d-43cc-8f3c-700c27d4976b/b7a3b806505c95c7095ca1e8c057e987/dotnet-sdk-7.0.410-win-x64.exe','dotnet-sdk-7.0.410-win-x64.exe')

Write-Host "Installing .NET SDK 7.0 ..."
$installerPath = "./dotnet-sdk-7.0.410-win-x64.exe"
Start-Process -FilePath $installerPath -ArgumentList $arguments -Wait
Write-Host "Installation succeeded." -ForegroundColor Green

Write-Host "Downloading .NET SDK 8.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/b6f19ef3-52ca-40b1-b78b-0712d3c8bf4d/426bd0d376479d551ce4d5ac0ecf63a5/dotnet-sdk-8.0.302-win-x64.exe','dotnet-sdk-8.0.302-win-x64.exe')

Write-Host "Installing .NET SDK 8.0 ..."
$installerPath = "./dotnet-sdk-8.0.302-win-x64.exe"
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
