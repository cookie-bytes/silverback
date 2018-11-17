﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silverback.Messaging
{
    public class FileSystemEndpoint : IEndpoint, IEquatable<FileSystemEndpoint>
    {
        private FileSystemEndpoint(string name, string basePath, bool useFileSystemWatcher)
        {
            Name = name;
            UseFileSystemWatcher = useFileSystemWatcher;
            Path = System.IO.Path.Combine(basePath, name);
        }

        public static FileSystemEndpoint Create(string name, string basePath, bool useFileSystemWatcher = false) =>
            new FileSystemEndpoint(name, basePath, useFileSystemWatcher);

        public string Name { get; }

        public string Path { get; }

        public bool UseFileSystemWatcher { get; }

        internal void EnsurePathExists()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #region IEquatable

        public bool Equals(FileSystemEndpoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Path, other.Path) && UseFileSystemWatcher == other.UseFileSystemWatcher;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileSystemEndpoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UseFileSystemWatcher.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
