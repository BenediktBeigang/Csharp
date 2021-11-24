using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Detection {
    static class VectorMath {

        public static Vector ConnectionVector(Point p1, Point p2) {
            return Point.Subtract(p2, p1);
        }

        public static double VectorLength(Vector v) {
            return Math.Sqrt((v.X * v.X) + (v.Y * v.Y));
        }

        public static double ScalarProduct(Point p1, Point p2) {
            return (p1.X * p2.X) + (p1.Y * p2.Y);
        }

        public static Vector NormalVector(Vector vector) {
            return new Vector(-vector.Y, vector.X);
        }

        public static bool IsPointOnLine(Point p, Vector2D vec, double tolerance) {
            Vector normal = NormalVector(vec.Vector);
            Vector2D normalVector = new Vector2D(p, normal);
            Point intersectionPoint = Intersection(vec, normalVector);
            double distance = ConnectionVector(p, intersectionPoint).Length;
            return (distance < tolerance) ? true : false;
        }

        public static bool IsPointInArea(Point midPoint, Point p1, Point p2, int tolerance) {
            bool b1 = Math.Min(p1.X, p2.X) <= midPoint.X + tolerance;
            bool b2 = midPoint.X < Math.Max(p1.X, p2.X) + tolerance;
            bool b3 = Math.Min(p1.Y, p2.Y) <= midPoint.Y + tolerance;
            bool b4 = midPoint.Y < Math.Max(p1.Y, p2.Y) + tolerance;
            if (b1 && b2 && b3 && b4) {
                return true;
            }
            return false;
        }

        public static Point Intersection(Vector2D vectorA, Vector2D vectorB) {
            Point p1 = vectorA.Position;
            Point p2 = vectorA.GetTargetPoint();
            Point p3 = vectorB.Position;
            Point p4 = vectorB.GetTargetPoint();

            // Get the segments' parameters.
            double dx12 = p2.X - p1.X;
            double dy12 = p2.Y - p1.Y;
            double dx34 = p4.X - p3.X;
            double dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            if (double.IsInfinity(t1)) {
                // The lines are parallel (or close enough to it).               
                return new Point(double.NaN, double.NaN);
            }
            //double t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            return new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);
        }

        private static double DistanceFromPointToBorder(Point p, int borderX, int borderY) {
            double l = p.X;
            double r = borderX - p.X;
            double u = p.Y;
            double d = borderY - p.Y;
            return Math.Min(Math.Min(l, r), Math.Min(u, d));
        }

        public static Point MinMaxPoint(List<Point> cloud) {
            double currentMinX = double.MaxValue;
            double currentMinY = double.MaxValue;
            double currentMaxX = double.MinValue;
            double currentMaxY = double.MinValue;
            foreach (Point p in cloud) {
                currentMinX = (currentMinX > p.X) ? p.X : currentMinX;
                currentMinY = (currentMinY > p.Y) ? p.Y : currentMinY;
                currentMaxX = (currentMaxX < p.X) ? p.X : currentMaxX;
                currentMaxY = (currentMaxY < p.Y) ? p.Y : currentMaxY;
            }
            return new Point(currentMinX, currentMinY);
        }

    }
}
