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
                    r.Height = Math.Max(r.Height, _r.Height);
                    r.Width = Math.Max(r.Width, _r.Width);
                }
                return r;

            }
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
