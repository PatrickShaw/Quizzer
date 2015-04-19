using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace Quizzer
{ 
   public static class ColourFunctions
    {
        public static Color  DarkenColour(Color colour, double darkeningFactor = 2.0){
        double R  = colour.R;
        double G  = colour.G;
        double B = colour.B;
        R /= darkeningFactor;
        G /= darkeningFactor;
        B /= darkeningFactor;
            
        return Color.FromRgb(Convert.ToByte(R), Convert.ToByte(G), Convert.ToByte(B));
    }
    }
}
