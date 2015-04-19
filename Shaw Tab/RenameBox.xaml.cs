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

namespace Shaw_Tab
{
    /// <summary>
    /// Interaction logic for ChangeEmpiricalPercentage.xaml
    /// </summary>
    public partial class RenameBox : Window
    {
        string originalName;
        public RenameBox(string originalNameT)
        {
            InitializeComponent();
            originalName = originalNameT;
            txtInput.Text = originalNameT;
            Title = "Rename: " + originalNameT;
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text == "") { txtInput.Text = originalName; }
            Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = originalName;
            Close();
        }
    }
}
