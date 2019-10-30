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
        struct DirectoryItem
        {
            public Uri BaseUri;

            public string AbsolutePath
            {
                get
                {
                    return string.Format("{0}/{1}", BaseUri, Name);
                }
            }

            public DateTime DateCreated;
            public bool IsDirectory;
            public string Name;
            public List<DirectoryItem> Items;
        }/*
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
        }/*
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
                }
                
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
                MessageBox.Show(ex.ToString(),"刪除過程異常");
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
            private List<FileStruct> GetList(string datastring)
            {
                List<FileStruct> myListArray = new List<FileStruct>();
                string[] dataRecords = datastring.Split('\n');
                FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
                foreach (string s in dataRecords)
                {
                    if (_directoryListStyle != FileListStyle.Unknown && s != "")
                    {
                        FileStruct f = new FileStruct();
                        f.Name = "..";
                        switch (_directoryListStyle)
                        {
                            case FileListStyle.UnixStyle:
                                f = ParseFileStructFromUnixStyleRecord(s);
                                break;
                            case FileListStyle.WindowsStyle:
                                f = ParseFileStructFromWindowsStyleRecord(s);
                                break;
                        }
                        if (!(f.Name == "." || f.Name == ".."))
                        {
                            myListArray.Add(f);
                        }
                    }
                }
                return myListArray;
            }
            public FileListStyle GuessFileListStyle(string[] recordList)
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
            public FileStruct ParseFileStructFromUnixStyleRecord(string Record)
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
            public FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
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
            public string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
            {
                int pos1 = s.IndexOf(c, startIndex);
                string retString = s.Substring(0, pos1);
                s = (s.Substring(pos1)).Trim();
                return retString;
            }
            public List<FileStruct> ListFilesAndDirectories(string ftpUri)
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
                return GetList(Datastring);;
            }
            public void DownFtpDir(string ftpDir, string saveDir)
            {
                List<FileStruct> files = ListFilesAndDirectories(ftpDir);
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }
                foreach (FileStruct f in files)
                {
                    if (f.IsDirectory) //文件夹，递归查询
                    {
                        DownFtpDir(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                    }
                    else //文件，直接下载
                    {
                        DownLoadFile(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                    }
                }
            }
        }
        */
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
            //string[] directory = GetDeleteFolderArray(txt_FileSearch.Text);
            //string[] directory = GetFtpFileList(txt_FileSearch.Text);
            List<string>  list = new List<string>();
            //string[] directory = GetFilesDetailList(txt_FileSearch.Text);
            List<orders> listOrders = new List<orders>();
            List<DirectoryItem> dir = new List<DirectoryItem>();
            //dir = GetDirectoryInformation("ftp://127.0.0.1", "Attlie", "12345");
            foreach (string strValue in list)
            {
                listOrders.Add(new orders(strValue));
                //list = GetAllFtpFiles(strValue);
            }
            //dataGridView1.DataSource = directory;
            dataGridView1.DataSource = listOrders;
        }
        private void Form1_Load(object sender, EventArgs e)
        {/*
            List<string> list = new List<string>();
            List<orders> ListShow = new List<orders>();
            NetworkCredential credentials = new NetworkCredential("Attlie", "12345");
            string url = "ftp://127.0.0.1/123/";
            string[] strs = GetFiles(url, credentials, SearchOption.AllDirectories);
            foreach (string str in strs)
                ListShow.Add(new orders(str));
            dataGridView1.DataSource = ListShow;
            //ListFtpDirectory(url, "", credentials, list);*/
            DownFtpDir(txt_FileSearch.Text,"D:\\456");
        }
        void ListFtpDirectory(string url, string rootPath, NetworkCredential credentials, List<string> list)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url + rootPath);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                string[] tokens =
                    line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[8];
                string permissions = tokens[0];

                string filePath = rootPath + name;

                if (permissions[0] == 'd')
                {
                    ListFtpDirectory(url, filePath + "/", credentials, list);
                }
                else
                {
                    list.Add(filePath);
                }
            }
            dataGridView1.DataSource = list;
        }
        public static string[] GetFiles(string path, NetworkCredential Credentials, SearchOption searchOption)
        {
            var request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = Credentials;
            List<string> files = new List<string>();
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new System.IO.StreamReader(responseStream);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line) == false)
                        {
                            if (!line.Contains("<DIR>"))
                            {
                                string[] details = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                string file = line.Replace(details[0], "")
                                    .Replace(details[1], "")
                                    .Replace(details[2], "")
                                    .Trim();
                                files.Add(file);
                            }
                            else
                            {
                                if (searchOption == SearchOption.AllDirectories)
                                {
                                    string dirName = line.Split(
                                            new string[] { "<DIR>" },
                                            StringSplitOptions.RemoveEmptyEntries
                                            ).Last().Trim();
                                    string dirFullName = String.Format("{0}/{1}", path.Trim('/'), dirName);
                                    files.AddRange(GetFiles(dirFullName, Credentials, searchOption));
                                }
                            }
                        }
                    }
                }
            }
            return files.ToArray();
        }
        public List<FileStruct> ListFilesAndDirectories(string ftpUri)
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
                throw new Exception("獲取文件列表出錯，錯誤訊息如下：" + ex.ToString());
            }
            string Datastring = ftpFileListReader.ReadToEnd();
            return GetList(Datastring);
        }
        public void DownLoadFile(string downloadUrl, string saveFileUrl)
        {
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;

            try
            {
                // string downloadUrl = "ftp://192.168.1.104/capture-2.avi";

                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(downloadUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                //string ftpUser = "yoyo";
                //string ftpPassWord = "123456";
                downloadRequest.Credentials = new NetworkCredential("Attlie", "12345");

                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();

                fileStream = File.Create(saveFileUrl);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("从ftp服务器下载文件出错，文件名：" + downloadUrl + "异常信息：" + ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray;
        }
        public void DownFtpDir(string ftpDir, string saveDir)
        {
            List<FileStruct> files = ListFilesAndDirectories(ftpDir);
            List<orders> listShow = new List<orders>();
            /*
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }*/
            foreach (FileStruct f in files)
            {
                if (f.IsDirectory) //文件夹，递归查询
                {
                    DownFtpDir(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                    listShow.Add(new orders(ftpDir + "/" + f.Name));
                }
                else //文件，直接下载
                {
                    DownLoadFile(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                    listShow.Add(new orders(ftpDir + "/" + f.Name));
                }
            }
            dataGridView1.DataSource = listShow;
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
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
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
        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
