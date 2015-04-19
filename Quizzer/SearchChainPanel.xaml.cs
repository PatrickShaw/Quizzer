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
    /// Interaction logic for SearchChainPanel.xaml
    /// </summary>
    public partial class SearchChainPanel : UserControl
    {
        ObservableCollection<Question> _questions = new ObservableCollection<Question>();
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }
        List<SearchModule> searchModules = new List<SearchModule>();
        public void AddSearchModule(SearchModule NewSearchModule)
        {
            searchModules.Add(NewSearchModule);
            stkSearchModules.Children.Add(NewSearchModule);
        }
        public void AddSearchModule(SearchModule NewSearchModule,int index)
        {
            searchModules.Insert(index, NewSearchModule);
            stkSearchModules.Children.Insert(index,NewSearchModule);
            // DONE: Change order
        }
        // DONE
        public void Filter()
        {
            List<Question> filteredQuestions = QuestionManager.questionList;
            for(int i = 0 ; i < searchModules.Count;i++)
            {
                searchModules[i].Questions = filteredQuestions;
                filteredQuestions = searchModules[i].FilteringQuestions;
            }
        }
        // DONE
        public void AddQuestion(Question NewQuestion)
        {
            for(int i = 0 ; i < searchModules.Count;i++)
            {
                bool questionIsSelected =searchModules[i].AddQuestion(NewQuestion);
                if (!questionIsSelected) { break; }
            }
        }
        public SearchChainPanel()
        {
            InitializeComponent();
        }
    }
}
