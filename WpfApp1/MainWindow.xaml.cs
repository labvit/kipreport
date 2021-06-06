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
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Object2d o = new Object2d();
            o.Style = new Style(Colors.Black, 3);
            o.Shapes = new List<DrawShape>();
            o.Shapes.Add(new DrawLine { Start = new DrawPoint(), End = new DrawPoint { x = 100, y = 100 } });
            o.Shapes.Add(new DrawCircle { Center = new DrawPoint { x = 150, y = 150 }, R = 50 });
            Scene sc = new Scene();
            sc.Objects = new List<Object2d>();
            sc.Objects.Add(o);

            Img.Scene = sc;
        }
    }
}
