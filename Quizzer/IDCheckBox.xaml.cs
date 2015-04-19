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
    /// <summary>
    /// Interaction logic for IndexedCheckBox.xaml
    /// </summary>
    public partial class IDCheckBox : CheckBox
    {
        public IDCheckBox()
        {
            InitializeComponent();
        }
        int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value ; }
        }
    }
}
