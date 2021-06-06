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
    /// Логика взаимодействия для Viewer2DControl.xaml
    /// </summary>
    public partial class Viewer2DControl : UserControl
    {
        WpfCanvasWrapper _WpfCanvas;
        public Viewer2DControl()
        {
            InitializeComponent();
            DrawingImage img = new DrawingImage();
            Img.Source = img;
            
            _WpfCanvas = new WpfCanvasWrapper(img);
        }

        public static readonly DependencyProperty SceneProperty = DependencyProperty.Register("Scene", typeof(Scene), typeof(Viewer2DControl),
            new PropertyMetadata((PropertyChangedCallback)SceneChanged));

        public static void SceneChanged(DependencyObject ob,DependencyPropertyChangedEventArgs e)
        {
            var control = ob as Viewer2DControl;
            control.BuildScene(e.NewValue as Scene);
        }

        public Scene Scene
        {
            get { return (Scene)GetValue(SceneProperty); }
            set { SetValue(SceneProperty, value); }
        }


        public void BuildScene(Scene sc)
        {
            //Object2d o = new Object2d();
            //o.Style= new Style( Colors.Black, 3);
            //o.Shapes = new List<DrawShape>();
            //o.Shapes.Add(new DrawLine { Start = new DrawPoint(), End = new DrawPoint { x = 100, y = 100 } });
            //o.Shapes.Add(new DrawCircle { Center = new DrawPoint { x = 150, y = 150 }, R = 50 });
            //Scene sc = new Scene();
            //sc.Objects = new List<Object2d>();
            //sc.Objects.Add(o);
            sc.DrawTo(_WpfCanvas);
            //sc.ScaleTo(Math.Max(ActualHeight / (Img.Source as DrawingImage).Drawing.Bounds.Height,
            //    ActualWidth / (Img.Source as DrawingImage).Drawing.Bounds.Width));
         
        }
    }
}
