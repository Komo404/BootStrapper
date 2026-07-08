using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Core.Helpers;
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
                TemplateManifest template = ManifestHelper.ReadManifest<TemplateManifest>(manifestPath);
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

    public static TemplateManifest CreateTemplate(UserConfig config, TemplateManifest templateInfo, List<TemplateNode> TemplateScripts)
    {   
        ArgumentNullException.ThrowIfNull(templateInfo, nameof(templateInfo));
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        templateInfo.Id = Guid.NewGuid();
        templateInfo.CreationDate = DateTime.Now;
        templateInfo.TemplatePath = Path.Combine(config.TemplatesFolder, templateInfo.Name);
        templateInfo.ManifestPath = Path.Combine(templateInfo.TemplatePath, "manifest.json");

        Directory.CreateDirectory(templateInfo.TemplatePath); // Cria pasta do template

        ManifestHelper.WriteManifest(templateInfo.ManifestPath, templateInfo); // Cria o manifest Json

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

    public static void UpdateTemplateManifest (TemplateManifest templateInfo)
    {
        ManifestHelper.WriteManifest(templateInfo.ManifestPath, templateInfo);
    }

}
