using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Youpload.Forms
{
    public class UploadNotificationFormConfig : IDisposable
    {       
        public Image Image { get; set; }
        public string Text { get; set; }
        public string FilePath { get; set; }
        public string URL { get; set; }

        public void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }
        }
    }
}
