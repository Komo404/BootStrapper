using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Models;

public class TemplateNode
{
    public string Name { get; set; }
    public bool IsFolder { get; set; }
    public string? RelativePath { get; set; }
    public List<TemplateNode> Children { get; set; } = new();
}
