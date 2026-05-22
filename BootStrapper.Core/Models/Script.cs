using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Models
{
    public class Script
    {
        public string Name { get; set; }
        public bool IsDirectory { get; set; }

        public string? RelativePath { get; set; }

        public List<Script> Children { get; set; } = new();
    }
}
