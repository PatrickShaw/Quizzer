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
using System.Windows.Shapes;

namespace Shaw_Tab
{
    /// <summary>
    /// Interaction logic for UndockedModule.xaml
    /// </summary>
    public partial class UndockedModule : Window
    {
        public bool tracked = true;
        public UndockedModule(UserControl userControl, string title, bool controlCacheTracking, bool beingUndocked = true)
        {
            InitializeComponent();
            theTabControl.AddTab(userControl, title);
            theWindow.Width = userControl.Width;
            theWindow.Height = userControl.Height;
            tracked = controlCacheTracking;
            WindowStatusUpdate(null, null);
        }
        public UndockedModule(UserControl userControl, string title, int copyNo)
        {
            InitializeComponent();
            if (tracked) { theTabControl.AddTab(userControl, title, copyNo); } else { theTabControl.AddTab(userControl, title, 0); }
            theWindow.Width = userControl.Width;
            theWindow.Height = userControl.Height;
            tracked = true;
            WindowStatusUpdate(null, null);
        }
        private void WindowStatusUpdate(object sender, RoutedEventArgs e)
        {
            int count = theTabControl.Items.Count;
            if (count == 1)
            {
                Title = (string)((TabItem)theTabControl.Items[0]).Header;
            }
            else
            {
                Title = ModuleCache.MainWindowTitle;
            }
        }
        public void AddTab(object control, string title, bool duplicatesAllowed = true)
        {
            theTabControl.AddTab(control, title, duplicatesAllowed);
            WindowStatusUpdate(null, null);
            ((TabItem)theTabControl.Items[theTabControl.Items.Count - 1]).Unloaded += new RoutedEventHandler(WindowStatusUpdate);
        }
        private void theWindow_Closed(object sender, EventArgs e)
        {
            if (!tracked) { return; }
            for (int i = 0; i < theTabControl.Items.Count; i++)
            {
                ModuleCache.userControlsOpen.Remove(((ModuleTabItem)theTabControl.Items[i]).Content.GetType());
            }
        }
    }
}
