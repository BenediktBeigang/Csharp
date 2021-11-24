using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Detection {
    
    class Vector2D {
        private Point position;
        public Point Position {
            get { return position; }
            set { position = value; }
        }

        private Vector vector;
        public Vector Vector {
            get { return vector; }
            set { vector = value; }
        }

        public Vector2D(Point p, Vector v) {
            position = p;
            vector = v;
        }

        public Vector2D(Point p1, Point p2) {
            position = p1;
            vector = VectorMath.ConnectionVector(p1, p2);
        }

        public Vector2D() {

        }

        public string Show() {
            return "(" + ((int)position.X).ToString() + "|" + ((int)position.Y).ToString() + ") " + "(" + ((int)vector.X).ToString() + "|" + ((int)vector.Y).ToString() + ")\n";
        }

        public Point GetTargetPoint() {
            return Point.Add(position, vector);
        }
    }
}
