using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
         Console.WriteLine("sd");
         //   MessageBox.Show("changed!");
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
