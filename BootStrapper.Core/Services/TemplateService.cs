using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;

namespace BootStrapper.Core.Services;

public class TemplateService
{

    /// <summary>
    ///     Gets the Directories from the templates folder path
    /// </summary>
    /// <param name="templatesFolderPath"></param>
    /// <returns>List of Type TemplateManifest</returns>
    public static List<TemplateManifest> GetAllTemplates(UserConfig config)
    {
        List<TemplateManifest> templates = [];

        // 1. Ler todos os manifest JSON
        string[] templatefolders = Directory.GetDirectories(config.TemplatesFolder);

        foreach (string folder in templatefolders)
        {
            string manifestPath = Path.Combine(folder, $"manifest.json");
            if (File.Exists(manifestPath))
            {
                TemplateManifest template = ManifestService.ReadManifest<TemplateManifest>(manifestPath);
                templates.Add(template);
            }
        }

        return templates;
    }


    /// <summary>
    ///     Get the  Directories from the templates folder path, filter by tag and returns the template of that tag
    /// </summary>
    /// <param name="config"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static List<TemplateManifest> GetTemplateByTag(UserConfig config, string tag)
    {
        List<TemplateManifest> allTemplates = GetAllTemplates(config);
        return allTemplates.FindAll(template => template.Tags.Contains(tag));
    }



    /// <summary>
    ///     Get the Directories from the templates folder path, filter by Id and  returns the template of that Id
    /// </summary>
    /// <param name="config"></param>
    /// <param name="templateId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static TemplateManifest GetTemplateById(UserConfig config, Guid templateId)
    {
        List<TemplateManifest> allTemplates = GetAllTemplates(config);

        TemplateManifest? foundTemplate = allTemplates.Find(template => template.Id == templateId);

        if (foundTemplate != null) {
            return foundTemplate;
        }

        throw new Exception($"Template with ID {templateId} not found.");
    }

    public static TemplateManifest CreateTemplate(UserConfig config, TemplateManifest templateInfo, ObservableCollection<TemplateNode> TemplateScripts)
    {   
        if (templateInfo == null) throw new ArgumentNullException(nameof(templateInfo));
        if (config == null) throw new ArgumentNullException(nameof(config));

        templateInfo.Id = Guid.NewGuid();
        templateInfo.CreationDate = DateTime.Now;
        templateInfo.TemplatePath = Path.Combine(config.TemplatesFolder, templateInfo.Id.ToString());
        templateInfo.ManifestPath = Path.Combine(templateInfo.TemplatePath, "manifest.json");

        Directory.CreateDirectory(templateInfo.TemplatePath); // Cria pasta do template

        ManifestService.WriteManifest(templateInfo.ManifestPath, templateInfo); // Cria o manifest Json

        foreach (string unityVersion in templateInfo.UnityVersions)
        {
            Directory.CreateDirectory(Path.Combine(templateInfo.TemplatePath, unityVersion));
        }

        if (TemplateScripts != null)
        {
            foreach (TemplateNode node in TemplateScripts)
            {
                foreach (string unityVersion in templateInfo.UnityVersions)
                {
                    FileSystemHelper.CopyDirectoryRecursively(Path.Combine(node.UserScriptFolderPath, node.RelativePath), Path.Combine(templateInfo.TemplatePath, unityVersion, node.Name));
                }
            }
        }

        return templateInfo;
        }

    public static async Task DeleteTemplate(UserConfig config, Guid templateId)
    {
        TemplateManifest template = GetTemplateById(config, templateId);

        await Task.Run(() =>
        {
            if (Directory.Exists(template.TemplatePath))
            {
                Directory.Delete(template.TemplatePath, true);
            }
        });
    }

    public static void UpdateTemplateManifest (UserConfig config, TemplateManifest templateInfo)
    {
        TemplateManifest updatedManifest = GetTemplateById(config, templateInfo.Id); // não precisa criar um novo TemplateManifest, é só atualizar o templateInfo e passar ele para o WriteManifest
        
        updatedManifest.Name = templateInfo.Name;
        updatedManifest.Description = templateInfo.Description;
        updatedManifest.UnityVersions = templateInfo.UnityVersions;
        updatedManifest.Category = templateInfo.Category;
        updatedManifest.Tags = templateInfo.Tags;
        updatedManifest.ManifestPath = Path.Combine(config.TemplatesFolder, templateInfo.Id.ToString(), $"manifest.json");

        ManifestService.WriteManifest(templateInfo.ManifestPath, updatedManifest);
    }

    public static ObservableCollection<TemplateNode> BuildScriptTree(string path, string root)
    {
        // Le o path recursivamente e a cada iteraçao adiciona um node para o arquivo existente na pasta com if folder
        ObservableCollection<TemplateNode> ScriptTree = [];
        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                string extension = Path.GetExtension(file);
                if (extension != null && (extension.Equals(".cs")))
                {
                    TemplateNode node = new TemplateNode
                    {
                        Name = Path.GetFileName(file),
                        IsFolder = false,
                        Children = [],
                        UserScriptFolderPath = root,
                        RelativePath = Path.GetRelativePath(root, file),
                    };
                    ScriptTree.Add(node);
                }
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                var childs = BuildScriptTree(directory, root);
                var folderNode = new TemplateNode
                {
                    Name = Path.GetFileName(directory),
                    IsFolder = true,
                    Children = new ObservableCollection<TemplateNode>(childs),
                    UserScriptFolderPath = root,
                    RelativePath = Path.GetRelativePath(root, directory),
                };
                ScriptTree.Add(folderNode); 
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return ScriptTree;
    }

    public static void RemoveTreeNode(ObservableCollection<TemplateNode> branch, TemplateNode selectedNode)
    {
        for (int i = 0; i <branch.Count(); i++)
        {
            var currentNode = branch[i];
            if (currentNode == selectedNode)
            {
                branch.Remove(currentNode);
                return;
            }

            RemoveTreeNode(currentNode.Children, selectedNode);
        }
    }
}