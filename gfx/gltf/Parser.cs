using System.Text.Json;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    class Parser
    {
        public Parser(in string path)
        {
            LoadDocument(in path);

            _options = new JsonSerializerOptions();
            _options.PropertyNameCaseInsensitive = true;
            _options.Converters.Add(new Json.Serialization.JsonStringEnumConverter());
        }

        public GLTF.Model Parse()
        {
            var model = new GLTF.Model();

            model.Images = ParseArray<Model.Image>("images");
            model.Textures = ParseArray<Model.Texture>("textures");
            model.Materials = ParseArray<Model.Material>("materials");
            model.Buffers = ParseArray<Model.Buffer>("buffers");
            model.Accessors = ParseArray<Model.Accessor>("accessors");
            model.BufferViews = ParseArray<Model.BufferView>("bufferViews");
            model.Meshes = ParseArray<Model.Mesh>("meshes");
            model.Nodes = ParseArray<Model.Node>("nodes");

            return model;
        }

        /* ----------------------------------------------------------------------------------  */

        private void LoadDocument(in string path)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open);
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);
            _document = document.RootElement;
        }

        private T[] ParseArray<T>(in string name)
        {
            try
            {
                JsonElement element = GetProperty(name);
                return JsonSerializer.Deserialize<T[]>(element, _options);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
            }

            return new T[0];
        }

        private Json.JsonElement GetProperty(in string name)
        {
            Json.JsonElement value;
            if (!_document.TryGetProperty(name, out value))
            {
                return new Json.JsonElement();
            }
            return value;
        }

        private JsonElement _document;
        private readonly JsonSerializerOptions _options;
    }
}
