using System.IO.Compression;

namespace BootStrapper.Core.Services;

public class BundlerService
{
    /// <summary>
    ///     Bundles the template folder into a zip file
    /// </summary>
    /// <param name="templatePath"></param>
    /// <param name="outputPath"></param>
    public static void BundleTemplate(string templatePath, string outputPath)
    {
        if (!Directory.Exists(templatePath))
        {
            throw new DirectoryNotFoundException($"Template folder not found at: {templatePath}");
        }

        ZipFile.CreateFromDirectory(templatePath, outputPath);
    }
}