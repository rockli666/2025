using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouldPoint
{
    public class POINT
    {
        public string ID;
        public double X;
        public double Y;
        public double Z;
        public double i;
        public double j;
        public string J="0";
    }
    
    public class Ceil
    {
        public double i;
        public double j;
        public List<POINT> points=new List<POINT>();
        public double high;
        public double Z;
        public double sigma;
    }

    public class Area
    {
        public POINT p1;
        public POINT p2;
        public POINT p3;
        public double A, B, C, D;
        public List<POINT> inarea = new List<POINT>();
        public List<POINT> outarea = new List<POINT>();
    }
}
