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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            txtTimeInterval.Text = CacheCS.QuestionIntervals.ToString();
            if (CacheCS.useExamTheme) { rdbExamTheme.IsChecked = true; }
            foreach(Question questionT in QuestionManager.questionList)
            {
                bool unique = true;
                for(int i = 0 ; i < lstSubjectsSelected.Items.Count;i++)
                {
                    if(((CheckBox)lstSubjectsSelected.Items[i]).Content.ToString() == SubjectManager.Subjects[questionT.SubjectIndex].Name)
                    {
                        unique = false;
                        break;
                    }
                }
                if(unique)
                {
                    CheckBox cmb = new CheckBox();
                    cmb.Content = SubjectManager.Subjects[questionT.SubjectIndex].Name;
                    lstSubjectsSelected.Items.Add(cmb);
                    cmb.IsChecked = true;
                    cmb.Foreground = Brushes.White;
                    cmb.FontSize = 14;
                }
            }
            for (int i = 0; i < lstSubjectsSelected.Items.Count;i++ )
            {
                CheckBox cb = ((CheckBox)lstSubjectsSelected.Items[i]);
                for(int o = 0 ; o < QuestionManager.subjectNotSelected.Count;o++)
                {
                    if (cb.Content == QuestionManager.subjectNotSelected[o]) { cb.IsChecked = false; }
                }
            }

        }

        private void txtTimeInterval_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !StringFunctions.IsNumeric(e.Text);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTimeInterval.Text == "") { return; }
                List<string> stringList = new List<string>();
                stringList.Add(txtTimeInterval.Text);
                CacheCS.QuestionIntervals = int.Parse(txtTimeInterval.Text);
                if (CacheCS.QuestionIntervals < 10000) { CacheCS.QuestionIntervals = 10000; }
                if(rdbExamTheme.IsChecked == true)
                {
                    CacheCS.useExamTheme = true;
                }
                else
                {
                    CacheCS.useExamTheme = false;
                }
                stringList.Add(rdbExamTheme.IsChecked.ToString());
                    QuestionManager.subjectNotSelected.Clear();
                    string subjectsSelected = "";
                for(int i = 0 ; i < lstSubjectsSelected.Items.Count;i++)
                {
                    CheckBox cb = (CheckBox)lstSubjectsSelected.Items[i];
                    if(cb.IsChecked == false)
                    {
                        subjectsSelected += cb.Content;
                        QuestionManager.subjectNotSelected.Add((string)cb.Content);
                        if (i == lstSubjectsSelected.Items.Count - 1) { continue; }
                        subjectsSelected += ",";
                    }
                }
                stringList.Add(subjectsSelected);
                File.Delete("settings.ini");
                using(File.Create("settings.ini"))
                {

                }
                File.AppendAllLines("settings.ini", stringList.ToArray());
            }
            catch { }
            Close();
        }

        private void btnResetScores_Click(object sender, RoutedEventArgs e)
        {        
            if( MessageBox.Show("You are about to remove the recorded correct and wrong questions that you have anwered. Are you shaw?", "Reset question statistics", MessageBoxButton.YesNo) == MessageBoxResult.No){ return;}
        if (MessageBox.Show("Are you sure you're Shaw?", "Reset question statistics", MessageBoxButton.YesNo) == MessageBoxResult.No){return;}

        foreach(Question swag in QuestionManager.questionList)
        {
            swag.TimesAnswered = 0;
            swag.TimesWrong = 0;
            swag.OverwriteMetadata();
        }
        }

    }
}
