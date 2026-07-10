using BootStrapper.Core.Models;
using BootStrapper.Core.Helpers;

namespace BootStrapper.Core.Services;

public class TemplateService
{

    public static List<TemplateManifest> GetAllTemplates(UserConfig config)
    {
        List<TemplateManifest> templates = [];

        // 1: Read all directories in the templates folder path
        string[] templatefolders = Directory.GetDirectories(config.TemplatesFolder, "*", SearchOption.TopDirectoryOnly);

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

    public static List<TemplateManifest> GetTemplateByTag(UserConfig config, string tag)
    {
        List<TemplateManifest> allTemplates = GetAllTemplates(config);
        return allTemplates.FindAll(template => template.Tags.Contains(tag));
    }

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

        Directory.CreateDirectory(templateInfo.TemplatePath); // Creates template folder


        foreach (string unityVersion in templateInfo.UnityVersions)
        {
            string versionPath = Path.Combine(templateInfo.TemplatePath, unityVersion);
            Directory.CreateDirectory(versionPath);
            ScriptTreeService.CopyScriptNodeFromTreeToTemplatePath(TemplateScripts, versionPath);
        }


        ManifestHelper.WriteManifest(templateInfo.ManifestPath, templateInfo);
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