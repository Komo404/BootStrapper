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
        // Lógica: 1. Ler todos os arquivos de template do diretório especificado.
        string[] manifests = System.IO.Directory.GetFiles(templateFolderPath, ".json");

        List<Template> templates = new List<Template>();
    
        // 2. Para cada arquivo, desserializar 
        foreach (string manifestPath in manifests)
        {
            Template template = ManifestService.ReadManifest(manifestPath);
            // Adicionar o template à lista de templates
            templates.Add(template);
        }
        return templates;
    }

    List<Template> GetTemplateByCategory(string templateFolderPath, string category) // Scan Templates Directory, filtra por categoria e retorna os templates daquela categoria
    {
        List<Template> allTemplates = GetAllTemplates(templateFolderPath);
        return allTemplates.FindAll(t => t.Category == category);
    }

    Template GetTemplateById(string templateFolderPath, Guid templateId) // Scan Templates Directory, filtra por ID e retorna o template correspondente
    {
        List<Template> allTemplates = GetAllTemplates(templateFolderPath);
        Template foundTemplate = allTemplates.Find(template => template.Id == templateId);
        if (foundTemplate != null)
            return foundTemplate;
        else
            throw new Exception($"Template with ID {templateId} not found.");
    }

    void CreateTemplate(string creationPath) 
    {
        Template newTemplate = new Template
        {
            Id = Guid.NewGuid(),
            Name = "New Template",
            Description = "Description",
            Version = "1.0.0",  
            CreationDate = DateTime.Now,
            maxUnityVersion = 2024,
            minUnityVersion = 0,
            Category = "Category"
        };

        ManifestService.WriteManifest(creationPath, newTemplate);
    }

    void DeleteTemplate(string templatePath)
    {
        if (System.IO.File.Exists(templatePath))
        {
            System.IO.File.Delete(templatePath);
        }
    }

    void UpdateTemplateManifest (string templatePath) 
    {
        Template updatedManifest = new Template
        {
            Name = "Updated Template",
            Description = "Updated Description",
            Version = "1.0.1",
            CreationDate = DateTime.Now,
            maxUnityVersion = 2024,
            minUnityVersion = 0,
            Category = "Updated Category"
        };

        ManifestService.WriteManifest(templatePath, updatedManifest);
    }
}
