using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Detection {
    static class PrimitiveParser {

        /*
         * This function mesures for all possible combinations of vectors (using cloud-points) the best fitting.
         * That means that the vectors represent the points as best as possible.
         * It returns three points: the corner-point and the intersection-points of the vectors with the border of the surrounding box.
         */
        public static Tuple<Point, Point, Point> CornerParser(Point[] cloud) {
            double minimalNormalSum = double.MaxValue;
            //Start permutation
            Vector2D bestVec1 = new Vector2D(cloud[0], cloud[1]);
            Vector2D bestVec2 = new Vector2D(cloud[1], cloud[2]);

            for (int p1 = 0; p1 < cloud.Length; p1++) {
                for (int p2 = p1 + 1; p2 < cloud.Length; p2++) {
                    for (int p3 = p2 + 1; p3 < cloud.Length; p3++) {
                        // With three points their are 3 possible variations of vector pairs
                        // Tuple consists of sumOfNormalDistances, vector1 and vector2
                        Tuple<double, Vector2D, Vector2D> pair1 = CalculateSumOfNormals(cloud, p1, p2, p3);
                        Tuple<double, Vector2D, Vector2D> pair2 = CalculateSumOfNormals(cloud, p3, p1, p2);
                        Tuple<double, Vector2D, Vector2D> pair3 = CalculateSumOfNormals(cloud, p2, p3, p1);
                        Tuple<double, Vector2D, Vector2D>[] pairArray = new Tuple<double, Vector2D, Vector2D>[3] { pair1, pair2, pair3 };

                        // Checking if permutation/(two vectors) is better than current best permutation
                        for (int i = 0; i < 3; i++) {
                            if (IsNewSolutionBetter(pairArray[i], new Tuple<double, Vector2D, Vector2D>(minimalNormalSum, bestVec1, bestVec2)) /*minimalNormalSum > pairArray[i].Item1*/) {
                                minimalNormalSum = pairArray[i].Item1;
                                bestVec1 = pairArray[i].Item2;
                                bestVec2 = pairArray[i].Item3;
                            }
                        }
                    }
                }
            }
            Point corner = VectorMath.Intersection(bestVec1, bestVec2);
            Tuple<Point, Point> borderPoints = ExtractIntersectionPointsFromVectors(cloud, corner, bestVec1, bestVec2);

            return new Tuple<Point, Point, Point>(borderPoints.Item1, corner, borderPoints.Item2);
        }

        /*
         * This function mesures how good a new permutation/(choosen vectors in this iteration) really is.
         * The first criteria is the length of a both vectors combined (larger is better)
         * The second is the sum of all normal lengths (calculatet in CalculateSumOfNormals) (smaller is better)
         */
        private static bool IsNewSolutionBetter(Tuple<double, Vector2D, Vector2D> newPair, Tuple<double, Vector2D, Vector2D> currentBestPair) {
            double length1 = newPair.Item2.Vector.Length + newPair.Item3.Vector.Length;
            double length2 = currentBestPair.Item2.Vector.Length + currentBestPair.Item3.Vector.Length;

            
            if (( length1 / (newPair.Item1 + 1)) > (length2 / (currentBestPair.Item1 + 1))) {
                return true;
            }

            return false;
        }

        /*
         * This function returns the intersections of a corner with the border
         */
        private static Tuple<Point, Point> ExtractIntersectionPointsFromVectors(Point[] cloud, Point corner, Vector2D vec1, Vector2D vec2) {
            Point p1 = ExtractBorderPoint(cloud, corner, vec1);
            Point p2 = ExtractBorderPoint(cloud, corner, vec2);
            return new Tuple<Point, Point>(p1, p2);
        }

        /*
         * This function calculates and chooses the correct intersection of a given vector with the surrounding box.
         * It returns this intersection-point.
         */
        private static Point ExtractBorderPoint(Point[] cloud, Point corner, Vector2D vec) {
            Vector2D frameAxis1 = new Vector2D(new Point(0, 0), new Vector(1, 0));
            Vector2D frameAxis2 = new Vector2D(new Point(0, 0), new Vector(0, 1));
            Vector2D frameAxis3 = new Vector2D(new Point(500, 500), new Vector(1, 0));
            Vector2D frameAxis4 = new Vector2D(new Point(500, 500), new Vector(0, 1));
            Vector2D[] frameAxisArray = new Vector2D[4] { frameAxis1, frameAxis2, frameAxis3, frameAxis4 };

            int[] pointCounter = new int[4];
            Point[] intersections = new Point[4];


            for (int i = 0; i < 4; i++) {
                intersections[i] = VectorMath.Intersection(vec, frameAxisArray[i]);
                if (VectorMath.IsPointInArea(intersections[i], new Point(0, 0), new Point(500, 500), 3)) { // is point in cell
                    for (int p = 0; p < cloud.Length; p++) { // for all points in cloud
                        if (VectorMath.IsPointInArea(cloud[p], intersections[i], corner, 3)) { 
                            pointCounter[i]++;
                        }
                    }
                }
            }

            int correctIntersectionIndex = 0;
            int maxPoints = 0;
            for (int i = 0; i < 4; i++) {
                if (pointCounter[i] > maxPoints) {
                    correctIntersectionIndex = i;
                    maxPoints = pointCounter[i];
                }
            }

            return intersections[correctIntersectionIndex];
        }

        /*
         * This function calcualtes/summens all distances from all points of the pointcloud to the given vectors.
         * In every iteration only one distance is used, because if a point is close to one vector, the impact on the sum shoud be minimal.
         * Thats the reason in every iteration only the smaller value is used.
         * The result is a sum that indicates how close the points are to the vectors, and the vectors itself.
         */
        private static Tuple<double, Vector2D, Vector2D> CalculateSumOfNormals(Point[] cloud, int p1, int p2, int p3) {
            Vector2D vec1 = new Vector2D(cloud[p1], cloud[p2]);
            Vector2D vec2 = new Vector2D(cloud[p2], cloud[p3]);
            double sumOfNormals = 0;

            for (int i = 0; i < cloud.Length; i++) {
                double length1 = LengthFromPointToAxis(cloud[i], vec1);
                double length2 = LengthFromPointToAxis(cloud[i], vec2);
                sumOfNormals += Math.Min(length1, length2);
            }

            return new Tuple<double, Vector2D, Vector2D>(sumOfNormals, vec1, vec2);
        }

        /*
         * This function calcualtes the minimal-distance from a point to a vector.
         * It uses the normal of the vector to calcualte the intersection of the mainVector and the point with normal as vector.
         * The distance of the intersection and the point is the searched distace.
         */
        private static double LengthFromPointToAxis(Point point, Vector2D mainAxis) {
            Vector2D normal = new Vector2D(point, VectorMath.NormalVector(mainAxis.Vector));
            Point intersection = VectorMath.Intersection(mainAxis, normal);
            Vector connection = VectorMath.ConnectionVector(point, intersection);
            return VectorMath.VectorLength(connection);
        }

        //TODO
        public static Tuple<Point, Point, Point> ArcParser(Point[] cloud, System.Windows.Controls.TextBlock consoleTextBlock) {
            throw new NotImplementedException();
        }


    }
}
