using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
namespace FTPConsoleApplication
{
    class Program
    {
        private static Boolean FTP_Upload()
        {
            Boolean Upload_check = false;
            /* 獲得日期 */
            string Date = DateTime.Now.ToString("yyyyMMdd");
            //Date = Date.Replace('/', '-');
            try
            {
                FTPExtensions.sFTPServerIP = "127.0.0.1";
                FTPExtensions.sUserName = "Attlie";
                FTPExtensions.sPassWord = "12345";
                FTPExtensions.sDirName = Date;
                string sourceDir = "12345";
                FTPExtensions.FTPCreateDir();
                FTPExtensions.sFromFileName = @"C:\" + sourceDir + @"\" + "5S_" + Date + ".csv"; /* 上傳檔案路徑 */
                FTPExtensions.sToFileName = "5S_" + Date + ".csv"; /* 上傳檔名 */
                Upload_check = FTPExtensions.FTPUploadFile(); /* 上傳 */
                return Upload_check;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        static void Main(string[] args)
        {
            FTP_Upload();
            Console.Read();
        }
    }
}
