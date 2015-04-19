using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzer
{
    public static class SortedQuestionLists 
    {
        private static List<QuestionData> _sortedByTitle = new List<QuestionData>();
        private static List<QuestionData> _sortedByNoAnswered = new List<QuestionData>();
        private static List<QuestionData> _sortedByNoRight = new List<QuestionData>();
        private static List<QuestionData> _sortedByNoWrong = new List<QuestionData>();
        private static List<QuestionData> _sortedByPercentRight = new List<QuestionData>();
        // NOTE: Question Manager should add the question here too if Question Manager needs to add a question to itself
        public static void Add_NoSort(QuestionData question)
        {

        }
        public static void Add_Sorted()
        {

        }
    }
}
