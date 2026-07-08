using BootStrapper.Core.Models;
using System.Diagnostics;

namespace BootStrapper.Core.Services;

public static class ScriptTreeService
{
    public static List<TemplateNode> BuildScriptTree(string path, string root)
    {
        List<TemplateNode> scriptTree = [];
        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                string extension = Path.GetExtension(file);
                if (extension != null && extension.Equals(".cs"))
                {
                    TemplateNode node = new TemplateNode
                    {
                        Name = Path.GetFileName(file),
                        IsFolder = false,
                        Children = [],
                        UserScriptFolderPath = root,
                        RelativePath = Path.GetRelativePath(root, file),
                    };
                    scriptTree.Add(node);
                }
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                var children = BuildScriptTree(directory, root);
                var folderNode = new TemplateNode
                {
                    Name = Path.GetFileName(directory),
                    IsFolder = true,
                    Children = children,
                    UserScriptFolderPath = root,
                    RelativePath = Path.GetRelativePath(root, directory),
                };
                scriptTree.Add(folderNode);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return scriptTree;
    }

    public static bool RemoveTreeNode(List<TemplateNode> branch, TemplateNode selectedNode)
    {
        for (int i = 0; i < branch.Count; i++)
        {
            var currentNode = branch[i];
            if (currentNode == selectedNode) 
            {
                branch.Remove(currentNode);
                return true;
            }

            if (RemoveTreeNode(currentNode.Children, selectedNode))
            {
                return true;
            }
        }
        return false;
    }

}
