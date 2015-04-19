using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
namespace Quizzer
{
    public  enum QSelectionMode
    {
        random,
        highestChance
    }
public static class QuestionManager
{
    private static AnswerRecord _totalTag = new AnswerRecord();
    private static AnswerRecord  _totalQuestion = new AnswerRecord(); 
    // TODO: Make updating more effecient, update only every now and then since averages probably won't change that often
    public static double AverageRightQuestionPercentage
    {
get{ return _totalQuestion.RightPercentage; }
    }
    public static double AverageWrongQuestionPercentage
    {
        get {  return _totalQuestion.WrongPercentage; }
    }
    public static double AverageTagRightPercentage
    {
        get {  return _totalTag.RightPercentage; }
    }
    public static double AverageTagWrongPercentage
    {
        get {  return _totalTag.WrongPercentage; }
    }
    // TODO: It would be more efficient if you added the data as questions got answered/deleted/edited
    // Answered = Would have to update questions and tag totals
    // Deleted = Update questiosn and tag totals
    // Edit = Have to update tag totals
    public static void CalculateAverages()
    {
        _totalQuestion.TimesAnswered =0 ;
        _totalQuestion.TimesWrong = 0 ;
        _totalTag.TimesAnswered =0;
        _totalTag.TimesWrong = 0;
        for(int i = 0 ; i < questionList.Count;i++)
        {
            _totalQuestion.TimesAnswered += questionList[i].TimesAnswered;
            _totalQuestion.TimesWrong += questionList[i].TimesWrong;
            for(int o = 0; o < questionList[i].TagIndexes.Count;o++)
            {
                _totalTag.TimesAnswered += TagManager.Tags[questionList[i].TagIndexes[o]].TimesAnswered;
                _totalTag.TimesWrong += TagManager.Tags[questionList[i].TagIndexes[o]].TimesWrong;
            }
        }
    }
    public static Color GetPercentageColour(int TimesWrong,int TimesAnswered)
    {
    if (TimesAnswered != 0) {
	double wPercent = (Convert.ToDouble(TimesWrong) / Convert.ToDouble(TimesAnswered));
	double cPercent = (Convert.ToDouble(TimesAnswered - TimesWrong)) / Convert.ToDouble(TimesAnswered);
	if (wPercent > 0.5){
        return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(255), Convert.ToByte(90 + cPercent * 2.0 * 165), Convert.ToByte(90));
 	} 
    else {
		return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(90 + 165.0 * wPercent * 2.0), Convert.ToByte(255), Convert.ToByte(90)); 
	}
}
    return Color.FromRgb(0, 0, 0);
    }
    public static int lowestAvailableQuestionID = 0;
    public static List<Question> questionList = new List<Question>();
    public static List<Question> Questions
    {
        get { return questionList; }
        set { questionList = value; }
    }
    public static List<long> questionHistory = new List<long>();
    public static List<string> subjectNotSelected = new List<string>();
    // minus the extension, not full path
    public static readonly string[] imageExtensions = new string[] { "jpg", "jpeg", "bmp", "png", "gif" };
    public static string FindImage (string path)
{
    foreach (string ext in imageExtensions) { if (File.Exists(path + "." + ext)) { return Path.GetFullPath(path + "." + ext); } }
        return null;
}
    public static List<string> GetSubjects()
    {
        List<string> subjectsT = new List<string>();
        for (int i = 0; i < SubjectManager.Subjects.Count; i++)
        { subjectsT.Add(SubjectManager.Subjects[i].Name); }
            return subjectsT;
    }
    public static void ReadQuestions()
    {
        TagManager.ReadTagData();
        if (!Directory.Exists(@".\Questions")) { Directory.CreateDirectory(@".\Questions"); }
        DirectoryManager.Directories.AddRange( Directory.GetDirectories(@".\Questions"));
        for (int i = 0; i < DirectoryManager.Directories.Count(); i++)
        {
            // Read metadata lines and ignore folders without metadata lines
            if (!File.Exists(DirectoryManager.Directories[i] + @"\" + "metadata")) { continue; }
            string[] metadataLines = File.ReadAllLines(DirectoryManager.Directories[i] + @"\" + "metadata");
            // Get Answering Form
            AnsweringFormat qType = (AnsweringFormat)Convert.ToInt32(metadataLines[2]);
            Question q = null;
            string actualAnswerString = File.ReadAllText(DirectoryManager.Directories[i] + @"\" + "Actual Answer.txt");
            switch (qType)
            {
                case AnsweringFormat.MultipleChoice:
                    MultipleChoice mc = new MultipleChoice();
                    string[] answerDirectories = Directory.GetDirectories(DirectoryManager.Directories[i] + @"\Answers").ToArray();
                    for (int o = 0; o < answerDirectories.Count(); o++)
                    {
                        string textAnswer = "";
                        if (File.Exists(answerDirectories[o].ToString() + @"\AnswerText.txt"))
                        {
                            textAnswer = File.ReadAllText(answerDirectories[o].ToString() + @"\AnswerText.txt");
                        }
                        mc.answers.Add(new AnswerC(textAnswer, o));
                    }
                    mc.actualAnswer = Convert.ToInt32(actualAnswerString);
                    q = mc;
                    break;
                case AnsweringFormat.NumericAnswer:
                    NumericAnswerQuestion va = new NumericAnswerQuestion();
                    va.actualAnswer = Convert.ToDouble(actualAnswerString);
                    q = va;
                    break;
                case AnsweringFormat.WordedAnswer:
                    WordedAnswerQuestion wa = new WordedAnswerQuestion();
                    wa.actualAnswer = actualAnswerString;
                    q = wa;
                    break;
            }
            q.ID = i;
            q.modifiedTime = File.GetLastWriteTime(DirectoryManager.Directories[i] + @"\" + "metadata");
            q.Data.TimesWrong = Convert.ToInt32(metadataLines[0]);
            q.Data.TimesAnswered = Convert.ToInt32(metadataLines[1]);
            q.Data.DirectoryIndex = i;
            int subjectIndex = SubjectManager.Subjects.FindIndex(x => x.Name == metadataLines[3]);
            if (subjectIndex == -1) { subjectIndex = SubjectManager.Subjects.Count; SubjectManager.AddSubject(metadataLines[3]); }
            q.SubjectIndex = subjectIndex;
            SubjectManager.Subjects[q.SubjectIndex]._noQuestions++;
            SubjectManager.Subjects[q.SubjectIndex].TimesAnswered += q.TimesAnswered;
            SubjectManager.Subjects[q.SubjectIndex].TimesWrong += q.TimesWrong;
            if (metadataLines.Count() == 5)
                if (metadataLines[4] != "")
                {
                    string[] splitTags = metadataLines[4].Split(',');
                    foreach (string tag in splitTags)
                    {
                        string sPass1 = "";
                        bool letterFound = false;
                        for (int z = 0; z < tag.Count(); z++)
                        {
                            if (letterFound) { sPass1 += tag[z]; }
                            else { if (char.IsWhiteSpace(tag[z])) { continue; } else { letterFound = true; sPass1 += tag[z]; } }
                        }
                        string sPass2 = "";
                        letterFound = false;
                        for (int z = sPass1.Count() - 1; z >= 0; z--)
                        {
                            if (letterFound) { sPass2 += sPass1[z]; }
                            else { if (char.IsWhiteSpace(sPass1[z])) { continue; } else { letterFound = true; sPass2 += tag[z]; } }
                        }
                        string sPass3 = "";
                        for (int z = sPass2.Count() - 1; z >= 0; z--)
                        {
                            sPass3 += sPass2[z];
                        }
                        // int tagIndex = int.Parse(sPass3);
                        int tagIndex = TagManager.Tags.FindIndex(x => x.Name == sPass3);
                        //Debug.WriteLine(sPass3 + "|" + tagIndex);
                        if (tagIndex == -1) { tagIndex = TagManager.Tags.Count - 1; }
                        q.TagIndexes.Add(tagIndex);
                    }
                }
             // Add Correct/Incorrect statistics to Tag Data
            for (int f = 0; f < q.TagIndexes.Count;f++ )
            {
                TagManager.Tags[q.TagIndexes[f]].TimesAnswered += (int)q.TimesAnswered;
                TagManager.Tags[q.TagIndexes[f]].TimesWrong += (int)q.TimesWrong;
            }
            
            // Now find the actual string of the question (if it has one)
                q.question = " ";
            DirectoryManager.Directories[q.Data.DirectoryIndex] = DirectoryManager.Directories[i] + "\\";
            if (File.Exists(DirectoryManager.Directories[i] + @"\QuestionText.txt")) { q.question = File.ReadAllText(DirectoryManager.Directories[i] + @"\QuestionText.txt"); }
            questionList.Add(q);
        }
        //for (int i = 0; i < QuestionManager.questionList.Count; i++)
            //QuestionManager.questionList[i].OverwriteMetadata();
        lowestAvailableQuestionID = DirectoryManager.Directories.Count();
        CalculateAverages();
            SelectQuestion(0, true);
         }
   static Random rnd = new Random();
    
    public static void AddQuestionToHistory(Question q)
   {
       questionHistory.Insert(0,q.ID);
       if (questionList.Count < 100) { questionList.RemoveAt(questionList.Count - 1); }
   }
    class TagPoints
    {
        public int tagName;
        public double noAnswered = 0;
        public double noWrong = 0;
        public double WrongFactor(double noQAnswered, double noQWrong)
        {
            return ((noWrong - noQWrong ) / (noAnswered - noQAnswered));
        }
    }  
    public static Question SelectQuestion(QSelectionMode selectionModeT = QSelectionMode.random, bool justRecalcChance = false)
    {
        if (questionList.Count == 0)
        {
            return null;
        }
        double points = 0;
        double totalQuestionsAnswered = 0;
        
        // TAG FACTOR
        // Go through each question, adding up the number of tags in stuff 
            for (int i = 0; i < questionList.Count; i++)
            { 
                totalQuestionsAnswered += questionList[i].TimesAnswered;
            }

        // OLDEST DATE
        DateTime oldestDate = DateTime.Now;
        for (int i = 0; i < questionList.Count; i++)
        {
            if (oldestDate > questionList[i].modifiedTime)
            {
                oldestDate = questionList[i].modifiedTime;
            }
        }
        double nowToOldestDate = (DateTime.Now - oldestDate).TotalSeconds; 
        double averageTimesAnswered = 0;
        int noQuestionsUsed = 0;
        for(int i = 0 ; i < QuestionManager.questionList.Count;i++)
        {
            if (QuestionManager.questionList[i].TimesAnswered == 0) { continue; }
            noQuestionsUsed += 1;
            averageTimesAnswered += questionList[i].TimesAnswered;
        }
        averageTimesAnswered /= (double)noQuestionsUsed;
        
        double[] qPoints = new double[questionList.Count];
        
        for (int i = 0; i < questionList.Count; i++)
        {
             qPoints[i] = questionList[i].Data.Points(averageTimesAnswered,nowToOldestDate);
             questionList[i].PercentChance = qPoints[i];
            points += qPoints[i];
        }
        for (int i = 0; i < questionList.Count; i++)
        { questionList[i].PercentChance /= points; }
        if (justRecalcChance) { return null; }
        Debug.WriteLine(points);
        double SelectedValue = rnd.NextDouble() * points;
        Debug.WriteLine(SelectedValue);
        if (QSelectionMode.random == selectionModeT)
        {
            double currentPoints = 0;
            for (int i = 0; i < questionList.Count; i++)
            {
                currentPoints += qPoints[i];
                if (SelectedValue <= currentPoints)
                {
                    AddQuestionToHistory(questionList[i]);
                    return questionList[i];
                }
            }
            MessageBox.Show("Could not find a question to use :(");
            return null;
        }
        else
        {
            double highestChance = 0;
            int bestIndex = -1;
            for(int i =0;i<qPoints.Count();i++)
            {
                if (qPoints[i] > highestChance) { bestIndex = i; highestChance = qPoints[i]; }
            }
            if (bestIndex == -1) { MessageBox.Show("Could not find a question to use :("); return null; }
            AddQuestionToHistory(questionList[bestIndex]);
            return questionList[bestIndex];
        }
    }
     

    public static void WriteBackAllQuestions()
    {
       
    } 
}
 
}
