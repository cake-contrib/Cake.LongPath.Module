#module nuget:?package=Cake.DotNetTool.Module&version=1.0.1
#tool dotnet:?package=DPI&version=2021.3.11.25
#load "nuget:?package=Cake.Recipe&version=1.1.2"

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
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs",
                            buildMSBuildToolVersion: MSBuildToolVersion.VS2019
                            );

Task("DPI")
    .IsDependentOn("Restore")
    .IsDependeeOf("Build")
    .Does(
        context =>
{
    var result = context.StartProcess(
        context.Tools.Resolve("dpi") ?? context.Tools.Resolve("dpi.exe"),
        new ProcessSettings {
            Arguments = new ProcessArgumentBuilder()
                                                .Append("nuget")
                                                .Append("--silent")
                                                .AppendSwitchQuoted("--output", "table")
                                                .Append(
                                                    (
                                                        string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID")) //AzurePipelines.Environment.PullRequest.IsPullRequest
                                                        &&
                                                        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("NuGetReportSettings_SharedKey"))
                                                        &&
                                                        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("NuGetReportSettings_WorkspaceId"))
                                                    )
                                                        ? "report"
                                                        : "analyze"
                                                    )
                                                .AppendSwitchQuoted("--buildversion", BuildParameters.Version.SemVersion)
        }
    );

    if (result != 0)
    {
        throw new Exception($"Failed to execute DPI ({result}");
    }
});


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

    CakeExecuteScript(
        testCakePath,
        new CakeSettings
        {
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", moduleToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", moduleToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", moduleToolsPath.Combine("Modules").FullPath }
            }
        });
});

Build.Run();
