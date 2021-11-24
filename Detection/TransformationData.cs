using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Detection {
    static class TransformationData {
        private static double scale = 1;
        public static double Scale {
            get { return scale; }
            set { scale = value; }
        }

        private static double speed = 1;
        public static double Speed {
            get { return speed; }
            set { speed = value; }
        }

        private static double deltaX = 0;
        public static double DeltaX {
            get { return deltaX; }
            set { deltaX = value; }
        }

        private static double deltaY = 0;
        public static double DeltaY {
            get { return deltaY; }
            set { deltaY = value; }
        }

        private static int pointThickness = 2;
        public static int PointThickness {
            get { return pointThickness; }
            set { pointThickness = value; }
        }

    }
}
