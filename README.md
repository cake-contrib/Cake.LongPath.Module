# Cake LongPath Module
Cake Module that adds long path support to build scripts running on Windows using Pri.LongPath

This module replaces the default `IFileSystem` with one that uses Pri.LongPath to give long path support.
This is an early alpha version, so still pre-release and made avail for testing purposes.

## Installation
Pre-compiled binaries are available on NuGet package id [Cake.LongPath.Module](https://www.nuget.org/packages/Cake.LongPath.Module),
Cake modules are automatically loaded based on the `tools/modules` folder relative to the Cake script being executed.

1. Create folder `tools\modules`
2. Install [Cake.LongPath.Module](https://www.nuget.org/packages/Cake.LongPath.Module) from nuget
```PowerShell
nuget.exe install Cake.LongPath.Module -PreRelease -ExcludeVersion -OutputDirectory "tools\modules" -Source https://www.nuget.org/api/v2/
```
