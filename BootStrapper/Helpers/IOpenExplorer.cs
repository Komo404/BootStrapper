using System.Threading.Tasks;

namespace BootStrapper.Helpers;

public interface IOpenExplorer
{
    Task<string?> OpenFolderDialogAsync();

    void OpenBootStrapperFolder(string folderPath);
}
