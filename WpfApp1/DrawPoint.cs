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
        
    }
}
