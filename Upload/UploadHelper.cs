using System.Net;

namespace Youpload
{
    class UploadHelper
    {
        public static bool UploadFile(string url, string filePath, ref byte[] result)
        {
            bool success = true;
            using (var webClient = new WebClient())
            {              
                try
                {
                    result = webClient.UploadFile(url, filePath);           
                }
                catch
                {                   
                    success = false;                             
                }
                
                webClient.Dispose();
                return success;
            }
        }
    }
}
