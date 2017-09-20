# PowerShell Troubleshooting

For more information refer to:
https://technet.microsoft.com/en-us/library/hh847850.aspx



### Error message
```
Error: Failed to install addin 'Cake.Powershell'
Could not find any assemblies compatible with .NETFramework, Version=4.5
```

### Solution
* Please be aware of the breaking changes that occurred with the release of [Cake v0.22.0](https://cakebuild.net/blog/2017/09/cake-v0.22.0-released), you will need to upgrade Cake in order to use Cake.Powershell [v0.4.0] or above.



### Error message
```
Connecting to remote server failed with the following error message : Access is denied. For
more information, see the about_Remote_Troubleshooting Help topic.
```

### Solution
* Run the following PowerShell command as an administrator to enable remote scripts:
```
Enable-PSRemoting -Force
```

* Run the following PowerShell command as an administrator to make sure the current user has the required privileges:
```
Set-PSSessionConfiguration Microsoft.PowerShell -ShowSecurityDescriptorUI
```



### Error message
```
cannot be loaded because running scripts is disabled on this system.
```

### Solution
* Run the following PowerShell command  as an administrator to enable the execution of all scripts:
```
Set-ExecutionPolicy Unrestricted
```



### Error message
```
method get_BufferSize is not implemented.
```

### Solution
* Disable output to console
```
settings.OutputToAppConsole = false;
```



### Error message
```
Error: Could not load file or assembly 'System.Management.Automation, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3
1bf3856ad364e35' or one of its dependencies. Strong name validation failed. (Exception from HRESULT: 0x8013141A)
```

### Solution
* Run the following PowerShell command to check the installed version:
```
$PSVersionTable.PSVersion
```

If the major version is less than 5 update Windows Management framework.
https://www.microsoft.com/en-us/download/details.aspx?id=50395
