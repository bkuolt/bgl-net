
using OpenTK.Graphics.OpenGL;

namespace bgl
{
    public class Model
    {
        readonly public Assimp.Scene? Scene;
        public Model(string path)
        {
            try
            {
                var importer = new Assimp.AssimpContext();
                Scene = importer.ImportFile(path, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);

                _meshes = new Mesh[Scene.MeshCount];
                for (int i = 0; i < Scene.MeshCount; ++i)
                {
                    _meshes[i] = new Mesh(Scene, Scene.Meshes[i]);
                }
            }
            catch (System.Exception exception)
            {
                throw new System.Exception("Assimp could not load scene: " + exception.Message);
            }
        }

        public void Draw(System.TimeSpan delta)
        {
            GL.ClearColor(211 / 256.0f, 211 / 256.0f, 211 / 256.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var mesh in _meshes)
            {
                mesh.Render(delta);
            }
        }

        Mesh[] _meshes;
    };


} // namespace bgl
