using Microsoft.Win32;
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

    public partial class wndAddWorded :Window
    {

        Mode mode = Mode.create;
        public void FillComboBox()
        {
            foreach (Question q in QuestionManager.questionList)
            {
                bool unique = true;
                for (int i = 0; i < cmbSubject.Items.Count; i++)
                {
                    if (((ComboBoxItem)cmbSubject.Items[i]).Content.ToString() == SubjectManager.Subjects[q.SubjectIndex]) { unique = false; break; }
                }
                if (unique == true) { ComboBoxItem cmb = new ComboBoxItem(); cmb.Content = SubjectManager.Subjects[q.SubjectIndex]; cmbSubject.Items.Add(cmb); }
            }
            if (cmbSubject.Items.Count > 0) { cmbSubject.SelectedIndex = 0; }
        }
        public wndAddWorded()
        {
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            txtQuestion.Focus();
            IDT = QuestionManager.lowestAvailableQuestionID;
            LoadDialogSettings();
            FillComboBox();

        }
        int noA = 0;
        int noW = 0;
        int IDT = -1;

        public wndAddWorded(WordedAnswerQuestion questionT)
        {
            // This call is required by the designer.
            InitializeComponent();
            Title = "Edit Worded Question";
            // Add any initialization after the InitializeComponent() call.
            FillComboBox(); 
            txtQuestion.Text = questionT.question;
            txtAnswer.Text = questionT.actualAnswer;
            for (int i = 0; i < cmbSubject.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbSubject.Items[i]).Content.ToString() == SubjectManager.Subjects[questionT.SubjectIndex].ToString())
                {
                    cmbSubject.SelectedIndex = i;
                }
            }
            if (questionT.TagIndexes.Count != 0)
            {
                for (int i = 0; i < questionT.TagIndexes.Count() - 1; i++)
                    txtTags.Text += TagManager.Tags[questionT.TagIndexes[i]] + ",";
                txtTags.Text += TagManager.Tags[questionT.TagIndexes[questionT.TagIndexes.Count() - 1]].Name;

            }
            string qPath = QuestionManager.FindImage(questionT.QuestionDirectory + @"QuestionImage");
            if (qPath != null)
            {
                txtImagePath.Text = qPath;
            }
            string aPath = QuestionManager.FindImage(questionT.QuestionDirectory + @"AnswerImage");
            if (aPath != null)
            {
                txtAnswerImagePath.Text = aPath;
            }
            IDT = questionT.ID;
            noA = questionT.TimesAnswered;
            noW = questionT.TimesWrong;
            mode = Mode.edit;
            LoadDialogSettings();
        }
        public wndAddWorded(NumericAnswerQuestion questionT)
        {
            // This call is required by the designer.
            InitializeComponent();
            Title = "Edit Worded Question";
            FillComboBox();
            if (questionT.TagIndexes.Count != 0)
            {
                for (int i = 0; i < questionT.TagIndexes.Count() - 1; i++)
                    txtTags.Text += TagManager.Tags[questionT.TagIndexes[i]].Name + ",";
                txtTags.Text += TagManager.Tags[questionT.TagIndexes[questionT.TagIndexes.Count() - 1]].Name;
            }
            // Add any initialization after the InitializeComponent() call. 
            txtQuestion.Text = questionT.question;
            txtAnswer.Text = questionT.actualAnswer.ToString();

            for (int i = 0; i < cmbSubject.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbSubject.Items[i]).Content.ToString() == SubjectManager.Subjects[questionT.SubjectIndex].ToString())
                {
                    cmbSubject.SelectedIndex = i;
                }
            }
            string qPath = QuestionManager.FindImage(questionT.QuestionDirectory + @"QuestionImage");
            if (qPath != null)
            {
                txtImagePath.Text = qPath;
            }
            IDT = questionT.ID;
            noA = questionT.TimesAnswered;
            noW = questionT.TimesWrong;
            mode = Mode.edit;
            LoadDialogSettings();
        }
        public void LoadDialogSettings()
        {
            loadDialog.DefaultExt = ".png";
            loadDialog.Filter = "Image Files Files|*.bmp;*.jpg;*.png";
            loadDialog.RestoreDirectory = true;
        }
        private void IsLetter(object sender, TextCompositionEventArgs e)
        {
            foreach (char letter in e.Text)
            {
                if (char.IsLetter(letter) == false & letter != '\'' )
                    e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            if (string.IsNullOrEmpty(txtQuestion.Text) & File.Exists(txtImagePath.Text) == false)
            {
                lblError.Content = "A question is required for a multiple choice question";
                return;
            }
            // Null answer validation
            if (txtAnswer.Text ==  "") { lblError.Content = "You need an answer to add a worded question."; return; }



            bool IsNumericAnswer = IsNumeric(txtAnswer.Text);

            // WORDED ANSWERS ===================================================================================
            if(IsNumericAnswer)
            {
                NumericAnswerQuestion question = new NumericAnswerQuestion();
                question.actualAnswer = Convert.ToDouble(txtAnswer.Text);
                SetQuestionData(question);
            }
            else
            {
                WordedAnswerQuestion question = new WordedAnswerQuestion();
                question.actualAnswer = txtAnswer.Text;
                SetQuestionData(question);
            }
            QuestionManager.SelectQuestion(0, true);
            Close();
        }
        private void SetQuestionData(Question question)
        {
            question.question = txtQuestion.Text;
            if (cmbSubject.Text == "") { lblError.Content = "You need to assign the question a subject"; return; }
            int subjectIndex = SubjectManager.Subjects.FindIndex(x => x.Name == cmbSubject.Text);
            if (subjectIndex == -1) { subjectIndex = SubjectManager.Subjects.Count; SubjectManager.AddSubject(new SubjectTag(cmbSubject.Text)); }
            question.SubjectIndex = subjectIndex;
            question.TimesAnswered = noA;
            question.TimesWrong = noW;
            // TAGS
            string[] splitTags = txtTags.Text.Replace(", ", ",").Split(',');
            for (int i = 0; i < splitTags.Count();i++ )
            {
                int tagIndex = TagManager.Tags.FindIndex(x => x.Name == splitTags[i]);
                if (tagIndex == -1) { TagManager.AddTag(splitTags[i]); tagIndex = TagManager.Tags.Count; }
                question.TagIndexes.Add(tagIndex);
                
            } 
            // WRITE METADATA, QUESTION AND ANSWERS
            if (mode == Mode.create)
            {
                question.ID = QuestionManager.lowestAvailableQuestionID;
                QuestionManager.lowestAvailableQuestionID += 1;
                question.Write();
                QuestionManager.questionList.Add(question);
            }
            if (mode == Mode.edit)
            {
                question.ID = IDT;
                for (int i = 0; i < QuestionManager.questionList.Count(); i++)
                {
                    if (QuestionManager.questionList[i].ID == question.ID)
                    {
                        question.Data.DirectoryIndex = QuestionManager.questionList[i].Data.DirectoryIndex;
                        QuestionManager.questionList[i] = question;
                        question.Write(true);
                        //question
                    }
                }
            }

            // Answer Image

            string iqAPath = QuestionManager.FindImage(question.QuestionDirectory + "AnswerImage");
            bool exitACheck = false;
        DoACheck:
            if (!string.IsNullOrEmpty(txtAnswerImagePath.Text) && exitACheck == false)
            {
                string[] fileSplit = txtAnswerImagePath.Text.Split('.');
                if (iqAPath != null) { if (iqAPath == txtAnswerImagePath.Text) { exitACheck = true; goto DoACheck; } }
                try
                {
                    File.Copy(txtAnswerImagePath.Text, question.QuestionDirectory + "AnswerImage" + "." + fileSplit[fileSplit.Count() - 1], true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                if (iqAPath != null && exitACheck == false) { File.Delete(iqAPath); }
            }
            // Question Image
            string iqPath = QuestionManager.FindImage(question.QuestionDirectory + "QuestionImage");
            bool exitCheck = false;
        DoCheck:
            if (!string.IsNullOrEmpty(txtImagePath.Text) && exitCheck == false)
            {
                string[] fileSplit = txtImagePath.Text.Split('.');
                if (iqPath != null) { if (iqPath == txtImagePath.Text) { exitCheck = true; goto DoCheck; } }
                try
                {
                    File.Copy(txtImagePath.Text, question.QuestionDirectory + "QuestionImage" + "." + fileSplit[fileSplit.Count() - 1], true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                if (iqPath != null && exitCheck == false) { File.Delete(iqPath); }
            }
            QuestionManager.SelectQuestion(0, true);
        Close();
        }
        private bool IsNumeric(string e)
        {
            bool foundDot = false;
            for (int i = 0; i < e.Length;i++ )
            {
                if (e[i] == '.')
                {
                    if (foundDot) { return false; } else { foundDot = true; continue; }
                }
                if (char.IsDigit(e[i])) { continue; }
                return false;
            }
                return true;
        }
        OpenFileDialog loadDialog = new OpenFileDialog();
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (loadDialog.ShowDialog() == true)
            {
                try
                {
                    txtImagePath.Text = loadDialog.FileName;

                }
                catch (Exception ex)
                {
                }
            }
        }


        private void txtQuestion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (txtQuestion.Text.Contains(","))
                e.Handled = true;
        } 

        private void txtTags_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(" ")) { e.Handled = true; }
        }

        private void cmbSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSubject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(",")) { e.Handled = true; } 
        }
         
        private void btnBrowseAnswerImage_Click(object sender, RoutedEventArgs e)
        {
            if (loadDialog.ShowDialog() == true)
            {
                try
                {
                    txtAnswerImagePath.Text = loadDialog.FileName;
                }
                catch (Exception ex)
                {

                }
            }
        }

    } 
}