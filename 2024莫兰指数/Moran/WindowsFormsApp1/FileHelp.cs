using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace WindowsFormsApp1
{
    class FileHelp
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static List<POINT> FileRead(string path, out string fileContent)
        {
            List<POINT> points = new List<POINT>();
            fileContent = File.ReadAllText(path);
            StreamReader re = new StreamReader(path);
            re.ReadLine();
            while(!re.EndOfStream)
            {
                string line = re.ReadLine();
                string[] lines = line.Split(',');
                POINT p = new POINT
                {
                    ID = lines[0],
                    X = double.Parse(lines[1]),
                    Y = double.Parse(lines[2]),
                    area_code = int.Parse(lines[3])       
                 };
                points.Add(p);
            }
            return points;
        }
    }
}
