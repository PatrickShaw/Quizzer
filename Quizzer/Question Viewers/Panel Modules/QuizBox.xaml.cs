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
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quizzer
{
    /// <summary>
    /// Interaction logic for QuizBox.xaml
    /// </summary>
    public partial class QuizBox : ToggleButton,INotifyPropertyChanged 
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        Question _question;
        public Question Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;  
            }
        }
        public string Title
        {
            get { return Question.question; }
            set { throw new Exception("An attempt to modify Read-Only property"); }
        }
        public QuizBox(Question QuizBoxQuestionData)
        {
            InitializeComponent();
            _question = QuizBoxQuestionData;
            RecalcQuestionBox();
        }
        void RecalcQuestionBox()
        {
            
        lblPercentChance.Text = Math.Round(_question.PercentChance * 100, 4).ToString() + "%";
        lblQuestion.Text = _question.question;
        lblAnsweringFormat.Text = _question.AnsweringFormat.ToString();
        for(int i  = lblAnsweringFormat.Text.Count() - 1; i >=  1; i--){
            if( Char.IsUpper(lblAnsweringFormat.Text[i])) { lblAnsweringFormat.Text = lblAnsweringFormat.Text.Insert(i, " ");}
        }
        lblSubject.Text = SubjectManager.Subjects[_question.SubjectIndex].ToString();
        if (_question.AnsweringFormat == AnsweringFormat.WordedAnswer){
            if (((WordedAnswerQuestion)_question ).actualAnswer.ToString().Count() == 1) { lblAnsweringFormat.Text = "Multiple Choice";}
                }
        else{
            for(int  i   = 1; i<lblAnsweringFormat.Text.Count();i++){
                if (char.IsWhiteSpace(lblAnsweringFormat.Text[i])){
                    lblAnsweringFormat.Text.Insert(i, " ");
                }
        }
        }
        RecalcMiniStats();
        }
        public void RecalcMiniStats()
        {
            lblCorrect.Text = "";
            Run c = new Run(Format((_question.TimesAnswered - _question.TimesWrong)));
            Run w = new Run(Format(_question.TimesWrong));
            Run slash = new Run(@"\");
            c.Foreground = new SolidColorBrush(Color.FromRgb(75, 255, 75));
            w.Foreground = new SolidColorBrush(Color.FromRgb(255, 75, 75));
            lblCorrect.Inlines.Add(c);
            lblCorrect.Inlines.Add(slash);
            lblCorrect.Inlines.Add(w);
            if(_question.TimesAnswered != 0)
            {
                double wPercent = (Convert.ToDouble(_question.TimesWrong) / Convert.ToDouble(_question.TimesAnswered));
                double cPercent = (Convert.ToDouble(_question.TimesAnswered - _question.TimesWrong) / Convert.ToDouble(_question.TimesAnswered));
                if (wPercent > 0.5d)
                {
                    rctMark.Fill = new SolidColorBrush(Color.FromRgb(255, (byte)(75 + cPercent * 2.0 * 180), 75));
                    lblQuestion.Foreground = new SolidColorBrush(Color.FromRgb(255, (byte)(180 + cPercent * 2 * 75), 180));
                }
                else
                {
                    rctMark.Fill = new SolidColorBrush(Color.FromRgb( (byte)(75 + wPercent * 2.0 * 180), 255, 75));
                    lblQuestion.Foreground = new SolidColorBrush(Color.FromRgb( (byte)(180 + wPercent * 2 * 75), 255, 180));
                }
            }
           
        }
        public string Format(int number)
    {
        if (number == 0) { return "-"; } else { return number.ToString(); }
    }

        private void ctrBox_Click(object sender, RoutedEventArgs e)
        {
            if((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))== false)
            {
                EditQuestion(null,null);
            //ctrBox.Background = new SolidColorBrush(Color.FromArgb(25,25,25,205)); 
            }
        } 

        private void ctrBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ctrBox.Background = new SolidColorBrush(Color.FromArgb(25, 25, 25, 205));
        }
        private void Check(Object sender, RoutedEventArgs e)
        {
            IsChecked = true;
        }

        private void EditQuestion(object sender, RoutedEventArgs e)
        {
            //Check(null,null);
            Window editForm = null;
            int index = 0;
            for (int i = 0; i < QuestionManager.questionList.Count; i++)
            {
                if (QuestionManager.questionList[i].ID == Question.ID) { index = i; }
            }
            switch (Question.AnsweringFormat)
            {
                case AnsweringFormat.MultipleChoice:
                    editForm = new wndAddMultiChoice((MultipleChoice)QuestionManager.questionList[index]);
                    break;
                case AnsweringFormat.NumericAnswer:
                    editForm = new wndAddWorded(((NumericAnswerQuestion)QuestionManager.questionList[index]));
                    break;
                case AnsweringFormat.WordedAnswer:
                    editForm = new wndAddWorded((WordedAnswerQuestion)QuestionManager.questionList[index]);
                    break;
            } 
            editForm.ShowDialog();
            RefreshQuestionData();
        } 
        public void RefreshQuestionData()
        {
            for(int i = 0 ;i < QuestionManager.questionList.Count;i++)
            {
                if (QuestionManager.questionList[i].ID == Question.ID) { Question =QuestionManager.questionList[i]; }
            }
            RecalcQuestionBox();
        }
    }
}
