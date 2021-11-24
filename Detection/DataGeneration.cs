using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Detection {
    static class DataGeneration {
        private static readonly Random rand = new Random();

        public static void LineCornerGeneration(int dataCount, double noisePercentage) {
            string[] table = new string[dataCount];
            table[0] = EmptyPointString();

            for (int i = 1; i < dataCount; i++) {

                int primitve = rand.Next(3);
                List<Point> cloud = new List<Point>();
                int pointCount = 10 + rand.Next(30);

                switch (primitve) {
                    case 0:
                        cloud.AddRange(NextLinePointCloud(pointCount, noisePercentage));
                        break;
                    case 1:
                        cloud.AddRange(NextCornerPointCloud(pointCount / 2, noisePercentage));
                        break;
                    case 2:
                        cloud.AddRange(NextMultiplePrimitves(pointCount, noisePercentage));
                        break;
                    default:
                        break;
                }

                string line = primitve.ToString() + ";";
                foreach (Point p in cloud) {
                    string x = ((int)Math.Round(p.X)).ToString();
                    string y = ((int)Math.Round(p.Y)).ToString();

                    line += x + ";" + y + ";";
                }
                for (int p = 0; p < (50 - cloud.Count); p++) {
                    line += "-1;-1;";
                }
                table[i] = line;
            }

            string folder = @"C:\Users\bened\Desktop\Clouddata\";
            string fileName = "primitiveData.csv";
            string fullPath = folder + fileName;
            File.WriteAllLines(fullPath, table);
        }

        public static void LineOrientationGeneration(int dataCount) {
            string[] table = new string[dataCount];

            for (int i = 0; i < dataCount; i++) {
                Point[] points = NextLinePointCloudWithOrientation();
                table[i] += Pointcloud.LineOrientation + ";";

                string pointString = "";
                for (int j = 0; j < points.Length; j++) {
                    pointString += points[j].X + ";" + points[j].Y + ";";
                }
                table[i] += pointString;
            }

            string folder = @"C:\Users\bened\Desktop\Clouddata\";
            string fileName = "data.csv";
            string fullPath = folder + fileName;
            File.WriteAllLines(fullPath, table);
        }

        public static Point[] NextMultiplePrimitves(int pointCount, double noisePercentage) {
            int primitveCount = 2 + rand.Next(2);
            List<Point> outputCloud = new List<Point>();
            int primitivePointCount = pointCount / primitveCount;

            for (int i = 0; i < primitveCount; i++) {                
                int primitve = rand.Next(2);
                if (primitve == 0){
                    outputCloud.AddRange(NextLinePointCloud(primitivePointCount, noisePercentage));
                } else if (primitve == 1) {
                    outputCloud.AddRange(NextCornerPointCloud(primitivePointCount / 2, noisePercentage));
                }
            }
            return outputCloud.ToArray();
        }

        public static Point[] NextNoiseCloud(int pointCount) {
            Point[] cloud = new Point[pointCount + 2];

            for (int i = 0; i < pointCount; i++) {
                int x = rand.Next(500);
                int y = rand.Next(500);
                cloud[i] = new Point(x, y);
            }

            return cloud;
        }

        public static Point[] NextLinePointCloud(int pointCount, double noisePercentage) {
            Point p1 = PointFromDouble(rand.Next(0, 2000));
            Point p2 = PointFromDouble(rand.Next(0, 2000));

            Vector line = VectorMath.ConnectionVector(p1, p2);

            Point[] cloud = new Point[pointCount + 2];
            double pointDistanceScalar = (double)1 / (pointCount + 1);

            cloud[0] = p1;
            for (int i = 1; i < pointCount + 1; i++) {

                // Creation of Points on line
                double scalar = i * pointDistanceScalar;
                Vector scaledVector = Vector.Multiply(scalar, line);
                scaledVector.X = (int)Math.Round(scaledVector.X);
                scaledVector.Y = (int)Math.Round(scaledVector.Y);
                cloud[i] = Point.Add(p1, scaledVector);

                // Shift on horizontal Axis
                int finalNoise = (int)Math.Round(line.Length * noisePercentage);
                cloud[i] = AddNoise(cloud[i], finalNoise);
            }
            cloud[pointCount + 1] = p2;
            return cloud;
        }

        public static Point[] NextCornerPointCloud(int pointCountPerVector, double noisePercentage) {
            Tuple<Point, Point, Point> points = CreateCornerPoints();

            Point p1 = points.Item1;
            Point p2 = points.Item2;
            Point p3 = points.Item3;

            Vector firstVector = VectorMath.ConnectionVector(p1, p2);
            Vector secondVector = VectorMath.ConnectionVector(p2, p3);

            int pointCount = (pointCountPerVector * 2) + 3;
            Point[] cloud = new Point[pointCount];
            double pointDistanceScalar = (double)1 / (pointCountPerVector + 1);

            cloud[0] = p1;
            for (int i = 1; i < pointCountPerVector + 1; i++) {
                double scalar = i * pointDistanceScalar;
                Vector scaledVector = Vector.Multiply(scalar, firstVector);
                cloud[i] = Vector.Add(scaledVector, p1);

                int finalNoise = (int)Math.Round(firstVector.Length * noisePercentage);
                cloud[i] = AddNoise(cloud[i], finalNoise);
            }
            cloud[pointCountPerVector + 1] = p2;
            for (int i = pointCountPerVector + 2; i < pointCount - 1; i++) {
                double scalar = (i % (pointCountPerVector + 1)) * pointDistanceScalar;
                Vector scaledVector = Vector.Multiply(scalar, secondVector);
                cloud[i] = Vector.Add(scaledVector, p2);

                int finalNoise = (int)Math.Round(secondVector.Length * noisePercentage);
                cloud[i] = AddNoise(cloud[i], finalNoise);
            }
            cloud[pointCount - 1] = p3;

            return cloud;
        }

        private static Tuple<Point, Point, Point> CreateCornerPoints() {
            Tuple<Point, Point, Point> outputPoints;

            double[] lengths = new double[3];

            do {
                Point p1 = PointFromDouble(rand.Next(0, 2000));
                Point p2 = new Point(rand.Next(0, 500), rand.Next(0, 500));
                Point p3 = PointFromDouble(rand.Next(0, 2000));
                outputPoints = new Tuple<Point, Point, Point>(p1, p2, p3);

                Vector firstVector = VectorMath.ConnectionVector(p1, p2);
                Vector secondVector = VectorMath.ConnectionVector(p2, p3);
                Vector thirdVector = VectorMath.ConnectionVector(p1, p3);

                lengths[0] = VectorMath.VectorLength(firstVector);
                lengths[1] = VectorMath.VectorLength(secondVector);
                lengths[2] = VectorMath.VectorLength(thirdVector);
                Array.Sort(lengths);

            } while (lengths[2] - lengths[0] > 150 || IsUnderMinimumDistance(lengths));

            return outputPoints;
        }

        public static Point[] NextLinePointCloudWithOrientation() {
            Pointcloud.LineOrientation = rand.Next(0, 7);
            Point p1;
            Point p2;

            switch (Pointcloud.LineOrientation) {
                case 0: //vertical
                    p1 = new Point(NextVal(), 0);
                    p2 = new Point(NextVal(), 500);
                    break;
                case 1: //horizontal
                    p1 = new Point(0, NextVal());
                    p2 = new Point(500, NextVal());
                    break;
                case 2: //left_down
                    p1 = new Point(0, NextVal());
                    p2 = new Point(NextVal(), 500);
                    break;
                case 3: //right_down
                    p1 = new Point(NextVal(), 500);
                    p2 = new Point(500, NextVal());
                    break;
                case 4: //right_top
                    p1 = new Point(500, NextVal());
                    p2 = new Point(NextVal(), 0);
                    break;
                case 5: //left_top
                    p1 = new Point(NextVal(), 0);
                    p2 = new Point(0, NextVal());
                    break;
                default: //left_down
                    p1 = new Point(NextVal(), 0);
                    p2 = new Point(NextVal(), 500);
                    Pointcloud.LineOrientation = 0;
                    break;
            }

            Vector line = VectorMath.ConnectionVector(p1, p2);

            Point[] cloud = new Point[11];
            for (int i = 0; i < 11; i++) {
                // Creation of Points on line
                double skalar = i / (double)10;
                Vector scaledVector = Vector.Multiply(skalar, line);
                scaledVector.X = (int)Math.Round(scaledVector.X);
                scaledVector.Y = (int)Math.Round(scaledVector.Y);
                cloud[i] = Point.Add(p1, scaledVector);


                // Shift on horizontal Axis
                int noise = (int)Math.Round(line.Length * 0.05);
                cloud[i] = AddNoise(cloud[i], noise);
            }
            return cloud;
        }

        private static bool IsUnderMinimumDistance(double[] lengths) {
            double value = 50;
            return lengths[0] < value || lengths[1] < value || lengths[2] < value;
        }

        private static Point AddNoise(Point p, int noise) {
            int shiftValueX = rand.Next(-noise, noise);
            int shiftValueY = rand.Next(-noise, noise);

            double newValX = p.X;
            double newValY = p.Y;

            if (newValX + shiftValueX < 500 && newValX + shiftValueX > 0) {
                newValX += shiftValueX;
            }
            if (newValY + shiftValueY < 500 && newValY + shiftValueY > 0) {
                newValY += shiftValueY;
            }
            return new Point(newValX, newValY);
        }

        private static Point PointFromDouble(double val) {
            if (val < 500) {
                return new Point(0, val);
            }
            if (val < 1000) {
                return new Point(val - 500, 500);
            }
            if (val < 1500) {
                return new Point(500, val - 1000);
            }
            if (val < 2000) {
                return new Point(val - 1500, 0);
            }
            return new Point(0, 0);
        }

        public static double NextVal() {
            return rand.Next(0, 500);
        }

        public static string EmptyPointString() {
            string output = "-1;";
            for (int i = 0; i < 50; i++) {
                output += "-1;-1;";
            }
            return output;
        }
    }
}
