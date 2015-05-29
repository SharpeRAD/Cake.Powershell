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

Run the following powershell command (as Admin):

Enable-PSRemoting -Force



### Error message

```
Connecting to remote server failed with the following error message : Access is denied. For 
more information, see the about_Remote_Troubleshooting Help topic.
```

### Solution

Run the following powershell command (as Admin):

Set-PSSessionConfiguration Microsoft.PowerShell -ShowSecurityDescriptorUI

(make sure the current user has the required priviledges)



### Error message

```
cannot be loaded because running scripts is disabled on this system.
```

### Solution

Run the following powershell command (as Admin):

Set-ExecutionPolicy Unrestricted

(make sure the current user has the required priviledges)