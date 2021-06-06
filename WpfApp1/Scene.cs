using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public class Scene 
    {
        public List<Object2d> Objects;
        public Object2d newObject()
        {
            var o = new Object2d();
            Objects.Add(o);
            return o;
        }

        public void DrawTo(IDrawCanvas canvas)
        {
            foreach (var ob in Objects)
               ob.DrawTo(canvas);
            
        }

        internal Rect BoundBox
        {
            get
            {
                var r = new Rect();
                foreach (var o in Objects)
                {
                    var _r = o.BoundBox;
                    r.X = Math.Min(r.X, _r.X);
                    r.Y = Math.Min(r.Y, _r.Y);
                    r.Height = Math.Max(r.Height, _r.Height);
                    r.Width = Math.Max(r.Width, _r.Width);
                }
                return r;
            }
        }
        internal void ScaleTo(WpfCanvasWrapper wpfCanvas)
        {
            var rect = BoundBox; // box of objects
            var scale = (float) Math.Max(wpfCanvas.Height / rect.Height, wpfCanvas.Width / rect.Width);
            ScaleTo(scale);
        }
        internal void ScaleTo(double scale)
        {
            foreach (var o in Objects)
                o.ScaleTo((float)scale);
        }
    }
}
