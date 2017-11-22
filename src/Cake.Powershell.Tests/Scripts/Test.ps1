param 
(                      
	[string] $Service = ""                     
)

if ($Service -eq "")
{
	Get-Service
}
else
{
	Get-Service $Service
}