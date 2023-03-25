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

        public GLTF.Model? Parse()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new Json.Serialization.JsonStringEnumConverter());
            return JsonSerializer.Deserialize<GLTF.Model>(_document, options);
        }

        private byte[]? LoadBuffer(in Model.Buffer buffer) {
            return null;
        }
        private byte[]? LoadImage(in Model.Image image) {
            return null;
        }

#if DEBUG
        public static void Test()
        {
        #if false
            try
            {
                Parser parser = new Parser("model.gltf");
                Model model = parser.Parse();
                System.Console.WriteLine(model.ToString());
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception.Message);
            }
         #endif
        }
#endif
        private readonly JsonElement _document;
    }
}
