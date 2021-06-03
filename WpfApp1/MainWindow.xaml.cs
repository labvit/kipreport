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
    interface IDrawCanvas
    {
        void addLine(DrawLine l);
        void addCircle(DrawCircle c);
        void addRectangle(DrawBox b);
    }
    abstract class DrawShape
    {
        public abstract void DrawTo();
    }

    class DrawCircle : DrawShape { }
    class DrawBox : DrawShape { }
    class DrawLine : DrawShape
    {
        public override void DrawTo()
        {
            throw new NotImplementedException();
        }
    }
    class ShapeGroup { }
    class Object2d { }

    class Scene
    {

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
