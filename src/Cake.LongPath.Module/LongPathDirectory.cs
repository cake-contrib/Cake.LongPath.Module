// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Path = Cake.Core.IO.Path;

namespace Cake.LongPath.Module
{
    internal class LongPathDirectory : IDirectory
    {
        private Pri.LongPath.DirectoryInfo Directory { get; }

        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        public DirectoryPath Path { get; }

        /// <summary>
        /// Gets the path to the entry.
        /// </summary>
        /// <value>The path.</value>
        Path IFileSystemInfo.Path => Path;

        public bool Exists => Directory.Exists;
        public bool Hidden => (Directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        /// <summary>
        /// Creates the directory.
        /// </summary>
        public void Create()
        {
            Directory.Create();
        }

        /// <summary>
        /// Moves the directory to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        public void Move(DirectoryPath destination)
        {
            Directory.MoveTo(destination.FullPath);
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        public void Delete(bool recursive)
        {
            if (!recursive)
            {
                Directory.Delete();
                return;
            }
            
            foreach (var file in GetFiles("*", SearchScope.Current))
            {
                file.Delete();
            }
            foreach (var directory in GetDirectories("*", SearchScope.Current))
            {
                directory.Delete(true);
            }

            Directory.Delete();
            Directory.Refresh();
        }

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return Directory.GetDirectories(filter, option)
                .Select(directoryInfo => new LongPathDirectory(directoryInfo));
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return Directory.GetFiles(filter, option)
                .Select(fileInfo => new LongPathFile(fileInfo));
        }

        public LongPathDirectory(DirectoryPath path)
        {
            Path = path;
            Directory = new Pri.LongPath.DirectoryInfo(path.FullPath);
        }

        public LongPathDirectory(Pri.LongPath.DirectoryInfo directory)
        {
            Directory = directory;
            Path = new DirectoryPath(directory.FullName);
        }
    }
}