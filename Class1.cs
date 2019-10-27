using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace ftpSearchFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class T
        {
            public string Device { get; set; }
            public string Station { get; set; }
            public string family { get; set; }
            public string 機型 { get; set; }
        }
        public class orders
        {
            public orders(string inOrderText) { orderText = inOrderText; }
            string orderText = string.Empty;
            public string OrderText
            {
                set { orderText = value; }
                get { return orderText; }
            }
        }
        public struct FileStruct
        {
            public string Flags;
            public string Owner;
            public string Group;
            public bool IsDirectory;
            public DateTime CreateTime;
            public string Name;
        }
        public enum FileListStyle
        {
            UnixStyle,
            WindowsStyle,
            Unknown
        }
        private static void DeleteFolder(string path)
        {
            FtpWebRequest f;
            try
            {
                f = (FtpWebRequest)WebRequest.Create(URLAddress);

                f.UseBinary = true;
                f.UsePassive = false;
                f.KeepAlive = false;
                f.Credentials = new NetworkCredential("Attlie", "12345");
                f.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)f.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static string[] GetDeleteFolderArray(string path)
        {
            FtpDirInfo ftpDirInfo = new FtpDirInfo();//这个类里的具体代码会在最后贴出 
            string[] deleteFolders;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential("Attlie", "12345");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //Encoding encoding = Encoding.GetEncoding("GB2312"); 
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String line = reader.ReadLine();
                bool flag = false;
                while (line != null)
                {
                    FileStruct f = new FileStruct();
                    f = ftpDirInfo.GetList(line);
                    String fileName = f.Name;
                    if (f.IsDirectory)
                    {
                        result.Append(fileName);
                        result.Append("\n");
                        flag = true;
                        line = reader.ReadLine();
                        continue;
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                if (flag)
                {
                    result.Remove(result.ToString().LastIndexOf("\n"), 1);
                    return result.ToString().Split('\n');
                }
                else
                {
                    deleteFolders = null;
                    return deleteFolders;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "获取文件夹数组过程中出现异常");
                deleteFolders = null;
                return deleteFolders;
            }
        }
        private static string[] GetDeleteFileArray(string path)
        {
            FtpDirInfo ftpDirInfo = new FtpDirInfo();
            string[] DeleteFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential("Attlie", "12345");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //Encoding encoding = Encoding.GetEncoding("GB2312"); 
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                bool flag = false;
                while (line != null)
                {
                    FileStruct f = new FileStruct();
                    f = ftpDirInfo.GetList(line);
                    String fileName = f.Name; //排除非文件夹 
                    if (!f.IsDirectory)
                    {
                        result.Append(fileName);
                        result.Append("\n");
                        flag = true;
                        line = reader.ReadLine();
                        continue;
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                if (flag)
                {
                    result.Remove(result.ToString().LastIndexOf("\n"), 1);
                    return result.ToString().Split('\n');
                }
                else
                {
                    DeleteFiles = null;
                    return DeleteFiles;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "获取文件数组过程中出现异常");
                DeleteFiles = null;
                return DeleteFiles;
            }
        }
        private static void DeleteFile(string path)
        {
            FtpWebRequest f;
            try
            {
                f = (FtpWebRequest)WebRequest.Create(URLAddress);

                f.UseBinary = true;
                f.UsePassive = false;
                f.KeepAlive = false;
                f.Credentials = new NetworkCredential("Attlie", "12345");
                f.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)f.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void DeleteDir(string path)
        {
            try
            {
                string[] folderArray = GetDeleteFolderArray(path);
                string[] fileArray = GetDeleteFileArray(path);
                ArrayList folderArrayList = new ArrayList();
                ArrayList fileArrayList = new ArrayList();
                if (folderArray.Length > 0)
                {
                    for (int i = 0; i < folderArray.Length; i++)
                    {
                        if (folderArray[i] == "." || folderArray[i] == ".." || folderArray[i] == "")
                        {

                        }
                        else
                        {
                            folderArrayList.Add(folderArray[i]);
                        }
                    }
                }/*
                if (fileArray.Length > 0)
                {
                    for (int i = 0; i < fileArray.Length; i++)
                    {
                        if (fileArray[i] == "")
                        {

                        }
                        else
                        {
                            fileArrayList.Add(fileArray[i]);
                        }
                    }
                }*/

                if (folderArrayList.Count == 0 && fileArrayList.Count == 0)
                {
                    DeleteFolder(path);
                }
                else if (folderArrayList.Count == 0 && fileArrayList.Count != 0)
                {
                    for (int i = 0; i < fileArrayList.Count; i++)
                    {
                        string fileUrl = path + "/" + fileArrayList[i];
                        DeleteFile(fileUrl);
                    }
                    DeleteFolder(path);
                }
                else if (folderArrayList.Count != 0 && fileArrayList.Count != 0)
                {
                    for (int i = 0; i < fileArrayList.Count; i++)
                    {
                        string fileUrl = path + "/" + fileArrayList[i];
                        DeleteFile(fileUrl);
                    }
                    for (int i = 0; i < folderArrayList.Count; i++)
                    {
                        string dirUrl = path + "/" + folderArrayList[i];
                        DeleteFile(dirUrl);
                    }
                    DeleteFolder(path);
                }
                else if (folderArrayList.Count != 0 && fileArrayList.Count == 0)
                {
                    for (int i = 0; i < folderArrayList.Count; i++)
                    {
                        string dirUrl = path + "/" + folderArrayList[i];
                        DeleteFile(dirUrl);
                    }
                    DeleteFolder(path);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "刪除過程異常");
            }
        }
        class FtpDirInfo
        {
            public FileStruct[] ListFiles(FileStruct[] listAll)
            {
                List<FileStruct> listFile = new List<FileStruct>();
                foreach (FileStruct file in listAll)
                {
                    if (!file.IsDirectory)
                    {
                        listFile.Add(file);
                    }
                }
                return listFile.ToArray();
            }
            public FileStruct[] ListDirectories(FileStruct[] listAll)
            {

                List<FileStruct> listDirectory = new List<FileStruct>();
                foreach (FileStruct file in listAll)
                {
                    if (!file.IsDirectory)
                    {
                        listDirectory.Add(file);
                    }
                }
                return listDirectory.ToArray();
            }
            public FileStruct GetList(string datastring)
            {
                FileStruct f = new FileStruct();
                string[] dataRecords = datastring.Split('\n');
                FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
                if (_directoryListStyle != FileListStyle.Unknown && datastring != "")
                {
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(datastring);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(datastring);
                            break;
                    }
                }
                return f;
            }
            private FileListStyle GuessFileListStyle(string[] recordList)
            {
                foreach (string s in recordList)
                {
                    if (s.Length > 10
                     && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                    {
                        return FileListStyle.UnixStyle;
                    }
                    else if (s.Length > 8
                     && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                    {
                        return FileListStyle.WindowsStyle;
                    }
                }
                return FileListStyle.Unknown;
            }
            private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
            {
                FileStruct f = new FileStruct();
                string processstr = Record.Trim();
                f.Flags = processstr.Substring(0, 10);
                f.IsDirectory = (f.Flags[0] == 'd');
                processstr = (processstr.Substring(11)).Trim();
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
                f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
                string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
                if (yearOrTime.IndexOf(":") >= 0)  //time
                {
                    processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
                }
                f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
                f.Name = processstr;   //最后就是名称
                return f;
            }
            private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
            {
                FileStruct f = new FileStruct();
                string processstr = Record.Trim();
                string dateStr = processstr.Substring(0, 8);
                processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
                string timeStr = processstr.Substring(0, 7);
                processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
                DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
                myDTFI.ShortTimePattern = "t";
                f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
                if (processstr.Substring(0, 5) == "<DIR>")
                {
                    f.IsDirectory = true;
                    processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
                }
                else
                {
                    string[] strs = processstr.Split(new char[] { ' ' }, 2);// StringSplitOptions.RemoveEmptyEntries);   // true);
                    processstr = strs[1];
                    f.IsDirectory = false;
                }
                f.Name = processstr;
                return f;
            }
            private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
            {
                int pos1 = s.IndexOf(c, startIndex);
                string retString = s.Substring(0, pos1);
                s = (s.Substring(pos1)).Trim();
                return retString;
            }
            private FileStruct ListFilesAndDirectories(string ftpUri)
            {
                WebResponse webresp = null;
                StreamReader ftpFileListReader = null;
                FtpWebRequest ftpRequest = null;
                try
                {
                    ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpUri));
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    ftpRequest.Credentials = new NetworkCredential("Attlie", "12345");
                    webresp = ftpRequest.GetResponse();
                    ftpFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.Default);
                }
                catch (Exception ex)
                {
                    throw new Exception("获取文件列表出错，错误信息如下：" + ex.ToString());
                }
                string Datastring = ftpFileListReader.ReadToEnd();
                return GetList(Datastring);
            }
        }
        private static string URLAddress = "FTP://127.0.0.1/";
        private string username = "Attlie";
        private string password = "12345";
        private static string FileOrDir = "";
        private static int iFTPReTry = 0;
        private static string strExtension = "LICENSE.txt";


        private static string[] GetDirectoryList(string url)
        {
            string[] directory = GetFilesDetailList(url);
            string[] dir = new string[directory.Length];
            int count = 0;
            if (directory == null)
                return null;
            //WIN
            foreach (string str in directory)
            {
                if (str.Trim().Contains("<DIR>"))
                {
                    dir[count] = str.Substring(str.IndexOf("<DIR>") + 5).Trim();
                    count = count + 1;
                }
            }
            if (count > dir.Length)
            {
                return dir;
            }
            //Sevr-U
            foreach (string str in directory)
            {
                if (str.Length > 0 && str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    dir[count] = str.Substring(54).Trim();
                    count = count + 1;
                }
            }
            return dir;
        }
        /// <summary>
        /// 获取ftp文件详细信息
        /// </summary>
        /// <param name="url">ftp服务器地址加文件名如：(ftp://10.0.0.10/1.xml)</param>
        /// <returns>返回文件详细信息</returns>
        private static string[] GetFilesDetailList(string url)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(URLAddress));
                ftp.Credentials = new NetworkCredential("Attlie", "12345");
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                if (line == null || line.Length < 0)
                    return null;
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static List<string> GetFile(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = RequedstPath;   //目標路徑 path為伺服器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp使用者名稱和密碼
                reqFTP.Credentials = new NetworkCredential("Attlie", "12345");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文檔名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        //string msg = line.Substring(39).Trim();
                        string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("獲取檔案出錯：" + ex.Message);
            }
            return strs;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FTPExtensions.sFTPServerIP = "127.0.0.1";
            FTPExtensions.sUserName = "Attlie";
            FTPExtensions.sPassWord = "12345";
            FTPExtensions.sDirName = "opencv";
            FTPExtensions.sFileName = "LICENSE.txt";
            FTPExtensions.sFileOrDir = "LICENSE.txt";
            richTextBox.Text = "";
            //List<string> ListResult = new List<string>();
            List<orders> ListShow = new List<orders>();
            List<FileStruct> list = new List<FileStruct>();
            //list = ListFilesAndDirectories("ftp://127.0.0.1/123");
            //ListResult = FTPExtensions.FTPQuery();//
            //ListResult = GetFile("ftp://127.0.0.1/");
            //ListResult = FTPExtensions.FTPDetailQuery();
            //int result = FTPExtensions.FTPGetFileSize();
            //ListResult = GetFtpList();
            //ListResult = GetFTPFolderList();
            //ListShow.Add(new orders(result.ToString()));
            //DeleteDir(txt_FileSearch.Text);
            DownloadDirectory("ftp://127.0.0.1/", "123", "d://test");
            int count = 0;/*
            foreach (var LR in list)
            {
                count += 1;
                ListShow.Add(new orders(LR));
                richTextBox.Text += ("項目" + count + ": " + LR + "\r\n");
            }*/
            //richTextBox.Text += "FTP Folder";
            //richTextBox.Text += result.ToString();
            //dataGridView1.DataSource = list.ToList();
        }
        private static List<string> GetFtpList()
        {
            List<string> strList = new List<string>();
            FtpWebRequest f = (FtpWebRequest)WebRequest.Create(URLAddress);
            f.Method = WebRequestMethods.Ftp.ListDirectory;
            f.UseBinary = true;
            f.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            f.Credentials = new NetworkCredential("Attlie", "12345");
            StreamReader sr = new StreamReader(f.GetResponse().GetResponseStream());
            string str = sr.ReadLine();
            string tmp_filenm = "";

            while (str != null)
            {
                strList.Add(str);
                str = sr.ReadLine();
            }
            sr.Close();
            sr.Dispose();
            f = null;
            return strList;
        }
        private static void searchFile(string path)
        {
            List<string> strList = new List<string>();
            try
            {
                FtpWebRequest f = (FtpWebRequest)WebRequest.Create(URLAddress);
                f.Method = WebRequestMethods.Ftp.ListDirectory;
                f.UseBinary = true;
                f.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                f.Credentials = new NetworkCredential("Attlie", "12345");
                StreamReader sr = new StreamReader(f.GetResponse().GetResponseStream());
                string str = sr.ReadLine();
                while (str != null)
                {
                    strList.Add(str);
                    str = sr.ReadLine();
                }
                sr.Close();
                sr.Dispose();
                f = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_FileSearch_Click(object sender, EventArgs e)
        {
            //downLoad(@"c:/123", "ftp://127.0.0.1");
            string[] directory = GetDeleteFolderArray(txt_FileSearch.Text);
            //string[] directory = GetFilesDetailList(txt_FileSearch.Text);
            List<orders> listOrders = new List<orders>();
            foreach (string strValue in directory)
            {
                listOrders.Add(new orders(strValue));
            }
            //dataGridView1.DataSource = directory;
            dataGridView1.DataSource = listOrders;
        }
        //获取子文件夹数组 
        private const string LogFileName = "成績單.csv";

        private void recordLog(string exM)
        {
            File.AppendAllText(LogFileName, DateTime.Now.ToString() + "-------------------------------" + Environment.NewLine);
            File.AppendAllText(LogFileName, exM);
        }
        private string[] GetFtpFileList(string ftpAdd, string wRMetod)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpAdd);
            if (ftpRequest != null)
            {
                StringBuilder fileListBuilder = new StringBuilder();
                ftpRequest.Method = wRMetod;
                try
                {
                    WebResponse ftpResponse = ftpRequest.GetResponse();
                    StreamReader ftpFileListReader = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default);

                    string line = ftpFileListReader.ReadLine();
                    while (line != null)
                    {
                        fileListBuilder.Append(line);
                        fileListBuilder.Append("/n");
                        line = ftpFileListReader.ReadLine();
                    }
                    ftpFileListReader.Close();
                    ftpResponse.Close();
                    fileListBuilder.Remove(fileListBuilder.ToString().LastIndexOf('\n'), 1);
                    return fileListBuilder.ToString().Split('\n');
                }
                catch (Exception ex)
                {
                    recordLog(ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public bool DownloadDirectory(string ftpAddress, string dirName, string localDir)
        {
            FtpDirInfo ftpDirInfo = new FtpDirInfo();
            String[] fileList = null;
            string downloadDir = string.Empty;
            if (dirName != string.Empty)
            {
                FileStruct f = new FileStruct();
                f = ftpDirInfo.GetList(dirName);
                //fileList = GetFileList(ftpAddress + dirName + "/", WebRequestMethods.Ftp.ListDirectoryDetails);
                downloadDir = localDir + "//" + dirName;
                if (!Directory.Exists(downloadDir))
                {
                    Directory.CreateDirectory(downloadDir);
                }
            }
            else
            {
                return false;
            }

            List<string> directory = new List<string>();
            string[] temp = null;
            string[] files = GetFtpFileList(ftpAddress + "/", WebRequestMethods.Ftp.ListDirectoryDetails);

            if (fileList != null)
            {
                foreach (string file in fileList)
                {
                    if (file.Contains("<DIR>")) //if this is a dir need recursion download
                    {
                        temp = file.Trim().Split(new string[] { "<DIR>" }, StringSplitOptions.None);
                        directory.Add(temp[1].Trim());
                        temp = null;
                    }
                }
            }
            else
            {
                return false;
            }
            FTPExtensions.sFTPServerIP = "127.0.0.1";
            FTPExtensions.sDirName = "";
            FTPExtensions.sFromFileName = "成績單2.csv";
            if (directory.Count > 0 && directory.Count < fileList.Length)
            {
                foreach (string fi in files)
                {
                    if (!directory.Contains(fi.Trim()))
                    {

                        FTPExtensions.FTPDownloadFile();
                    }
                }
            }
            else if (directory.Count == 0 && fileList.Length > 0)
            {
                foreach (string fi in files)
                {
                    FTPExtensions.FTPDownloadFile();
                }
            }

            if (directory.Count > 0)
            {
                foreach (string dir in directory)
                {
                    FileStruct f = new FileStruct();
                    f = ftpDirInfo.GetList(dirName);
                    DownloadDirectory(ftpAddress + "/", dir, downloadDir);
                }
            }
            return true;
        }

        public bool DownloadFileFromFtp(string FtpAdd, string fileName, string localDir)
        {
            return true;
        }
    }
}
