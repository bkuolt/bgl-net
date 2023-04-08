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

        public static void LoadModel(string path)
        {
            _model = new Model(path);
            _listView?.SetScene(_model.Scene);
        }

        public void SetListView(bgl.ListView listView)
        {
            _listView = listView;
        }

        public void Render(System.TimeSpan delta)
        {
            // TODO: measure frames per second
            if (_model != null)
            {
                _model.Draw(delta);
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



        static Model? _model;
        static bgl.ListView? _listView;
    }

} // namespace bgl
