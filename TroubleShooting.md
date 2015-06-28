Cake Powershell add-on Troubleshooting
=============

# Remote scripting problems

For more information refer to:
https://technet.microsoft.com/en-us/library/hh847850.aspx



### Error message
```
Connecting to remote server failed with the following error message : Access is denied. For 
more information, see the about_Remote_Troubleshooting Help topic.
```

### Solution

* Run the following powershell command as an administrator ro enable remote scripts:

Enable-PSRemoting -Force

*Run the following powershell command as an administrator to make sure the current user has the required priviledges:

Set-PSSessionConfiguration Microsoft.PowerShell -ShowSecurityDescriptorUI



### Error message
```
cannot be loaded because running scripts is disabled on this system.
```

### Solution

Run the following powershell command  as an administrator to enable the execution of all scripts:

Set-ExecutionPolicy Unrestricted