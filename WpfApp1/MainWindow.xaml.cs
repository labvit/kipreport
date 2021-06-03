using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    struct Point
    {
        float x;
        float y;
    }
    interface IDrawCanvas
    {
        void addLine(DrawLine l);
        void addCircle(DrawCircle c);
        void addRectangle(DrawBox b);
    }
    abstract class DrawShape
    {
        public abstract void DrawTo(IDrawCanvas canvas);
    }

    class DrawCircle : DrawShape
    {
        public override void DrawTo(IDrawCanvas canvas)
        {
            canvas.addCircle(this);
        }
    }
    class DrawBox : DrawShape
    {
        public override void DrawTo(IDrawCanvas canvas)
        {
            canvas.addRectangle(this);
        }
    }
    class DrawLine : DrawShape
    {
        public Point Start;
        public Point End;
        public override void DrawTo(IDrawCanvas canvas)
        {
            canvas.addLine(this);
        }
    }
    class Style
    {
        public Color Color;
        public float Width;
    }
    class ShapeGroup {
    
    }
    class Object2d 
    {
        public bool Selected;
        public List<Shape> Shapes;
    }

    class Scene
    {
        List<Object2d> Objects;
        public Object2d newObject()
        {
            var o = new Object2d();
            Objects.Add(o);
            return o;
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
