#-------------------------------
# Installation
#-------------------------------

Write-Host "Downloading .NET SDK 6.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/9b8baa92-04f4-4b1a-8ccd-aa6bf31592bc/3a25c73326e060e04c119264ba58d0d5/dotnet-sdk-6.0.418-win-x64.exe','dotnet-sdk-6.0.418-win-x64.exe')

Write-Host "Installing .NET SDK 6.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-6.0.418-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green


Write-Host "Downloading .NET SDK 7.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/febc46ff-cc68-4bee-83d2-c34786b5ca68/524ef9b25d29dc90efdb0fba0f589779/dotnet-sdk-7.0.405-win-x64.exe','dotnet-sdk-7.0.405-win-x64.exe')

Write-Host "Installing .NET SDK 7.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-7.0.405-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green

Write-Host "Downloading .NET SDK 8.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/cb56b18a-e2a6-4f24-be1d-fc4f023c9cc8/be3822e20b990cf180bb94ea8fbc42fe/dotnet-sdk-8.0.101-win-x64.exe','dotnet-sdk-8.0.101-win-x64.exe')

Write-Host "Installing .NET SDK 8.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-8.0.101-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green


