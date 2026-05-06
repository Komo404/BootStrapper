using BootStrapper.Core.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BootStrapper.Core.Service
{
    internal class TemplateManifestService
    {
    
        public void WriteTemplateManifest(string manifestPath, Template template) // Fluxo: Serializa um OBJETO para JSON e gera um arquivo manifest.json no path recebido
        {
            string manifest = SerializeItem(template);
            File.WriteAllText(manifestPath, manifest);
        }

        public Template ReadTemplateManifest(string manifestpath) // Fluxo: Lê o arquivo de manifest que está naquele path, desserializa e retorna um objeto com as informações do template
        {
            string? manifest = File.Exists(manifestpath) ? File.ReadAllText(manifestpath) : null;
            Template template = DeserializeItem(manifest);
            return template;
        }

        string SerializeItem(Template template)
        {
            string json = JsonSerializer.Serialize(template);
            return json;
        }

        Template DeserializeItem(string manifest)
        {
            Template template = JsonSerializer.Deserialize<Template>(manifest);
            return template;
        }
}
}
