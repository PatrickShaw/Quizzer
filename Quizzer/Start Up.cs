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

namespace Quizzer
{
    public class StartUp : Application
    {
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static void Main()
        {
            StartUp app = new StartUp();
            app.InitializeComponent();
            app.Run();
        }
        public void InitializeComponent()
        {
                this.StartupUri = new Uri("Main_Window.xaml", System.UriKind.Relative);
            
        }
    }
}
