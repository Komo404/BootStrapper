using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BootStrapper.Core.Models;

public class TemplateNode
{
    public required string Name { get; set; } = string.Empty;
    public required bool IsFolder { get; set; } = false;
    public required string UserScriptFolderPath { get; set; } = string.Empty;
    public required string? RelativePath { get; set; } = string.Empty;
    public required ObservableCollection<TemplateNode> Children { get; set; } = [];
}
