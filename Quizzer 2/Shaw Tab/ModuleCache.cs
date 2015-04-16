using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;

namespace Shaw_Tab
{
     static class ModuleCache
    {
        public static List<Type> userControlsOpen = new List<Type>();
         public const string MainWindowTitle = "Science Talent Search - MySci";
        public static List<ModuleTabItem> draggedTabs = new List<ModuleTabItem>();
        public static double sWidth = SystemParameters.PrimaryScreenWidth;
        public static double sHeight = SystemParameters.PrimaryScreenHeight; 
        public static bool CheckIsNumeric(string sender)
        {
            decimal result;
            bool dot = sender.IndexOf(".") < 0 && sender.Equals(".") && sender.Length > 0;
            if (!(Decimal.TryParse(sender, out result) || dot))
            {
                return true;
            }
            return false;
        }
        public static int NoOpened(Type userControlType)
        {
            int count = 0;
            for (int i = 0; i < userControlsOpen.Count(); i++)
            {
                if (userControlsOpen[i] == userControlType) { count++;}
            }
            return count;
        } 
    
    }
}
