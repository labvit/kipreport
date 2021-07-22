namespace WpfApp1
{
    public interface IDrawCanvas
    {
        void addLine(DrawLine l, IStyle style);
        void addCircle(DrawCircle c, IStyle style);
        void addRectangle(DrawBox b, IStyle style);
        void BeginDraw(Object2d ob);
        void EndDraw();
        float Height { get; }
        float Width { get; }
    }
}
