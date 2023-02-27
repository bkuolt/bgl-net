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

        Model.Image ParseImage(in Json.JsonElement element)
        {
            return null; // TODO
        }

        Model.Texture ParseTexture(in Json.JsonElement element)
        {
            return null; // TODO
        }

        Model.Buffer ParseBuffer(in Json.JsonElement element)
        {
            return null; // TODO
        }

        Model.BufferView ParseBufferView(in Json.JsonElement element)
        {
            return null; // TODO
        }

        /* ----------------------------------------------------------------------------------  */

        private Model.Image[] ParseImages()
        {
#if false
            var imageNodes = GetArray("images");
            Images = new Image[imageNodes.Length];
            for (uint i = 0; i < imageNodes.Length; ++i)
            {
                Images[i] = new Image(imageNodes[i]);
            }
#endif
            return null;
        }

        private Model.Mesh[] ParseMeshes()
        {
#if false
            var meshNodes = GetArray("meshes");
            Meshes = new Mesh[meshNodes.Length];
            for (uint i = 0; i < meshNodes.Length; ++i)
            {
                Meshes[i] = new Mesh(meshNodes[i]);
            }
#endif
            return null;
        }

        private Model.Material[] ParseMaterials()
        {
#if false
            var materialNodes = GetArray("materials");
            Materials = new Material[materialNodes.Length];
            for (uint i = 0; i < materialNodes.Length; ++i)
            {
                Materials[i] = new Material(materialNodes[i]);
            }
#endif
            return null;
        }

        private Model.Texture[] ParseTextures()
        {
#if false
            var textureNodes = GetArray("textures");
            Textures = new Texture[textureNodes.Length];
            for (uint i = 0; i < textureNodes.Length; ++i)
            {
                Textures[i] = new Texture(textureNodes[i]);
            }
#endif
            return null;
        }

        private Model.Buffer[] ParseBuffers()
        {
#if false
            var bufferNodes = GetArray("buffers");
            Buffers = new Buffer[bufferNodes.Length];
            for (uint i = 0; i < bufferNodes.Length; ++i)
            {
                Buffers[i] = new Buffer(bufferNodes[i]);
            }
#endif
            return null;
        }

        private Json.JsonElement[] GetArray(in string name)
        {
            Json.JsonElement arrayNode;
            if (!_root.TryGetProperty(name, out arrayNode))
            {
                return new Json.JsonElement[0];
            }

            int size = arrayNode.GetArrayLength();
            Json.JsonElement[] array = new Json.JsonElement[size];
            uint index = 0;
            foreach (var node in arrayNode.EnumerateArray())
            {
                array[index++] = node;
            }

            return array;
        }

        private Json.JsonElement[] GetProperty(in string name)
        {
            return null; // TODO
        }

        Model.Accessor[] ParseAccessors()
        {
            return null; /* TODO */
        }

        Model.BufferView[] ParseBufferViews()
        {
            return null; /* TODO */
        }

        Model.Node[] ParseNodes()
        {
            return null; // TODO
        }

#if DEBUG
        public static void Test()
        {
            try {
                var parser = new GLTF.Parser("model.gltf");
                var model = parser.Parse();
            } catch (System.Exception e) {
                System.Console.WriteLine("Could not load GLTF file: " + e.Message);
            }

            // TODO: print some info/stats
        }
#endif
    }
}
