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
using System.Windows.Shapes;

namespace Quizzer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class wndIsCorrect : Window
    {
        public bool finished = false;
        public bool saysCorrect = false;
        public wndIsCorrect()
        {
            InitializeComponent();
        }
        
           
         

        private void btnCorrect_Click(object sender, RoutedEventArgs e)
        {
            saysCorrect = true;
            finished = true;
            Close();
        }

        private void btnIncorrect_Click(object sender, RoutedEventArgs e)
        {
            saysCorrect = false;
            finished = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (finished) { return; }
            e.Cancel = true;
        }
    }
}
