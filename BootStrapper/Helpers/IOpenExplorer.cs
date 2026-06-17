using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BootStrapper.Helpers;

public interface IOpenExplorer
{
    Task<string?> OpenFolderDialogAsync();
}
