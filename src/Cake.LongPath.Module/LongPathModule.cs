// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.IO;

namespace Cake.LongPath.Module
{
    /// <summary>
    /// Cake Module that adds long path support to build scripts running on Windows using Pri.LongPat
    /// </summary>
    public class LongPathModule : ICakeModule
    {
        /// <summary>
        /// Registers the replacement <see cref="IFileSystem"/>.
        /// </summary>
        /// <param name="registry"></param>
        public void Register(ICakeContainerRegistrar registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            registry.RegisterType<LongPathFileSystem>().As<IFileSystem>().Singleton();
        }
    }
}