using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ini_ConsoleApplication
{
    using System.Runtime.InteropServices;
    using System.IO;

    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string filepath;

        public IniFile(string fileName)
        {
            this.filepath = Path.GetFullPath(fileName);
        }

        public void WriteIni(string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, filepath);
        }

        public string ReadIni(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, filepath);
            return temp.ToString();
        }

        internal void w(string p1, string p2, string p3)
        {
            throw new NotImplementedException();
        }
    }
}