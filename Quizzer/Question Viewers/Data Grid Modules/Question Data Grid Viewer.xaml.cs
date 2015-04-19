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
using System.Collections.ObjectModel;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for Question_Data_Grid_Viewer.xaml
    /// </summary>
    public partial class Question_Data_Grid_Viewer : UserControl
    {
        private ObservableCollection<Question> _questions = new ObservableCollection<Question>();
        public ObservableCollection<Question> Questions
        {
            get
            {
                return _questions;
            }
            set { _questions = value; }
        }
        public Question_Data_Grid_Viewer()
        {
            InitializeComponent();
          //  dgrdQuestions.ItemsSource = QuestionManager.questionList.Select(qstClass => new { qstClass.Data.ID, qstClass.Data.TimesAnswered, qstClass.Data.TimesRight, qstClass.Data.TimesWrong, qstClass.question, qstClass.QuestionDirectory }).ToList();
            //srcModule.FieldBeingSearched = "Subject";
            //srcModule.Questions = QuestionManager.questionList;
            
        }
    }
}
