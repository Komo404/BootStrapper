using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BootStrapper.Helpers;

public interface IFileSystemHelper
{
    public abstract static void CopyDirectoryRecursively(string sourcePath, string destinationPath);
}
