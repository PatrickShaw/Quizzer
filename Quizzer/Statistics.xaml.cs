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

namespace Quizzer
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl
    {
        public class SubjectStats
        {
            public string name = "";
            public int noTimesAnswered = 0;
            public int noTimesWrong = 0;
            public int noQuestions = 0;
            public double GetPercentWrong()
            {
                if (noTimesAnswered == 0) { return 1; }
                return Convert.ToDouble(noTimesWrong) / Convert.ToDouble(noTimesAnswered);
            }
        }
        public List<SubjectTag> subjectStatList = new List<SubjectTag>();
        public List<Tag> tagStatList = new List<Tag>();
        public void LoadQuestionGraph()
        {
            double topVal = 0;
            for (int i = 0; i < subjectStatList.Count; i++)
            {
                if (topVal < subjectStatList[i].TimesAnswered)
                {
                    topVal = subjectStatList[i].TimesAnswered;
                }
                if(topVal < subjectStatList[i]._noQuestions)
                {
                    topVal = subjectStatList[i]._noQuestions;
                }
            }
            for (int i = 0; i < subjectStatList.Count; i++)
            {
                AddRectangleToQuestionGraph(grdQuestionGraph, Color.FromRgb(255, 255, 255), 0, i, subjectStatList[i]._noQuestions, topVal);
                AddRectangleToQuestionGraph(grdQuestionGraph, Color.FromRgb(255, 255, 75), 1, i, subjectStatList[i].TimesAnswered, topVal);
                AddRectangleToQuestionGraph(grdQuestionGraph, Color.FromRgb(75, 255, 75), 2, i, subjectStatList[i].TimesAnswered - subjectStatList[i].TimesWrong, topVal);
                AddRectangleToQuestionGraph(grdQuestionGraph, Color.FromRgb(255, 75, 75), 3, i, subjectStatList[i].TimesWrong, topVal);
                
                for (int q = 0; q < 4; q++)
                {
                    ColumnDefinition columnDef1 = new ColumnDefinition();
                    columnDef1.Width = new GridLength(1, GridUnitType.Star);
                    grdQuestionGraph.ColumnDefinitions.Add(columnDef1);
                }
                TextBlock txbSubjectT;
                txbSubjectT = new TextBlock();
                grdQuestionGraph.Children.Add(txbSubjectT);
                Grid.SetColumn(txbSubjectT, grdQuestionGraph.ColumnDefinitions.Count - 5);
                Grid.SetRow(txbSubjectT, 2);
                Grid.SetColumnSpan(txbSubjectT, 4);
                txbSubjectT.Text = Convert.ToString(subjectStatList[i].Name);
                txbSubjectT.TextAlignment = TextAlignment.Center;
                txbSubjectT.TextWrapping = TextWrapping.Wrap;
            }
            txbHighestQuestions.Text = topVal.ToString();
            txbLowestQuestions.Text = "0";
            grdQuestionGraph.ColumnDefinitions.RemoveAt(grdQuestionGraph.ColumnDefinitions.Count - 1);
            Grid.SetColumnSpan(txbQuestionsGraphTitle, grdQuestionGraph.ColumnDefinitions.Count - 1); 
        }
        public void AddRectangleToQuestionGraph(Grid grd, Color startColour, int rectNo,int subNo, double currentVal, double topVal)
        {
            Rectangle rect = new Rectangle();
            Color endColour = ColourFunctions.DarkenColour(startColour,1.5);
            Color borderColour = ColourFunctions.DarkenColour(endColour,1.2);
            rect.Stroke = new SolidColorBrush(borderColour);
            rect.Fill = new LinearGradientBrush(startColour, endColour,0);
            grd.Children.Add(rect);
            Grid.SetRow(rect,1);
            Grid.SetColumn(rect, subNo* 4 + rectNo + 1);
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Height = grdQuestionGraph.RowDefinitions[1].Height.Value  * (currentVal/topVal);
            TextBlock txt = new TextBlock();
            txt.Text = currentVal.ToString();
            txt.FontSize = 12;
            txt.Foreground = new SolidColorBrush(Color.FromRgb(50,50,255));
            txt.HorizontalAlignment = HorizontalAlignment.Stretch;
            txt.TextAlignment = TextAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Bottom;
            grd.Children.Add(txt);
            Grid.SetRow(txt,1);
            Grid.SetColumn(txt,subNo * 4 + rectNo +1);
        }
        public void LoadTagStats()
        {
            trsTags.RecordsName = "Tag";
            trsTags.Tags = TagManager.Tags;
        }
        public Statistics()
        {
            InitializeComponent();
            tagStatList.Clear();
            LoadTagStats();
            trsSubjects.RecordsName = "Subject";
            List<Tag> subjectTags = new List<Tag>();
            for(int i =0 ; i < SubjectManager.Subjects.Count;i++)
                subjectTags.Add(SubjectManager.Subjects[i]);
            trsSubjects.Tags = subjectTags;
            // List box thingy
            subjectStatList.Clear();
            subjectStatList = SubjectManager.Subjects.OrderByDescending(x => 1 - x.RightPercentage).ToList();
                string bestSubject = "Requires more questinos to be answered";
                double bestSubjectPoints = 2;
                string worstSubject = "Requires more questionis to be answered";
                double worstSubjectPoints = -1;
            foreach(Tag SST in subjectStatList)
            {
                double wrongRatio = Convert.ToDouble(SST.TimesWrong)/ Convert.ToDouble(SST.TimesAnswered);
                if(SST.TimesAnswered >= 8)
                {
                    if(wrongRatio >= worstSubjectPoints)
                    {
                        worstSubject = SST.Name;
                        worstSubjectPoints = wrongRatio;
                    }
                    if(wrongRatio <= bestSubjectPoints)
                    {
                        bestSubjectPoints = wrongRatio;
                        bestSubject = SST.Name;
                    }
                }
                Rectangle rect = new Rectangle();
                Color startColour = QuestionManager.GetPercentageColour(SST.TimesWrong, SST.TimesAnswered);
                Color endColour = ColourFunctions.DarkenColour(startColour,1.5);
                Color borderColour = ColourFunctions.DarkenColour(endColour,1.2);
               rect.Stroke = new SolidColorBrush(borderColour);
               
                rect.Fill = new LinearGradientBrush(startColour,endColour,0);
                grdGraph.Children.Add(rect);
                Grid.SetColumn(rect, grdGraph.ColumnDefinitions.Count - 1);
                Grid.SetRow(rect,1);
                TextBlock txbSubjectT = new TextBlock();
                txbSubjectT.TextWrapping = TextWrapping.Wrap;
                txbSubjectT.HorizontalAlignment = HorizontalAlignment.Stretch;
                txbSubjectT.TextAlignment = TextAlignment.Center;
                txbSubjectT.VerticalAlignment = VerticalAlignment.Stretch;
                txbSubjectT.Text = SST.Name;
                grdGraph.Children.Add(txbSubjectT);
                Grid.SetRow(txbSubjectT,2);
                Grid.SetColumn(txbSubjectT, grdGraph.ColumnDefinitions.Count - 1);
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect.VerticalAlignment = VerticalAlignment.Bottom;
                rect.Height = grdGraph.RowDefinitions[1].Height.Value * (1.0 - wrongRatio);
                TextBlock textBlock = new TextBlock();
                textBlock.Text = Math.Round((SST.RightPercentage) * 100, 2).ToString() + "%";
                grdGraph.Children.Add(textBlock);
                textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBlock.VerticalAlignment = VerticalAlignment.Bottom;
                textBlock.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock, grdGraph.ColumnDefinitions.Count - 1);
                    Grid.SetRow(textBlock,1);
                textBlock.FontSize = 16;
                textBlock.Foreground  = Brushes.Black;
                ColumnDefinition columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(1, GridUnitType.Star);
                grdGraph.ColumnDefinitions.Add(columnDef);
            }
            grdGraph.ColumnDefinitions.RemoveAt(grdGraph.ColumnDefinitions.Count - 1);
            ColumnDefinition columnDefLast = new ColumnDefinition();
            columnDefLast.Width = new GridLength(10, GridUnitType.Pixel);
            grdGraph.ColumnDefinitions.Add(columnDefLast); 
            Grid.SetColumnSpan(txbGraphTitle, grdGraph.ColumnDefinitions.Count - 1);
            LoadQuestionGraph();
        }

    }
}
