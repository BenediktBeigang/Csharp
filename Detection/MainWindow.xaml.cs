using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Detection {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private bool Drawable { get; set; } = false;

        public MainWindow() {
            InitializeComponent();
        }

        private void Reset() {
            context.Children.Clear();
            Expected.Text = "Orientation: ";
            Prediction.Text = "Prediction: ";
            ConsoleTextBlock.Text = "Console:\n";
            Draw.DrawRaster(context);
        }

        private void NewLinePrediction_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("\n--------------------------------------------------");
            Reset();
            Point[] points = DataGeneration.NextLinePointCloudWithOrientation();
            Pointcloud.PrintCloud(points);
            Expected.Text = "Orientation: " + Pointcloud.LineOrientation;
            Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict(points)) + " | Time: " + MachineLearning.Timer;
            Draw.DrawPointCloud(points, context, 5, 3, true);
        }

        private void NewCorner_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("\n--------------------------------------------------");
            Reset();
            Point[] points = DataGeneration.NextCornerPointCloud(10, 0.05);

            Tuple<Point, Point, Point> result = PrimitiveParser.CornerParser(points);
            Draw.DrawMark(result.Item1, context, 15, "red");
            Draw.DrawMark(result.Item2, context, 15, "yellow");
            Draw.DrawMark(result.Item3, context, 15, "red");

            //ML
            Point[] formatetPoint = MachineLearning.TransformData(points);
            Expected.Text = "Expected: 1";
            Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict2(formatetPoint));

            Draw.DrawPointCloud(points, context, 5, 3, true);
        }

        private void NewLine_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("\n--------------------------------------------------");
            Reset();
            Point[] points = DataGeneration.NextLinePointCloud(10, 0.05);

            //ML
            Point[] formatetPoint = MachineLearning.TransformData(points);
            Expected.Text = "Expected: 0";
            Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict2(formatetPoint));

            Draw.DrawPointCloud(points, context, 5, 3, true);
        }

        private void NewNoise_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("\n--------------------------------------------------");
            Reset();
            Point[] points = DataGeneration.NextNoiseCloud(100);

            //ML
            Point[] formatetPoint = MachineLearning.TransformData(points);
            Expected.Text = "Expected: 2";
            Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict2(formatetPoint));

            Draw.DrawPointCloud(points, context, 5, 3, true);
        }

        private void NewMix_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("\n--------------------------------------------------");
            Reset();
            Point[] points = DataGeneration.NextMultiplePrimitves(10, 0.1);

            //ML
            Point[] formatetPoint = MachineLearning.TransformData(points);
            Expected.Text = "Expected: 2";
            Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict2(formatetPoint));

            Draw.DrawPointCloud(points, context, 5, 3, true);
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e) {
            Reset();
        }

        private void Draw_Self_Button_Click(object sender, RoutedEventArgs e) {
            if (Drawable) {
                Point[] formatetPoint = MachineLearning.TransformData(Draw.CanvasCloud.ToArray());
                Expected.Text = "Expected: NaN";
                Prediction.Text = "Prediction: " + Math.Round(MachineLearning.Predict2(formatetPoint));
                DrawSelfButton.Content = "Draw Self";
                Drawable = false;
            } else {
                DrawSelfButton.Content = "drawing";
                Reset();
                Draw.CanvasCloud.Clear();
                Drawable = true;
            }
        }

        private void Data_Button_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("--------------------------------------------------");
            //DataGeneration.LineOrientationGeneration(100000);
            DataGeneration.LineCornerGeneration(100000, 0.1);
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e) {
            Parser.Parse("example_data");
            Parser.NormalizeCloud();
            Pointcloud.Cloud = Parser.ToArray();
        }

        private void Draw_Plan_Button_Click(object sender, RoutedEventArgs e) {
            Reset();
            ScaleTextBlock.Text = TransformationData.Scale.ToString();
            SpeedTextBlock.Text = TransformationData.Speed.ToString();
            StrokeWidthTextBlock.Text = TransformationData.PointThickness.ToString();
            Point[] pc = Pointcloud.TransformCloud(Pointcloud.Cloud, TransformationData.Scale, TransformationData.DeltaX, TransformationData.DeltaY);
            Draw.DrawPointCloud(pc, context, TransformationData.PointThickness, TransformationData.Scale, false);
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double value = Math.Round(SpeedSlider.Value / 10, 1);
            TransformationData.Speed = value;
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double value = Math.Round(ScaleSlider.Value / 10, 1);
            TransformationData.Scale = value;
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            int value = (int)Math.Round(ThicknessSlider.Value);
            TransformationData.PointThickness = value;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e) {
            Reset();
            Point[] pc = new Point[0];
            double dx = Convert.ToDouble(DeltaXTextBlock.Text);
            double dy = Convert.ToDouble(DeltaYTextBlock.Text);
            double scale = Convert.ToDouble(ScaleTextBlock.Text);
            double speed = TransformationData.Speed;

            switch (e.Key) {
                case Key.Up:
                    dy += speed;
                    dy = Math.Round(dy, 1);
                    DeltaYTextBlock.Text = dy.ToString();
                    break;
                case Key.Down:
                    dy -= speed;
                    dy = Math.Round(dy, 1);
                    DeltaYTextBlock.Text = dy.ToString();
                    break;
                case Key.Left:
                    dx += speed;
                    dx = Math.Round(dx, 1);
                    DeltaXTextBlock.Text = dx.ToString();
                    break;
                case Key.Right:
                    dx -= speed;
                    dx = Math.Round(dx, 1);
                    DeltaXTextBlock.Text = dx.ToString();
                    break;
                case Key.PageUp:
                    scale += 0.1;
                    scale = Math.Round(scale, 1);
                    ScaleSlider.Value = scale;
                    ScaleTextBlock.Text = scale.ToString();
                    break;
                case Key.PageDown:
                    scale -= 0.1;
                    scale = Math.Round(scale, 1);
                    ScaleSlider.Value = scale * 10;
                    ScaleTextBlock.Text = scale.ToString();
                    break;
                default:
                    break;
            }

            TransformationData.DeltaX = dx;
            TransformationData.DeltaY = dy;
            TransformationData.Scale = scale;
            ScaleSlider.Value = scale;
            ScaleTextBlock.Text = scale.ToString();
            DeltaXTextBlock.Text = dx.ToString();
            DeltaYTextBlock.Text = dy.ToString();

            pc = Pointcloud.TransformCloud(Pointcloud.Cloud, scale, dx, dy);
            Draw.DrawPointCloud(pc, context, int.Parse(StrokeWidthTextBlock.Text), Convert.ToDouble(ScaleTextBlock.Text), false);
        }

        private void Context_MouseDown(object sender, MouseButtonEventArgs e) {
            if (Drawable) {
                Point newPoint = Mouse.GetPosition((IInputElement)sender);
                Draw.CanvasCloud.Add(newPoint);
                Draw.DrawPoint(newPoint, context, 5, "purple");
            }
        }
    }
}
