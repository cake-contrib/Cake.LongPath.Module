// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.LongPath.Module
{
    internal sealed class LongPathFileSystem : IFileSystem
    {
        public IFile GetFile(FilePath path)
        {
            return new LongPathFile(path);
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new LongPathDirectory(path);
        }
    }
}
