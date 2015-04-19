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
    /// Interaction logic for NumericAnswerBox.xaml
    /// </summary>
    public partial class NumericAnswerBox : UserControl, AnswerBox 
    {
        NumericAnswerQuestion vq;
        public NumericAnswerBox( NumericAnswerQuestion vqT)
        {
            InitializeComponent();
            vq = vqT;
            try
            {
                string[] split = vqT.actualAnswer.ToString().Split('.'); 
                txbSignificantFigures.Text = "Round to: " + split[1].Count().ToString() + " decimal places"; 
            }
            catch(Exception ex)
            {
                txbSignificantFigures.Text = "Answer is a whole number.";
            }

            txtActualAnswer.Text = vq.actualAnswer.ToString();
        }
        public  void PromptCorrection(ref Questions questionFormT)
        {
            lblActualAnswer.Visibility = System.Windows.Visibility.Visible;
            txtActualAnswer.Visibility = System.Windows.Visibility.Visible;
            if (vq.actualAnswer.ToString() == txtAnswer.Text)
            {
                txtAnswer.BorderBrush = Brushes.LightGreen;
                  txtAnswer.Foreground = Brushes.LightGreen;
                  questionFormT.FinishCorrection(true); return;
            }
            txtAnswer.BorderBrush = Brushes.Red;
            txtAnswer.Foreground = Brushes.Red;
            questionFormT.FinishCorrection(false);
        }
    }
}
