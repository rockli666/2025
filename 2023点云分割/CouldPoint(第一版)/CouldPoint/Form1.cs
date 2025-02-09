using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouldPoint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Cal cal;
        List<Ceil> ceils;
        List<Area> areasJ1;
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                string text;
                List<POINT> points = new List<POINT>();
                points = FileHelp.ReadFile(op.FileName, out text);
                cal = new Cal(points);
                richTextBox1.Text = text;

                POINT P=cal.points.Find(p => p.ID == "P5");
                richTextBox2.Text = $"P5 的坐标分量 x：{P.X:F3}"+"\n";
                richTextBox2.Text += $"P5 的坐标分量 y：{P.Y:F3}" + "\n";
                richTextBox2.Text += $"P5 的坐标分量 z：{P.Z:F3}" + "\n";

                double temp = cal.points.Min(p => p.X);
                richTextBox2.Text += $"坐标分量x的最小值：{temp:F3}" + "\n";
                temp = cal.points.Max(p => p.X);
                richTextBox2.Text += $"坐标分量x的最大值 xmax：{temp:F3}" + "\n";
                temp = cal.points.Min(p => p.Y);
                richTextBox2.Text += $"坐标分量y的最小值 ymin：{temp:F3}" + "\n";
                temp = cal.points.Max(p => p.Y);
                richTextBox2.Text += $"坐标分量y的最大值 ymax：{temp:F3}" + "\n";
                temp = cal.points.Min(p => p.Z);
                richTextBox2.Text += $"坐标分量z的最小值 zmin：{temp:F3}" + "\n";
                temp = cal.points.Max(p => p.Z);
                richTextBox2.Text +=$"坐标分量z的最大值 zmax：{temp:F3}" + "\n";
            }

        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ceils = cal.Calij(ref cal.points);
            POINT P = cal.points.Find(p => p.ID == "P5");
            richTextBox2.Text += $"P5 点的所在栅格的行i：{P.i:F3}" + "\n";
            richTextBox2.Text += $"P5 点的所在栅格的行j：{P.j:F3}" + "\n";

            Ceil ceil = ceils.Find(p => p.i == 3&&p.j==2);
            richTextBox2.Text += $"栅格C 中的点的数量：{ceil.points.Count()}" + "\n";

            cal.CalCeil(ref ceils);
            ceil = ceils.Find(p => p.i == 3 && p.j == 2);
            richTextBox2.Text += $"栅格C 中的平均高度：{ceil.high:f3}" + "\n";
            richTextBox2.Text += $"栅格C 中高度的最大值：{ceil.points.Max(p=>p.Z):f3}" + "\n";
            richTextBox2.Text += $"栅格C 中的高度差：{ceil.Z:f3}" + "\n";
            richTextBox2.Text += $"栅格C 中的高度方差：{ceil.sigma:f3}" + "\n";

            double s = 0;
            cal.OnFace(cal.points[0], cal.points[1], cal.points[2], out s);
            richTextBox2.Text += $"P1-P2-P3 构成三角形的面积：{s:f6}" + "\n";
            string text = "";
            Area area = cal.CalArea(cal.points, cal.points[0], cal.points[1], cal.points[2],ref text);
            richTextBox2.Text += $"拟合平面S1 的参数A：{area.A:f6}" + "\n";
            richTextBox2.Text += $"拟合平面S1 的参数B：{area.B:f6}" + "\n";
            richTextBox2.Text += $"拟合平面S1 的参数C：{area.C:f6}" + "\n";
            richTextBox2.Text += $"拟合平面S1 的参数D：{area.D:f6}" + "\n";
            richTextBox2.Text += text;
            richTextBox2.Text += $"拟合平面S1 的内部点数量：{area.inarea.Count()}" + "\n";
            richTextBox2.Text += $"拟合平面S1 的外部点数量：{area.outarea.Count()}" + "\n";
            areasJ1 = new List<Area>();
                for (int i = 0; i < 900; i=i+3)
                {
                    double a;
                    if(!cal.OnFace(cal.points[i], cal.points[i + 1], cal.points[i + 2], out a))
                    {
                     string text1="";
                     Area area0 = cal.CalArea(cal.points, cal.points[i], cal.points[i+1], cal.points[i+2], ref text1);
                    area0.p1 = cal.points[i];
                    area0.p2 = cal.points[i+1];
                    area0.p3 = cal.points[i+2];
                    areasJ1.Add(area0);
                    }
                }
                //找J1
            double max = 0;
            Area J1=new Area();
            foreach (Area areat in areasJ1)
            {
                if(areat.inarea.Count()>max)
                {
                    max = areat.inarea.Count();
                    J1 = areat;
                }
            }
            richTextBox2.Text += $"最佳分割平面J1 的参数A：{J1.A:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J1 的参数B：{J1.B:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J1 的参数C：{J1.C:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J1 的参数D：{J1.D:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J1 的内部点数量：{J1.inarea.Count()}" + "\n";
            richTextBox2.Text += $"最佳分割平面J1 的外部点数量：{J1.outarea.Count()}" + "\n";
            
            foreach(POINT p in cal.points)
            {
                foreach (POINT p1 in J1.inarea)
                    if (p1.ID == p.ID)
                        p.J = "J1";
            }//分区后结果

            List<Area> AreaJ2 = new List<Area>();
            List<POINT> J1out = J1.outarea;
            for (int i = 0; i < 240; i = i + 3)
            {
                double a;
                if (!cal.OnFace(J1out[i], J1out[i + 1], J1out[i + 2], out a))
                {
                    string text1 = "";
                    Area area0 = cal.CalArea(J1out, J1out[i], J1out[i + 1], J1out[i + 2], ref text1);
                    area0.p1 = J1out[i];
                    area0.p2 = J1out[i + 1];
                    area0.p3 = J1out[i + 2];
                    AreaJ2.Add(area0);
                }
            }

            //找J2
            max = 0;
            Area J2 = new Area();
            foreach (Area areat in AreaJ2)
            {
                if (areat.inarea.Count() > max)
                {
                    max = areat.inarea.Count();
                    J2 = areat;
                }
            }
            richTextBox2.Text += $"最佳分割平面J2 的参数A：{J2.A:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J2 的参数B：{J2.B:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J2 的参数C：{J2.C:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J2 的参数D：{J2.D:f6}" + "\n";
            richTextBox2.Text += $"最佳分割平面J2 的内部点数量：{J2.inarea.Count()}" + "\n";
            richTextBox2.Text += $"最佳分割平面J2 的外部点数量：{J2.outarea.Count()}" + "\n";
        }
    }
}
