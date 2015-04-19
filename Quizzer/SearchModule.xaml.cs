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
    /// Interaction logic for SearchModule.xaml
    /// </summary>
    public partial class SearchModule : UserControl
    {
        string _moduleTitle = "Module Title";
        public string ModuleTitle
        {
            get { return _moduleTitle; }
            set { _moduleTitle = value; }
        }
        // Buckets are what hold the filtered questions
        public class FilteringBucket
        {
            public FilteringBucket(string BucketSignature)
            {
                _signature = BucketSignature;   
            }
            List<Question> _questions = new List<Question>();
            public List<Question> Questions
            {
                get { return _questions; }
                set { _questions = value;  }
            }
            public int Count
            {
                get { return _questions.Count; }
                set { throw new Exception("An attempt ot modify Read-Only property"); }
            }
            public void Add(Question Question)
            {
                _questions.Add(Question);
            }
            string _signature;
            // All questions in the bucket will have the same questionSignature.
            public string Signature
            {
                get { return _signature; } 
            }
            bool _selected = false;
            public bool Selected
            {
                get { return _selected; }
                set { _selected = value; }
            }
        }
        string _fieldBeingSearched;
        public string FieldBeingSearched
        {
            get { return _fieldBeingSearched; }
            set { _fieldBeingSearched = value; DestroyBuckets(); CreateBuckets(); }
        }
        /// <summary>
        /// Note: THIS IS MUCH MUCH FASTER THAN Questions.Add so use this to save performance
        /// Adds a question to the search module
        /// </summary>
        /// <returns></returns>
        public bool AddQuestion(Question NewQuestion)
        {
            Questions.Add(NewQuestion);
            string questionSignature = (string)typeof(Question).GetProperty(_fieldBeingSearched, typeof(string), null).GetValue(NewQuestion, null);
                for(int i = 0 ; i < _filteringBuckets.Count;i++)
            {
                if (questionSignature != _filteringBuckets[i].Signature) { continue; }
                _filteringBuckets[i].Add(NewQuestion);
                RefreshItemSource();
                return _filteringBuckets[i].Selected;
            }
            // At this point we no that the question being added does not have a bucket and the module needs to make a new bucket.
                NewBucket(questionSignature);
                RefreshItemSource();
                return false;
        }
        public void NewBucket(string Signature)
        {
          
        } 
        public void DestroyBuckets()
        {
            _filteringBuckets.Clear();
        }
        public void CreateBuckets()
        {
            if (_questions == null) { return; }
            for(int i = 0 ; i < _questions.Count;i++)
            {
                bool isUnique = true;
                string questionSignature = (string)typeof(Question).GetProperty(_fieldBeingSearched).GetValue(_questions[i], null);
                 // bI = Bucket Index
                for(int bI = 0; bI < _filteringBuckets.Count;bI++)
                {
                    if (questionSignature == _filteringBuckets[bI].Signature) { isUnique = false; break; }
                    _filteringBuckets[bI].Add(_questions[i]);
                }
               // Make a new Bucket if there isn't one for the questionSignature
                if (isUnique)
                {
                    FilteringBucket filteringBucket = new FilteringBucket(questionSignature);
                    filteringBucket.Add(_questions[i]);
                    NewBucket(questionSignature);
                    _filteringBuckets.Add(filteringBucket);
                }
            }
        }
       
        List<Question> _questions = new List<Question>();
        public List<Question> Questions
        { 
            get { return _questions; }
            set { _questions = value; CreateBuckets(); RefreshItemSource(); }
        }
        ObservableCollection<FilteringBucket> _filteringBuckets = new ObservableCollection<FilteringBucket>();
        // For binding
        public ObservableCollection<FilteringBucket> FilteringBuckets
        {
            get { return _filteringBuckets; } 
    }
        public List<Question> FilteringQuestions
        {
            get { 
                List<Question> filteredQuestions = new List<Question>();
                for (int i = 0; i < _filteringBuckets.Count; i++)
                {
                    if (!_filteringBuckets[i].Selected) { continue; }
                    for(int o = 0 ; o < _filteringBuckets[i].Count;o++)
                    {
                        filteredQuestions.Add(_filteringBuckets[i].Questions[o]);
                    }
                }
                return filteredQuestions;
            }
        }
        public void RefreshItemSource()
        {
        }
        public SearchModule()
        {

            InitializeComponent(); 
       
        }
    }
}
