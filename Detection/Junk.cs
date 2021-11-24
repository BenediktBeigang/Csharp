using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Detection {
    class Junk {
        
        public static Tuple<Point, Point> LineParser(Point[] cloud, System.Windows.Controls.TextBlock consoleTextBlock) {
            int p1 = 0;
            int p2 = 1;
            double currentMaxDistance = 0;

            List<Point> shortCloudList = new List<Point>();
            for (int i = 0; i < cloud.Length; i++) {
                if (IsBorderPoint(cloud[i], 30)) {
                    shortCloudList.Add(cloud[i]);
                }
            }
            Point[] shortCloud = shortCloudList.ToArray();

            for (int i = 0; i < shortCloud.Length - 1; i++) {
                for (int j = i + 1; j < shortCloud.Length; j++) {
                    Vector connection = VectorMath.ConnectionVector(shortCloud[i], shortCloud[j]);
                    double distance = VectorMath.VectorLength(connection);
                    if (distance > currentMaxDistance) {
                        p1 = i;
                        p2 = j;
                        currentMaxDistance = distance;
                    }
                }
            }

            consoleTextBlock.Text = shortCloud[p1] + "\n" + shortCloud[p2] + "\n";
            return new Tuple<Point, Point>(shortCloud[p1], shortCloud[p2]);
        }

        private static bool IsBorderPoint(Point p, int borderArea) {
            if (p.X < borderArea || p.X > 500 - borderArea || p.Y < borderArea || p.Y > 500 - borderArea) {
                return true;
            }
            return false;
        }

        public static Tuple<Point, Point, Point> CornerParser(Point[] cloud, System.Windows.Controls.TextBlock consoleTextBlock) {
            Tuple<Point, Point> edgePoints = LineParser(cloud, consoleTextBlock);
            Point[] shortCloud = Pointcloud.DeletePoints(cloud, new Point[] { edgePoints.Item1, edgePoints.Item2 });
            Vector2D mainAxis = new Vector2D(edgePoints.Item1, VectorMath.ConnectionVector(edgePoints.Item1, edgePoints.Item2));

            int cornerPointIndex = 0;

            double minSumOfNormalLengths = double.MaxValue;

            for (int i = 0; i < shortCloud.Length; i++) {
                Vector2D vec1 = new Vector2D(edgePoints.Item1, VectorMath.ConnectionVector(edgePoints.Item1, shortCloud[i]));
                Vector2D vec2 = new Vector2D(shortCloud[i], VectorMath.ConnectionVector(shortCloud[i], edgePoints.Item2));

                double sumOfNormalLengths = 0;
                for (int p = 0; p < i; p++) {
                    sumOfNormalLengths += LengthFromPointToAxis(shortCloud[p], vec1);
                }
                for (int p = i + 1; p < shortCloud.Length; p++) {
                    sumOfNormalLengths += LengthFromPointToAxis(shortCloud[p], vec2);
                }
                consoleTextBlock.Text += (int)shortCloud[i].X + "|" + (int)shortCloud[i].Y + " ||| " + sumOfNormalLengths + "\n";
                if (sumOfNormalLengths < minSumOfNormalLengths) {
                    minSumOfNormalLengths = sumOfNormalLengths;
                    cornerPointIndex = i;
                }
            }

            consoleTextBlock.Text += "\n\n" + minSumOfNormalLengths + "\n";
            return new Tuple<Point, Point, Point>(edgePoints.Item1, shortCloud[cornerPointIndex], edgePoints.Item2);
        }

        private static double LengthFromPointToAxis(Point point, Vector2D vec1) {
            throw new NotImplementedException();
        }
    }
}
