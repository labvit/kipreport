using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
namespace Core.DrawAssembler
{
    /// <summary>
    /// Реализует линию
    /// </summary>
    public class DrawLine : ILine
    {
        public enum Type
        {
            Continuous,
            GOST2__303___5

        }
        public DrawLine(Point2D start, Point2D end, Type style = Type.Continuous)
        {
            if (start == null)
                start = new Point2D();
            if (end == null)
                end = new Point2D();
            Start = start;
            End = end;

            Style = style;
        }
        private Type Style { get; set; }
        public string LineStyle
        {
            get
            {
                string s = Style.ToString();
                s = s.Replace("___", " ");
                s = s.Replace("__", ".");              
                return s;
            }
        }
        public Point2D Start { get; set; }
        public Point2D End { get; set; }
        public double MinX
        {
            get
            {
                return Math.Min(Start.X, End.X);
            }
        }
        public double MinY
        {
            get
            {
                return Math.Min(Start.Y, End.Y);
            }
        }
        public double MaxX
        {
            get
            {
                return Math.Max(Start.X, End.X);
            }
        }
        public double MaxY
        {
            get
            {
                return Math.Max(Start.Y, End.Y);
            }
        }

        public string Linetype { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
