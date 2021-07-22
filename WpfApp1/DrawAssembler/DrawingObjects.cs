using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Core.DrawAssembler
{
    /// <summary>
    /// Элементы Рисунка
    /// </summary>
    public static class DrawingObjects
    {
        ///// <summary>
        ///// Геометрическая привязка
        ///// </summary>
        //public enum Binding
        //{
        //    LeftUp,
        //    LeftCenter,
        //    LeftDown,
        //    UpCenter,
        //    Center,
        //    DownCentr,
        //    RightUp,
        //    RightCenter,
        //    RightDown
        //}
        //public enum Orien
        //{
        //    Vertical,
        //    Horizontal
        //}
        ///// <summary>
        ///// Интерфейс реализует свойства рисовательного объекта
        ///// </summary>
        //public interface IDrawingObject
        //{
        //    /// <summary>
        //    /// Список линий отрисовывающих объект
        //    /// </summary>
        //    List<DrawLine> DrawLines { get; }
        //    //геометрические точки объекта
        //    Point2D LeftUp { get; }
        //    Point2D LeftCenter { get; }
        //    Point2D LeftDown { get; }
        //    Point2D UpCenter { get; }
        //    Point2D Center { get; }
        //    Point2D DownCentr { get; }
        //    Point2D RightUp { get; }
        //    Point2D RightCenter { get; }
        //    Point2D RightDown { get; }
        //    //геометрические точки объекта
        //    /// <summary>
        //    /// Описывает прямоугольник в который вписан рисовательный объект
        //    /// </summary>
        //    DrawLine WorkPlace { get; }
        //    /// <summary>
        //    /// Дочерние рисовательные объекты
        //    /// </summary>
        //    List<IDrawingObject> ChildDrawingObjects { get; set; }
        //    /// <summary>
        //    /// Родительский рисовательный объект
        //    /// </summary>
        //    IDrawingObject ParentDrawingObject { get; set; }
        //    /// <summary>
        //    /// Привязка к родителю
        //    /// </summary>
        //    Binding BindingParent { get; set; }
        //    /// <summary>
        //    /// Привязка Данного объекта
        //    /// </summary>
        //    Binding BindingThis { get; set; }
        //    /// <summary>
        //    /// Метод отрисовки объекта
        //    /// </summary>
        //    void Rendering(out List<DrawLine> drawLines, out string text, out Binding binding);
        //    /// <summary>
        //    /// Добавить ребенка
        //    /// </summary>
        //    /// <param name="child"></param>
        //    void AddChild(IDrawingObject child);
        //}
        /// <summary>
        /// Класс Реализует функции рисовательного объекта
        /// </summary>
        public abstract class DrawingObject
        {
            public DrawingObject(Binding bindingParent, Binding bindingThis)
            {
                BindingParent = bindingParent;
                BindingThis = bindingThis;
                ChildDrawingObjects = new List<IDrawingObject>();
            }
            public virtual List<ILine> DrawLines { get; set; }
            public virtual Point2D LeftUp
            {
                get
                {
                    return WorkPlace.Start;
                }
            }
            public virtual Point2D LeftCenter
            {
                get
                {
                    return new Point2D(WorkPlace.MinX, (WorkPlace.MaxY + WorkPlace.MinY) / 2);
                }
            }
            public virtual Point2D LeftDown
            {
                get
                {
                    return new Point2D(WorkPlace.MinX, WorkPlace.MaxY);
                }
            }
            public virtual Point2D UpCenter
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX) / 2, WorkPlace.MinY);
                }
            }
            public virtual Point2D Center
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX)/2, (WorkPlace.MaxY + WorkPlace.MinY) / 2);
                }
            }
            public virtual Point2D DownCentr
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX) / 2, WorkPlace.MaxY);
                }
            }
            public virtual Point2D RightUp
            {
                get
                {
                    return new Point2D(WorkPlace.MaxX, WorkPlace.MinY);
                }
            }
            public virtual Point2D RightCenter
            {
                get
                {
                    return new Point2D(WorkPlace.MaxX, (WorkPlace.MaxY+WorkPlace.MinY)/2);
                }
            }
            public virtual Point2D RightDown
            {
                get
                {
                    return WorkPlace.End;
                }
            }
            public virtual ILine WorkPlace
            {
                get
                {
                    double maxX = DrawLines.OrderByDescending(i => i.MaxX).First().MaxX;
                    double maxY = DrawLines.OrderByDescending(i => i.MaxY).First().MaxY;

                    double minX = DrawLines.OrderBy(i => i.MinX).First().MinX;
                    double minY = DrawLines.OrderBy(i => i.MinY).First().MinY;
    
                    return new DrawLine(new Point2D(minX, minY), new Point2D(maxX, maxY));
                }
            }
            public List<IDrawingObject> ChildDrawingObjects { get; set; }
            public IDrawingObject ParentDrawingObject { get; set; }
            public Binding BindingParent { get; set; }
            public Binding BindingThis { get; set; }
            /// <summary>
            /// Возвращает текущую позицию относительно родителей и привязок к ним
            /// </summary>
            protected Point2D CurrentPosition
            {
                get
                {
                    if (ParentDrawingObject == null)
                        return new Point2D();
                    else if(ParentDrawingObject as Grid != null && BindingParent== Binding.Center && BindingThis == Binding.Center)
                    {
                        Point2D mypoint = Center;

                        int index = ParentDrawingObject.ChildDrawingObjects.IndexOf(this as IDrawingObject);
                        Point2D parentpoint = ((Grid)ParentDrawingObject).ChildPos(index);
                        return new Point2D(parentpoint.X - mypoint.X, parentpoint.Y - mypoint.Y);
                    }
                    else
                    {
                        Point2D mypoint = new Point2D();
                        if (BindingThis == Binding.Center)
                            mypoint = Center;
                        else if (BindingThis == Binding.DownCentr)
                            mypoint = DownCentr;
                        else if (BindingThis == Binding.LeftCenter)
                            mypoint = LeftCenter;
                        else if (BindingThis == Binding.LeftDown)
                            mypoint = LeftDown;
                        else if (BindingThis == Binding.LeftUp)
                            mypoint = LeftUp;
                        else if (BindingThis == Binding.RightCenter)
                            mypoint = RightCenter;
                        else if (BindingThis == Binding.RightDown)
                            mypoint = RightDown;
                        else if (BindingThis == Binding.RightUp)
                            mypoint = RightUp;
                        else if (BindingThis == Binding.UpCenter)
                            mypoint = UpCenter;

                        Point2D parentpoint = new Point2D();
                        if (BindingParent == Binding.Center)
                            parentpoint = ParentDrawingObject.Center;
                        else if (BindingParent == Binding.DownCentr)
                            parentpoint = ParentDrawingObject.DownCentr;
                        else if (BindingParent == Binding.LeftCenter)
                            parentpoint = ParentDrawingObject.LeftCenter;
                        else if (BindingParent == Binding.LeftDown)
                            parentpoint = ParentDrawingObject.LeftDown;
                        else if (BindingParent == Binding.LeftUp)
                            parentpoint = ParentDrawingObject.LeftUp;
                        else if (BindingParent == Binding.RightCenter)
                            parentpoint = ParentDrawingObject.RightCenter;
                        else if (BindingParent == Binding.RightDown)
                            parentpoint = ParentDrawingObject.RightDown;
                        else if (BindingParent == Binding.RightUp)
                            parentpoint = ParentDrawingObject.RightUp;
                        else if (BindingParent == Binding.UpCenter)
                            parentpoint = ParentDrawingObject.UpCenter;

                        return new Point2D(parentpoint.X - mypoint.X, parentpoint.Y - mypoint.Y);
                    }
                }
            }
            public virtual void Rendering(out List<ILine> drawLines, out string text, out Binding binding)
            {
                Point2D current = CurrentPosition;
                binding = BindingThis;
                foreach (var one in DrawLines)
                {
                    one.Start = new Point2D(one.Start.X + current.X, one.Start.Y + current.Y);
                    one.End = new Point2D(one.End.X + current.X, one.End.Y + current.Y);
                }
                drawLines = DrawLines;
                text = null;
            }
            public virtual void AddChild(IDrawingObject child)
            {
                child.ParentDrawingObject = this as IDrawingObject;
                ChildDrawingObjects.Add(child);
            }
        }
        /// <summary>
        /// Соеденитель
        /// </summary>
        public class Connector: DrawingObject, IDrawingObject
        {
            public Connector(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(10, 0));
                DrawLine p2 = new DrawLine(new Point2D(10, 0), new Point2D(10, 10));
                DrawLine p3 = new DrawLine(new Point2D(10, 10), new Point2D(0, 10));
                DrawLine p4 = new DrawLine(new Point2D(0, 10), new Point2D(0, 0));

                DrawLine p5 = new DrawLine(new Point2D(-4, 1), new Point2D(-6.83f, 2.17f));
                DrawLine p6 = new DrawLine(new Point2D(-6.83f, 2.17f), new Point2D(-8, 5));
                DrawLine p7 = new DrawLine(new Point2D(-8, 5), new Point2D(-6.83f, 7.83f));
                DrawLine p8 = new DrawLine(new Point2D(-6.83f, 7.83f), new Point2D(-4, 9));
                DrawLine p9 = new DrawLine(new Point2D(-4, 9), new Point2D(-1.17f, 7.83f));
                DrawLine p10 = new DrawLine(new Point2D(-1.17f, 7.83f), new Point2D(0, 5));
                DrawLine p11 = new DrawLine(new Point2D(0, 5), new Point2D(-1.17f, 2.17f));
                DrawLine p12 = new DrawLine(new Point2D(-1.17f, 2.17f), new Point2D(-4, 1));

                DrawLines = new List<ILine> { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };
            }
        }
        /// <summary>
        /// Кабельный вывод
        /// </summary>
        public class CableExhoust: DrawingObject, IDrawingObject
        {
            public CableExhoust(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(10, 0));
                DrawLine p2 = new DrawLine(new Point2D(10, 0), new Point2D(10, 10));
                DrawLine p3 = new DrawLine(new Point2D(10, 10), new Point2D(0, 10));
                DrawLine p4 = new DrawLine(new Point2D(0, 10), new Point2D(0, 0));
                DrawLine p5 = new DrawLine(new Point2D(3, 10), new Point2D(3, 40));
                DrawLine p6 = new DrawLine(new Point2D(7, 10), new Point2D(7, 40));
                DrawLines = new List<ILine> { p1, p2, p3, p4, p5, p6 };
            }
        }
        /// <summary>
        /// Грид
        /// </summary>
        public class Grid : DrawingObject, IDrawingObject
        {
            public Grid(Orien ori, Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                orie = ori;
            }
            public override ILine WorkPlace
           {
                get
                {
                    if (wp == null)
                    {
                        double lenght = 0;
                        double alt = 0;
                        foreach (IDrawingObject dob in ChildDrawingObjects.Where(f => f.BindingParent == Binding.Center && f.BindingThis == Binding.Center))
                        {
                            if (orie == Orien.Horizontal)
                            {
                                lenght += dob.WorkPlace.MaxX - dob.WorkPlace.MinX;
                                if (alt < dob.WorkPlace.MaxY - dob.WorkPlace.MinY)
                                    alt = dob.WorkPlace.MaxY - dob.WorkPlace.MinY;
                            }
                            else
                            {
                                alt += dob.WorkPlace.MaxY - dob.WorkPlace.MinY;
                                if (lenght < dob.WorkPlace.MaxX - dob.WorkPlace.MinX)
                                    lenght = dob.WorkPlace.MaxX - dob.WorkPlace.MinX;
                            }
                        }
                        wp= new DrawLine(new Point2D(), new Point2D(lenght, alt));
                    }
                    return wp;
                }
            }
            private DrawLine wp;
            private Orien orie { get; set; }
            public override void Rendering(out List<ILine> drawLines, out string text, out Binding binding)
            {
                binding = BindingThis;
                Point2D current = CurrentPosition;
                WorkPlace.Start = new Point2D(WorkPlace.Start.X + current.X, WorkPlace.Start.Y + current.Y);
                WorkPlace.End = new Point2D(WorkPlace.End.X + current.X, WorkPlace.End.Y + current.Y);
                drawLines = new List<ILine>();
                text = null;
            }
            public Point2D ChildPos(int index)
            {
                double X = 0;
                double Y = 0;
                for(int i = 0; i<=index; i+=1)
                {
                    if (ChildDrawingObjects[i].BindingParent == Binding.Center && ChildDrawingObjects[i].BindingThis == Binding.Center)
                    {
                        if (orie == Orien.Horizontal)
                        {
                            if (i != index)
                                X += ChildDrawingObjects[i].WorkPlace.MaxX - ChildDrawingObjects[i].WorkPlace.MinX;
                            else
                                X += (ChildDrawingObjects[i].WorkPlace.MaxX - ChildDrawingObjects[i].WorkPlace.MinX) / 2;
                        }
                        else
                        {
                            if (i != index)
                                Y += ChildDrawingObjects[i].WorkPlace.MaxY - ChildDrawingObjects[i].WorkPlace.MinY;
                            else
                                Y += (ChildDrawingObjects[i].WorkPlace.MaxY - ChildDrawingObjects[i].WorkPlace.MinY) / 2;
                        }
                    }
                }
                if (orie == Orien.Horizontal)
                    return new Point2D(LeftCenter.X + X, LeftCenter.Y + Y);
                else
                    return new Point2D(UpCenter.X + X, UpCenter.Y + Y);
            }
        }
        /// <summary>
        /// Круг
        /// </summary>
        public class Circle : DrawingObject, IDrawingObject
        {
            public Circle(Binding bindingParent, Binding bindingThis, double Radius) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 1), new Point2D(0.5, 0.866));
                DrawLine p2 = new DrawLine(new Point2D(0.5, 0.866), new Point2D(0.866, 0.5));
                DrawLine p3 = new DrawLine(new Point2D(0.866, 0.5), new Point2D(1, 0));
                DrawLine p4 = new DrawLine(new Point2D(1, 0), new Point2D(0.866, -0.5));
                DrawLine p5 = new DrawLine(new Point2D(0.866, -0.5), new Point2D(0.5, -0.866));
                DrawLine p6 = new DrawLine(new Point2D(0.5, -0.866), new Point2D(0, -1));
                DrawLine p7 = new DrawLine(new Point2D(0, -1), new Point2D(-0.5, -0.866));
                DrawLine p8 = new DrawLine(new Point2D(-0.5, -0.866), new Point2D(-0.866, -0.5));
                DrawLine p9 = new DrawLine(new Point2D(-0.866, -0.5), new Point2D(-1, 0));
                DrawLine p10 = new DrawLine(new Point2D(-1, 0), new Point2D(-0.866, 0.5));
                DrawLine p11 = new DrawLine(new Point2D(-0.866, 0.5), new Point2D(-0.5, 0.866));
                DrawLine p12 = new DrawLine(new Point2D(-0.5, 0.866), new Point2D(0, 1));
    
                DrawLines = new List<ILine> { p1, p2, p3, p4, p5 , p6 , p7 , p8 , p9 , p10 , p11 , p12 };

                foreach (DrawLine d in DrawLines) { d.Start.X *= Radius; d.Start.Y *= Radius; d.End.X *= Radius; d.End.Y *= Radius; }
            }
        }
        /// <summary>
        /// Линия разрыва
        /// </summary>
        public class BreakLine : DrawingObject, IDrawingObject
        {
            public BreakLine(Binding bindingParent, Binding bindingThis, double Lenght, Orien orientation) : base(bindingParent, bindingThis)
            {
                if (orientation == Orien.Vertical)
                {
                    DrawLine p1 = new DrawLine(new Point2D(-5, 0), new Point2D(0, 3));
                    DrawLine p2 = new DrawLine(new Point2D(5, 0), new Point2D(-5, 0));
                    DrawLine p3 = new DrawLine(new Point2D(0, -3), new Point2D(5, 0));
                    DrawLines = new List<ILine> { p1, p2, p3};

                    if (Lenght / 2 - 3 > 0)
                    {
                        DrawLines.Add(new DrawLine(new Point2D(0, -3), new Point2D(0, -Lenght / 2)));
                        DrawLines.Add(new DrawLine(new Point2D(0, 3), new Point2D(0, Lenght / 2)));
                    }                  
                }
                else
                {
                    DrawLine p1 = new DrawLine(new Point2D(-3, 0), new Point2D(0, -5));
                    DrawLine p2 = new DrawLine(new Point2D(0, -5), new Point2D(0, 5));
                    DrawLine p3 = new DrawLine(new Point2D(0, 5), new Point2D(3, 0));
                    DrawLines = new List<ILine> { p1, p2, p3 };

                    if (Lenght / 2 - 3 > 0)
                    {
                        DrawLines.Add(new DrawLine(new Point2D(-3, 0), new Point2D(-Lenght / 2, 0)));
                        DrawLines.Add(new DrawLine(new Point2D(3, 0), new Point2D(Lenght / 2, 0)));
                    }
                }
            }
        }
        /// <summary>
        /// Кабельный короб
        /// </summary>
        public class CableDuct : DrawingObject, IDrawingObject
        {
            public CableDuct(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(100, 0));
                DrawLine p2 = new DrawLine(new Point2D(0, 15), new Point2D(100, 15));
                DrawLines = new List<ILine> { p1, p2 };

                AddChild(new Circle(Binding.LeftCenter, Binding.RightCenter, 12));
                AddChild(new Line(Orien.Horizontal, Binding.LeftUp, Binding.RightCenter, 2.63));
                AddChild(new Line(Orien.Horizontal, Binding.LeftDown, Binding.RightCenter, 2.63));
                AddChild(new BreakLine(Binding.RightCenter, Binding.Center, 21, Orien.Vertical));
            }
        }
        /// <summary>
        /// Прямоугольник
        /// </summary>
        public class Rectangle: DrawingObject, IDrawingObject
        {
            public Rectangle(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(50, 0));
                DrawLine p2 = new DrawLine(new Point2D(50, 0), new Point2D(50, 30));
                DrawLine p3 = new DrawLine(new Point2D(50, 30), new Point2D(0, 30));
                DrawLine p4 = new DrawLine(new Point2D(0, 30), new Point2D(0, 0));
                DrawLines =  new List<ILine> { p1, p2, p3, p4 };
            }
        }
        /// <summary>
        /// Овал
        /// </summary>
        public class Oval : DrawingObject, IDrawingObject
        {
            public Oval(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(30, 0));
                DrawLine p2 = new DrawLine(new Point2D(30, 30), new Point2D(0, 30));
                DrawLine p3 = new DrawLine(new Point2D(30, 30), new Point2D(0, 30));
                DrawLine p4 = new DrawLine(new Point2D(0, 0), new Point2D(-8.2f, 7.5f));
                DrawLine p5 = new DrawLine(new Point2D(-8.2f, 7.5f), new Point2D(-10, 15));
                DrawLine p6 = new DrawLine(new Point2D(-10, 15), new Point2D(-8.2f, 22.5f));
                DrawLine p7 = new DrawLine(new Point2D(-8.2f, 22.5f), new Point2D(0, 30));
                DrawLine p8 = new DrawLine(new Point2D(30, 0), new Point2D(38.2f, 7.5f));
                DrawLine p9 = new DrawLine(new Point2D(38.2f, 22.5f), new Point2D(40,15));
                DrawLine p10 = new DrawLine(new Point2D(40, 15), new Point2D(38.2f, 7.5f));
                DrawLine p11 = new DrawLine(new Point2D(38.2f, 22.5f), new Point2D(30,30));

                DrawLines = new List<ILine> { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11};
            }
        }
        /// <summary>
        /// Риска кароче кастыльная
        /// </summary>
        public class Lin : DrawingObject, IDrawingObject
        {
            public Lin(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(-2, 2));
                DrawLines = new List<ILine> { p1 };
            }
        }
        /// <summary>
        /// Квадрат
        /// </summary>
        public class Square : DrawingObject, IDrawingObject
        {
            public Square(Binding bindingParent, Binding bindingThis, double widht = 30, double height = 30) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(widht, 0));
                DrawLine p2 = new DrawLine(new Point2D(widht, 0), new Point2D(widht, height));
                DrawLine p3 = new DrawLine(new Point2D(widht, height), new Point2D(0, height));
                DrawLine p4 = new DrawLine(new Point2D(0, height), new Point2D(0, 0));
                DrawLines = new List<ILine> { p1, p2, p3, p4 };
            }
        }
        /// <summary>
        /// Линия
        /// </summary>
        public class Line : DrawingObject, IDrawingObject
        {
            public Line(Orien orientation, Binding bindingParent, Binding bindingThis, double lenght = 30, DrawLine.Type s = DrawLine.Type.Continuous) : base(bindingParent, bindingThis)
            {
                DrawLine p1;
                if(orientation == Orien.Horizontal)
                    p1 = new DrawLine(new Point2D(0, 0), new Point2D(lenght, 0), style: s);
                else
                    p1 = new DrawLine(new Point2D(0, 0), new Point2D(0, lenght), style: s);

                DrawLines = new List<ILine> { p1 };
            }
        }
        /// <summary>
        /// Текст
        /// </summary>
        public class Text : DrawingObject, IDrawingObject
        {
            public Text(Orien orientation, string text, Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(0, 0), new Point2D(7, 0));
                if (orientation == Orien.Vertical)
                    p1.End.Y = Math.PI / 2;

                int diapazon = 2;

                switch(bindingThis)
                {
                    case Binding.LeftDown:
                        p1.Start.X += diapazon;
                        p1.Start.Y += -diapazon;
                        break;
                    case Binding.LeftCenter:
                        p1.Start.X += diapazon;
                        break;
                    case Binding.LeftUp:
                        p1.Start.X += diapazon;
                        p1.Start.Y += diapazon;
                        break;
                    case Binding.UpCenter:
                        p1.Start.Y += diapazon;
                        break;
                    case Binding.DownCentr:
                        p1.Start.Y += -diapazon;
                        break;
                    case Binding.RightDown:
                        p1.Start.X += -diapazon;
                        p1.Start.Y += -diapazon;
                        break;
                    case Binding.RightCenter:
                        p1.Start.X += -diapazon;
                        break;
                    case Binding.RightUp:
                        p1.Start.X += -diapazon;
                        p1.Start.Y += diapazon;
                        break;
                }

                DrawLines = new List<ILine> { p1 };

                thisText = text;
            }
            internal string thisText { get; set; }
            public override void Rendering(out List<ILine> drawLines, out string text, out Binding binding)
            {
                binding = BindingThis;
                Point2D current = CurrentPosition;
                text = thisText;

                foreach (DrawLine one in DrawLines)
                {
                    one.Start = new Point2D(one.Start.X + current.X, one.Start.Y + current.Y);
                }
                drawLines = DrawLines;
            }
        }
        /// <summary>
        /// Выноска
        /// </summary>
        public class Note : DrawingObject, IDrawingObject
        {
            public Note(Binding bindingParent, Binding bindingThis) : base(bindingParent, bindingThis)
            {
                DrawLine p1 = new DrawLine(new Point2D(), new Point2D());
                DrawLine p2 = new DrawLine(new Point2D(), new Point2D());      
                DrawLines = new List<ILine> { p1, p2 };

                IDrawingObject d = new Lin(Binding.RightDown, Binding.Center);
                AddChild(d);
                d.BindingParent = BindingThis;
            }
            private int lenght { get; set; }
            public override Point2D LeftUp
            {
                get
                {
                    return WorkPlace.End;

                }
            }
            public override Point2D LeftCenter
            {
                get
                {
                    return new Point2D(WorkPlace.MaxX, (WorkPlace.MaxY + WorkPlace.MinY) / 2);
                }
            }
            public override Point2D LeftDown
            {
                get
                {
                    return new Point2D(WorkPlace.MaxX, WorkPlace.MinY);
                }
            }
            public override Point2D UpCenter
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX) / 2, WorkPlace.MinY);
                }
            }
            public override Point2D Center
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX) / 2, (WorkPlace.MaxY + WorkPlace.MinY) / 2);
                }
            }
            public override Point2D DownCentr
            {
                get
                {
                    return new Point2D((WorkPlace.MaxX + WorkPlace.MinX) / 2, WorkPlace.MaxY);
                }
            }
            public override Point2D RightUp
            {
                get
                {
                    return new Point2D(WorkPlace.MinX, WorkPlace.MaxY);
            
                }
            }
            public override Point2D RightCenter
            {
                get
                {
                    return new Point2D(WorkPlace.MinX, (WorkPlace.MaxY + WorkPlace.MinY) / 2);
                    
                }
            }
            public override Point2D RightDown
            {
                get
                {
                    return WorkPlace.Start;
                }
            }
            public override void AddChild(IDrawingObject child)
            {
                if (BindingThis == Binding.LeftUp || BindingThis == Binding.Center)
                    child.BindingParent = Binding.RightDown;
                else if (BindingThis == Binding.LeftDown)
                    child.BindingParent = Binding.RightUp;
                else if (BindingThis == Binding.RightUp)
                    child.BindingParent = Binding.LeftDown;
                else if (BindingThis == Binding.RightDown)
                    child.BindingParent = Binding.LeftUp;
                else if (BindingThis == Binding.LeftCenter || BindingThis == Binding.UpCenter)
                    child.BindingParent = Binding.RightCenter;
                else if (BindingThis == Binding.RightCenter || BindingThis == Binding.DownCentr)
                    child.BindingParent = Binding.LeftCenter;

                child.ParentDrawingObject = this as IDrawingObject;
                ChildDrawingObjects.Add(child);

                if (child as Text != null && ((Text)child).thisText.Length > lenght)
                {
                    lenght = ((Text)child).thisText.Length;
                    resize();
                }
            }
            private void resize()
            {
                if (BindingThis == Binding.LeftUp || BindingThis == Binding.Center)
                {
                    DrawLines[0] = new DrawLine(new Point2D(), new Point2D(lenght * 6 + 20, 0));
                    DrawLines[1] = new DrawLine(new Point2D(lenght * 6 + 20, 0), new Point2D(lenght * 6 + 20 + 8, 15));
                }
                else if (BindingThis == Binding.LeftDown)
                {
                    DrawLines[0] = new DrawLine(new Point2D(), new Point2D(lenght * 6 + 20, 0));
                    DrawLines[1] = new DrawLine(new Point2D(lenght * 6 + 20, 0), new Point2D(lenght * 6 + 20 + 8, -15));
                }
                else if (BindingThis == Binding.RightUp)
                {
                    DrawLines[0] = new DrawLine(new Point2D(), new Point2D(-lenght * 6 - 20, 0));
                    DrawLines[1] = new DrawLine(new Point2D(-lenght * 6 - 20, 0), new Point2D(-lenght * 6 - 20 - 8, 15));
                }
                else if (BindingThis == Binding.RightDown)
                {
                    DrawLines[0] = new DrawLine(new Point2D(), new Point2D(-lenght * 6 - 20, 0));
                    DrawLines[1] = new DrawLine(new Point2D(-lenght * 6 - 20, 0), new Point2D(-lenght * 6 - 20 - 8, -15));
                }
                else if (BindingThis == Binding.LeftCenter || BindingThis == Binding.RightCenter || BindingThis == Binding.UpCenter || BindingThis == Binding.DownCentr)
                {
                    DrawLines[0] = new DrawLine(new Point2D(), new Point2D(lenght * 6 + 20, 0));
                    DrawLines[1] = new DrawLine(new Point2D(), new Point2D());
                }
            }
        }
    }
}
