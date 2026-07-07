namespace BootStrapper.Core.Helpers;

public interface IFileSystemHelper
{
    public abstract static void CopyDirectoryRecursively(string sourcePath, string destinationPath);
}
