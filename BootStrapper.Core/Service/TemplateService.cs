using System;
using System.Collections.Generic;
using System.Text;
using BootStrapper.Core.Service;
using BootStrapper.Core.Models;

namespace BootStrapper.Core.Service;

public class TemplateService // Pseudo código as in 20-05-26
{

    void GetAllTemplates(string manifestFolderPath) //Scan Templates Directory 
    {
        ManifestService.ReadManifest(manifestFolderPath);
    }

    void GetTemplateByCategory(string manifestFolderPath) // Scan Templates Directory, filtra por categoria e retorna os templates daquela categoria
    {
        ManifestService.ReadManifest(manifestFolderPath);
    }

    void GetTemplateById(string manifestFolderPath) 
    {
        ManifestService.ReadManifest(manifestFolderPath);
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

    void DeleteTemplate(string manifestFolderPath)
    {

    }

    void UpdateTemplate (string desiredTemplatePath) 
    {
        ManifestService.WriteManifest(desiredTemplatePath, updatedTemplate);
    }
}
