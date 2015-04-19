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
using System.Threading;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for NumericAnswerBox.xaml
    /// </summary>
    public partial class WordedAnswerBox : UserControl,AnswerBox
    {
        WordedAnswerQuestion vq;
        Questions qFormRef = null;
        public WordedAnswerBox(WordedAnswerQuestion  vqT)
        {
            InitializeComponent();
            vq = vqT;
            txtAnswer.Text = vq.actualAnswer;

           double heightT = Math.Abs(txtAnswer.Text.Split('\n').Count() * txtAnswer.FontSize) + 14;
           txtAnswer.Text = "";
           txtAnswer.Height = heightT;
            txtActualAnswer.Text = vqT.actualAnswer;
           
        }
     


        private string LowerCaserfy(string word)
        {
            string lowerCaseV = "";
            for(int i = 0 ; i<word.Count();i++)
            {
                lowerCaseV += Char.ToLower(word[i]);
            }
            return lowerCaseV;
        }
        public void PromptCorrection(ref Questions questionFormT)
        {
            txtActualAnswer.Visibility = System.Windows.Visibility.Visible;
            lblActualAnswer.Visibility = System.Windows.Visibility.Visible;

            qFormRef = questionFormT;
            if ((LowerCaserfy(vq.actualAnswer) == LowerCaserfy(txtAnswer.Text)) && vq.actualAnswer != "")
            { 
                 Correct(null,null); return;
            }
            if (vq.actualAnswer.Count() == 1)
            { 
                 Incorrect(null,null); return; 
            }
            qFormRef.btnOkay.IsEnabled = false;
            qFormRef.btnSkip.IsEnabled = false;
            qFormRef.btnExit.IsEnabled = false;
            qFormRef.correcting = true;
            wndIsCorrect wndCorrect = new wndIsCorrect();
            wndCorrect.btnCorrect.Click += new RoutedEventHandler(Correct);
            wndCorrect.btnIncorrect.Click += new RoutedEventHandler(Incorrect);
            wndCorrect.Show();
        }
  public void Correct(object s, RoutedEventArgs arg)
        {
            vq.Right();
            txtAnswer.BorderBrush = Brushes.LightGreen;
            txtAnswer.Foreground = Brushes.LightGreen;
            qFormRef.FinishCorrection(true);
            RenableFunctions();
            
        }
  public void Incorrect(object s, RoutedEventArgs arg)
  {
      vq.Wrong();
      txtAnswer.BorderBrush = Brushes.Red;
      txtAnswer.Foreground = Brushes.Red;
      qFormRef.FinishCorrection(false);
      RenableFunctions();
  }
        public void RenableFunctions()
  {
      qFormRef.btnOkay.IsEnabled = true;
      qFormRef.btnSkip.IsEnabled = true;
      qFormRef.btnExit.IsEnabled = true;
      qFormRef.correcting = false;
  }
    }
}
