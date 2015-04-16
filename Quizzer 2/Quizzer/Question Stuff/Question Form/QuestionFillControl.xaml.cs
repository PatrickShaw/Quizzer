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
    /// Interaction logic for Question_Fill_Control.xaml
    /// </summary>
    public partial class QuestionFillControl : UserControl
    {

        public QuestionFillControl()
        {
            InitializeComponent();
            rdbQuestion.IsChecked = true;
        }

        private void rdbSection_Checked(object sender, RoutedEventArgs e)
        {
            stkSubQuestions.Visibility = System.Windows.Visibility.Visible;
        }

        private void rdbQuestion_Checked(object sender, RoutedEventArgs e)
        { 
            stkSubQuestions.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            stkSubQuestions.Children.Add(new QuestionFillControl());
        }
         public void Write(string path)
        {
             if(rdbQuestion.IsChecked == true)
             { 
                 string elementInformation = "";
                 for(int i = 0; i < stkQuestionElements.Children.Count;i++)
                 {
                     Draggable_Element questionElement = (Draggable_Element)stkQuestionElements.Children[i];
                     questionElement.Write(path + i.ToString());
                 }
             }
             else
             {
                 string subSectionPath = @"\Sub Sections\";
                 for(int i = 0 ; i < stkSubQuestions.Children.Count;i++)
                 {
                     QuestionFillControl subSection = (QuestionFillControl)stkSubQuestions.Children[i];
                     subSection.Write(subSectionPath + " " + i.ToString()+"\\");
                 }
             }
        }
    }
}
