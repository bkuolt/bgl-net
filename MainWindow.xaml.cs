using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.GL;
using OpenTK.Graphics;


using System.Text.Json;
using bgl;
namespace wpf_demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public static TreeViewItem gltfTreeView;

        public void ChooseFile(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".gltf";
            // dlg.Filter =  "*.gltf"; 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result != true)
            {
                return;  // TODO: throw exception
            }

            // Open document
            try
            {
                gltfTreeView = LoadGLTFModel(dlg.FileName);
                gltfTreeView.Header = dlg.FileName;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Loading failed: " + exception.Message,
                                "Loading failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private static System.Text.Json.JsonElement GetProperty(System.Text.Json.JsonElement document, string name)
        {
            try
            {
                return document.GetProperty(name);
            }
            catch (Exception e)
            {
                // nothing to do yet
                return new System.Text.Json.JsonElement();
            }
        }

        // ------------------------------------------
        public static void OnImageItemSelected(object sender, EventArgs e)
        {
            var imageItem = sender as TreeViewItem;
            //MessageBox.Show(imageItem.Header.ToString());
            //
            //  UpdateImageView(imageItem, imageItem.Header.ToString());

        }
        private static TreeViewItem CreateImageTreeView(System.Text.Json.JsonElement node)
        {
            var imageTreeView = new TreeViewItem();
            imageTreeView.Header = "Images";

            for (int i = 0; i < node.GetArrayLength(); ++i)
            {
                string name = node[i].GetProperty("uri").ToString();

                var item = new TreeViewItem();
                item.Header = name;
                imageTreeView.Items.Add(item);

                imageTreeView.Selected += OnImageItemSelected;

                //   "https://e7.pngegg.com/pngimages/820/630/png-clipart-cat-kitten-cartoon-cat-cats-illustration-cartoon-character-painted-thumbnail.png"

            }

            return imageTreeView;
        }

        ///////////////////
        ///////////////////


        class TreeViewBuilder
        {

            public TreeViewBuilder(JsonDocument document)
            {
                JsonElement root = document.RootElement;
                images = GetProperty(root, "images");
                meshes = GetProperty(root, "meshes");
                materials = GetProperty(root, "materials");
                textures = GetProperty(root, "textures");
                buffers = GetProperty(root, "buffers");
                buffers = GetProperty(root, "buffers");
            }

            public JsonElement images;
            public JsonElement meshes;
            public JsonElement materials;
            public JsonElement textures;
            public JsonElement cameras;

            public JsonElement buffers;
            public JsonElement extensions;


            JsonElement GetImage(uint index) { return new JsonElement(); }
            JsonElement GetMesh(uint index) { return new JsonElement(); }
            JsonElement GetMaterial(uint index) { return new JsonElement(); }


        }
        ///////////////////
        ///////////////////

        //////////////////////


        private static TreeViewItem CreateMaterialTreeView(System.Text.Json.JsonElement materials)
        {
            var materialTreeView = new TreeViewItem();
            materialTreeView.Header = "Materials";

            for (int i = 0; i < materials.GetArrayLength(); ++i)
            {
                var material = materials[i];

                string normalTextureTexture = GetProperty(material, "normalTexture").ToString();
                string occlusionTextureTexture = GetProperty(material, "occlusionTexture").ToString();
                string metallicRoughnessTexture = GetProperty(material, "pbrMetallicRoughness").ToString();
                string emissiveTextureTexture = GetProperty(material, "emissiveTexture").ToString();

                var treeView = new TreeViewItem();
                treeView.Header = material.GetProperty("name").ToString();

                if (normalTextureTexture.Length > 0)
                {
                    treeView.Items.Add("Normal Texture");
                }
                if (occlusionTextureTexture.Length > 0)
                {
                    treeView.Items.Add("Occlusion Texture");
                }
                if (metallicRoughnessTexture.Length > 0)
                {
                    treeView.Items.Add("MetallicRoughnessTexture");
                }
                if (emissiveTextureTexture.Length > 0)
                {
                    treeView.Items.Add("emissiveTextureTexture");
                }

                materialTreeView.Items.Add(treeView);
            }

            return materialTreeView;
        }

        private static TreeViewItem GetExtensionList(System.Text.Json.JsonElement root)
        {
            var extensionList = new TreeViewItem();
            extensionList.Header = "Extensions";

            System.Text.Json.JsonElement extensionNode = GetProperty(root, "extensionsUsed");
            for (int i = 0; i < extensionNode.GetArrayLength(); ++i)
            {
                extensionList.Items.Add(extensionNode[i]);
            }

            return extensionList;
        }


        public TreeViewItem LoadGLTFModel(string modelPath)
        {
            System.IO.FileStream stream = System.IO.File.Open(modelPath, System.IO.FileMode.Open);
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);

            var root = document.RootElement;
            System.Text.Json.JsonElement element = root;

            TreeViewBuilder treeViewBuilder = new TreeViewBuilder(document);


            var images = GetProperty(root, "images");
            var meshes = GetProperty(root, "meshes");
            var materials = GetProperty(root, "materials");
            var textures = GetProperty(root, "textures");
            var buffers = GetProperty(root, "buffers");

            var extensions = GetExtensionList(root);
            TreeViewItem tree = new TreeViewItem();


            tree.Items.Add(extensions);
            tree.Items.Add(CreateImageTreeView(images));
            tree.Items.Add(CreateMaterialTreeView(materials));
            return tree;
        }

        private static void UpdateImageView(object element, string uri)
        {
            var image = (System.Windows.Controls.Image)element;
            if (image == null)
            {
                return;
            }

            try
            {
                var bitmapImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri));
                image.Source = bitmapImage;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }
        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public MainWindow()
        {
            InitializeComponent();
            TreeView tree = (TreeView)this.FindName("treeView");

            const string uri = "https://e7.pngegg.com/pngimages/820/630/png-clipart-cat-kitten-cartoon-cat-cats-illustration-cartoon-character-painted-thumbnail.png";
            UpdateImageView(this.FindName("image"), uri);

            ChooseFile(null, null);
            TreeViewItem model = gltfTreeView;

            tree.Items.Add(model);
            var window = (System.Windows.Window)this.FindName("Fenster");
            window.Width = 1000;

            bgl.GLTF.MainProgram();



            // --------------------------------- //

            var viewport = new Viewport();

            var grid = (Grid)this.FindName("Grid");
            grid.Children.Add(viewport);
            Grid.SetRow(viewport, 0);
            Grid.SetColumn(viewport, 2);
        }

        // -------------------------------------------------------------


        public class Viewport : OpenTK.Wpf.GLWpfControl
        {

            public Viewport()
            {
                var settings = new GLWpfControlSettings
                {
                    MajorVersion = 4,
                    MinorVersion = 0
                };

                this.Render += OnRender;
                this.Width = 300;
                this.Height = 300;
                this.Start(settings);
            }

            protected void OnRender(TimeSpan delta)
            {
                GL.ClearColor(1, 0, 0, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
        }


        /// ///////////////////////////////////
        /// </summary>



        private void TestClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked me at " + e.GetPosition(this).ToString());
        }

        private void GetSchedule(object sender, RoutedEventArgs e)
        {

            TreeViewItem treeView = (TreeViewItem)sender;
            try
            {
                var treeView2 = (Button)sender;
                Console.Write("treeview2: " + (treeView2 != null));
            }
            catch (Exception eee)
            {
                //MessageBox.Show("Failed");
                return;
            }

            treeView.Items.Add("sd");


            var parent = (TreeViewItem)treeView.Parent;
            parent.Header = "Header";

            MessageBox.Show("You clicked me at!!" + treeView.Header);
            //Perform actions when a TreeViewItem
            //controls is selected
        }
    }


}
