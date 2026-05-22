using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BootStrapper.Core.Service;

public class ManifestService
{
    public static void WriteManifest<T>(string manifestPath, T obj) // Fluxo: Serializa um OBJETO para JSON e gera um arquivo manifest.json no path recebido
    {
        string manifest = SerializeObject<T>(obj); // Resultado: String JSON

        if (manifest != null)
        {
            File.WriteAllText(manifestPath, manifest);
        }
    }

    public static T ReadManifest<T>(string manifestpath) // Fluxo: Lê o arquivo de manifest que está naquele path, desserializa e retorna um objeto com as informações do template
    {
        if (!File.Exists(manifestpath))
        {
            throw new FileNotFoundException("Arquivo não encontrado em", manifestpath);
        }

        string manifest = File.ReadAllText(manifestpath);
        T manifestJSON = DeserializeJson<T>(manifest);
        return manifestJSON; 
    }

    static string SerializeObject<T>(T item)
    {
        string serialized = JsonSerializer.Serialize(item);
        return serialized;
    }

    static T DeserializeJson<T>(string json)
    {
        T? deserialized = JsonSerializer.Deserialize<T>(json);
        if (deserialized != null)
            return deserialized;
        else throw new JsonException("Erro ao desserializar o JSON");
    }
}
