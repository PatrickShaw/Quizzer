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
    /// Interaction logic for ModuleTabItem.xaml
    /// </summary>
    public partial class ModuleTabItem : TabItem
    {
        public int copy = 0;
        public string title = "Name required";
        bool imdead = false;
        public ModuleTabItem(string titleT, int copyNoT)
        {
            title = titleT;
            copy = copyNoT;
            if (copy == 0)
            {
                Header = titleT;
            }
            else
            {
                Header = titleT + " " + copy.ToString();
            }
            InitializeComponent();
        }

        private void mnuClose_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as TabControl).Items.Remove(this);
        }

        private void mnuRename_Click(object sender, RoutedEventArgs e)
        {
            RenameBox rawr = new RenameBox((string)Header);
            rawr.ShowDialog();
            Header = rawr.txtInput.Text;
        }

        private void mnuUndock_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as TabControl).Items.Remove(this);
            UndockedModule rawr = new UndockedModule((UserControl)this.Content, title, copy);
            imdead = true;
            rawr.Width = Width;
            rawr.Height = Height;
            rawr.Show();

        }


        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        public void Detach()
        {
            ModuleTabControl oldTabControl = (this.Parent as ModuleTabControl);
            if (oldTabControl != null)
            {
                oldTabControl.Items.Remove(this);
                oldTabControl.CheckItems(); 
            }

        }
        private void Panel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (beingDragged)
            //{
            //    this.ReleaseMouseCapture();
            //    Cache.draggedTabs.Clear();
            //    beingDragged = false;
            //}
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                mnuUndock_Click(null, null);
            }
        }

        private void Panel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataObject dataObj = new DataObject(this);
            DragDrop.DoDragDrop(this.Parent as ModuleTabControl, dataObj, DragDropEffects.Move);
            //this.CaptureMouse();
            //Cache.draggedTabs.Clear();
            //Cache.draggedTabs.Add(this);
            //beingDragged = true;
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            ModuleCache.userControlsOpen.Add(this.Content.GetType());
            this.Unloaded += new RoutedEventHandler((Parent as ModuleTabControl).TabItem_Unloaded);
        }

        private void TabItem_DragLeave(object sender, DragEventArgs e)
        {
            //ModuleTabControl moduleTabControl = (this.Parent as ModuleTabControl);
            //if (moduleTabControl != null)
            //{
            //    if (moduleTabControl.Items.Count > 1)
            //    {
            //        mnuUndock_Click(sender, null);
            //    }
            //}
        }

        private void TabItem_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //if (imdead) { e.Action = DragAction.Cancel; }
        }

    }
}
