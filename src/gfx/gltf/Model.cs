using System.Collections.Generic;

namespace BGL.GLTF
{
    using Json = System.Text.Json;

    public class Model
    {
        public Image[]? Images;
        public Mesh[]? Meshes;
        public Material[]? Materials;
        public Texture[]? Textures;
        public Buffer[]? Buffers;
        public Accessor[]? Accessors;
        public BufferView[]? BufferViews;
        public Node[]? Nodes;
        public Camera[]? Cameras;
        public Scene[]? Scenes;

        /* --------------------------------------------------------------- */
        public abstract class Object
        {
            public Json.JsonElement Extensions;
            public string? Extras;
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-scene
        public class Scene : Object
        {
            public int[]? Nodes; // indices of each root node
            public string? Name;
        }

        public class Node : Object
        {
            public int? Camera;
            public int[]? Children;
            public int? Skin;
            public float[]? Matrix;
            public int Mesh;
            public float[]? Rotation;
            public float[]? Scale;
            public float[]? Translation;
            public float[]? Weights;
            public string? Name;
        }

        public class Camera : Object
        {
            public OrthographicCamera? Orthographic;
            public PerspectiveCamera? Perspective;
            public string? Type;
            public string? Name;
        }

        public class OrthographicCamera : Object
        {
            public float XMag;
            public float YMag;
            public float ZFar;
            public float ZNear;
        }

        public class PerspectiveCamera : Object
        {
            public float AspectRatio;
            public float YFov;
            public float YFar;
            public float ZNear;
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-buffer
        public class Buffer : Object
        {
            public string? Uri;
            public int ByteLength;
            public string? Name;
        }

        /// <summary>
        /// TODO
        /// <a href="https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-bufferview">See more</a>
        /// </summary>
        public class BufferView : Object
        {
            public int Buffer;
            public int ByteOffset;
            public int ByteLength;
            public int ByteStride;
            public int Target;
            public string? Name;
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-accessor
        public class Accessor : Object
        {
            public int BufferView;
            public int ByteOffset;
            public int ComponentType;
            public bool Normalized;
            public int Count;
            public string? Type;
            public float Max;
            public float Min;
            public SparseAccesor? Sparse;

            public class SparseAccesor
            {
                // TODO
            }
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-mesh
        //See <a href="link">this link</a>
        public class Mesh
        {
            public Primitive[]? Primitives;
            public float[]? Weights;
            public string? Name;

            public class Primitive : Object
            {
                public Dictionary<string, int>? Attributes;
                public int indices;
                public int Material;
                public int Mode;
                public int[] Targets = new int[0]; // morph targets
            }
        }

        //https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-image
        public class Image : Object
        {
            public string? Uri;
            public string MimeType ="";
            public int BufferView;
        }

        public class Texture : Object
        {
            public int Sampler;
            public int Source;
            public string Name = "";
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-sampler
        public class Sampler : Object
        {
            public int? MagFilter;
            public int? MinFilter;
            public int WrapS;
            public int WrapT;
            public string? name;
        }

        // https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-textureinfo
        public class TextureInfo : Object
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

        public class Material : Object
        {
            public MetallicRoughness? PBRMetallicRoughness;
            public NormalTextureInfo? NormalTexture;
            public OclusionTextureInfo? OcclusionTexture;
            public TextureInfo? EmissiveTexture;
            public float[] EmissiveFactor = { 0.0f, 0.0f, 0.0f };
            public string AlphaMode = "OPAQUE";
            public float AlphaCutoff = 0.5f;
            public bool DoubleSided = false;

            public class MetallicRoughness : Object
            {
                public float[] BaseColorFactor = new float[0];
                public float MetallicFactor;
                public float RoughnessFactor;
                public TextureInfo? MetallicRoughnessTexture;
            }
        }
        
        public override string ToString()
        {
            return "";  // TODO
        }


    }

} // namespace bgl
