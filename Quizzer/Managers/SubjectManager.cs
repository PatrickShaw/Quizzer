using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace Quizzer
{
    public struct ColourTag
    {
        public ColourTag( Color TagColour,string TagName)
        {
            _tagName = TagName;
            _tagColour = TagColour;
        }
        string _tagName;
        public string Name
        {
            get{return _tagName;}
            set{_tagName = value;}
        }
        Color _tagColour;
        public Color Colour
        {
            get{return _tagColour;}
            set{_tagColour = value;}
        }
    }
    public class SubjectTag:Tag
    {
        public Color Colour;
        public int _noQuestions = 0;
        public SubjectTag(string SubjectName)
            : base(SubjectName)
        {
        }
    }
    public static class SubjectManager
    {
        public static List<SubjectTag> Subjects = new List<SubjectTag>();
        readonly static ColourTag[] DefaultSubjects = new ColourTag[] { new ColourTag(Color.FromRgb(230, 130, 130), "Mathematical Methods"),
                                                                        new ColourTag(Color.FromRgb(230,130,230),"Physics")};
        public static void SetColour(int index,Color SubjectColour)
        {
            Subjects[index].Colour = SubjectColour;
        }
        public static void SetColour(SubjectTag Subject, Color Colour)
        {
            Subject.Colour = Colour;
        }
        public static void AddSubject(string SubjectName)
        {
            SubjectTag subjectTemp = new SubjectTag(SubjectName);
            for (int i = 0; i < DefaultSubjects.Count();i++ )
            {
                if (DefaultSubjects[i].Name == SubjectName) { subjectTemp.Colour = DefaultSubjects[i].Colour; }
            }
                Subjects.Add(subjectTemp);
        }
    }
}
