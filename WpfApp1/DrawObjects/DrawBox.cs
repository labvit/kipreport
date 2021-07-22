using System;
using System.Windows;

namespace WpfApp1
{
    public class DrawBox : DrawShape
    {
        public DrawPoint Start;
        public DrawPoint End;
        public override void DrawTo(IDrawCanvas canvas, IStyle style)
        {
            canvas.addRectangle(this,style);
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
            return p.X >= Start.x - Zero && p.X < End.x + Zero && p.Y <= End.y + Zero && p.X < End.x + Zero;
        }
    }
}
