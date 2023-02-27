using System.Text.Json;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    public class Model
    {
        public Image[] Images;
        public Mesh[] Meshes;
        public Material[] Materials;
        public Texture[] Textures;
        public Buffer[] Buffers;
        public Accessor[] Accessors;
        public BufferView[] BufferViews;
        public Node[] Nodes;

        // TODO: scene
        // TODO: nodes
        // TODO: cameras

        /* --------------------------------------------------------------- */
        public class Node {
            // TODO
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-buffer
        public class Buffer
        {
            string uri;
            int byteLength;
            string name;

            Json.JsonElement extensions;
            string extras;
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-bufferview

        public class BufferView
        {
            int buffer;
            int byteOffset;
            int byteLength;
            int byteStride;
            int target;
            string name;

            Json.JsonElement extensions;
            string extras;
            // TODO
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-accessor
        public class Accessor
        {
            int bufferView;
            int byteOffset;
            int componentType;
            bool normalized;
            int count;
            string type;
            float max;
            float min;

            // TODO: sparse

            Json.JsonElement extensions;
            string extras;
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-mesh
        //See <a href="link">this link</a>
        public class Mesh { }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-image
        public class Image { }

        // TODO: ImageView

        public class Texture { }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-textureinfo
        public class TextureInfo
        {
            public uint Index; // texture index
            public int TexCoord;
        }

        public class NormalTextureInfo : TextureInfo
        {
            public float Scale = 1.0f;
        }

        public class OclusionTextureInfo : TextureInfo
        {
            public float Strength = 1.0f;
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

            public record MetallicRoughness
            {
                float[] baseColorFactor;
                float metallicFactor;
                float roughnessFactor;
                TextureInfo metallicRoughnessTexture;
            }
        }

        static void TestGenerics<T>(ref T value)
        {
            System.Console.WriteLine(value);
        }
    }
} // namespace bgl
