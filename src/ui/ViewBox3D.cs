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

using System.Text.Json;
using bgl;


using System.Threading;

namespace wpf_demo
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ViewBox3D : System.Windows.Controls.Viewport3D
    {
        double angle = 0;
        double position = 3;

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {

            var camera = new System.Windows.Media.Media3D.PerspectiveCamera();
            camera.Position = new System.Windows.Media.Media3D.Point3D(0, 0, position /*3*/);
            camera.LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -1);
            camera.FieldOfView = 60;

            angle = Math.PI / 2;
            camera.LookDirection = new System.Windows.Media.Media3D.Vector3D(Math.Cos(angle), 0, -Math.Sin(angle));

            this.Camera = camera;
            //     MessageBox.Show("" + this.Children.Count());
        }

        void PrintTime(object state)
        {
            return;

           // position += 0.0001;

            //MessageBox.Show("sdsdsd");
        }

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        //  System.Windows.Threading.DispatcherTimer.Tick handler
        //
        //  Updates the current seconds display and calls
        //  InvalidateRequerySuggested on the CommandManager to force 
        //  the Command to raise the CanExecuteChanged event.
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            angle += 0.5;

            var camera = new System.Windows.Media.Media3D.PerspectiveCamera();
            camera.Position = new System.Windows.Media.Media3D.Point3D(0, 0, position /*3*/);
            camera.LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -1);
            camera.FieldOfView = 60;


            var radians  =  angle / (Math.PI * 2);
            camera.LookDirection = new System.Windows.Media.Media3D.Vector3D(Math.Cos(radians), 0, -Math.Sin(radians));

            this.Camera = camera;

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }
        public ViewBox3D()
        {
            //  DispatcherTimer setup
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
          //  dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();
        }

        private void TestClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked me at " + e.GetPosition(this).ToString());
        }
    }

}
