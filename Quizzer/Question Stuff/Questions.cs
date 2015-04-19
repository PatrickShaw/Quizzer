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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Input.StylusPlugIns; 
using System.Windows.Ink;
using System.ComponentModel; 
namespace Quizzer
{
        
   // QUIZZEr ===============================================================================
    public enum AnsweringFormat
    {
        MultipleChoice,
        WordedAnswer,
        NumericAnswer
    } 
    public class QuestionData : AnswerRecord
    {
        public string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        } 
        public int _directoryIndex;
        public int DirectoryIndex
        {
            get { return _directoryIndex; }
            set { _directoryIndex = value; }
        }
        int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Directory
        {
            get { return DirectoryManager.Directories[_directoryIndex]; }
        }

        public List<int> _tagIndexes = new List<int>();
        public List<int> TagIndexes
        {
            get { return _tagIndexes; }
            set { _tagIndexes = value; }
        }
        private DateTime _lastTimeAnswered = DateTime.Now;
        public DateTime MostRecentTimeAnswered
        {
            get { return _lastTimeAnswered; }
            set { _lastTimeAnswered = value; }
        }

        private double RecentnessPointsFactor()
        {

            // the lower the index the more recent
            double factor = 1;
            for (int i = 0; i < QuestionManager.questionHistory.Count(); i++)
            {
                if (QuestionManager.questionHistory[i] == ID) { factor *= (double)i / (double)QuestionManager.questionHistory.Count(); }
            }
            return factor;
        } 
        public double Points(double AverageTimesAnswered, double DifferenceBetweenNowAndLeastRecentTimeAnswered)
        { 
            // TAG FACTOR
            // If total tag percentage wrong is higher, the chance also gets higher 
            double tagPercentageWrong = 0; 
            int totalTagTimesAnswered = 0;
            int totalTagTimesWrong = 0;
            for(int i = 0 ; i < TagIndexes.Count;i++)
            {
                // Note: Minus is there to negate the questions own tag data
                int tempTagTimesAnswered = TagManager.Tags[TagIndexes[i]].TimesAnswered - TimesAnswered;
                if (tempTagTimesAnswered == 0) { continue; }
                totalTagTimesAnswered += tempTagTimesAnswered;
                totalTagTimesWrong += TagManager.Tags[TagIndexes[i]].TimesWrong - TimesWrong;
            }
            if(totalTagTimesAnswered != 0)
            {
                tagPercentageWrong = (double)totalTagTimesWrong / (double)totalTagTimesAnswered;
            }
            else
            {
                tagPercentageWrong = QuestionManager.AverageTagWrongPercentage;
            }
            
            // TimesAnswered Factor
            // If over the AverageTimesAnswered THEN Decrease chance
            // If under the AverageTimesAnswered THEN Increase chance (Actually don't do that)
            // If twice the number of questions answered then half the chance
            // If quadruple the number of questions answered the quarter the chance
            double timesAnsweredFactor = TimesAnswered - AverageTimesAnswered;
            if (timesAnsweredFactor <=0)
            {
                timesAnsweredFactor = 1;
            }
            else
            {
                timesAnsweredFactor = (double)TimesAnswered / (double)AverageTimesAnswered;
                timesAnsweredFactor = 1.0 / timesAnsweredFactor;
            }
            // Time
            // More recent = Lower chance
            double recentnessFactor = (double)(DateTime.Now - _lastTimeAnswered).TotalSeconds / (double)DifferenceBetweenNowAndLeastRecentTimeAnswered;
            
            const double QuestionWrongImportance = 0.9;
            const double TagWrongImportance = 0.9;
            const double RecentnessImportance = 0.99;
            const double TimesAnsweredImportance = 0.5;
            double points =1;
            points *= WrongPercentage * QuestionWrongImportance + (1 - QuestionWrongImportance);
            points *= tagPercentageWrong * TagWrongImportance + (1 - TagWrongImportance);
            points *= recentnessFactor * RecentnessImportance + (1 - RecentnessImportance);
            points *= timesAnsweredFactor * TimesAnsweredImportance + (1 - TimesAnsweredImportance);
            points *= RecentnessPointsFactor(); // TODO: Make it so you woon't need this one anymore
            return points;
        }
    }
    public abstract class Question : INotifyPropertyChanged
    {
        public string question
        {
            get { return Data.Title; }
            set { Data.Title = value; OnPropertyChanged("Question"); }
        }
   public AnsweringFormat AnsweringFormat = AnsweringFormat.MultipleChoice;
   public QuestionData Data = new QuestionData(); 
   public int ID
   {
       get { return Data.ID; }
       set { Data.ID = value; }
   }
        public DateTime modifiedTime
   {
       get { return Data.MostRecentTimeAnswered; }
       set { Data.MostRecentTimeAnswered = value; }
   }
        public string QuestionDirectory
   {
       get { return Data.Directory; }
   }
        public int DirectoryIndex
        {
            get { return Data.DirectoryIndex; }
        }
        public int TimesWrong
        {
            get { return Data.TimesWrong; }
            set { Data.TimesWrong = value; }
        }
        public int TimesAnswered
        {
            get { return Data.TimesAnswered; }
            set { Data.TimesAnswered = value; }
        }
   public int _subjectIndex;
        public int SubjectIndex
   {
       get { return _subjectIndex; }
       set { _subjectIndex = value; }
   }
        public string Subject
        {
            get { return SubjectManager.Subjects[_subjectIndex].Name; }
        }
        public void IncreaseTimesAnswered()
   {
       Data.TimesAnswered += 1;
            for(int i = 0; i < TagIndexes.Count;i++)
            {
                TagManager.Tags[TagIndexes[i]].TimesAnswered++;
            }

            SubjectManager.Subjects[SubjectIndex].TimesAnswered++;
   }
        public void Right()
        {
            IncreaseTimesAnswered();
        }
        public void Wrong()
   {
       IncreaseTimesAnswered();
       Data.TimesWrong += 1;
            for(int i = 0 ; i < TagIndexes.Count;i++)
            {
                TagManager.Tags[TagIndexes[i]].TimesWrong++;
            }
            SubjectManager.Subjects[SubjectIndex].TimesWrong++;
   } 
        public List<int> TagIndexes
   {
       get { return Data.TagIndexes; }
       set { Data.TagIndexes = value; }
   }
   private double _percentChance = 0;
   public double PercentChance
   {
       get { return _percentChance ; }
       set { _percentChance = value; OnPropertyChanged("PercentChance"); }
   }

   public event PropertyChangedEventHandler PropertyChanged;
   protected void OnPropertyChanged(string name)
   {
       PropertyChangedEventHandler handler = PropertyChanged;
       if (handler != null)
       {
           handler(this, new PropertyChangedEventArgs(name));
       }
   }
   public virtual UserControl CreateAnswerBox()
   {
       return null;
   }
        public void OverwriteMetadata()
        {
            if (File.Exists(QuestionDirectory + "metadata")) { File.Delete(DirectoryManager.Directories[Data.DirectoryIndex] + "metadata"); }
            using (FileStream fs = File.Create(QuestionDirectory + "metadata")) { }
            List<string> metadataLines = new List<string>(); 
            metadataLines.Add(TimesWrong.ToString());
            metadataLines.Add(TimesAnswered.ToString());
            metadataLines.Add(((int)AnsweringFormat).ToString());
            metadataLines.Add(SubjectManager.Subjects[SubjectIndex].Name);
            string tagLine = "";
            if (TagIndexes.Count >= 1) { tagLine += TagIndexes[0].ToString(); }
            for (int i = 1; i < TagIndexes.Count; i++)
            { tagLine += "," + TagIndexes[i].ToString();
            Debug.WriteLine(TagManager.Tags[TagIndexes[i]]);
            }
            metadataLines.Add(tagLine);
            File.AppendAllLines(QuestionDirectory + "metadata", metadataLines.ToArray()); 
        }
    public virtual void Write(bool rewrite = false)
   { 
        if(!rewrite)
        {
            Data.DirectoryIndex = QuestionManager.questionList.Count;
            string subjectPatherised = SubjectManager.Subjects[SubjectIndex];
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                subjectPatherised = subjectPatherised.Replace(c, '_');
            }
        string path = @".\Questions\" + subjectPatherised + " - " + SummaryWords(question.ToString(),16)  ;
            int copies = 0;
            if (Directory.Exists(path)) { copies += 1; }
        Begin:
            if (Directory.Exists(path + " - " + copies.ToString()))
            {
                copies += 1;
                goto Begin;
            }
            if (copies != 0) { path += " - " + copies.ToString(); }
            DirectoryManager.Directories[Data.DirectoryIndex] = path + @"\";
        }
        Directory.CreateDirectory(QuestionDirectory);
        Directory.CreateDirectory(QuestionDirectory + @"Answers\");
            OverwriteMetadata(); 
        if (question != "")
        {
            if (File.Exists(QuestionDirectory + "QuestionText.txt")) { File.Delete(QuestionDirectory + "QuestionText.txt"); }
            using (FileStream fs = File.Create(QuestionDirectory + @"QuestionText.txt")) { }
            File.AppendAllText(QuestionDirectory + "QuestionText.txt", question);
        }
        using (File.Create(QuestionDirectory + "Actual Answer.txt")) { }
   }
    protected static string SummaryWords(string word, int length = 3)
    {
        string sT = "";

        for (int i = 0; i < word.Count() && i < length;i++)
        {
            if ((!Char.IsLetterOrDigit(word[i]) && !(word[i]==' '))) { continue; }
            sT += Char.ToUpper(word[i]);
        }
        return sT;
    }
    private double PercentWrong()
    {
        if (TimesAnswered  == 0) {
            return QuestionManager.AverageTagRightPercentage;
            //if (AnsweringFormat != AnsweringFormat.WordedAnswer) { return -0.025d; /* -0.05 to stop new questions from appearing (cause you already remember them*/}
            //else  { return 0.01d; }
        }
        double wrongP = (double)TimesWrong / (double)TimesAnswered;
        return wrongP;
    } 
}
    public class NumericAnswerQuestion : Question
    {
        public new double actualAnswer;
        public override void Write(bool rewrite = false)
        {
            base.Write(rewrite);
            File.AppendAllText(QuestionDirectory + "Actual Answer.txt", actualAnswer.ToString());
        }
        public override UserControl CreateAnswerBox()
        {
            return new NumericAnswerBox(this);
        }
        public NumericAnswerQuestion()
        {
            AnsweringFormat = AnsweringFormat.NumericAnswer;
        }
    }
public class WordedAnswerQuestion : Question
{
    public new string actualAnswer = "";
    public WordedAnswerQuestion()
    {
        AnsweringFormat = AnsweringFormat.WordedAnswer;
    }
    public override UserControl CreateAnswerBox()
    {
        return new WordedAnswerBox(this);
    }
    public override void Write(bool rewrite = false)
    {
       base.Write(rewrite);
       File.AppendAllText(QuestionDirectory + "Actual Answer.txt", actualAnswer.ToString());
    }
}
public class MultipleChoice: Question
{ 
    public List<AnswerC> answers = new List<AnswerC>();
    public int actualAnswer = 0;
    public MultipleChoice()
    {
        AnsweringFormat = AnsweringFormat.MultipleChoice;
    }
    public override UserControl CreateAnswerBox()
    {
        return new MCBox(this);
    }
    public override void Write(bool rewrite = false)
    {
        base.Write(rewrite);
        File.AppendAllText(QuestionDirectory + "Actual Answer.txt", actualAnswer.ToString());
        for (int i = 0; i < answers.Count; i++)
        {
            string aPath = QuestionDirectory + @"Answers\" + "Answer " + i.ToString() + @"\";
            Directory.CreateDirectory(aPath);
            if (answers[i].ToString() != "")
            {
                using (File.Create(aPath + "AnswerText.txt")) { }
                File.AppendAllText(aPath + "AnswerText.txt", answers[i].ToString());
            }
        }

    }
}
public class AnswerC{
    string answer;
    public int id;
   public AnswerC(string answerT, int idT)
    {
        id = idT;
        answer = answerT;
    }
    public override string ToString()
    {
        return  answer;
    }
}
        
    
}
