using System;
using System.Collections.Generic;
using System.Linq;
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

        public void MoveSelected(Point vec)
        {
            foreach (var ob in Objects)
                if (ob.Selected)
                {
                    ob.Location.x += (float) vec.X;
                    ob.Location.y += (float)vec.Y;
                }

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

        private Object2d findObject(Point p)
        {
            double area = 1e10;
            Object2d minArOb = null;
            foreach (var ob in Objects)
            {
                var bb = ob.BoundBox;
                if (bb.Contains(p) )
                    if(ob.Contains(p))
                    if (area > bb.Height * bb.Width)
                    {
                        area = bb.Height * bb.Width;
                        minArOb = ob;
                    }
            }
            return minArOb;
        }

        internal void SelectObject(Point point)
        {
            var ob = findObject(point);
            if (ob != null)
                ob.Selected = true;
        }

        internal void UnselectObject(Point point)
        {
            var ob = findObject(point);
            if (ob != null)
                ob.Selected = false;
        }
    }
}
