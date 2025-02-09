using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouldPoint
{
    public class Cal
    {
        public List<POINT> points;

        public Cal(List<POINT> points)
        {
            this.points = points;
        }
        /// <summary>
        /// 计算行列
        /// </summary>
        /// <param name="points"></param>
        public List<Ceil> Calij(ref List<POINT> points)
        {
            List<Ceil> ceils = new List<Ceil>();
            foreach(POINT p in points)
            {
                bool get = true;
                p.i = Math.Floor(p.Y / 10.0);
                p.j = Math.Floor(p.X / 10.0);
                //在已有区域上添加点
                foreach (Ceil c in ceils)
                {
                    if (c.i == p.i && c.j == p.j)
                    {
                        c.points.Add(p);
                        get = false;
                        continue;
                    }
                }
                //增加没有的区域
                if (get)
                {
                    Ceil ceil = new Ceil
                    {
                        i = p.i,
                        j = p.j,
                    };
                    ceil.points.Add(p);
                    ceils.Add(ceil);
                }
            }
            return ceils;
        }
        /// <summary>
        /// 计算栅格单元的高度方差
        /// </summary>
        /// <param name="ceils"></param>
        public void CalCeil(ref List<Ceil> ceils)
        {
            foreach(Ceil ceil in ceils)
            {
                ceil.high = 1.0 / ceil.points.Count() * ceil.points.Sum(p=>p.Z);
                ceil.Z = ceil.points.Max(p => p.Z) - ceil.points.Min(p => p.Z);
                ceil.sigma = 1.0 / ceil.points.Count() * ceil.points.Sum(p=>Math.Pow(p.Z - ceil.high, 2));
            }
        }
        /// <summary>
        /// 平面拟合和内部点外部点计算
        /// </summary>
        /// <param name="poins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public Area CalArea(List<POINT>points, POINT p1,POINT p2,POINT p3,ref string reslut)
        {
            List<POINT> inp = new List<POINT>();
            List<POINT> outp = new List<POINT>();

            double A = (p2.Y - p1.Y) * (p3.Z - p1.Z) - (p3.Y - p1.Y) * (p2.Z - p1.Z);
            double B = (p2.Z - p1.Z) * (p3.X - p1.X) - (p3.Z - p1.Z) * (p2.X - p1.X);
            double C = (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
            double D = -1 * A * p1.X - B * p1.Y - C * p1.Z;
  
            //计算内部外部点
            foreach(POINT p in points)
            {
                if(p.ID!=p1.ID&& p.ID != p2.ID&& p.ID != p3.ID)
                {
                    double d = Math.Abs(A * p.X + B * p.Y + C * p.Z + D)/Math.Sqrt(A*A+B*B+C*C);
                    if (d < 0.1)
                        inp.Add(p);
                    else
                        outp.Add(p);

                    if (p1.ID == "P1" && (p.ID == "P1000" || p.ID == "P5"))
                        reslut += $"{p.ID}到拟合平面S1 的距离{d:f3}"+"\n";
                }
            }
            //初始化
            Area area = new Area
            {
                A = A,
                B = B,
                C = C,
                D = D,
                inarea = inp,
                outarea =outp
            };
            return area;
        }
        /// <summary>
        /// 判断共线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public bool OnFace(POINT p1,POINT p2,POINT p3,out double s)
        {
            bool reslut = false;
            double a = dis(p1, p2);
            double b = dis(p2, p3);
            double c = dis(p3, p1);

            double p = (a + b + c) / 2;

            double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            s = S;
            if (S <= 0.1)
                reslut = true;
            return reslut;
        }
        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double dis(POINT p1,POINT p2)
        {
            double dis = 0;
            dis = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            return dis;
        }
    }
}
