using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Models;

public class TemplateNode
{
    public string Name { get; set; } = string.Empty;
    public bool IsFolder { get; set; } = false;
    public string? RelativePath { get; set; } = string.Empty;
    public List<TemplateNode> Children { get; set; } = [];
}
