// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.IO;

namespace Cake.LongPath.Module
{
    public class LongPathModule : ICakeModule
    {
        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            registry.RegisterType<LongPathFileSystem>().As<IFileSystem>().Singleton();
        }
    }
}