var fileSystemType = Context.FileSystem.GetType();
var file = Context.FileSystem.GetFile("test.cake");
var directory = Context.FileSystem.GetFile("tools");

if (fileSystemType.ToString()=="Cake.LongPath.Module.LongPathFileSystem")
{
    Information("Sucessfully loaded {0}", fileSystemType.Assembly.Location);
}
else
{
    Error("Failed to load Cake.LongPath.Module");
}

Information(
    "IFileSystem: {0}\r\n\tIFile: {1}\r\n\tIDirectory: {2}",
    Context.FileSystem.GetType(),
    file.GetType(),
    directory.GetType()
    );