@ECHO OFF
IF NOT EXIST "tools" (md "tools")
IF NOT EXIST "tools\modules" (md "tools\modules")
IF NOT EXIST "tools\nuget.exe" (@powershell -NoProfile -ExecutionPolicy Bypass -Command "(New-Object System.Net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe','tools/nuget.exe')")
IF NOT EXIST "tools\Cake" (tools\nuget.exe install Cake -ExcludeVersion -OutputDirectory "Tools" -Source https://api.nuget.org/v3/index.json)
IF NOT EXIST "tools\modules\Cake.LongPath.Module" (tools\nuget.exe install Cake.LongPath.Module -OutputDirectory "Tools\Modules" -Source https://api.nuget.org/v3/index.json)
tools\Cake\Cake.exe test.cake
