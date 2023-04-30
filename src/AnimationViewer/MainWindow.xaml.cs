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

namespace AnimationViewer
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
                @"animations\Dead (1).png",
                @"animations\Dead (2).png",
                @"animations\Dead (3).png",
                @"animations\Dead (4).png",
                @"animations\Dead (6).png",
                @"animations\Dead (7).png",
                @"animations\Dead (8).png",
                @"animations\Dead (9).png",
                @"animations\Dead (10).png",
                @"animations\Dead (11).png",
                @"animations\Dead (12).png",
                @"animations\Dead (13).png",
                @"animations\Dead (14).png",
                @"animations\Dead (15).png",
                @"animations\Dead (16).png",
                @"animations\Dead (17).png"
            };

            FillGrid(grid, files);
            var treeView = this.FindName("FrameList") as System.Windows.Controls.TreeViewItem;
            FillList(treeView, files);
        }

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
            const String AssetPath = @"C:\Users\Bastian\Code\bgl-net\AnimationViewer\";

            //    grid.ColumnDefinitions.

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
