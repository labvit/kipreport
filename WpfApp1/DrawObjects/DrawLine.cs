using System;
using System.Windows;

namespace WpfApp1
{
    public class DrawLine : DrawShape
    {
        public DrawPoint Start;
        public DrawPoint End;
        public override void DrawTo(IDrawCanvas canvas, IStyle style)
        {
            canvas.addLine(this,style);
        }
        public override Rect BoundBox => new Rect
        {
            X = Math.Min(Start.x, End.x),
            Y = Math.Min(End.y, Start.y),
            Height = Math.Abs(Start.y - End.y),
            Width = Math.Abs(Start.x - End.x)
        };
        public override void ScaleTo(float scale)
        {
            Start *= scale;
            End *= scale;
        }

        internal override bool Contains(Point p)
        {
            var line = End - Start;
            var pp =  p - Start;
            var pl = Math.Sqrt(DrawPoint.dot(pp, pp));
            var ll = Math.Sqrt(DrawPoint.dot(line,line )) ;
            return Math.Abs(DrawPoint.dot(pp, line) / (pl * ll) - 1e0) <= Zero
                && pl <= ll + Zero;
                ;
        }
    }
}
