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
using System.IO;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for DraggableElement.xaml
    /// </summary>
    
    public class Text: QElement
    {
        private string _text = "";
        public override UIElement Expand()
        {
            TextBlock txb = new TextBlock();
            txb.Text = _text;
            return txb;
        }
    }
    public abstract class QElement
    {
        public abstract UIElement Expand();
    }
    public partial class Question2 : UserControl
    {
        string _path;
        public Question2(string QuestionDirectory)
        {
            InitializeComponent();  
            _path = QuestionDirectory;
        }
        public List<UIElement> ReadElements()
        {
            // TODO Do something here
            return null;
        }
    }
}
