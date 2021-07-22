using System.Windows.Media;

namespace WpfApp1
{
    class Style : IStyle
    {
        private Color _Color;
        private float _Width;
        public Style(Color c, float w)
        {
            _Width = w;
            _Color = c;
        }
        public Color Color => _Color;

        public float Width => _Width;
    }
}
