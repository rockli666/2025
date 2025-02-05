using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 所需要数据
        /// </summary>
        Cal cal;
        string textContent;
        string Correct="";
        double[,] W; 

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<POINT> points = FileHelp.FileRead(op.FileName,out textContent);
                    richTextBox1.Text = textContent;
                    cal = new Cal(points);
                    //输出结果
                    POINT p6 = cal.points.FirstOrDefault(p => p.ID == "P6");
                    Correct += $"ID: {p6.ID}, X: {p6.X:F2}, Y: {p6.Y:F2}, Area Code: {p6.area_code}"+"\n";
                    richTextBox2.Text = Correct;
                }
                catch
                {
                    MessageBox.Show("文件打开失败");
                }
            }
        }

        private void 程序计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<int, Area> result = cal.CountEventsByArea(cal.points);
            W = Cal.CalW(cal.points,ref result);
            double Moran = cal.CalMoran(W, cal.points, result);
            richTextBox2.Text += Moran+"\n";
            Cal.CalI(W, cal.points, ref result);
            richTextBox2.Text += $"1区莫兰指数:{result[1].Moran}"+"\n";
            Cal.CalZ(cal.points, ref result);
            richTextBox2.Text += $"1区Z得分为:{result[1].Z}";
        }
    }
}
