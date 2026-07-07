using System.Text.Json;

namespace BootStrapper.Core.Helpers;

public class ManifestHelper
{
    public static void WriteManifest<T>(string manifestPath, T obj) // Serialize an OBJECT to JSON and generates a manifest.json file at the specified path
    {
        string manifest = JsonSerializer.Serialize(obj); // Resultado: String JSON

        if (manifest != null)
        {
            if (Directory.Exists(manifestPath))
            {
                manifestPath = Path.Combine(manifestPath, "manifest.json");
            }

            File.WriteAllText(manifestPath, manifest);
        }
    }

    public static T ReadManifest<T>(string manifestpath) // Reads the manifest file at the specified path, deserializes it, and returns an object with the template information
    {
        if (!File.Exists(manifestpath))
        {
            throw new FileNotFoundException("File not found at: " + manifestpath);
        }

        string manifest = File.ReadAllText(manifestpath);
        T manifestJSON = JsonSerializer.Deserialize<T>(manifest) ?? throw new JsonException("Error at deserializing the manifest file at: " + manifestpath);
        return manifestJSON; 
    }
}