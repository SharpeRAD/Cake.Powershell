#-------------------------------
# Installation
#-------------------------------

Write-Host "Downloading .NET SDK 6.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/343dc654-80b0-4f2d-b172-8536ba8ef63b/93cc3ab526c198e567f75169d9184d57/dotnet-sdk-6.0.101-win-x64.exe','dotnet-sdk-6.0.101-win-x64.exe')

Write-Host "Installing .NET SDK 6.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-6.0.101-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green


Write-Host "Downloading .NET SDK 7.0 ..."
(New-Object System.Net.WebClient).DownloadFile('https://download.visualstudio.microsoft.com/download/pr/c6ad374b-9b66-49ed-a140-588348d0c29a/78084d635f2a4011ccd65dc7fd9e83ce/dotnet-sdk-7.0.202-win-x64.exe','dotnet-sdk-7.0.202-win-x64.exe')

Write-Host "Installing .NET SDK 7.0 ..."
Invoke-Command -ScriptBlock { ./dotnet-sdk-7.0.202-win-x64.exe /S /v/qn }
Write-Host "Installation succeeded." -ForegroundColor Green


