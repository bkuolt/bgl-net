using System.Text.Json;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    class Parser
    {
        private Json.JsonElement _root;

        public Parser(in string path)
        {
            LoadDocument(in path);
        }

        public GLTF.Model Parse()
        {
            var model = new GLTF.Model();

            model.Images = ParseImages();
            model.Textures = ParseTextures();
            model.Materials = ParseMaterials();
            model.Buffers = ParseBuffers();
            model.Accessors = ParseAccessors();
            model.BufferViews = ParseBufferViews();
            model.Meshes = ParseMeshes();
            model.Nodes = ParseNodes();

            return model;
        }

        private void LoadDocument(in string path)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open);
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);
            _root = document.RootElement;
        }

        /* ----------------------------------------------------------------------------------  */

        private Model.Image[] ParseImages()
        {
            try
            {
                JsonElement arrayNode = GetProperty("images");
                return JsonSerializer.Deserialize<Model.Image[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Image[0];
            }
        }

        private Model.Mesh[] ParseMeshes()
        {
            try
            {
                JsonElement arrayNode = GetProperty("meshes");
                return JsonSerializer.Deserialize<Model.Mesh[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Mesh[0];
            }
        }

        private Model.Material[] ParseMaterials()
        {
            try
            {
                JsonElement arrayNode = GetProperty("materials");
                return JsonSerializer.Deserialize<Model.Material[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Material[0];
            }
        }

        private Model.Texture[] ParseTextures()
        {
            try
            {
                JsonElement arrayNode = GetProperty("materials");
                return JsonSerializer.Deserialize<Model.Texture[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Texture[0];
            }
        }

        private Model.Buffer[] ParseBuffers()
        {
            try
            {
                JsonElement arrayNode = GetProperty("buffers");
                return JsonSerializer.Deserialize<Model.Buffer[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Buffer[0];
            }
        }

        private Model.Accessor[] ParseAccessors()
        {
            try
            {
                JsonElement arrayNode = GetProperty("accessors");
                return JsonSerializer.Deserialize<Model.Accessor[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Accessor[0];
            }
        }

        private Model.BufferView[] ParseBufferViews()
        {
            try
            {
                JsonElement arrayNode = GetProperty("bufferViews");
                return JsonSerializer.Deserialize<Model.BufferView[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.BufferView[0];
            }
        }

        private Model.Node[] ParseNodes()
        {
            try
            {
                JsonElement arrayNode = GetProperty("nodes");
                return JsonSerializer.Deserialize<Model.Node[]>(arrayNode);
            }
            catch (System.Exception e)
            {
                // TODO: handle error
                return new Model.Node[0];
            }
        }

        /* ----------------------------------------------------------------------------------  */

        private Json.JsonElement GetProperty(in string name)
        {
            Json.JsonElement value;
            if (!_root.TryGetProperty(name, out value))
            {
                return new Json.JsonElement();
            }
            return value;
        }

#if DEBUG
        public static void Test()
        {
            try
            {
                var parser = new GLTF.Parser("model.gltf");
                var model = parser.Parse();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Could not load GLTF file: " + e.Message);
            }

            // TODO: print some info/stats
        }
#endif
    }
}
