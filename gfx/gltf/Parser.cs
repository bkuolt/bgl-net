using System.Text.Json;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    class Parser
    {
        public Parser(in string path)
        {
            LoadDocument(in path);

            ParseImages();
            ParseMeshes();
            ParseMaterials();
            ParseTextures();
            ParseBuffers();
        }

        private void ParseImages()
        {
#if false
            var imageNodes = GetArray("images");
            Images = new Image[imageNodes.Length];
            for (uint i = 0; i < imageNodes.Length; ++i)
            {
                Images[i] = new Image(imageNodes[i]);
            }
#endif
        }

        private void ParseMeshes()
        {
#if false
            var meshNodes = GetArray("meshes");
            Meshes = new Mesh[meshNodes.Length];
            for (uint i = 0; i < meshNodes.Length; ++i)
            {
                Meshes[i] = new Mesh(meshNodes[i]);
            }
#endif
        }

        private void ParseMaterials()
        {
#if false
            var materialNodes = GetArray("materials");
            Materials = new Material[materialNodes.Length];
            for (uint i = 0; i < materialNodes.Length; ++i)
            {
                Materials[i] = new Material(materialNodes[i]);
            }
#endif
        }

        private void ParseTextures()
        {
#if false
            var textureNodes = GetArray("textures");
            Textures = new Texture[textureNodes.Length];
            for (uint i = 0; i < textureNodes.Length; ++i)
            {
                Textures[i] = new Texture(textureNodes[i]);
            }
#endif
        }

        private void ParseBuffers()
        {
#if false
            var bufferNodes = GetArray("buffers");
            Buffers = new Buffer[bufferNodes.Length];
            for (uint i = 0; i < bufferNodes.Length; ++i)
            {
                Buffers[i] = new Buffer(bufferNodes[i]);
            }
#endif
        }

        private Json.JsonElement root;

        private void LoadDocument(in string path)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open);
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);
            root = document.RootElement;
        }

        private Json.JsonElement[] GetArray(in string name)
        {
            Json.JsonElement arrayNode;
            if (!root.TryGetProperty(name, out arrayNode))
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
    }
}
