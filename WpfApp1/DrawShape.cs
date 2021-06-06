using System.Windows;

namespace WpfApp1
{
    public abstract class DrawShape
    {
        public abstract Rect BoundBox { get;}

        public abstract void DrawTo(IDrawCanvas canvas, IStyle style);

        public abstract void ScaleTo(float scale);
        
    }
}
