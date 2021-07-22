using System;
using System.Windows;

namespace WpfApp1
{
    public abstract class DrawShape
    {
        protected const float Zero = 1e-2f;
        public abstract Rect BoundBox { get;}

        public abstract void DrawTo(IDrawCanvas canvas, IStyle style);

        public abstract  void ScaleTo(float scale);

        internal abstract bool Contains(Point p);
    }
}
