using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";
            // 建立一個OpenFileDialog物件
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // 設定OpenFileDialog屬性
            openFileDialog1.Title = "選擇要開啟的CSV檔案";
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // 喚用ShowDialog方法，打開對話方塊

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {                
                string theFile = openFileDialog1.FileName; //取得檔名
                Encoding enc = Encoding.GetEncoding("big5"); //設定檔案的編碼
                string[] readText = System.IO.File.ReadAllLines(theFile, enc); //以指定的編碼方式讀取檔案
                foreach (string s in readText)
                {
                    tbResult.Text += s + "\r\n";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";
            // 建立一個OpenFileDialog物件
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // 設定OpenFileDialog屬性
            openFileDialog1.Title = "選擇要開啟的CSV檔案";
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // 喚用ShowDialog方法，打開對話方塊

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string theFile = openFileDialog1.FileName; //取得檔名
                Encoding enc = Encoding.GetEncoding("big5"); //設定檔案的編碼
                string[] readText = System.IO.File.ReadAllLines(theFile, enc); //以指定的編碼方式讀取檔案
                foreach (string s in readText)
                {
                    string[] ss = s.Split(',');
                    tbResult.Text += ss[0] + "  " + ss[1] + "  " + ss[2] + "  " + ss[3] + "\r\n";
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";
            Encoding enc = Encoding.GetEncoding("big5"); //設定檔案的編碼
            string[] lines = File.ReadAllLines(@"D:\成績檔.csv", enc);

            var query = lines.SkipWhile(x => x.Split(',')[0] != textBox1.Text).TakeWhile(x => x.Split(',')[0] == textBox1.Text || x.Split(',')[0] == "").Skip(1);
//            var query = lines.SkipWhile(x => x.Split(',')[0] != "").TakeWhile(x => x.Split(',')[0] == textBox1.Text || x.Split(',')[0] == "").Skip(1);
            foreach (var item in query){
                tbResult.Text += item ;
            }
        }
    }
}
