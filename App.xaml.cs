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
        App() {
         //   this.IsActive = true 
        }
    
        protected override void  OnActivated (EventArgs e)
        {
         Console.WriteLine("sd");
         //   MessageBox.Show("changed!");
        }
    }


}
