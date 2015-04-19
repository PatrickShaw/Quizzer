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
    /// Interaction logic for TagRecordStatistics.xaml
    /// </summary>
    public partial class TagRecordStatistics : UserControl
    {
        string _recordName = "";
        public string RecordsName
        {
            set { _recordName = value;
            txbBestRecordTitle.Text = "Best " + _recordName;
            txbWorstRecordTitle.Text = "Worst " + _recordName;
            }
            get { return _recordName; }
        }
        List<Tag> _tags = new List<Tag>();
        public List<Tag> Tags
        { 
            set 
            {
                _tags = value;
                GenerateRecord();
            }
            get { return _tags; }
        }
        public void GenerateRecord()
        {
            lstRecordVSMarks.Items.Clear();
            
                string bestSubject = "Requires more questinos to be answered";
                double bestSubjectPoints = 2;
                string worstSubject = "Requires more questionis to be answered";
                double worstSubjectPoints = -1;
                _tags = _tags.OrderBy(x => x.WrongPercentage).ThenByDescending(y => (y.TimesRight - y.TimesWrong)).ThenBy(z => z.TimesRight).ToList();
                foreach (Tag SST in _tags)
                { 
                    ListBoxItem lstSST = new ListBoxItem();
                    TextBlock txbT = new TextBlock();
                    txbT.FontSize = 14;
                    txbT.TextAlignment = TextAlignment.Right;
                    Run subject = new Run(SST.Name + " - ");
                    Run c = new Run((SST.TimesAnswered - SST.TimesWrong).ToString());
                    Run w = new Run(SST.TimesWrong.ToString());
                    Run slash = new Run("\\");
                    c.Foreground = new SolidColorBrush(Color.FromRgb(75, 255, 75));
                    w.Foreground = new SolidColorBrush(Color.FromRgb(255, 75, 75));
                    txbT.Inlines.Add(subject);
                    txbT.Inlines.Add(c);
                    txbT.Inlines.Add(slash);
                    txbT.Inlines.Add(w);
                    lstSST.Content = txbT;
                    lstRecordVSMarks.Items.Add(lstSST);
                    double wrongRatio = Convert.ToDouble(SST.TimesWrong) / Convert.ToDouble(SST.TimesAnswered);
                    if (SST.TimesAnswered >= 4)
                    {
                        if (wrongRatio >= worstSubjectPoints)
                        {
                            worstSubject = SST.Name;
                            worstSubjectPoints = wrongRatio;
                        }
                        if (wrongRatio <= bestSubjectPoints)
                        {
                            bestSubjectPoints = wrongRatio;
                            bestSubject = SST.Name;
                        }
                    }
                    txbBestRecord.Text = bestSubject;
                    txbWorstRecord.Text = worstSubject;
                }
        }
        public TagRecordStatistics()
        {
            InitializeComponent();
        }
    }
}
