using System;
using System.Drawing;
using System.Windows.Forms;

namespace Youpload
{
    class ClipboardHelper
    {
        public static void WriteUrl(string url)
        {
            Clipboard.SetText(url);          
        }

        public static void WriteImage(Image img)
        {
            Clipboard.SetImage(img);
        }

        public static string GetText()
        {
            return Clipboard.GetText();
        }
    }
}
