#-------------------------------
# Installation
#-------------------------------

Write-Host "Downloading .NET Core SDK 3.1 ..."

(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/cc28204e-58d7-4f2e-9539-aad3e71945d9/d4da77c35a04346cc08b0cacbc6611d5/dotnet-sdk-3.1.406-win-x64.exe','dotnet-sdk-3.1.406-win-x64.exe')
# Invoke-WebRequest "https://go.microsoft.com/fwlink/?linkid=841686" -OutFile "dotnet-core-sdk.exe"

Write-Host "Installing .NET Core SDK 3.1 ..."

Invoke-Command -ScriptBlock { ./dotnet-sdk-3.1.406-win-x64.exe /S /v/qn }

Write-Host "Installation succeeded." -ForegroundColor Green