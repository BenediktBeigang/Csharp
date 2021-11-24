using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Detection {
    static class Draw {
        public static List<Point> CanvasCloud { get; } = new List<Point>();

        public static void DrawPointCloud(Point[] points, Canvas context, int pointThickness, double scale, bool primitive) {            
            Point[] pointsNew;
            if (primitive) {
                pointsNew = points;
            } else {
                double per = 1;
                if (scale < 2) {
                    per = 0.1;
                } else if (scale < 3) {
                    per = scale - 1.9;
                }
                pointsNew = Pointcloud.ShortenCloud(points, per);
            }

            pointsNew = points;

            for (int i = 0; i < pointsNew.Length; i++) {
                if (pointsNew[i].X < 1000 && pointsNew[i].Y < 1000 && pointsNew[i].X >= 0 && pointsNew[i].Y >= 0) {
                    if (i == 0 || i == pointsNew.Length - 1) {
                        DrawPoint(pointsNew[i], context, pointThickness, "red");
                    } else if (i == (int) (pointsNew.Length / 2)) {
                        DrawPoint(pointsNew[i], context, pointThickness, "yellow");
                    } else if (i < (int) (pointsNew.Length / 2)) {
                        DrawPoint(pointsNew[i], context, pointThickness, "green");
                    } else {
                        DrawPoint(pointsNew[i], context, pointThickness, "purple");
                    }
                }
            }
            DrawRaster(context);
        }

        public static void DrawMarks(Point[] points, Canvas context, int circleThickness) {
            for (int i = 0; i < points.Length; i++) {
                DrawMark(points[i], context, circleThickness, "black");
            }
        }

        public static void DrawMark(Point point, Canvas context, int circleThickness, string colorTag) {
            SolidColorBrush color = Brushes.Black;
            if (colorTag.Equals("blue")) {
                color = Brushes.Blue;
            } else if (colorTag.Equals("yellow")) {
                color = Brushes.Yellow;
            } else if (colorTag.Equals("red")) {
                color = Brushes.Red;
            }

            Ellipse circle = new Ellipse() {
                Width = circleThickness,
                Height = circleThickness,
                Stroke = color,
                StrokeThickness = 2
            };
            Canvas.SetLeft(circle, point.X - circleThickness / 2);
            Canvas.SetTop(circle, point.Y - circleThickness / 2);
            context.Children.Add(circle);
        }

        public static void DrawPoint(Point point, Canvas context, int pointThickness, string colorTag) {
            SolidColorBrush color = ParseColor(colorTag);

            System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle {
                Stroke = color,
                Fill = color,
                Width = pointThickness,
                Height = pointThickness
            };
            Canvas.SetLeft(rect, point.X);
            Canvas.SetTop(rect, point.Y);
            context.Children.Add(rect);
        }

        public static void DrawLine(Vector2D vec, Canvas context, double thickness, string colorTag) {
            SolidColorBrush color = ParseColor(colorTag);

            System.Windows.Shapes.Line line;
            line = new System.Windows.Shapes.Line {
                Stroke = color,
                StrokeThickness = thickness,
                X1 = vec.Position.X,
                Y1 = vec.Position.Y,
                X2 = vec.Position.X + vec.Vector.X,
                Y2 = vec.Position.Y + vec.Vector.Y
            };
            Canvas.SetLeft(line, 0);
            Canvas.SetTop(line, 0);
            context.Children.Add(line);
        }

        public static void DrawRaster(Canvas context) {
            DrawTinyRaster(context, 0, 0);
            var line1 = new Line {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2,
                X1 = 500,
                Y1 = 0,
                X2 = 500,
                Y2 = 1000
            };
            var line2 = new Line {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2,
                X1 = 0,
                Y1 = 500,
                X2 = 1000,
                Y2 = 500
            };

            Canvas.SetTop(line1, 0);
            Canvas.SetLeft(line1, 0);
            Canvas.SetTop(line2, 0);
            Canvas.SetLeft(line2, 0);

            context.Children.Add(line1);
            context.Children.Add(line2);
        }

        public static void DrawTinyRaster(Canvas context, int x, int y) {
            for (int i = 0; i < 5; i++) {
                var line1 = new Line {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    X1 = (i * 100),
                    Y1 = 0,
                    X2 = (i * 100),
                    Y2 = 500
                };
                Canvas.SetTop(line1, x);
                Canvas.SetLeft(line1, y);
                context.Children.Add(line1);

                var line2 = new Line {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    X1 = 0,
                    Y1 = (i * 100),
                    X2 = 500,
                    Y2 = (i * 100)
                };
                Canvas.SetTop(line2, x);
                Canvas.SetLeft(line2, y);
                context.Children.Add(line2);
            }
        }

        private static SolidColorBrush ParseColor(string colorTag) {
            SolidColorBrush color = new SolidColorBrush(Colors.Black);
            switch (colorTag) {
                case "red":
                    color = new SolidColorBrush(Colors.Red);
                    break;
                case "purple":
                    color = new SolidColorBrush(Colors.Purple);
                    break;
                case "green":
                    color = new SolidColorBrush(Colors.Green);
                    break;
                case "yellow":
                    color = new SolidColorBrush(Colors.Yellow);
                    break;
                case "blue":
                    color = new SolidColorBrush(Colors.Blue);
                    break;
            }
            color = new SolidColorBrush(Colors.White);
            return color;
        }

    }
}
