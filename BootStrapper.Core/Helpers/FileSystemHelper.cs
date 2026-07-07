namespace BootStrapper.Core.Helpers;

public class FileSystemHelper
{
    public static void CopyDirectoryRecursively(string sourcePath, string destinationPath)
    {
        Directory.CreateDirectory(destinationPath); // Ensure the destination directory exists

        var folders = Directory.GetDirectories(sourcePath);

        foreach (var folder in folders)
        {
            var folderRelativePath = Path.GetRelativePath(sourcePath, folder);
            var newFolderPath = Path.Combine(destinationPath, folderRelativePath);

            Directory.CreateDirectory(newFolderPath);

            CopyDirectoryRecursively(folder, newFolderPath);
        }

        var files = Directory.GetFiles(sourcePath);

        foreach (var file in files)
        {
            var fileRelativePath = Path.GetRelativePath(sourcePath, file);
            var newFilePath = Path.Combine(destinationPath, fileRelativePath);
            File.Copy(file, newFilePath, true);
        }
    }
}
