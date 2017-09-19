@ECHO OFF
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '.\build.ps1' -Target 'Skip-Restore'"
PAUSE