// break if debugger is attached
#break
IFile file;
IDirectory directory;

DirectoryPath   testPath,
                binPath,
                debugPath,
                objPath,
                node_modulesPath,
                moveSourcePath,
                moveDestPath;

FilePath      solutionFile;
FilePath[]    debugBinFiles,
              objFiles;

Setup(
  context=>{
    file = Context.FileSystem.GetFile("test.cake");
    directory = Context.FileSystem.GetDirectory("tools");
    Information(
        "IFileSystem: {0}\r\n\tIFile: {1}\r\n\tIDirectory: {2}",
        Context.FileSystem.GetType(),
        file.GetType(),
        directory.GetType()
        );

    testPath = Directory("testpath");
    binPath = testPath.Combine("bin");
    debugPath = binPath.Combine("debug");
    objPath = testPath.Combine("obj");
    node_modulesPath = testPath.Combine("node_modules");
    moveSourcePath = testPath.Combine("moveSource");
    moveDestPath = testPath.Combine("moveDest");
  });

Teardown(context=>{
  DeleteDirectory(testPath, recursive:true);
  });

Task("Create-Directory-Structure")
    .Does(()=>{
      EnsureDirectoryExists(testPath);
      EnsureDirectoryExists(binPath);
      EnsureDirectoryExists(debugPath);
      EnsureDirectoryExists(objPath);
      EnsureDirectoryExists(node_modulesPath);
      EnsureDirectoryExists(moveSourcePath);
      EnsureDirectoryExists(moveDestPath);
});

Task("Create-Dummy-Test-Files")
    .IsDependentOn("Create-Directory-Structure")
    .Does(()=>{
        solutionFile = CreateRandomDataFile(Context, testPath.CombineWithFilePath("test.sln"));
        debugBinFiles = CreateRangeOfRandomDataFiles(Context, debugPath);
        objFiles = CreateRangeOfRandomDataFiles(Context, objPath);

        CreateRandomDataFile(Context, moveSourcePath.CombineWithFilePath("marker.txt"));

        Information("DebugBinFiles created: {0}", debugBinFiles.Length);
        Information("ObjFiles created: {0}", objFiles.Length);
  });

Task("Create-100-Levels-Of-Sub-node_modules")
  .IsDependentOn("Create-Dummy-Test-Files")
  .Does(()=>{
        DirectoryPath bad_node_modulePath = node_modulesPath;
        for(var level = 0; level< 100;level ++)
        {
            bad_node_modulePath = bad_node_modulePath.Combine("node_modules");
            EnsureDirectoryExists(bad_node_modulePath);
            CreateRangeOfRandomDataFiles(Context, bad_node_modulePath);
        }
        Information("Result path length: {0}", bad_node_modulePath.FullPath.Length);
});

Task("Get-Solution-Files")
  .IsDependentOn("Create-100-Levels-Of-Sub-node_modules")
  .Does(()=>{
      var solutions = GetFiles("./**/*.sln").ToArray();
      Information("Solutions: {0}", solutions.Length);
      foreach(var solution in solutions)
      {
        Information("Solution: {0}", solution);
      }
      if (solutions.Length != 1 || solutions[0].FullPath!=MakeAbsolute(solutionFile).FullPath)
      {
        throw new Exception("Failed to fetch solutions");
      }
});

Task("Clean-Directories")
    .IsDependentOn("Get-Solution-Files")
    .Does(()=>{
      Information("Cleaning \"./testpath/**/bin/debug\"");
      CleanDirectories("./testpath/**/bin/debug");
      if (debugBinFiles.Any(file=>FileExists(file.FullPath)))
      {
        throw new Exception("Bin files not cleaned");
      }

      Information("Cleaning \"./testpath/**/obj\"");
      CleanDirectories("./testpath/**/obj");
      if (objFiles.Any(file=>FileExists(file.FullPath)))
      {
        throw new Exception("Bin files not cleaned");
      }
  });

Task("Move-Folder")
  .Does(() => {
      var source = Context.FileSystem.GetDirectory(moveSourcePath);
      var target = moveDestPath.Combine("target");
      
      Information("Moving {0} to {1}", source, target);
      source.Move(target);
      
      if(!FileExists(target.CombineWithFilePath("marker.txt")))
      {
        throw new Exception("Folder not moved correctly");
      }
  });

Task("Default")
  .IsDependentOn("Create-Directory-Structure")
  .IsDependentOn("Create-Dummy-Test-Files")
  .IsDependentOn("Create-100-Levels-Of-Sub-node_modules")
  .IsDependentOn("Get-Solution-Files")
  .IsDependentOn("Move-Folder")
  .IsDependentOn("Clean-Directories")
  .Does(()=>{
    Information("Done");
});

RunTarget("Default");

// break if debugger is attached
#break

// Utility methods for creating dummy data
using System.Security.Cryptography;
public static FilePath[] CreateRangeOfRandomDataFiles(ICakeContext context, DirectoryPath targetPath)
{
    return Enumerable.Range(1, 10)
      .Select(
        index=>CreateRandomDataFile(context, targetPath.CombineWithFilePath(string.Format("{0}.{1:000}",
              Guid.NewGuid(),
              index
              )))
        ).ToArray();
}

public static FilePath CreateRandomDataFile(ICakeContext context, FilePath filePath)
{
    context.Verbose("Creating file {0}...", filePath);
    var file = context.FileSystem.GetFile(filePath);

    using (var rngCsp = new RNGCryptoServiceProvider())
    {
        var data = new byte[128];
        rngCsp.GetBytes(data);
        var base64Data = Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
        using(var stream = file.OpenWrite())
        {
            using(var writer = new System.IO.StreamWriter(stream, Encoding.ASCII))
            {
                writer.WriteLine(base64Data);
            }
        }
    }
    context.Verbose("File {0} created.", filePath);
    return new FilePath(filePath.FullPath);
}
