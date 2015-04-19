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

using System.IO;
using System.Windows.Threading;
using System.ComponentModel;
using System.Reflection; 
namespace Quizzer
{
    public partial class Question_Panel_Viewer : UserControl
    {
        public enum OrderMode
        {
            firstname,
            lastname
        }
        OrderMode orderBy = OrderMode.lastname;
        List<Question> students = new List<Question>();
        List<QuizBox> studentBoxes = new List<QuizBox>();
        List<TextBlock> letters = new List<TextBlock>();
        public Question_Panel_Viewer()
        {
        // This call is required by the designer.
        InitializeComponent();

        MouseWheel += scrollViewer_MouseWheel;
        string curDir = Directory.GetCurrentDirectory();
        // f,l,dob,street,sub,po,parn,contact,contactalt,noat
        // Add any initialization after the InitializeComponent() call.  
        scrollViewer.Focus();
        scrollViewer.Focusable = true;
        if (File.Exists(".\\Background.png"))
        {
            ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.GetFullPath(".\\Background.png"))));
            imgBrush.Stretch = Stretch.UniformToFill;
            grd.Background = imgBrush;
        } Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(OrderByLastName));
        }
        
	public void CreatestudentBoxes()
	{
		if (orderBy == OrderMode.firstname) {
			OrderByFirstName();
		} else {
			OrderByLastName();
		}
	}
	public void ResetBoxes(object sender, RoutedEventArgs e)
	{
		if ((Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)) == false) {
			foreach (QuizBox box in studentBoxes) {
				
				box.IsChecked = false;
			}
		}
	}

	string oldSearchText = "";
	public void OrderByFirstName()
	{
		txbTotalQuestions.Text = "Total Questions: " + QuestionManager.questionList.Count.ToString();
		studentBoxes.Clear();
		wrpStudents.Children.Clear();

		if (students.Count == 0) {
			foreach (Question student in QuestionManager.questionList) { 
				students.Add(student);
			}
		}

		students.Sort((x, y) => x.question.CompareTo(y.question));
		char letter = '|';
		foreach (Question student  in students) {
			
			VirtualizingStackPanel sk = new VirtualizingStackPanel();
			try {
				dynamic @null = student.question[0];
			} catch (Exception ex) {
				student.question += " ";
			}
			if ((letter != student.question[0])) {
				letter = student.question[0];
				TextBlock label = new TextBlock();
				TextBlock label2 = new TextBlock();
				label.Text = Environment.NewLine + " " + letter;
				//wrpStudents.Children.Add(label)
				label.FontSize = 22;
				label.FontWeight = FontWeights.Bold;
				label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
				sk.Children.Add(label);
				letters.Add(label);
			}
			QuizBox studentBox = new QuizBox(student);
			sk.Children.Add(studentBox);
			studentBoxes.Add(studentBox);
			wrpStudents.Children.Add(sk);
			studentBox.Click += ResetBoxes;
			studentBox.mnuDoQuestion.Click += btnDoQuestion_Click;
			studentBox.mnuAddMultiChoice.Click += btnAddstudent_Click;
			studentBox.mnuAddWordedQuestion.Click += btnAddWordedQuestion_Click;
			studentBox.mnuEditQuestion.Click += btnEditstudents_Click;
			studentBox.mnuDeleteQuestion.Click += btnDeletestudents_Click; 
		}
	}
	public void OrderByLastName()
	{
		txbTotalQuestions.Text = "Total Questions: " + QuestionManager.questionList.Count.ToString();

		studentBoxes.Clear();
		wrpStudents.Children.Clear();
		if (students.Count == 0) {
			foreach (Question student  in QuestionManager.questionList) {
				
				students.Add(student);
			}
		}

		students = students.OrderBy(x => SubjectManager.Subjects[x.SubjectIndex].ToString()).ThenBy(y => y.question.ToString()).ToList();
		string letter = "a";

		foreach (Question student  in students) {
			
			VirtualizingStackPanel sk = new VirtualizingStackPanel();
			if ((letter != SubjectManager.Subjects[student.SubjectIndex].ToString())) {
				letter = SubjectManager.Subjects[student.SubjectIndex].ToString();
				TextBlock label = new TextBlock();
				TextBlock label2 = new TextBlock();
				label.Text = Environment.NewLine + " " + letter;
				//wrpStudents.Children.Add(label)
				label.FontSize = 22;
				label.FontWeight = FontWeights.Bold;
				label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
				sk.Children.Add(label);
				letters.Add(label);
			}
			QuizBox studentBox = new QuizBox(student);
			sk.Children.Add(studentBox);
			studentBoxes.Add(studentBox);
			wrpStudents.Children.Add(sk);
			studentBox.Click += ResetBoxes;
			studentBox.mnuDoQuestion.Click += btnDoQuestion_Click;
			studentBox.mnuAddMultiChoice.Click += btnAddstudent_Click;
			studentBox.mnuAddWordedQuestion.Click += btnAddWordedQuestion_Click;
			studentBox.mnuEditQuestion.Click += btnEditstudents_Click;
			studentBox.mnuDeleteQuestion.Click += btnDeletestudents_Click;
		}
	}
	public void CheckLetters( TextBlock letter)
	{
		bool studentBoxExists = false;
		char letterT = letter.Text[3];
		foreach (QuizBox quizBox in studentBoxes) { 
			if (orderBy == OrderMode.firstname) {
				if (quizBox.lblQuestion.Text[0] > letterT)
					break; // TODO: might not be correct. Was : Exit For
				if (quizBox.lblQuestion.Text[0] == letterT & quizBox.Visibility == Visibility.Visible)
					studentBoxExists = true;
			} else {
				if (SubjectManager.Subjects[quizBox.Question.SubjectIndex].ToString()[0] > letterT)
					break; // TODO: might not be correct. Was : Exit For
				if (SubjectManager.Subjects[quizBox.Question.SubjectIndex].ToString()[0] == letterT & quizBox.Visibility == Visibility.Visible)
					studentBoxExists = true;
			}
		}
		if (studentBoxExists) {
			letter.Visibility = Visibility.Visible;
		} else {
			letter.Visibility = Visibility.Collapsed;
		}
	}
	public void CheckstudentBoxes( QuizBox quizBox)
	{
		string lwrTxtSearch = LowerCasify(txtSearch.Text);
		bool tagFound = false;
		for (int i = 0; i <= quizBox.Question.TagIndexes.Count - 1; i++) {
			if (LowerCasify(TagManager.Tags[quizBox.Question.TagIndexes[i]].Name).Contains(lwrTxtSearch))
				tagFound = true;
		}
		if (tagFound | (LowerCasify(quizBox.lblQuestion.Text).Contains(lwrTxtSearch) | LowerCasify(quizBox.lblAnsweringFormat.Text).Contains(lwrTxtSearch) | LowerCasify(quizBox.lblSubject.Text).Contains(lwrTxtSearch))) {
			quizBox.Visibility = Visibility.Visible;
		} else {
			quizBox.Visibility = Visibility.Collapsed;
		}
	}
	public void InitiateSearch()
	{
		string search = ""; 
		if (LowerCasify(txtSearch.Text).Contains(LowerCasify(oldSearchText))) {
			// FOR TYPING =============================
			foreach (QuizBox quizBox in studentBoxes) {
				
				if (quizBox.Visibility == Visibility.Collapsed)
					continue;
				CheckstudentBoxes( quizBox);
			}
			foreach (TextBlock letter in letters) {
				
				if (letter.Visibility == Visibility.Collapsed)
					continue;
				CheckLetters( letter);
			}
		} else {
			// WHEN YOUR CHANGING LOTS IN THE SEARCH BOX
			foreach (QuizBox quizBox in studentBoxes) { 
				CheckstudentBoxes( quizBox);
			}
			foreach (TextBlock letter in letters) {
				
				CheckLetters( letter);
			}
		}
		oldSearchText = LowerCasify(txtSearch.Text);
	}
	public string LowerCasify(string stringT)
	{
		string LCaseStringT = "";
		foreach (char letter in stringT) {
			
			LCaseStringT += char.ToLower(letter);
		}
		return LCaseStringT;
	}
	private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
	{
		Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(InitiateSearch));
	}

	private void btnAddstudent_Click(object sender, RoutedEventArgs e)
	{
		wndAddMultiChoice studentForm = new wndAddMultiChoice();
		studentForm.Resources = Resources;
		studentForm.ShowDialog();
		RecalcBoxes();
	}
	public void RecalcBoxes()
	{
			OrderByFirstName();
		if (orderBy == OrderMode.firstname) {
		} else {
			OrderByLastName();
		}
	}
	private void btnEditstudents_Click(object sender, RoutedEventArgs e)
	{
		int noSelected = 0;
		foreach (QuizBox quizBox in studentBoxes) { 
			if (quizBox.IsChecked==true) {
				Window studentForm = null;
				switch (quizBox.Question.AnsweringFormat) {
					case AnsweringFormat.MultipleChoice:
						studentForm = new wndAddMultiChoice((MultipleChoice)quizBox.Question);
						break;
					case AnsweringFormat.WordedAnswer:
						studentForm = new wndAddWorded((WordedAnswerQuestion)quizBox.Question);
						break;
					case AnsweringFormat.NumericAnswer:
						studentForm = new wndAddWorded((NumericAnswerQuestion)quizBox.Question);
						break;
				}
				studentForm.Resources = Resources;
				studentForm.ShowDialog();
				noSelected += 1;
			}
		}
		RecalcBoxes();
		if (noSelected == 0)
			System.Windows.MessageBox.Show("You need to select a student box (Hold CTR and left click on students)");
	}

	private void lstOrder_Click(object sender, RoutedEventArgs e)
	{
		if (orderBy == OrderMode.firstname) {
			orderBy = OrderMode.lastname;
			OrderByLastName();
			lstOrder.Content = "Order by question";
		} else {
			orderBy = OrderMode.firstname;
			lstOrder.Content = "Order by subject";
			OrderByFirstName();
		}
		txtSearch.Text = "";
	}

	private void scrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);

	}

	private void btnDeletestudents_Click(object sender, RoutedEventArgs e)
	{
		int noSelected = 0;
		if (System.Windows.MessageBox.Show("Are you sure you want to remove the students selected?", "Remove students", MessageBoxButton.YesNo) == MessageBoxResult.No)
			return;
		Begin:
		foreach (QuizBox box in studentBoxes) { 
			try {
				if (box.IsChecked == true) {
					try {
						Directory.Delete(box.Question.QuestionDirectory, true);
					} catch (Exception ex) {
						System.Windows.MessageBox.Show(ex.Message);
					}
					noSelected += 1;
					QuestionManager.questionList.Remove(box.Question);
					students.Remove(box.Question);
					wrpStudents.Children.Remove(box);
					studentBoxes.Remove(box);
					goto Begin;
				}

			} catch (Exception ex) {
				System.Windows.MessageBox.Show(ex.Message);
			}
		}
		RecalcBoxes();
		if (noSelected == 0)
			System.Windows.MessageBox.Show("You need to select a studentbox to delete a student. (Hold CTRL)");
	}


	private void btnDoQuestion_Click(object sender, RoutedEventArgs e)
	{
		bool Skip = false;
		foreach (QuizBox box in studentBoxes) { 
			try {
				if (box.IsChecked == true) {
					if (Skip == false) {
						Questions rawr = new Questions(box.Question, true);
						QuestionManager.AddQuestionToHistory(box.Question);
						rawr.ShowDialog();
						if (rawr.ForcedExitQuestioning)
							Skip = true;
					}
					box.RecalcMiniStats();
				}


			} catch {
			}
		}
	} 
	private void btnAddWordedQuestion_Click(object sender, RoutedEventArgs e)
	{
		wndAddWorded studentForm = new wndAddWorded();
		studentForm.Resources = Resources;
		studentForm.ShowDialog();
		RecalcBoxes();
	}
	private void Viewbox_Loaded(object sender, RoutedEventArgs e)
	{
	}
	public void HideScriptErrors(System.Windows.Controls.WebBrowser wb, bool Hide)
	{
		FieldInfo fiComWebBrowser = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
		if (fiComWebBrowser == null) {
			return;
		}
		object objComWebBrowser = fiComWebBrowser.GetValue(wb);
		if (objComWebBrowser == null) {
			return;
		}
		objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
	}
	System.Windows.Forms.WebBrowser webBrowser;
	private void wb_LoadCompleted(object sender, NavigationEventArgs e)
	{
		string script = "document.body.style.overflow ='hidden'";
		System.Windows.Controls.WebBrowser wb = (System.Windows.Controls.WebBrowser)sender;
		wb.InvokeScript("execScript", new Object[] {
			script,
			"JavaScript"
		});
	}
	private void webBrowser2_Navigated(object sender, NavigationEventArgs e)
	{
		/*webBrowser2.Focus();
		System.Windows.Forms.SendKeys.SendWait("^{ADD}");
		System.Windows.Forms.SendKeys.SendWait("^{ADD}");
		System.Windows.Forms.SendKeys.SendWait("^{ADD}");
		System.Windows.Forms.SendKeys.SendWait("^{ADD}");*/
		scrollViewer.Focus();
	}
    }
}
