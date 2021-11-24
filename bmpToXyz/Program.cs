using System;
using System.Collections.Generic;
using System.Drawing;

namespace bmpToXyz
{
    class Program
    {
        static public string currentFilename = "paralleleLinien_wide";

        static void Main(string[] args)
        {
            Console.WriteLine("Programm started!");
            SaveXYZ(LoadBitMap());
        }

        /*
         * This function loads a BitMap and converts it to a pointcloud
         */
        static private List<PointF> LoadBitMap(){
            List<PointF> points = new List<PointF>();
            string filename = currentFilename;
            string end = ".pbm";
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\bened\Meine Ablage\Code\C#\bmpToXyz\PBMs\" + filename + end);

            string[] widthHeightArray = lines[2].Split(' ');
            int width = Convert.ToInt32(widthHeightArray[0]);
            int height = Convert.ToInt32(widthHeightArray[1]);

            int currentWidth = 0;
            int currentHeight = 0;
            for(int i = 3; i < lines.Length; i++){
                for(int j = 0; j < 70; j++){ 
                                               
                    if(j < lines[i].Length && lines[i][j] == '1'){
                        points.Add(new PointF(currentWidth, currentHeight));
                    }
                    currentWidth++;

                    if(currentWidth >= width){
                        currentWidth = 0;
                        currentHeight++;
                    }
                }
                
            }

            return points;
        }

        /*
         * This function takes a pointcloud and writes them in a ".xyz"-File
         */
        static private void SaveXYZ(List<PointF> points){
            string[] lines = new string[points.Count];
            for(int i = 0; i < lines.Length; i++){
                lines[i] = points[i].X + " " + points[i].Y + " 0 255";
            }

            string filename = currentFilename;
            string end = ".xyz";
            System.IO.File.WriteAllLines(@"C:\Users\bened\Meine Ablage\Code\C#\bmpToXyz\xyz\" + filename + end, lines);
        }
    }
}
