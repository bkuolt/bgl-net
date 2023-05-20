using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AnimationEditor
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
        
            InitializeComponent();
            var grid = this.FindName("FrameGrid") as System.Windows.Controls.Grid;

            var files = new String[] {
                @"Dead (1).png",
                @"Dead (2).png",
                @"Dead (3).png",
                @"Dead (4).png",
                @"Dead (6).png",
                @"Dead (7).png",
                @"Dead (8).png",
                @"Dead (9).png",
                @"Dead (10).png",
                @"Dead (11).png",
                @"Dead (12).png",
                @"Dead (13).png",
                @"Dead (14).png",
                @"Dead (15).png",
                @"Dead (16).png",
                @"Dead (17).png"
            };

            FillGrid(grid, files);
            var treeView = this.FindName("FrameList") as System.Windows.Controls.TreeViewItem;
            FillList(treeView, files);

       // run console
        
            this.AllowDrop = true;
        }


        private static bool IsSupportedFile(System.IO.FileInfo fileInfo) {
            return true;  // TODO
        }




        private static void AddFile(System.IO.FileInfo fileInfo)
        {
  
            
            var pathList = new List<String>();

            if (IsSupportedFile(fileInfo))
            {
             //   pathList.Add(path);
            }
            
        }
        
        
        

        private static void AddDirectory(System.IO.FileInfo fileInfo)
        {
            foreach (var file in fileInfo.Directory.GetFiles())
            {
                if (IsSupportedFile(fileInfo))
                {
                    pathList.Add(path);
                }
                // TODO: check if extension is supported
            }
        }

        private static void AddAssets(System.Windows.DragEventArgs event)
        {
            var path =  event.Data.GetData(DataFormats.FileDrop);
              
         
            
            var fileInfo = new System.IO.FileInfo(path);
            bool isDirectory = fileInfo.Directory.Exists;
            bool isFile = fileInfo.Exists;

            if (isDirectory)
            {
                AddDirectory(fileInfo);
            }
            else if(isFile)
            {
                AddFile(fileInfo);
            }
            else
            {
                // TODO: throw exceptiom
            }
        }

        
        protected override void OnDrop(DragEventArgs event)
        {
            AddAssets(event);
          
            
            //
#if DEBUG
            var sb = new System.Text.StringBuilder();
            foreach (var path in pathList) {
            //    sb.AppendLine($"\t{path}");
            }
     //       MessageBox.Show(sb.ToString(), $"Added {pathList.Count} files", MessageBoxButton.OK, MessageBoxImage.Information);
#endif
        }

        protected override void OnDragEnter( DragEventArgs e)
        {
            // TODO: start async loading
        }

        protected override void OnDragLeave( DragEventArgs e)
        {
            // TODO: stop async loading
        }











        // TODO: SaveFile()
        // TODO: void OpenFile()
        // TODO: ShowAboutDialog()

        // TODO: on click image
        // TODO: select TreeViewItem
        // Drag & Droip Support
        // STatusBar

        Image LoadImage(String uri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri);
            bitmapImage.EndInit();

            var image = new Image();
            image.Source = bitmapImage;
            return image;
        }


        void FillList(System.Windows.Controls.TreeViewItem treeView, String[] paths)
        {
            foreach(var path in paths) {
                var item = new System.Windows.Controls.TreeViewItem();
                item.Header = path;
                treeView.Items.Add(item);
            }
        }

        void FillGrid(System.Windows.Controls.Grid grid, String[] paths)
        {
            const String AssetPath = @"C:\Users\Bastian\Code\bgl-net\AnimationViewer\animations\";



            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();

            var root = (int) Math.Log2(paths.Length);
            int columns = root;
            int rows = root;

            for (int column = 0; column < columns; ++column)
            {
                var columnDefinition = new System.Windows.Controls.ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(columnDefinition);
            }

            for (int row = 0; row < rows; ++row)
            {
                var rowDefinition = new System.Windows.Controls.RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDefinition);
            }

            int i = 0;
            for (int row = 0; row < rows; ++row)
            {
                for (int column = 0; column < columns; ++column, ++i)
                {
                    if (i >= paths.Length) return;

                    System.Windows.Controls.Image image = LoadImage(AssetPath + paths[i]);
                    grid.Children.Add(image);
                    Grid.SetColumn(image, column);
                    Grid.SetRow(image, row);
                }
            }
        }



    }


}
