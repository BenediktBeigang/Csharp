using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Detection {
    static class Parser {
        private static List<Point> floorPlanCloud = new List<Point>();
        public static List<Point> FloorPlanCloud {
            get { return floorPlanCloud; }
            set { floorPlanCloud = value; }
        }

        public static void Parse(string filename) {
            floorPlanCloud.Clear();
            filename = "example_points";
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\bened\Meine Ablage\Code\C#\Detection\cloudData\" + filename + ".xyz");

            foreach (string line in lines) {
                string[] lineSegments = line.Split(' ');

                if (lineSegments.Length - CountEmpty(lineSegments) == 4) {
                    Point p = new Point(Convert.ToDouble(lineSegments[0], new System.Globalization.CultureInfo("en-US")), Convert.ToDouble(lineSegments[1], new System.Globalization.CultureInfo("en-US")));
                    floorPlanCloud.Add(p);
                }
            }
        }

        public static int CountEmpty(string[] str) {
            int count = 0;
            foreach (string element in str) {
                count = element == "" ? count + 1 : count;
            }
            return count;
        }

        public static void NormalizeCloud() {
            Point minPoint = VectorMath.MinMaxPoint(floorPlanCloud);
            minPoint.X *= -1;
            minPoint.Y *= -1;
            for (int i = 0; i < floorPlanCloud.Count; i++) {
                Point p = floorPlanCloud[i];
                p.X += minPoint.X;
                p.Y += minPoint.Y;
                floorPlanCloud[i] = p;
            }
        }

        private static void FilterCloud(double minX, double minY, double maxX, double maxY) {
            List<Point> newCloud = new List<Point>();
            foreach (Point p in floorPlanCloud) {
                if (p.X == 0 && p.Y == 0) {
                    Console.WriteLine("stop");
                }
                bool b1 = p.X >= minX;
                bool b2 = p.X < maxX;
                bool b3 = p.Y >= minY;
                bool b4 = p.Y < maxY;
                if (b1 && b2 && b3 && b4) {
                    newCloud.Add(p);
                }
            }
            floorPlanCloud = newCloud;
        }

        public static void SelectSector(int x, int y) {
            double minX = x * 0.05;
            double minY = y * 0.05;
            double maxX = (x * 0.05) + 0.05;
            double maxY = (y * 0.05) + 0.05;
            FilterCloud(minX, minY, maxX, maxY);
        }

        public static Point[] ToArray() {
            Point[] array = new Point[floorPlanCloud.Count];
            for (int i = 0; i < array.Length; i++) {
                array[i] = floorPlanCloud[i];
            }
            return array;
        }
    }
}
