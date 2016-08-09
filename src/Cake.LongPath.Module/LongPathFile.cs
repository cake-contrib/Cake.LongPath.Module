// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using System.IO;
using Path = Cake.Core.IO.Path;

namespace Cake.LongPath.Module
{
    internal sealed class LongPathFile : IFile
    {
        private Pri.LongPath.FileInfo File { get; }

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path { get; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        public long Length => File.Length;

        /// <summary>
        /// Gets the path to the entry.
        /// </summary>
        /// <value>The path.</value>
        Path IFileSystemInfo.Path => Path;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LongPathFile"/> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists => File.Exists;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo"/> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool Hidden => (File.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        public void Copy(FilePath destination, bool overwrite)
        {
            File.CopyTo(destination.FullPath, overwrite);
        }

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        public void Move(FilePath destination)
        {
            File.MoveTo(destination.FullPath);
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public void Delete()
        {
            File.Delete();
            File.Refresh();
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return File.Open(fileMode, fileAccess, fileShare);
        }

        public LongPathFile(FilePath path)
        {
            Path = path;
            File =  new Pri.LongPath.FileInfo(path.FullPath);
        }

        public LongPathFile(Pri.LongPath.FileInfo file)
        {
            Path = new FilePath(file.FullName);
            File = file;
        }
    }
}
