using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    class WpfCanvasWrapper : IDrawCanvas
    {
        DrawingImage _Canvas;
        DrawingGroup _DG;
        GeometryDrawing _CGD = null;
        GeometryGroup _CGG = null;
        DrawPoint ObLocation;
        public WpfCanvasWrapper(DrawingImage canvas)
        {
            _Canvas = canvas;
            _DG = new DrawingGroup();
            _Canvas.Drawing = _DG;
        }
        public void addCircle(DrawCircle c, IStyle style)
        {
            if (_CGG != null)
            {
                var circle = new EllipseGeometry(new Point(c.Center.x + ObLocation.x, c.Center.y+ObLocation.y), c.R,c.R);
                _CGG.Children.Add(circle);
            }
        }

        public void addLine(DrawLine l, IStyle style)
        {
            if(_CGG != null)
            {
                var line = new LineGeometry(new Point(l.Start.x+ObLocation.x, l.Start.y+ObLocation.y), new Point(l.End.x + ObLocation.x, l.End.y + ObLocation.y));
                _CGG.Children.Add(line);
            }
        }

        public void addRectangle(DrawBox b, IStyle style)
        {
            if(_CGG != null)
            {
                var r = new Rect();
                r.X = b.Start.x + ObLocation.x;
                r.Y = b.Start.y + ObLocation.y;
                r.Width = b.End.x - b.Start.x;
                r.Height = b.End.y - b.Start.y;

                var bb = new RectangleGeometry(r);
               
                _CGG.Children.Add(bb);
            }
        }

        public void BeginDraw(Object2d ob)
        {
            _CGD = new GeometryDrawing();
            var Brush = new SolidColorBrush(ob.Style.Color);
            ObLocation = ob.Location;
            if (ob.Selected)
                Brush = new SolidColorBrush(Colors.Blue);
            _CGD.Pen = new Pen(Brush, ob.Style.Width);
            
            _CGG = new GeometryGroup();
        }

        public void EndDraw()
        {
            _CGD.Geometry = _CGG;
            _DG.Children.Add(_CGD);
            _CGG = null;
            _CGD = null;
            ObLocation.x = 0e0f;
            ObLocation.y = 0e0f;
        }

        public float Height
        {
            get => (float) _Canvas.Height;
        }
        public float Width
        {
            get => (float)_Canvas.Width;
        }
    }
}
