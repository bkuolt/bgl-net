using System.Windows;
using OpenTK.Graphics.OpenGL;

namespace bgl
{
    public class Renderer
    {
        public Renderer()
        {
        }

        public static void LoadModel()
        {
            try
            {
                string? path = ChooseFile();
                if (path != null && path.Length > 0)
                {
                    LoadModel(path);
                }
            }
            catch (System.Exception exception)
            {
                System.Windows.MessageBox.Show(
                    exception.Message,
                    "Error",
                    MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        public static void LoadModel(string fileName)
        {
            try
            {
                var importer = new Assimp.AssimpContext();
                _scene = importer.ImportFile(fileName, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);

                _meshes = new Mesh[_scene.MeshCount];
                for (int i = 0; i < _scene.MeshCount; ++i)
                {
                    _meshes[i] = new Mesh(_scene, _scene.Meshes[i]);
                }
            }
            catch (System.Exception exception)
            {
                throw new System.Exception("Assimp could not load scene: " + exception.Message);
            }

            _listView?.SetScene(_scene);
        }

        public void SetListView(bgl.ListView listView)
        {
            _listView = listView;
        }

        public void Render(System.TimeSpan delta)
        {
            // TODO: measure frames per second
            GL.ClearColor(211 / 256.0f, 211 / 256.0f, 211 / 256.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var mesh in _meshes)
            {
                mesh.Render(delta);
            }
        }

        // --------------------------------------------------------------------------------------------------

        /// <summary>
        /// Opens a FileDialog in order to select a GLTF file.
        /// </summary>
        /// <returns> The path to the GLTF model. </returns>
        static string? ChooseFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".gltf";

            bool? result = dialog.ShowDialog();
            if (result == null)
            {
                return null;
            }

            return dialog.FileName;
        }

        static Mesh[] _meshes = new Mesh[0];

        static Assimp.Scene? _scene;
        static bgl.ListView? _listView;
    }

} // namespace bgl
