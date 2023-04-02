param 
(                      
	[string] $Service = ""                     
)

if ($Service -eq "")
{
	# Get-Service can throw unwanted exception, see https://github.com/PowerShell/PowerShell/issues/10371 
	Get-Service -ErrorAction SilentlyContinue
}
else
{
	Get-Service $Service
}