@ECHO OFF
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '.\build.ps1' -Tools 'tools' -Verbosity 'Diagnostic'"
PAUSE
