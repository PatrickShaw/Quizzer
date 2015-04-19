using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Quizzer
{
    /// <summary>
    /// Interaction logic for QuestionTagModule.xaml
    /// </summary>
    public partial class QuestionTagModule : UserControl
    {
        bool _addTagVisibility = true;
        public bool AddTagVisibility
        {
            get { return _addTagVisibility; }
            set
            {
                _addTagVisibility = value;
                if(value)
                {
                    btnAddTag.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    btnAddTag.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
        List<int> _tagIndexes = new List<int>();
        List<IDCheckBox> checkBoxes = new List<IDCheckBox>();
        public int[] GetSelectedTagIndexes()
        {
            return _tagIndexes.ToArray();
        }
        public QuestionTagModule()
        {
            InitializeComponent();
            for(int i = 0 ; i < TagManager.Tags.Count;i++)
            {
                 IDCheckBox chkTemp = new IDCheckBox();
                 chkTemp.Content = (TagManager.Tags[i]);
                chkTemp.ID = i;
                checkBoxes.Add(chkTemp);
            }
            checkBoxes.Sort((x,y) => x.Content.ToString().CompareTo(y.Content.ToString()));
 for(int i = 0 ; i<TagManager.Tags.Count;i++)
 {
     ListBoxItem containerTemp = new ListBoxItem();
     containerTemp.Content = checkBoxes[i];
     lstTags.Items.Add(containerTemp);
 }
        }
        public void CheckBox_Checked(IDCheckBox sender, RoutedEventArgs e)
        {
            _tagIndexes.Add(sender.ID);
        }

        public void CheckBox_Unchecked(IDCheckBox sender, RoutedEventArgs e)
        {
            _tagIndexes.RemoveAt(_tagIndexes.FindIndex(x => x == sender.ID));
        }

        string oldSearchString;
        private void TextBox_TextChanged(object senderT, TextChangedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate() { TagSearch(senderT); }), DispatcherPriority.Background);
        }
        private void TagSearch(object senderT)
        { 
            TextBox sender = (TextBox)senderT;
            bool freshSearch = true;
            // Reveal everything if the searchbox is empty
            if (string.IsNullOrEmpty(sender.Text))
            {
                for (int i = 0; i < lstTags.Items.Count; i++)
                {
                    ((ListBoxItem)lstTags.Items[i]).Visibility = System.Windows.Visibility.Visible;
                }
                oldSearchString = "";
                return;
            }
            // Perform a faster search if the user is adding letters onto the textbox
            if (!string.IsNullOrEmpty(oldSearchString))
            {
                if (oldSearchString == sender.Text.Remove(sender.Text.Count() - 1, 1))
                {
                    for (int i = 0; i < lstTags.Items.Count; i++)
                    {
                        if (((ListBoxItem)lstTags.Items[i]).Visibility == System.Windows.Visibility.Collapsed) { continue; }
                        if (checkBoxes[i].Content.ToString().Count() < sender.Text.Count()) { ((ListBoxItem)lstTags.Items[i]).Visibility = System.Windows.Visibility.Collapsed; continue; }
                        if (checkBoxes[i].Content.ToString().ToLower().Contains(sender.Text.ToLower())) { } else { ((ListBoxItem)lstTags.Items[i]).Visibility = System.Windows.Visibility.Collapsed; continue; }
                    }
                    freshSearch = false;
                }
            }
            // Standard linear search 
            if (freshSearch)
            {

                for (int i = 0; i < lstTags.Items.Count; i++)
                {
                    if (checkBoxes[i].Content.ToString().ToLower().Contains(sender.Text.ToLower())) { ((ListBoxItem)lstTags.Items[i]).Visibility = System.Windows.Visibility.Visible; } else { ((ListBoxItem)lstTags.Items[i]).Visibility = System.Windows.Visibility.Collapsed; }
                }
            }
            oldSearchString = sender.Text;
        } 
        private void lstTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public ListBoxItem CreateTextBlockListItem(string name)
        {
            ListBoxItem lbiTemp = new ListBoxItem();
            TextBlock txbTemp = new TextBlock();
            txbTemp.Text = name;
            lbiTemp.Content = txbTemp;
            return lbiTemp;
        }
        public ListBoxItem CreateCheckBoxListItem(string TagName, int ID)
        {

            ListBoxItem lbiTemp = new ListBoxItem();
            IDCheckBox chkTemp = new IDCheckBox();
            chkTemp.Content = TagName;
            chkTemp.ID = ID;
            lbiTemp.Content = chkTemp;
            return lbiTemp; 
        }
        private void btnAddTag_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTagSearch.Text)) { return; }
            if (string.IsNullOrWhiteSpace(txtTagSearch.Text)) { return; }
            txtTagSearch.Text = txtTagSearch.Text.Trim();
            // DONE: Truncate whitespace and tabs on tags
            // Don't do anything if this has already been selected/added
            for (int i = 0; i < lstSelectedTags.Items.Count;i++ )
            {
                if( ((TextBlock)((ListBoxItem)lstSelectedTags.Items[i]).Content).Text.ToLower() == txtTagSearch.Text.ToLower())
                {
                    return;
                }
            }
            // Okay at this point we're going to add the string to the selected Tags box
            lstSelectedTags.Items.Add(CreateTextBlockListItem(txtTagSearch.Text));
            // Now check if this tag has never been seen before
            bool copyFound = false;
                for (int i = 0; i < lstTags.Items.Count; i++)
                {
                    // Only check the tags that have not been filtered out to help with performance
                    if (((ListBoxItem)lstTags.Items[i]).Visibility == System.Windows.Visibility.Collapsed) { continue; }
                    if (((string)((IDCheckBox)((ListBoxItem)lstTags.Items[i]).Content).Content).ToLower() == txtTagSearch.Text.ToLower()) {((IDCheckBox)((ListBoxItem)lstTags.Items[i]).Content).IsChecked = true; copyFound = true;  break; }
                }
                if (copyFound)
                {                     
                    // Now we know we have to add an item to the selected tags list so add one
                    // At this point we know that the tag is unique to all other tags so add it to the two listboxes and add it to the tag manager as well
                    TagManager.AddTag(txtTagSearch.Text);
                    lstTags.Items.Add(CreateCheckBoxListItem(txtTagSearch.Text, TagManager.Tags.Count - 1));
                }
        }
    }
}
