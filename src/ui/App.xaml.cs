using System;
using System.Windows;

namespace wpf_demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() {
         //   this.IsActive = true 
        }
    
        protected override void  OnActivated (EventArgs e)
        {
            // App started
        }
    }

    public static class Program
    {
        [STAThread]
        static public void Main()
        {
            App application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }

}
