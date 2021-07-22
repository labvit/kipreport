namespace WpfApp1
{
    public struct DrawPoint
    {
        public float x;
        public float y;
        public static DrawPoint operator*(DrawPoint p, float s)
        {
            return new DrawPoint { x = p.x * s, y = p.y*s };
        }
        public static DrawPoint operator -(DrawPoint a, System.Windows.Point b)
        {
            return new DrawPoint { x = (float)b.X - a.x, y = (float)b.Y - a.y };
        }
        public static DrawPoint operator -( System.Windows.Point b, DrawPoint a)
        {
            return new DrawPoint { x = (float)b.X - a.x, y = (float)b.Y - a.y };
        }
        public static DrawPoint operator -(DrawPoint b, DrawPoint a)
        {
            return new DrawPoint { x = b.x - a.x, y = b.y - a.y };
        }
        public static float dot(DrawPoint a, DrawPoint b)
        {
            return a.x * b.x + a.y * b.y;
        }


    }
}
