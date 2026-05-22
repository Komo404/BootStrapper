using System;
using System.Collections.Generic;
using System.Text;
using BootStrapper.Core.Service;
using BootStrapper.Core.Models;

namespace BootStrapper.Core.Service;

public class TemplateService
{

    List<Template> GetAllTemplates(string templateFolderPath) // Scan Templates Directory 
    {
        // Lógica: 1. Ler todos os manifest JSON
        string[] manifests = Directory.GetFiles(templateFolderPath, "*.json");

        List<Template> templates = new List<Template>();

        // 2. Para cada arquivo, desserializar
        foreach (string manifest in manifests)
        {
            Template template = ManifestService.ReadManifest<Template>(manifest);
            templates.Add(template);
        }

        return templates;
    }

    List<Template> GetTemplateByTag(string templateFolderPath, string tag) // Scan Templates Directory, filtra por tag e retorna os templates daquela tag
    {
        List<Template> allTemplates = GetAllTemplates(templateFolderPath);
        return allTemplates.FindAll(template => template.Tags.Contains(tag));
    }

    Template GetTemplateById(string templateFolderPath, Guid templateId) // Scan Templates Directory, filtra por ID e retorna o template correspondente
    {
        List<Template> allTemplates = GetAllTemplates(templateFolderPath);

        Template? foundTemplate = allTemplates.Find(template => template.Id == templateId);

        if (foundTemplate != null) {
            return foundTemplate;
        }

        throw new Exception($"Template with ID {templateId} not found.");
    }

    void CreateTemplate(string creationPath, Template templateInfo) 
    {
        Template newTemplate = new Template
        {
            Id = Guid.NewGuid(),
            Name = templateInfo.Name,
            Description = templateInfo.Description,
            Version = "1.0.0",
            CreationDate = DateTime.Now,
            maxUnityVersion = templateInfo.maxUnityVersion,
            minUnityVersion = templateInfo.minUnityVersion,
            Tags = new List<string>(),
            ScriptStructure = new List<Script>()
        };

        ManifestService.WriteManifest(creationPath, newTemplate);
    }

    void DeleteTemplate(string templateFolderPath, Guid templateId)
    {
        Template template = GetTemplateById(templateFolderPath, templateId);

        if (Directory.Exists(template.TemplatePath))
        {
            Directory.Delete(template.TemplatePath, true);
        }
    }

    void UpdateTemplateManifest (string templatePath, Template templateInfo) 
    {
        Template updatedManifest = new Template
        {
            Name = templateInfo.Name,
            Description = templateInfo.Description,
            Version = templateInfo.Version,
            CreationDate = DateTime.Now,
            maxUnityVersion = templateInfo.maxUnityVersion,
            minUnityVersion = templateInfo.minUnityVersion,
            Tags = new List<string>(),
            ScriptStructure = new List<Script>()
        };

        ManifestService.WriteManifest(templatePath, updatedManifest);
    }
}
