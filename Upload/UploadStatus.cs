using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youpload
{
    class UploadStatus
    {
        public const int STATUS_OK = 200;
        public const int STATUS_ERROR = 500;

        public int status { get; set; }
        public string data { get; set; }
    }
}
