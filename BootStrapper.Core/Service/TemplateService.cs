using System;
using System.Collections.Generic;
using System.Text;
using BootStrapper.Core.Service;
using BootStrapper.Core.Models;

namespace BootStrapper.Core.Service;

public class TemplateService
{
    /// <summary>
    ///     Get the Directories from the templates folder path
    /// </summary>
    /// <param name="templatesFolderPath"></param>
    /// <returns>List of Type TemplateManifest</returns>
    public static List<TemplateManifest> GetAllTemplates(UserConfig config)
    {
        List<TemplateManifest> templates = new List<TemplateManifest>();

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

    public static void CreateTemplate(UserConfig config, TemplateManifest templateInfo) 
    {
        var manifestId = Guid.NewGuid();
        var manifestPath = Path.Combine(config.TemplatesFolder, manifestId.ToString(), "manifest.json");

        var newTemplateManifest = new TemplateManifest
        {
            Id = manifestId,
            Name = templateInfo.Name,
            Description = templateInfo.Description,
            Version = "1.0.0",
            CreationDate = DateTime.Now,
            MaxUnityVersion = templateInfo.MaxUnityVersion,
            MinUnityVersion = templateInfo.MinUnityVersion,
            Tags = new List<string>(),
            TemplatePath = Path.Combine(config.TemplatesFolder, manifestId.ToString()),
            ManifestPath = manifestPath
        };

        Directory.CreateDirectory(newTemplateManifest.TemplatePath); // Cria pasta do template

        ManifestService.WriteManifest(newTemplateManifest.ManifestPath, newTemplateManifest); // Cria manifest Json

        string scriptsFolderPath = Path.Combine(newTemplateManifest.TemplatePath, "Scripts");
        Directory.CreateDirectory(scriptsFolderPath); // Cria pasta de scripts 

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
        TemplateManifest updatedManifest = GetTemplateById(config, templateInfo.Id); // não precisa ser criado um novo TemplateManifest, é só atualizar o templateInfo e passar ele para o WriteManifest
        
        updatedManifest.Name = templateInfo.Name;
        updatedManifest.Description = templateInfo.Description;
        updatedManifest.Version = templateInfo.Version;
        updatedManifest.MinUnityVersion = templateInfo.MinUnityVersion;
        updatedManifest.MaxUnityVersion = templateInfo.MaxUnityVersion;
        updatedManifest.Tags = templateInfo.Tags;
        updatedManifest.ManifestPath = Path.Combine(config.TemplatesFolder, templateInfo.Id.ToString(), $"manifest.json");

        ManifestService.WriteManifest(templateInfo.ManifestPath, updatedManifest);
    }

    public static void UpdateTemplateScripts(TemplateManifest template, List<string> newScripts)
    {
        string scriptsFolderPath = Path.Combine(template.TemplatePath, "Scripts");

        if (!Directory.Exists(scriptsFolderPath))
        {
            Directory.CreateDirectory(scriptsFolderPath);
        }

        foreach (string script in newScripts)
        {
            string scriptPath = Path.Combine(scriptsFolderPath, $"{script}.cs");
            File.WriteAllText(scriptPath, $"// Script: {script}");
        }
    }
}