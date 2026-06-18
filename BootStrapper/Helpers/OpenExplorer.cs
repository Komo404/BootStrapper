using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootStrapper.Helpers;

public class OpenExplorer : IOpenExplorer
{
    readonly TopLevel toplevel;
    public OpenExplorer(TopLevel _toplevel)
    {
        toplevel = _toplevel;
    }

    public async Task<string?> OpenFolderDialogAsync()
    {
        var options = new FolderPickerOpenOptions();

        var result = await toplevel.StorageProvider.OpenFolderPickerAsync(options);
        if (result == null || !result.Any()) return null;

        return result.First().TryGetLocalPath();
    }

    public void OpenTemplateFolder(string folderPath)
    {
        var directoryinfo = new DirectoryInfo(folderPath);
        toplevel.Launcher.LaunchDirectoryInfoAsync(directoryinfo);
    }
}
