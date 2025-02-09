using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CouldPoint
{
    internal class FileHelp
    {
        public static List<POINT> ReadFile(string path,out string content)
        {
            content = File.ReadAllText(path);
            List<POINT> points = new List<POINT>();

            StreamReader re = new StreamReader(path);
            re.ReadLine();
            while(!re.EndOfStream)
            {
               string text = re.ReadLine();
                string[] texts = text.Split(',');
                POINT p = new POINT
                {
                    ID = texts[0],
                    X = double.Parse(texts[1]),
                    Y = double.Parse(texts[2]),
                    Z = double.Parse(texts[3])
                };
                points.Add(p);
            }

            return points;
        }
    }
}
