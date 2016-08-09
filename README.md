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
3. Next time you execute your build script the new module should be loaded.

Folder structure after installation should look something like this
```
|   build.cake
\---tools
    +---Cake
    |       Cake.exe
    |       ...
    |
    \---modules
        \---Cake.LongPath.Module
                Cake.LongPath.Module.dll
                ...
```

You can find an minimal example of module installation script in [moduleusageexample/build.cmd](moduleusageexample/build.cmd), that should output something like below if everything works:
```
Sucessfully loaded [path to moduleusageexample]\tools\modules\Cake.LongPath.Module\Cake.LongPath.Module.dll
IFileSystem: Cake.LongPath.Module.LongPathFileSystem
        IFile: Cake.LongPath.Module.LongPathFile
        IDirectory: Cake.LongPath.Module.LongPathFile
```

## Test

The project is setup to debug using [moduletest/test.cake](moduletest/test.cake), you can also execute it from the command-line using [moduletest/test.cmd](moduletest/test.cmd).
