#module nuget:?package=Cake.LongPath.Module

Task("IFileSystem")
    .Does(() => {
    var fileSystemType = Context.FileSystem.GetType();

    Information(
        "IFileSystem: {0}",
        Context.FileSystem.GetType());

    if (fileSystemType.ToString()=="Cake.LongPath.Module.LongPathFileSystem")
    {
        Information("Sucessfully loaded {0}", fileSystemType.Assembly.Location);
    }
    else
    {
        throw new Exception("Failed to load Cake.LongPath.Module");
    }
});

Task("IFile")
    .IsDependentOn("IFileSystem")
    .Does(() => {
    var file = Context.FileSystem.GetFile("build.cake");

    Information(
        "IFile: {0}\r\n\t{1}",
        file.GetType(),
        file.Path);
});

Task("IDirectory")
    .IsDependentOn("IFileSystem")
    .Does(() => {
    var directory = Context.FileSystem.GetFile("tools");

    Information(
        "IDirectory: {0}\r\n\t{1}",
        directory.GetType(),
        directory.Path);
});

Task("Default")
    .IsDependentOn("IFileSystem")
    .IsDependentOn("IFile")
    .IsDependentOn("IDirectory");


RunTarget("Default");