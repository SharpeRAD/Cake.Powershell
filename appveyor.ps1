#-------------------------------
# Installation
#-------------------------------

Write-Host "Downloading .NET Core SDK 3.1 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/cc28204e-58d7-4f2e-9539-aad3e71945d9/d4da77c35a04346cc08b0cacbc6611d5/dotnet-sdk-3.1.406-win-x64.exe','dotnet-sdk-3.1.406-win-x64.exe')
# Invoke-WebRequest "https://go.microsoft.com/fwlink/?linkid=841686" -OutFile "dotnet-core-sdk.exe"

Write-Host "Installing .NET Core SDK 3.1 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-3.1.406-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green

Write-Host "Downloading .NET SDK 5.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/44069ee2-ce02-41d7-bcc5-2168a1653278/cfc5131c81ae00a5f77f05f9963ec98d/dotnet-sdk-5.0.404-win-x64.exe')

Write-Host "Installing .NET SDK 5.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-5.0.404-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green

Write-Host "Downloading .NET SDK 6.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/343dc654-80b0-4f2d-b172-8536ba8ef63b/93cc3ab526c198e567f75169d9184d57/dotnet-sdk-6.0.101-win-x64.exe','dotnet-sdk-6.0.101-win-x64.exe')

Write-Host "Installing .NET SDK 6.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-6.0.101-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green