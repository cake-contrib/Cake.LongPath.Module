#load "nuget:?package=Cake.Recipe&version=1.0.0"

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Longpath.Module",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.Longpath.Module",
                            appVeyorAccountName: "cakecontrib");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                                BuildParameters.RootDirectoryPath + "/src/Cake.Longpath.Module.Tests/**/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Task("Integration-Tests")
    .IsDependentOn("Build")
    .IsDependeeOf("Test")
    .Does(()=> {

    var moduleTestPath = MakeAbsolute(Directory("moduletest"));
    var moduleToolsPath = moduleTestPath.Combine("tools");
    var moduleModulesPath = moduleToolsPath.Combine("modules").Combine("Cake.LongPath.Module");
    var testCakePath = moduleTestPath.CombineWithFilePath("test.cake");
    var longPathModulePath = BuildParameters.Paths.Directories.PublishedLibraries.Combine("Cake.LongPath.Module");

    if (DirectoryExists(moduleToolsPath))
    {
        DeleteDirectory(moduleToolsPath,
            new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }

    EnsureDirectoryExists(moduleToolsPath);
    EnsureDirectoryExists(moduleModulesPath);

    CopyDirectory(longPathModulePath, moduleModulesPath);

    CakeExecuteScript(testCakePath);
});

Build.Run();
