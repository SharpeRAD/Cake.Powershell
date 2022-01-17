
param 
(                      
	[string] $Service = ""                     
)

Function Get-Service-Linux {
	[CmdletBinding()]

	Param(
		[Parameter( Position = 0, ValueFromPipeline = $True )][String]$Name
	)

	Process {
		If ( $Name ) {
			$services = & systemctl list-units $Name --type=service --no-legend --all --no-pager
		}
		Else {
			$services = & systemctl list-units --type=service --no-legend --all --no-pager
		}

		$services = $services -Split "`n"

		$services = $services | ForEach-Object {
			$service = $_ -Split '\\s+'

			[PSCustomObject]@{
				"Name"        = ($service[0] -Split "\\.service")[0]
				"Unit"        = $service[0]
				"State"       = $service[1]
				"Active"      = (($service[2] -EQ 'active') ? $true : $false)
				"Status"      = $service[3]
				"Description" = ($service[4..($service.count - 2)] -Join " ")
			}
		}

		$services
	}
}

Function Get-Service-MacOS {
	[CmdletBinding()]

	Param(
		[Parameter( Position = 0, ValueFromPipeline = $True )][String]$Name
	)

	Process {
		If ( $Name ) {
			$services = & sudo launchctl list | grep -i $Name
		}
		Else {
			$services = & sudo launchctl list
		}

		$services
	}
}

if ($Service -eq "") {
	If ( $IsLinux ) {
		Get-Service-Linux
	} ElseIf ( $IsMacOS) {
		Get-Service-MacOS
	}
}
else {
	If ( $IsLinux ) {
		Get-Service-Linux $Service
	} ElseIf ( $IsMacOS) {
		Get-Service-MacOS $Service
	}
}



