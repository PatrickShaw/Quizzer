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
    public static class TagManager
    { 
        
        public static List<Tag> Tags = new List<Tag>();
        public static void AddTag(string TagName)
        {
            // TODO: You can use a binary-like search with a heuristic using the first letter of the tag name
            // Tags will have to be alphabetical
            for(int i = 0 ; i < Tags.Count;i++)
            {
                if(TagName == Tags[i].Name){return;}
            }
            // TODO: Write a list of tags that the program contains and keep the index of the tag in question metadata
            // This will save read times when reading out tags in the metadata ie. "1" will take less time to read than "electromagnetism"
                Tags.Add(new Tag(TagName));
                string TagLine = "";
                if (Tags.Count != 0) { TagLine += "\n"; }
                TagLine += TagName;//Tags.Count.ToString();
                File.AppendAllText(@".\TagData",TagLine);
                
        } 
        public static void ReadTagData()
        {
            string[] tags = File.ReadAllLines(@".\TagData");
            for(int i = 0 ; i < tags.Count();i++)
            { 
                Tags.Add(new Tag(tags[i]));
            }
        }
    }
    public class AnswerRecord: INotifyPropertyChanged
    {

        int _noAnswered = 0;
        public int TimesAnswered
        {
            get { return _noAnswered; }
            set { _noAnswered = value; EvaluatePercentages();  }
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
        int _noWrong = 0;
        public int TimesWrong
        {
            get { return _noWrong; }
            set { _noWrong = value; EvaluatePercentages();   }
        }
        public int TimesRight
        {
            get
            {
                return TimesAnswered - TimesWrong;
            }
        }
        public void EvaluatePercentages()
        {
            if (_noAnswered == 0) { return; }
            _wrongPercentage = (double)_noWrong / (double)_noAnswered;
            _rightPercentage = 1 - (double)_wrongPercentage; 
        }
        double _rightPercentage = 0;
        public double RightPercentage
        {
            get { return _rightPercentage; }
        }
        double _wrongPercentage = 0;
        public double WrongPercentage
        {
            get { return _wrongPercentage; }
        }
    }
 public class Tag : AnswerRecord
 {
     public Tag(string TagName)
     {
         Name = TagName;
     }
     protected string _name = "";
     public string Name
     {
         get{ return _name;}
         set{_name = value;}
     }

        public static bool operator ==(Tag me, Tag you)
        { 
            if(me._name == you._name){return true;}
            return false;
        }
     public static bool operator !=(Tag me, Tag you)
        {
            if (me._name == you._name) { return false; }
            return true;
        }
    public static implicit operator string (Tag me)
     {
         return me._name;
     }
    public override string ToString()
    {
        return Name;
    }

 }  
}
