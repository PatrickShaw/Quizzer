using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Quizzer;
using System.IO;
using System.Globalization;
using System.Windows.Ink;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging; 
/// <summary>
/// CHANGE LOG v4.81
/// Quiz window
/// Now scrolls down to the buttom after answering a question.
/// </summary>
/// <remarks></remarks> 
namespace Quizzer
{
    public partial class Questions : Window
    {
        Question mc;
        List<RadioButton> rc = new List<RadioButton>();
        UserControl answerBox;
        Image imgPicture = new Image();
        bool SelectedQuestion = false;
        bool imgPresent = false;
        public bool ForcedExitQuestioning = false;
        public bool correcting = false;
        public bool highestChanceSelection = false;
        public Questions(Question mcT, bool SelectedQuestionT = false)
        {
            // This call is required by the designer.
            SelectedQuestion = SelectedQuestionT;
            InitializeComponent();
            MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            if (CacheCS.useExamTheme)
            {
                ResourceDictionary resourceDic = new ResourceDictionary();
                resourceDic.Source = new Uri("ExamStyle.xaml", UriKind.RelativeOrAbsolute);
                Resources = resourceDic;
            }
            else
            { 
                ResourceDictionary resourceDic = new ResourceDictionary();
                resourceDic.Source = new Uri("Styles.xaml", UriKind.RelativeOrAbsolute);
                Resources = resourceDic;
            }
            mc = mcT;
            // Add any initialization after the InitializeComponent() call.
            txbQuestion.Text += mc.question + Environment.NewLine;
            txbQuestion.FontSize = 14;
            if (ImageHandler.GetImagePath(mc) != null)
            {
                try
                {
                    imgPicture.Source = ImageHandler.GetImagePath(mc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            answerBox = mcT.CreateAnswerBox();
            answerBox.Resources = Resources;
            if (mcT.AnsweringFormat == AnsweringFormat.NumericAnswer)
            {
                btnOkay.IsDefault = true;
            }

            if (imgPicture.Source != null)
            {
                imgPicture.MaxHeight = imgPicture.Source.Height * 1.2;
                if (imgPicture.MaxHeight > Height * 0.95)
                {
                    imgPicture.MaxHeight = Height * 0.95;
                }
                // This is not the same as imgPicture
                inkQuestion.MaxHeight = Height * 0.95;
                txbQuestion.Width = imgPicture.Width;
            }

            InlineUIContainer iuc = new InlineUIContainer(imgPicture);
            txbQuestion.Inlines.Add(iuc);
            dckAnswer.Children.Add(answerBox);
            DockPanel.SetDock(answerBox, Dock.Bottom);
            try
            {
                if (File.Exists(QuestionManager.FindImage(mc.QuestionDirectory + "answerImage")))
                {
                    imgAnswerImage.Source = new BitmapImage(new Uri(QuestionManager.FindImage(mc.QuestionDirectory + "answerImage")));
                    imgAnswerImage.MaxHeight = imgAnswerImage.Source.Height * 1.2;
                    imgAnswerImage.MaxWidth = imgAnswerImage.Source.Width * 1.2;
                    imgAnswerImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    imgPresent = true;
                }
                imgAnswerImage.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (SelectedQuestion == false)
            {
                btnExit.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (Width >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.9)
            {
                Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.9;
            }
            Width += 20;
            Focus();
            Title = SubjectManager.Subjects[mcT.SubjectIndex] + " - " + mcT.question;

            Top = 0;
        }
        public Size MeasureString(string candidate)
        {
            FormattedText FormattedText = new FormattedText(candidate, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(this.txbQuestion.FontFamily, this.txbQuestion.FontStyle, this.txbQuestion.FontWeight, this.txbQuestion.FontStretch), this.txbQuestion.FontSize, Brushes.Black);

            return new Size(FormattedText.Width, FormattedText.Height);
        }
        bool beenAnswered = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (beenAnswered == true)
            {
                Close();
                return;
            }
            if (imgPresent)
            {
                imgAnswerImage.Visibility = System.Windows.Visibility.Visible;
                Top = 0;
            } 
            scrWholeThing.ScrollToBottom();
            answerBox.GetType().GetMethod("PromptCorrection").Invoke(answerBox, new object[] { this });
        }
        public void FinishCorrection(bool correct)
        {
            foreach (Question q in QuestionManager.questionList)
            {
                if (q.ID == mc.ID)
                {
                    q.TimesAnswered += 1;
                    if (correct == false)
                        q.TimesWrong += 1;
                }
            }
            if (correct)
            {
                MessageBox.Show("Correct!");
                grpAnswer.Foreground = Brushes.Green;
                grpQuestion.Foreground = Brushes.Green;
            }
            else
            {
                MessageBox.Show("Incorrect!");
                grpAnswer.Foreground = Brushes.Red;
                grpQuestion.Foreground = Brushes.Red;
            }
            mc.modifiedTime = DateTime.Now;
            btnOkay.Content = "Finish";
            btnSkip.Content = "Next Question";
            beenAnswered = true;
            mc.OverwriteMetadata();
        }
        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            Close();
            if (SelectedQuestion == false)
            {
                Questions rawr = null;
                if (highestChanceSelection) { rawr = new Questions(QuestionManager.SelectQuestion(QSelectionMode.highestChance)); rawr.highestChanceSelection = true; }
                else
                {
                    rawr = new Questions(QuestionManager.SelectQuestion());
                }
                rawr.ShowDialog();
            }
        } 
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            ForcedExitQuestioning = true;
        } 
        private void txbQuestion_Loaded(object sender, RoutedEventArgs e)
        {
            txbQuestion.FontSize = 18;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (correcting == true)
                e.Cancel = true;
        }
        private void inkQuestion_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            IReadOnlyCollection<GestureRecognitionResult> gestures = e.GetGestureRecognitionResults();
            foreach (GestureRecognitionResult gest in gestures)
            {
                switch ((gest.ApplicationGesture))
                {
                    case ApplicationGesture.ScratchOut:
                        if ((gest.RecognitionConfidence == RecognitionConfidence.Strong))
                        {
                            for (int i = inkQuestion.Strokes.Count - 1; i >= 0; i += -1)
                            {
                                if (inkQuestion.Strokes[i].GetBounds().IntersectsWith(e.Strokes.GetBounds()))
                                {
                                    try
                                    {
                                        inkQuestion.Strokes.RemoveAt(i);

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }

        }
        private void inkQuestion_Loaded(object sender, RoutedEventArgs e)
        {

            inkQuestion.DefaultDrawingAttributes.FitToCurve = true;
        }

    }

}