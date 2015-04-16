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
using System.Diagnostics;
namespace Shaw_Tab
{
    /// <summary>
    /// Interaction logic for ModuleTabControl.xaml
    /// </summary>
    public partial class ModuleTabControl : TabControl
    {
        public bool tracked = true;
        public Point startPoint;
        public void AddTab(object control, string title, bool duplicatesAllowed = true)
        {
            int copies = ModuleCache.NoOpened(control.GetType()); 
            if (copies >= 1 && !duplicatesAllowed)
            {
                for(int i= 0 ; i < ModuleCache.draggedTabs.Count;i++)
                {
                    MessageBox.Show(ModuleCache.draggedTabs[i].Content.GetType().ToString());
                    if(ModuleCache.draggedTabs[i].Content.GetType() == control.GetType())
                    {
                        MessageBox.Show("");
                        ModuleTabControl.SetIsSelected(ModuleCache.draggedTabs[i],true);
                    }
                }
            }
            ModuleTabItem tabItemT = new ModuleTabItem(title, ModuleCache.NoOpened(control.GetType())); 
            tabItemT.Content = control;
            Items.Add(tabItemT);
            SelectedIndex = Items.Count - 1;
            SelectedItem = tabItemT;
            tabItemT.IsSelected = true;
            CheckItems();
        }
        public void AddTab(object control, string title, int copyNo, bool duplicatesAllowed = true)
        {
            ModuleTabItem tabItemT = new ModuleTabItem(title, copyNo);
            tabItemT.Content = control;
            Items.Add(tabItemT);
            SelectedIndex = Items.Count - 1;
            SelectedItem = tabItemT;
            tabItemT.IsSelected = true;
            CheckItems();
            //((UserControl)tabItemT.Content).Width = Cache.sWidth;
            //((UserControl)tabItemT.Content).Height = Cache.sHeight;
        }
        public void CheckItems()
        { 
            /*
            if (!(this.Parent is ModuleTabControl)) { return; }
            if (Items.Count <= 1)
            {
                foreach (TabItem tab in Items)
                {
                    tab.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                foreach (TabItem tab in Items)
                {
                    tab.Visibility = System.Windows.Visibility.Visible;
                }
            }*/
        }

        public void TabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            if (tracked)
            {
                ModuleCache.userControlsOpen.Remove(((TabItem)sender).Content.GetType());
            }
            Items.Remove(sender);
            CheckItems();
            if (Items.Count == 0)
            {
                UndockedModule undockedModule = Parent as UndockedModule;
                if (undockedModule != null)
                {
                    undockedModule.Close();
                }
            }
        }

        public ModuleTabControl(bool trackedT)
        {
            tracked = trackedT;
            InitializeComponent();
        }
        public ModuleTabControl()
        {
            InitializeComponent();
        }

        private void TabControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    e.Handled = true;
                    break;
                default:
                    break;

            }
        }


        private void TabControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine(Cache.draggedTabs.Count);
            //for (int i = Cache.draggedTabs.Count -1; 0<=i ;i--)
            //{
            //Begin:
            //    if (i<0) { break; }
            //    for (int o = Items.Count-1; 0<=o;o--)
            //    {
            //        if (Cache.draggedTabs[i] == Items[o]) { i--; goto Begin; }
            //    }
            //Cache.draggedTabs[i].Detach();
            //Items.Add(Cache.draggedTabs[i]);
            //}
            //CheckItems();
        } 
        private void TabControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
        }

        private void TabControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void TabControl_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void TabControl_Drop(object sender, DragEventArgs e)
        {
            ModuleTabItem tabItem = e.Data.GetData(typeof(ModuleTabItem)) as ModuleTabItem;
            ModuleTabControl oldControl = tabItem.Parent as ModuleTabControl;
            //if (e.Source.Equals(this)) { return; }
            oldControl.Items.Remove(tabItem);
            Items.Add(tabItem);
        }

        private void TabControl_PreviewDrop(object sender, DragEventArgs e)
        {
            try
            {
                ModuleTabItem tabItem = e.Data.GetData(typeof(ModuleTabItem)) as ModuleTabItem;
                ModuleTabControl oldControl = tabItem.Parent as ModuleTabControl;
                if (oldControl.Equals(this))
                {
                    e.Handled = true;
                }
            }
            catch
            {
                e.Handled = true;
            }
        }
    }
}