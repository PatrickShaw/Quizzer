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
using System.Windows.Shapes;
using System.IO;
using Shaw_Tab;
using System.Windows.Threading;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for Main_Window.xaml
    /// </summary>
    public partial class Main_Window : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public void TimerQuestion(object sender, EventArgs e)
        {
            Questions questionForm = new Questions(QuestionManager.SelectQuestion());
            timer.Stop();
            questionForm.ShowDialog();
            timer.Interval = TimeSpan.FromMilliseconds(CacheCS.QuestionIntervals * 0.9 + CacheCS.QuestionIntervals * CacheCS.rng.NextDouble() * 0.1);
            timer.Start();
        } 
        public Main_Window()
        { 
            // This call is required by the designer.
            InitializeComponent();
            string[] lines = File.ReadAllLines(".\\settings.ini");
            CacheCS.QuestionIntervals = Convert.ToInt32(lines[0]);
            CacheCS.useExamTheme = Convert.ToBoolean(lines[1]);
            try
            {
                QuestionManager.subjectNotSelected.AddRange(lines[2].Split(','));

            }
            catch (Exception ex)
            {
            }
            timer.Interval = TimeSpan.FromMilliseconds(CacheCS.QuestionIntervals * 0.9 + CacheCS.QuestionIntervals * CacheCS.rng.NextDouble() * 0.1);
            timer.Tick += TimerQuestion;
            timer.IsEnabled = true;
            // Add any initialization after the InitializeComponent() call.
            QuestionManager.ReadQuestions();
            btnStudentManager_Click(null, null);
            WindowState = WindowState.Minimized;
            WindowState = WindowState.Maximized; 
        }

        private void btnStudentManager_Click(object sender, RoutedEventArgs e)
        {
            if (ModuleCache.NoOpened(typeof(Question_Panel_Viewer)) == 1)
            {
                ModuleCache.FocusOnClosestTab(typeof(Question_Panel_Viewer));
                return;
            }
            Question_Panel_Viewer studentManager = studentManager = new Question_Panel_Viewer();
            tbcModules.AddTab(studentManager, "Question Manager", false);
        }
        private void btnRandomQuestion_Click(object sender, RoutedEventArgs e)
        {
            Question q = QuestionManager.SelectQuestion();
            if (q == null) { return; }
            Questions questionairre = new Questions(q);
            questionairre.ShowDialog();
        } 
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings wndSetting = new Settings();
            wndSetting.ShowDialog();
        } 
        private void btnSelectHardest_Click(object sender, RoutedEventArgs e)
        {
            Question q = QuestionManager.SelectQuestion(QSelectionMode.highestChance);
            if (q == null) { return; }
            Questions questionairre = new Questions(q);
            questionairre.highestChanceSelection = true;
            questionairre.ShowDialog();
        } 
        private void btnNewQuestionThing_Click(object sender, RoutedEventArgs e)
        {
            QuestionForm thing = new QuestionForm();
            thing.ShowDialog();
        } 
        private void btnNewQuestionThing_Copy_Click(object sender, RoutedEventArgs e)
        {
            Question_Data_Grid_Viewer studentManager = new Question_Data_Grid_Viewer();
            tbcModules.AddTab(studentManager, "awerawer", false);
        } 
        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            if(QuestionManager.Questions.Count == 0)
            { 
            MessageBox.Show("You can't access the statistics page until you answer a question.");
            return;
            } Statistics stats = new Statistics();
            tbcModules.AddTab(stats,"Statistics");
        }
    }


    public enum Days
    {
        DAYNOTSET = 0,
        monday = 1,
        tuesday = 2,
        wednesday = 3,
        thursday = 4,
        friday = 5,
        saturday = 6,
        sunday = 7
    }
    public enum MemberType
    {
        student,
        instructor,
        none
    }

}
