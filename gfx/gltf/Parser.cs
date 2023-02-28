using System.Text.Json;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    class Parser
    {
        public Parser(in string path)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open);
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);
            _document = document.RootElement;
        }

        public GLTF.Model Parse()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new Json.Serialization.JsonStringEnumConverter());
            return JsonSerializer.Deserialize<GLTF.Model>(_document, options);
        }

        /* ----------------------------------------------------------------------------------  */

        private readonly JsonElement _document;
    }
}
