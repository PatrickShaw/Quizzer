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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for EditAnswerBox.xaml
    /// </summary>
    public partial class EditMultiChoiceAnswerBox : UserControl
    {

        System.Windows.Forms.OpenFileDialog loadDialog = new System.Windows.Forms.OpenFileDialog();
        public EditMultiChoiceAnswerBox()
        {
            InitializeComponent();
        }
        public EditMultiChoiceAnswerBox(MultipleChoice mc, int answerIndex)
        {

            InitializeComponent(); 
            
            txtAnswer.Text = mc.answers[answerIndex].ToString(); 
            if (answerIndex == mc.actualAnswer)
            {
                rdbCorrectAnswer.IsChecked = true;
            }
            string imagePath = QuestionManager.FindImage(mc.QuestionDirectory + @"Answers\Answer " + answerIndex + @"\AnswerImage");
            if (imagePath != null)
            {
                txtImagePath.Text = imagePath;
            }
        }
        
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            loadDialog.ShowDialog();
            txtImagePath.Text = loadDialog.FileName;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stk = this.Parent as StackPanel;
            if (stk == null) { return; }
            stk.Children.Remove(this);
        }

    }
}
