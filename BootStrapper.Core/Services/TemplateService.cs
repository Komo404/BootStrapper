using System;
using System.Collections.Generic;
using System.Text;
using BootStrapper.Core.Services;
using BootStrapper.Core.Models;
using System.Security.Principal;

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

    public static void CreateTemplate(UserConfig config, TemplateManifest templateInfo, string userScriptsFolderPath)
    {   
        if (templateInfo == null) throw new ArgumentNullException(nameof(templateInfo));
        if (config == null) throw new ArgumentNullException(nameof(config));

        templateInfo.Id = Guid.NewGuid();
        templateInfo.CreationDate = DateTime.Now;
        templateInfo.Tags = templateInfo.Tags;
        templateInfo.TemplatePath = Path.Combine(config.TemplatesFolder, templateInfo.Id.ToString());
        templateInfo.ManifestPath = Path.Combine(templateInfo.TemplatePath, "manifest.json");

        Directory.CreateDirectory(templateInfo.TemplatePath); // Cria pasta do template

        ManifestService.WriteManifest(templateInfo.ManifestPath, templateInfo); // Cria manifest Json

        string TemplatescriptsFolderPath = Path.Combine(templateInfo.TemplatePath, "Scripts");
        Directory.CreateDirectory(TemplatescriptsFolderPath); // Cria pasta de scripts

        if(userScriptsFolderPath != null)
        {
            System.Diagnostics.Debug.WriteLine(userScriptsFolderPath);
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(userScriptsFolderPath, TemplatescriptsFolderPath, true);
        }

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
        updatedManifest.Version = templateInfo.Version;
        updatedManifest.UnityVersion = templateInfo.UnityVersion;
        updatedManifest.MaxUnityVersion = templateInfo.MaxUnityVersion;
        updatedManifest.Tags = templateInfo.Tags;
        updatedManifest.ManifestPath = Path.Combine(config.TemplatesFolder, templateInfo.Id.ToString(), $"manifest.json");

        ManifestService.WriteManifest(templateInfo.ManifestPath, updatedManifest);
    }

    public static void UpdateTemplateScripts(TemplateManifest template, List<string> newScriptsPath)
    {
        string scriptsFolderPath = Path.Combine(template.TemplatePath, "Scripts");
        string scriptPath;

        if (!Directory.Exists(scriptsFolderPath))
        {
            Directory.CreateDirectory(scriptsFolderPath);
        }

        foreach (string script in newScriptsPath)
        {
            if(Directory.Exists(script))
            {
                continue;
            }
            else
            {
                scriptPath = Path.Combine(scriptsFolderPath, $"{script}.cs"); // CS
            }

            scriptPath = Path.Combine(scriptsFolderPath, $"{script}"); // Folder
            File.WriteAllText(scriptPath, $"// Script: {script}");
        }
    }
}