using System.Text.Json;


namespace bgl.gltf
{
    using Json = System.Text.Json;

    public class Model
    {
        public Image[] Images;
        public Mesh[] Meshes;
        public Material[] Materials;
        public Texture[] Textures;
        public Buffer[] Buffers;

        // TODO: scene
        // TODO: nodes
        // TODO: buffer views
        // TODO: image views
        // TODO: cameras


        public Model(in string path)
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
            var imageNodes = GetArray("images");
            Images = new Image[imageNodes.Length];
            for (uint i = 0; i < imageNodes.Length; ++i)
            {
                Images[i] = new Image(imageNodes[i]);
            }
        }

        private void ParseMeshes()
        {
            var meshNodes = GetArray("meshes");
            Meshes = new Mesh[meshNodes.Length];
            for (uint i = 0; i < meshNodes.Length; ++i)
            {
                Meshes[i] = new Mesh(meshNodes[i]);
            }
        }

        private void ParseMaterials()
        {
            var materialNodes = GetArray("materials");
            Materials = new Material[materialNodes.Length];
            for (uint i = 0; i < materialNodes.Length; ++i)
            {
                Materials[i] = new Material(materialNodes[i]);
            }
        }

        private void ParseTextures()
        {
            var textureNodes = GetArray("textures");
            Textures = new Texture[textureNodes.Length];
            for (uint i = 0; i < textureNodes.Length; ++i)
            {
                Textures[i] = new Texture(textureNodes[i]);
            }
        }

        private void ParseBuffers()
        {
            var bufferNodes = GetArray("buffers");
            Buffers = new Buffer[bufferNodes.Length];
            for (uint i = 0; i < bufferNodes.Length; ++i)
            {
                Buffers[i] = new Buffer(bufferNodes[i]);
            }
        }

        /* --------------------------------------------------------------- */
        public class Buffer
        {
            public Buffer(in Json.JsonElement element)
            {
                // TODO
            }
        }

        public class Accessor
        {
            // TODO
        }

        public class BufferView
        {
            // TODO
        }

        public class Mesh
        {
            public Mesh(in Json.JsonElement element)
            {
                // TODO
            }
        }
        public class Image
        {
            public Image(in Json.JsonElement element)
            {
                // TODO
            }
        }
        public class Texture
        {
            public Texture(in Json.JsonElement element)
            {
                // TODO
            }
        }

        public class TextureInfo
        {
            public uint Index;  // texture index
            public int TexCoord;

            public TextureInfo(in Json.JsonElement element)
            {
                // TODO
            }
        }

        public class NormalTextureInfo : TextureInfo
        {
            public float Scale = 1.0f;

            public NormalTextureInfo(in Json.JsonElement element)
                : base(element)
            {
                // TODO
            }
        }

        public class OclusionTextureInfo : TextureInfo
        {
            public float Strength = 1.0f;

            public OclusionTextureInfo(in Json.JsonElement element)
                : base(element)
            {
                // TODO
            }
        }

        public class Material
        {
            public MetallicRoughness PBRMetallicRoughness;
            public NormalTextureInfo NormalTexture;
            public OclusionTextureInfo OcclusionTexture;
            public TextureInfo EmissiveTexture;
            public float[] EmissiveFactor = { 0.0f, 0.0f, 0.0f };
            public string AlphaMode = "OPAQUE";
            public float AlphaCutoff = 0.5f;
            public bool DoubleSided = false;

            public Material(in Json.JsonElement element)
            {
                // TODO
            }

            public record MetallicRoughness
            {
                float[] baseColorFactor;
                float metallicFactor;
                float roughnessFactor;
                TextureInfo metallicRoughnessTexture;
            }
        }



        private void LoadNodes()
        {

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
            return null;  // TODO
        }


        void Test()
        {
#if false
        for (int i = 0; i < materials.GetArrayLength(); ++i) {
            var material = materials[i];
            string name =  material.GetProperty("name").ToString();
            // TODO: aonymous functions

            string normalTextureTexture =  material.GetProperty("normalTexture").ToString();
            string occlusionTextureTexture =  material.GetProperty("occlusionTexture").ToString();
            string metallicRoughnessTexture =  material.GetProperty("pbrMetallicRoughness").ToString();
            string emissiveTextureTexture =  material.GetProperty("emissiveTexture").ToString();

            // TODO: number indices
            // TODO: number vertices
        }
#endif
        }
    }

}