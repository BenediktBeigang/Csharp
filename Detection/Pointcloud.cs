using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Detection {
    static class Pointcloud {
        public static int LineOrientation { get; set; } = -5;
        public static Point[] Cloud { get; set; } = new Point[0];

        public static Point[] TransformCloud(Point[] pc, double scale, double dx, double dy) {
            Point[] transformedCloud = new Point[pc.Length];
            Array.Copy(pc, transformedCloud, pc.Length);

            double scaleValue = Math.Pow(10, scale);
            for (int i = 0; i < transformedCloud.Length; i++) {
                Point p = transformedCloud[i];
                p.X = (p.X + dx) * scaleValue;
                p.Y = (p.Y + dy) * scaleValue;
                transformedCloud[i] = p;
            }
            return transformedCloud;
        }

        public static Point[] ShortenCloud(Point[] points, double percentage) {
            List<Point> newPoints = new List<Point>();
            Random rand = new Random();

            for (int i = 0; i < points.Length * percentage; i++) {
                newPoints.Add(points[rand.Next(0, points.Length)]);
            }
            return newPoints.ToArray();
        }

        public static void PrintCloud(Point[] pc) {
            Console.WriteLine(GetFormatetCloud(pc));
            Console.WriteLine("\n--------------------------------------------------");
        }

        public static string GetFormatetCloud(Point[] pc) {
            string output = "";
            for (int i = 0; i < pc.Length; i++) {
                output += pc[i].X + " " + pc[i].Y + " ";
            }
            return output;
        }

        public static Point[] DeletePoints(Point[] cloud, Point[] pointsToDelete) {
            List<Point> cloudList = cloud.ToList();
            foreach (Point p in pointsToDelete) {
                cloudList.Remove(p);
            }
            return cloudList.ToArray();
        }

        public static float[] GetCloudAsArray(Point[] pc) {
            float[] output = new float[pc.Length * 2];
            for (int i = 0; i < pc.Length; i++) {
                output[i * 2] = (float)pc[i].X;
                output[(i * 2) + 1] = (float)pc[i].Y;
            }
            return output;
        }

    }
}
