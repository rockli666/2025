using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Cal
    {
        public List<POINT> points;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="points"></param>
        public Cal(List<POINT> points)
        {
            this.points = points;
        }
        /// <summary>
        /// 统计每个区的事件数
        /// </summary>
        public Dictionary<int, Area> CountEventsByArea(List<POINT> points)
        {
            return points.GroupBy(p => p.area_code)
                         .ToDictionary(g => g.Key, g => new Area
                         {
                             point = g.ToList(),      // 存入分组的点
                             area_count = g.Count(),   // 统计该区域的点数
                         });
        }
        /// <summary>
        /// 计算误差椭圆
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public string CalEllipse(List<POINT> points)
        {
            string text = "";
            double Xba = points.Average(p => p.X);
            double Yba =points.Average(p => p.Y);
            text+=$"Xba: {Xba:F2}, Yba: {Yba:F2}" + "\n";
            double A = points.Zip(points, (x, y) => Math.Pow(x.X - Xba, 2) - Math.Pow(y.Y - Yba, 2)).Sum();
            double sumin = points.Zip(points, (x, y) => (x.X - Xba) * (y.Y - Yba)).Sum();
            double B = Math.Sqrt(A * A + 4 * sumin * sumin);
            double C = 2 * sumin;
            return text;
        }
        /// <summary>
        /// 计算权重矩阵
        /// </summary>
        /// <param name="points"></param>
        /// <param name="areap"></param>
        /// <returns></returns>
        public static double[,] CalW(List<POINT> points,ref Dictionary<int,Area>areap)
        {
            double[,] W = new double[7, 7];
            foreach (var kvp in areap)
            {
                kvp.Value.Xj = kvp.Value.point.Average(p => p.X); // 区域点的 X 坐标平均值
                kvp.Value.Yj = kvp.Value.point.Average(p => p.Y); // 区域点的 Y 坐标平均值
            }

                for(int i=0;i<7;i++)
                    for(int j=0;j<7;j++)
                {
                    if (i == j)
                        W[i, j] = 0;
                    else
                    {
                        // 计算两点之间的距离
                        double dx = areap[i+1].Xj - areap[j+1].Xj;
                        double dy = areap[i + 1].Yj - areap[j + 1].Yj;
                        double distance = Math.Sqrt(dx * dx + dy * dy);
                        W[i, j] = 1000.0 / distance;
                    }
                }
                return W;
        }
        /// <summary>
        /// 计算莫兰指数
        /// </summary>
        /// <param name="W"></param>
        /// <param name="points"></param>
        /// <param name="areap"></param>
        /// <returns></returns>
        public double CalMoran(double[,]W,List<POINT>points,Dictionary<int, Area> areap)
        {
            double Moran = 0;
            double Xba = points.Count() / 7.0;

            double S0 = 0;
            double down = 0;
            double on = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    S0 += W[i, j];
                    on += W[i, j] * (areap[i+1].area_count - Xba) * (areap[j + 1].area_count - Xba);
                }
                down += Math.Pow((areap[i + 1].area_count - Xba), 2);
            }
            Moran = 7.0 * on / (S0 * down);

            return Moran;
        }
        /// <summary>
        /// 计算局部莫兰指数
        /// </summary>
        /// <param name="w"></param>
        /// <param name="points"></param>
        /// <param name="areap"></param>
        public static void CalI(double[,] w, List<POINT> points,ref Dictionary<int, Area> areap)
        {
            double valueba = points.Count() / 7.0;
            for (int i = 0; i < 7; i++)
            {
                double on = areap[i+1].area_count - valueba;
                double Si2 = 0;
                double on2 = 0;
                for (int j = 0; j < 7; j++)
                {
                    if (i != j)
                    {
                        Si2 += Math.Pow((areap[j + 1].area_count - valueba), 2) / (7 - 1);
                        on2 += w[i, j] * (areap[j + 1].area_count - valueba);
                    }
                }
                double I = on * on2 / Si2;
                areap[i + 1].Moran = I;
            }
        }
        /// <summary>
        /// 计算Z得分
        /// </summary>
        /// <param name="points"></param>
        /// <param name="areap"></param>
        public static void CalZ(List<POINT> points, ref Dictionary<int, Area> areap)
        {
            double u = areap.Average(p => p.Value.Z);
            double sigma = 0;

            for (int j = 0; j < 7; j++)
            {
                sigma += Math.Pow(areap[j+1].Moran - u, 2);
            }
            sigma = Math.Sqrt(sigma / (7 - 1));
            for (int i = 0; i < 7; i++)
            {
                double z = (areap[i + 1].Moran - u) / sigma;
                areap[i + 1].Z = z;
            }
        }
    }
}
