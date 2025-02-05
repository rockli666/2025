using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
        class POINT
    {
        public string ID = "";
        public double X = 0;
        public double Y = 0;
        public int area_code = 0;
     }

        class Area
    {
        public List<POINT> point = new List<POINT>();
        public int area_count;
        public double Xj;
        public double Yj;
        public double Moran;
        public double Z;
    }
}
