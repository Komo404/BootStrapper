using BootStrapper.Core.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BootStrapper.Core.Service
{
    internal class ManifestService
    {
        public static void WriteTemplateManifest(string manifestPath, UserConfig template) // Fluxo: Serializa um OBJETO para JSON e gera um arquivo manifest.json no path recebido
        {
            string manifest = SerializeItem(template);

            if (manifest != null)
            {
                File.WriteAllText(manifestPath, manifest);
            }
        }

        public static UserConfig ReadTemplateManifest(string manifestpath) // Fluxo: Lê o arquivo de manifest que está naquele path, desserializa e retorna um objeto com as informações do template
        {
            if (!File.Exists(manifestpath))
            {
                throw new FileNotFoundException("Arquivo não encontrado em", manifestpath);
            }

            string manifest = File.ReadAllText(manifestpath);
            UserConfig template = DeserializeItem(manifest);
            return template;
        }

        static string SerializeItem(UserConfig template)
        {
            string json = JsonSerializer.Serialize(template);
            return json;
        }

        static UserConfig DeserializeItem(string manifest)
        {
            ArgumentNullException.ThrowIfNull(manifest);

            UserConfig? template = JsonSerializer.Deserialize<UserConfig>(manifest);
            return template ?? throw new InvalidOperationException("Erro ao desserializar");
        }
    }
}
