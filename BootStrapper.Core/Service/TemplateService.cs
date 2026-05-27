using System;
using System.Collections.Generic;
using System.Text;
using BootStrapper.Core.Service;
using BootStrapper.Core.Models;

namespace BootStrapper.Core.Service;

public class TemplateService
{
    public List<TemplateManifest> GetAllTemplates(string templatesFolderPath) // Scan Templates Directory 
    {
        List<TemplateManifest> templates = new List<TemplateManifest>();

        // 1. Ler todos os manifest JSON
        string[] templatefolders = Directory.GetDirectories(templatesFolderPath);
        List<string> tempList = new List<string>();

        foreach (string folder in templatefolders)
        {
            string manifestPath = Path.Combine(folder, $"{Path.GetFileName(folder)}.json");
            if (File.Exists(manifestPath))
            {
                tempList.Add(manifestPath);
            }
        }

        foreach (string manifest in tempList) // Ler cada manifest e chamar ReadManifest para ler
        {
            TemplateManifest template = ManifestService.ReadManifest<TemplateManifest>(manifest);
            templates.Add(template);
        }

        return templates;
    }

    public List<TemplateManifest> GetTemplateByTag(string templatesFolderPath, string tag) // Scan Templates Directory, filtra por tag e retorna os templates daquela tag
    {
        List<TemplateManifest> allTemplates = GetAllTemplates(templatesFolderPath);
        return allTemplates.FindAll(template => template.Tags.Contains(tag));
    }

    public TemplateManifest GetTemplateById(string templatesFolderPath, Guid templateId) // Scan Templates Directory, filtra por ID e retorna o template correspondente
    {
        List<TemplateManifest> allTemplates = GetAllTemplates(templatesFolderPath);

        TemplateManifest? foundTemplate = allTemplates.Find(template => template.Id == templateId);

        if (foundTemplate != null) {
            return foundTemplate;
        }

        throw new Exception($"Template with ID {templateId} not found.");
    }

    public void CreateTemplate(string templatesFolderPath, TemplateManifest templateInfo) 
    {
        TemplateManifest newTemplateManifest = new TemplateManifest
        {
            Id = Guid.NewGuid(),
            Name = templateInfo.Name,
            Description = templateInfo.Description,
            Version = "1.0.0",
            CreationDate = DateTime.Now,
            MaxUnityVersion = templateInfo.MaxUnityVersion,
            MinUnityVersion = templateInfo.MinUnityVersion,
            Tags = new List<string>(),
            TemplatePath = Path.Combine(templatesFolderPath, templateInfo.Id.ToString()),
            ManifestPath = Path.Combine(templatesFolderPath, templateInfo.Id.ToString(), $"{templateInfo.Name}.json")
        };

        string manifestPath = Path.Combine(newTemplateManifest.TemplatePath, $"{newTemplateManifest.Name}.json");
        string scriptsFolderPath = Path.Combine(newTemplateManifest.TemplatePath, "Scripts"); 

        Directory.CreateDirectory(newTemplateManifest.TemplatePath); // Cria pasta do template

        ManifestService.WriteManifest(manifestPath, newTemplateManifest);

        Directory.CreateDirectory(scriptsFolderPath); // Cria pasta de scripts 


    }

    public void DeleteTemplate(string templatePath, Guid templateId)
    {
        TemplateManifest template = GetTemplateById(templatePath, templateId);

        if (Directory.Exists(template.TemplatePath))
        {
            Directory.Delete(template.TemplatePath, true);
        }
    }

    public void UpdateTemplateManifest (string templatesFolderPath, TemplateManifest templateInfo)
    {
        TemplateManifest updatedManifest = GetTemplateById(templatesFolderPath, templateInfo.Id); // não precisa ser criado um novo TemplateManifest, é só atualizar o templateInfo e passar ele para o WriteManifest
        updatedManifest.Name = templateInfo.Name;
        updatedManifest.Description = templateInfo.Description;
        updatedManifest.Version = templateInfo.Version;
        updatedManifest.MinUnityVersion = templateInfo.MinUnityVersion;
        updatedManifest.MaxUnityVersion = templateInfo.MaxUnityVersion;
        updatedManifest.Tags = templateInfo.Tags;
        updatedManifest.ManifestPath = Path.Combine(templatesFolderPath, templateInfo.Id.ToString(), $"{templateInfo.Name}.json");

        ManifestService.WriteManifest(templateInfo.ManifestPath, updatedManifest);
    }

    public void UpdateTemplateScripts(string templatePath, List<string> newScripts)
    {
        string scriptsFolderPath = Path.Combine(templatePath, "Scripts");
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
    