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
    }
}
