using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DrawAssembler
{
    enum AcAttachmentPoint
    {
        acAttachmentPointTopLeft = 1,
        acAttachmentPointTopCenter = 2,
        acAttachmentPointTopRight = 3,
        acAttachmentPointMiddleLeft = 4,
        acAttachmentPointMiddleCenter = 5,
        acAttachmentPointMiddleRight = 6,
        acAttachmentPointBottomLeft = 7,
        acAttachmentPointBottomCenter = 8,
        acAttachmentPointBottomRight = 9
    }
    /// <summary>
    /// Отрисовщик
    /// </summary>
    public static class DrawModule
    {
        /// <summary>
        /// Вектор описывает рабочую плоскость на шаблоне
        /// </summary>
        private static DrawLine WorkPlace(IDrawApp obj, int posss)
        {
            Core.ILine pos = obj.FramePosition(posss);
            return new DrawLine(new Point2D(280 + pos.Start.X, pos.Start.Y-5), new Point2D(390 + pos.Start.X, pos.Start.Y-260));
        }
        /// <summary>
        /// Отрисовать векторный рисунок
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="Draw"></param>
        /// <param name="showWorkPlace"></param>
        public static void CreateDraw(IDrawApp obj, int loyoutCount, Core.IDrawingObject rec)
        {
            //Создаем канвас который описывает наш WorkPlace
           /* Microsoft.Office.Interop.Word.Shape canvas = doc.Shapes.AddCanvas(WorkPlace.Start.X, WorkPlace.Start.Y, WorkPlace.End.X - WorkPlace.Start.X, WorkPlace.End.Y - WorkPlace.Start.Y, doc.Content);
            canvas.Width = WorkPlace.End.X - WorkPlace.Start.X;
            canvas.Height = WorkPlace.End.Y - WorkPlace.Start.Y; */  
            //Собираем в ассемблере рисунок из структуры
            DrawAssembler asemb = new DrawAssembler(rec);
            //растушевка
            Shading(asemb);

            //масштабируем рисунок
            resize(asemb, WorkPlace(obj, loyoutCount));

            //Переносим собранный рисунок на рабочую плоскость  !!ЭТО ТЕПЕРЬ НЕ НАДО!   ОПЯТЬ НАДО!
             ToWorkPlaceAssembler(asemb, WorkPlace(obj, loyoutCount));

            //Отрисовываем Рисунок
            draw(obj, asemb);
        }
        /// <summary>
        /// Перемасштабирование рисунка на workplace
        /// </summary>
        private static void resize(DrawAssembler da, DrawLine wp)
        {
            double wwidht = Math.Abs(wp.MaxX - wp.MinX);
            double wheight =Math.Abs(wp.MaxY - wp.MinY);

            var t = da.DrawTexts.OrderByDescending(s => s.Item2.MaxX + s.Item2.MinX).First();
            double dwidht = Math.Abs(Math.Max(da.DrawLines.OrderByDescending(s => s.MaxX).First().MaxX, t.Item2.MaxX+t.Item2.MinX));

            var t2 = da.DrawTexts.OrderByDescending(s => s.Item2.MaxY + s.Item2.MinY).First();
            double dheight = Math.Abs(Math.Max(da.DrawLines.OrderByDescending(s => s.MaxY).First().MaxY, t2.Item2.MaxY + t2.Item2.MinY));

            double masW = wwidht / dwidht;
            double masH = wheight / dheight;

            foreach (var one in da.DrawLines)
            {
                one.Start.X *= Math.Min(masH, masW);
                one.Start.Y*= Math.Min(masH, masW);
                one.End.X *= Math.Min(masH, masW);
                one.End.Y *= Math.Min(masH, masW);
            }
            foreach (var one in da.DrawTexts)
            {
                one.Item2.Start.X *= Math.Min(masH, masW);
                one.Item2.Start.Y *= Math.Min(masH, masW);
                one.Item2.End.X *= Math.Min(masH, masW);
            }
        }
        /// <summary>
        /// Отрисовывает рисунок в документе
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="Draw"></param>
        private static void draw(IDrawApp obj, DrawAssembler asemb)
        {
            DrawLine d = WorkPlace(obj, 0);

            foreach (DrawLine one in asemb.DrawLines)
            {
                // Microsoft.Office.Interop.Word.Shape l = doc.Shapes.AddLine(one.Start.X, one.Start.Y, one.End.X, one.End.Y);
                //Microsoft.Office.Interop.Word.Shape l = canvas.CanvasItems.AddLine(one.Start.X, one.Start.Y, one.End.X, one.End.Y);
               Core.ILine l = obj.AddLine(new double[3] { one.Start.X, one.Start.Y-d.MinY,0 }, new double[3] { one.End.X, one.End.Y - d.MinY, 0 });
               l.Linetype = one.LineStyle;
            }
            foreach (Tuple<string, ILine, Binding> one in asemb.DrawTexts)
            {
                // Microsoft.Office.Interop.Word.Shape sh = Shapes.AddTextbox(one.Item3, one.Item2.Start.X, one.Item2.Start.Y, one.Item2.End.X, one.Item2.End.Y);
                // Microsoft.Office.Interop.Word.Shape sh = canvas.CanvasItems.AddTextbox(one.Item3, one.Item2.Start.X, one.Item2.Start.Y, one.Item2.End.X, one.Item2.End.Y);
                IText text = obj.AddText(new double[3] { one.Item2.Start.X, one.Item2.Start.Y*-1, 0 }, one.Item2.End.X * one.Item1.Length, one.Item1);

                switch (one.Item3)
                {
                    case Binding.LeftDown:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointBottomLeft;
                        break;
                    case Binding.LeftCenter:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointMiddleLeft;
                        break;
                    case Binding.LeftUp:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointTopLeft;
                        break;
                    case Binding.RightDown:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointBottomRight;
                        break;
                    case Binding.RightCenter:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointMiddleRight;
                        break;
                    case Binding.RightUp:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointTopRight;
                        break;
                    case Binding.DownCentr:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointBottomCenter;
                        break;
                    case Binding.UpCenter:
                        text.AttachmentPoint = (int)AcAttachmentPoint.acAttachmentPointTopCenter;
                        break;
                    default:
                        text.AttachmentPoint = (int) AcAttachmentPoint.acAttachmentPointMiddleCenter;
                        break;
                }


                text.Height = one.Item2.End.X;
               // text.AttachmentPoint = AcAttachmentPoint.acAttachmentPointMiddleCenter;
                text.StyleName="PluginStyle";
               
                text.Rotate(new double[3] { one.Item2.Start.X, one.Item2.Start.Y*-1, 0 }, one.Item2.End.Y);
               // sh.TextFrame.TextRange.Text = one.Item1;
               // sh.Line.Visible = MsoTriState.msoFalse;
               // sh.TextFrame.TextRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }  
        }
        /// <summary>
        /// Переносит собранный рисунок на указанную рабочую плоскость
        /// </summary>
        /// <param name="da"></param>
        /// <param name="workPlace"></param>
        private static void ToWorkPlaceAssembler(DrawAssembler da, DrawLine workPlace)
        {
            foreach (var one in da.DrawLines)
            {
                one.Start = new Point2D(one.Start.X + workPlace.Start.X,( one.Start.Y*-1 + workPlace.Start.Y));
                one.End = new Point2D(one.End.X + workPlace.Start.X, (one.End.Y*-1 + workPlace.Start.Y));
            }

            foreach (var one in da.DrawTexts)
                one.Item2.Start = new Point2D(one.Item2.Start.X + workPlace.Start.X, (one.Item2.Start.Y - workPlace.Start.Y));
        }
        /// <summary>
        /// Растушевка
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static void Shading(DrawAssembler da)
        {
            List<Point2D> points = new List<Point2D>();

            foreach (var one in da.DrawLines)
                points.Add(new Point2D(one.MinX, one.MinY));

            foreach (var one in da.DrawTexts)
                points.Add(new Point2D(one.Item2.MinX, one.Item2.MinY));

            double x = points.OrderBy(t => t.X).First().X;
            double y = points.OrderBy(t => t.Y).First().Y;

            foreach (DrawLine d in da.DrawLines)
            {
                d.Start.X -= x;
                d.End.X -= x;
                d.Start.Y -= y;
                d.End.Y -= y;
            }

            foreach (var t in da.DrawTexts)
            {
                t.Item2.Start.X -= x;
                t.Item2.Start.Y -= y;
            }
        }
    }

    

}
