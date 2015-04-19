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
    public partial class wndAddMultiChoice :Window
    {
        // TODO: DirectoryIndex things are totally fucked
        Mode mode = Mode.create;

        public wndAddMultiChoice()
        {
            // This call is required by the designer.
            InitializeComponent();
            FillComboBox();

            // Add any initialization after the InitializeComponent() call.
            txtQuestion.Focus();
            IDT = QuestionManager.lowestAvailableQuestionID;
            LoadDialogSettings(); 
        }
        int noA = 0;
        int noW = 0;
        int IDT = -1;
        int pathIndex = -1;
        public wndAddMultiChoice(MultipleChoice questionT)
        {
            // This call is required by the designer.
            InitializeComponent();
            Title = "Edit Multiple Choice Question";
            FillComboBox();
            // Add any initialization after the InitializeComponent() call. 
            txtQuestion.Text = questionT.question;
            if(questionT.TagIndexes.Count != 0)
            {
                for (int i = 0; i < questionT.TagIndexes.Count() - 1; i++)
                    txtTags.Text += TagManager.Tags[questionT.TagIndexes[i]] + ",";
                txtTags.Text += TagManager.Tags[questionT.TagIndexes[questionT.TagIndexes.Count() - 1]];

            }
                for (int i = 0; i <= questionT.answers.Count - 1; i++)
                {
                    EditMultiChoiceAnswerBox answerBoxT = new EditMultiChoiceAnswerBox(questionT, i);
                    AddAnswerBox(answerBoxT);
                }
            string qPath = QuestionManager.FindImage(questionT.QuestionDirectory +@"QuestionImage");
            if (qPath != null)
            {
                txtImagePath.Text = qPath;
            }
            ResetBoxes();
            for (int i = 0; i < cmbSubject.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbSubject.Items[i]).Content.ToString() == SubjectManager.Subjects[questionT.SubjectIndex].ToString())
                {
                    cmbSubject.SelectedIndex = i;
                }
            }
            IDT = questionT.ID;
            noA = questionT.Data.TimesAnswered;
            noW = questionT.Data.TimesWrong; 
            mode = Mode.edit;
            LoadDialogSettings();
        }
        public void FillComboBox()
        {
            foreach(Question q in QuestionManager.questionList)
            {
                bool unique = true;
                for (int i = 0; i < cmbSubject.Items.Count; i++) {
                    if (((ComboBoxItem)cmbSubject.Items[i]).Content.ToString() == SubjectManager.Subjects[q.SubjectIndex]) { unique = false; break; }
                }
                if (unique == true) { ComboBoxItem cmb = new ComboBoxItem(); cmb.Content = SubjectManager.Subjects[q.SubjectIndex]; cmbSubject.Items.Add(cmb); }
            }
            if (cmbSubject.Items.Count > 0) { cmbSubject.SelectedIndex = 0; }
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
            if (stkAnswers.Children.Count <= 1)
            {
                lblError.Content = "One or more answers are required for a multiple choice question";
                return;
            }

            bool hasAnswer = false;
            for (int i = 0; i <= stkAnswers.Children.Count - 1; i++)
            {
                EditMultiChoiceAnswerBox rawr = stkAnswers.Children[i] as EditMultiChoiceAnswerBox;
                if (rawr.rdbCorrectAnswer.IsChecked == true)
                {
                    hasAnswer = true;
                }
            }
            if (hasAnswer == false)
            {
                lblError.Content = "Your question does not have a correct answer, please check the correct answer.";
                return;
            }
            bool[] hasImagePaths = new bool[stkAnswers.Children.Count];
            // NO IMAGE NO ANSWER TEXT VALIDATION
            for (int i = 0; i <= stkAnswers.Children.Count - 1; i++)
            {
                bool hasImagePath = false;
                string errorTemp = "Answer No. " +(i + 1).ToString() + " ";
                EditMultiChoiceAnswerBox rawr = stkAnswers.Children[i] as EditMultiChoiceAnswerBox;
                // Checking if the multiple choice answer has a image for an answer
                if(rawr.txtImagePath.Text != "")
                {
                    try { if (File.Exists(Path.GetFullPath(rawr.txtImagePath.Text))) { MessageBox.Show(rawr.txtImagePath.Text); hasImagePath = true; } }
                catch {
                    lblError.Content = errorTemp + "does not have a valid image path" ;
                    return;
                }
                    if (!hasFileExtension(rawr.txtImagePath.Text)) { lblError.Content = errorTemp + "does not have a valid file extension"; return; }
                }
                    if (string.IsNullOrEmpty(rawr.txtAnswer.Text) & hasImagePath)
                    {
                        rawr.txtAnswer.Text = " ";
                    }
                    if (string.IsNullOrEmpty(rawr.txtAnswer.Text) & !hasImagePath)
                    {
                        lblError.Content = errorTemp +" does not have an image-path nor a text-based answer";
                        return; 
                    }
                
                    hasImagePaths[i] = hasImagePath;
            }
            bool hasCorrectAnswerImagePath = false;
            if(txtAnswerImagePath.Text != "")
            {
                try
                { File.Exists(Path.GetFullPath(txtAnswerImagePath.Text)); { hasCorrectAnswerImagePath = true; } }
                    catch
                {
                    lblError.Content = "Correct Answer Image does not have a valid image path.";
                    return;
                    }
                if (!hasFileExtension(txtAnswerImagePath.Text)) { lblError.Content = "The 'Correct Answer Image' does not have a valid image file extension."; }
            }
            // MULTIPLE CHOICE QUESTION ===================================================================================
            MultipleChoice question = new MultipleChoice();
            question.question = txtQuestion.Text;
            if (cmbSubject.Text == "") { lblError.Content = "You need to assign the question a subject"; return; } 
            int subjectIndex = SubjectManager.Subjects.FindIndex(x => x.Name == cmbSubject.Text);
            if (subjectIndex == -1) { subjectIndex = SubjectManager.Subjects.Count; SubjectManager.AddSubject(new SubjectTag(cmbSubject.Text)); }
            question.SubjectIndex = subjectIndex;
            question.TimesAnswered = noA;
            question.TimesWrong = noW;
            // TAGS
            if(txtTags.Text != "")
            {
                string[] splitTags = txtTags.Text.Replace(", ", ",").Split(',');
                foreach (string tag in splitTags)
                {
                    int tagIndex = TagManager.Tags.FindIndex(i => i.Name == tag);
                    if (tagIndex == -1) { tagIndex = TagManager.Tags.Count; TagManager.AddTag(new Tag(tag)); }
                    question.TagIndexes.Add(tagIndex);
                }
            }
            // ANSWERS            
            for (int i = 0; i <= stkAnswers.Children.Count - 1; i++)
            {
                EditMultiChoiceAnswerBox answerBoxT = stkAnswers.Children[i] as EditMultiChoiceAnswerBox;
                question.answers.Add(new AnswerC(answerBoxT.txtAnswer.Text, i));
                if (answerBoxT.rdbCorrectAnswer.IsChecked == true)
                question.actualAnswer = i;
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
                for (int i = 0; i < QuestionManager.questionList.Count();i++)
                {
                    if(QuestionManager.questionList[i].ID == question.ID)
                    {
                        question.Data.DirectoryIndex = QuestionManager.questionList[i].Data.DirectoryIndex ;
                        QuestionManager.questionList[i] = question;
                        question.Write(true);
                        //question
                    }
                }
            }
            for (int i = 0; i <= stkAnswers.Children.Count - 1; i++)
            {
                EditMultiChoiceAnswerBox answerBoxT = stkAnswers.Children[i] as EditMultiChoiceAnswerBox;
                // IMAGE
                if (hasImagePaths[i]) { continue; }
                try
                {
                    string iPath = QuestionManager.FindImage(question.QuestionDirectory  + @"Answers\" + @"Answer " + i.ToString() + @"\AnswerImage");
                    if ((string.IsNullOrEmpty(answerBoxT.txtImagePath.Text)))
                    {
                        if (iPath != null) { File.Delete(iPath); }
                        continue;
                    }

                    string fullPath = Path.GetFullPath(answerBoxT.txtImagePath.Text);

                    string[] splittedPath = fullPath.Split('.');
                    string ext = splittedPath[splittedPath.Count() - 1];
                    string aImagePathT = question.QuestionDirectory + @"Answers\" + @"Answer " + i.ToString() + @"\AnswerImage" + "." + ext;
                    if (iPath == aImagePathT) { continue; }
                    File.Copy(fullPath, aImagePathT,true);
                }
                catch (Exception ex)
                { 
                }
            }
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
                }
            }
            else
            {
                if(iqPath != null && exitCheck == false) {File.Delete(iqPath);}
            }

               // Answer Image
            string iqAPath = QuestionManager.FindImage(question.QuestionDirectory + "AnswerImage");
               bool exitACheck = false;
           DoACheck:
               if (!string.IsNullOrEmpty(txtAnswerImagePath.Text
                   ) && exitACheck == false)
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

           QuestionManager.SelectQuestion(0, true);
            Close();
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
        private void ResetBoxes(object sender =  null, RoutedEventArgs e = null)
        {

            foreach (EditMultiChoiceAnswerBox box in stkAnswers.Children)
            {
                if (box.rdbCorrectAnswer.IsChecked == true )
                {
                    box.rctBackground.Fill = new SolidColorBrush(Color.FromRgb(10, 90, 30));
                }
                else
                {
                    box.rctBackground.Fill = new SolidColorBrush(Color.FromRgb(90, 30, 10));
                }
            }
        }
        private void AddAnswerBox(EditMultiChoiceAnswerBox boxT)
        {
            stkAnswers.Children.Add(boxT);
            boxT.rdbCorrectAnswer.Click += new RoutedEventHandler( ResetBoxes);
        }
        private void btnAddAnswer_Click(object sender, RoutedEventArgs e) 
        {
            AddAnswerBox(new EditMultiChoiceAnswerBox());
            ResetBoxes();
        }

        private void txtTags_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(" ")) { e.Handled = true; }
        }

        private void cmbSubject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(",")) { e.Handled = true; } 
        }
          bool hasFileExtension(string str)
    {
            string[] acceptedFormats = {"jpg",
                                       "png","bmp","jpeg","gif"};
            string[] stringT = str.Split('.');
              for(int i =0; i<acceptedFormats.Length;i++)
              {
                  if (stringT[stringT.Length - 1] == acceptedFormats[i]) { return true; }
              }
              return false;
    }
    }
    
    public enum Mode
    {
        create
        ,
        edit
            , nomodeselected
    }
}