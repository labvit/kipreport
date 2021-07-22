using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public class Object2d
    {
        public bool Selected;
        public List<DrawShape> Shapes;
        public IStyle Style;
        public DrawPoint Location;
        public Rect BoundBox
        {
            get
            {
                Rect r = new Rect();
                foreach(var sh in Shapes)
                {
                    Rect _r = sh.BoundBox;
                    r.X = Math.Min(r.X, _r.X);
                    r.Y = Math.Min(r.Y, _r.Y);
                    r.Height = Math.Max(r.Height, _r.Height + _r.Y);
                    r.Width = Math.Max(r.Width, _r.Width + r.X);
                }
                r.X += Location.x;
                r.Y += Location.y;
                return r;

            }
        }

        public bool Contains(Point p)
        {
            p.X -= Location.x;
            p.Y -= Location.y;
            bool result = false;
            foreach(var sh in Shapes)
            {
                if (sh.Contains(p))
                    result = true;
            }
            return result;
        }
        public void DrawTo(IDrawCanvas canvas)
        {
            canvas.BeginDraw(this);
            foreach (var sh in Shapes)
                sh.DrawTo(canvas,Style);
            canvas.EndDraw();
        }

        internal void ScaleTo(float scale)
        {
            foreach (var sh in Shapes)
                sh.ScaleTo(scale);
        }
    }
}
