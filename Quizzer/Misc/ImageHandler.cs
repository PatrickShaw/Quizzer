using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace Quizzer
{
    public static class ImageHandler
    {
        public  static BitmapImage GetImagePath(Question mc)
        {
            string path = QuestionManager.FindImage(mc.QuestionDirectory + "QuestionImage");
            if (path == null)
                return null;
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
}

    }