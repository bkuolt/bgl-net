using System.Text.Json;
using System.Collections.Generic;
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
        public Camera[] Cameras;

        // TODO: scene
        // TODO: cameras

        /* --------------------------------------------------------------- */
        public class Node {
            // TODO
        }

        public class Camera {
            // TODO
        }

        public abstract class Object {
            public Json.JsonElement Extensions;
            public string Extras;
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-buffer
        public class Buffer : Object
        {
            public string Uri;
            public int ByteLength;
            public string Name;

            // TODO: GetData()
        }

        // 

        /// <summary>
        /// TODO
        /// <a href="https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-bufferview">See more</a>
        /// </summary>
        public class BufferView : Object
        {
            int Buffer;
            int ByteOffset;
            int ByteLength;
            int ByteStride;
            int Target;
            string Name;
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-accessor
        public class Accessor : Object
        {
            public int bufferView;
            public int byteOffset;
            public int componentType;
            public bool normalized;
            public int count;
            public string type;  // TODO
            public float max;
            public float min;
            public SparseAccesor Sparse ;

            public class SparseAccesor {
                // TODO
            }
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-mesh
        //See <a href="link">this link</a>
        public class Mesh {
            // TODO
            Primitive[] primitivess
            float[] weifhts;
            string name;

            class Primitive : Object {
                Dictionary<string, int> attributes;
                int indexAccessor;
                int material;
                int mode;
                int[] targets;  // morph targets
            }
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-image
        public class Image : Object{
            string uri;
            string mimeType;
            int bufferView;
        }

        /* */
        public class Texture : Object {
            int sampler;
            int source;
            string name;

            class Sampler {
                // TODO
            }   
        }

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
