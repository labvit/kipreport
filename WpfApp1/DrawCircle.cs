using System.Windows;

namespace WpfApp1
{
    public class DrawCircle : DrawShape
    {
        public DrawPoint Center;
        public float R;
        public override void DrawTo(IDrawCanvas canvas, IStyle style)
        {
            canvas.addCircle(this,style);
        }
        public override Rect BoundBox => new Rect { X = Center.x - R, Y = Center.y - R,Height = 2*R,Width=2*R};
        public override void ScaleTo(float scale)
        {
            Center.x *= scale;
            R *= scale;
        }
    }
}
