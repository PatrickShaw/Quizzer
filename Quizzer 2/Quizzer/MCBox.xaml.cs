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
using System.IO;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for MCBox.xaml
    /// </summary>
    public interface AnswerBox
    {
void PromptCorrection(ref Questions questionFormT);
    }
    public partial class MCBox : UserControl, AnswerBox
    {
        private MultipleChoice question;
        List<MCRadioButton> rbList = new List<MCRadioButton>();

        public void PromptCorrection(ref Questions questionFormT)
        {
            MCRadioButton selectedRadioButton = null;
            // HIGHLIGHT SELECTED AS RED
            for (int i = 0; i < rbList.Count; i++)
            {
                if (rbList[i].IsChecked == true) { rbList[i].Foreground = Brushes.DarkRed; rbList[i].Background = Brushes.DarkRed; selectedRadioButton = rbList[i]; }
            }

            // HIGHLIGHT CORRECT
            for (int i = 0; i < rbList.Count; i++)
            {
                if (rbList[i].id == question.actualAnswer)
                {
                    rbList[i].Foreground = Brushes.DarkGreen;
                    rbList[i].Background = Brushes.DarkGreen;
                }
            }

            // HIGHLIGHT ALL AS RED IF NULL
            if (selectedRadioButton == null)
            {
                for (int i = 0; i < rbList.Count; i++)
                { rbList[i].Foreground = Brushes.DarkRed; rbList[i].Background = Brushes.DarkRed; selectedRadioButton = rbList[i]; questionFormT.FinishCorrection(false); return; }
            }
            // RETURN TRUE OR FALSE
            if (selectedRadioButton.id == question.actualAnswer) { questionFormT.FinishCorrection(true); return; }
            { questionFormT.FinishCorrection(false); }
        }
        public MCBox( MultipleChoice mc)
        {
            InitializeComponent();
            question = mc;
            List<MCRadioButton> rbTempList = new List<MCRadioButton>();

            for (int i = 0; i < question.answers.Count(); i++ )
            { 
                MCRadioButton rb = new MCRadioButton(i);
                StackPanel stk = new StackPanel();
                TextBlock txb = new TextBlock();
                txb.Text = mc.answers[i].ToString().Replace("~", "").Replace("`", "");
                stk.Children.Add(txb);
                try{
                    string path = QuestionManager.FindImage(question.QuestionDirectory + @"Answers\" + "Answer " + mc.answers[i].id.ToString() +@"\" + "AnswerImage");
                    Image img = new Image();
                    if (path != null)
                    {
                        img.Source = new BitmapImage(new Uri(path,UriKind.RelativeOrAbsolute));
                        img.Stretch = Stretch.None;
                        stk.Children.Add(img);
                    }
                }catch{

                }
                rb.Content = stk;
                rb.Foreground = Brushes.White;
                rb.FontSize = 20;
                rb.Content = stk;
                 rbTempList.Add(rb);
            }
            Random rnd = new Random();
            for (int i = rbTempList.Count - 1; i >= 0; i-- )
            {
                int sharonspoo = (int)(rbTempList.Count * rnd.NextDouble() - 1);
                MCRadioButton selected = rbTempList[sharonspoo];
                rbList.Add(selected);
                rbTempList.RemoveAt(sharonspoo);
            }
            for(int i = 0 ; i < rbList.Count; i ++)
            {
                stkChoices.Children.Add(rbList[i]);
            }
            InitializeComponent();
        }
    }
}
