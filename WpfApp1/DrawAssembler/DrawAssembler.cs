using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DrawAssembler
{
    /// <summary>
    /// Сборщик рисунка из структуры
    /// </summary>
    public class DrawAssembler
    {
        public DrawAssembler(IDrawingObject draw)
        {
            DrawLines = new List<ILine>();
            DrawTexts = new List<Tuple<string, ILine, Binding>>();
            Recursie(draw, DrawLines, DrawTexts);
        }
        private void Recursie(IDrawingObject draw, List<ILine> drawLines, List<Tuple<string, ILine, Binding>> drawtexts)
        {
            List<ILine> renddrawLines;
            string text;
            double ori;
            Binding binding;
            draw.Rendering(out renddrawLines, out text, out binding);
            if (text != null)
                drawtexts.Add(new Tuple<string, ILine, Binding>(text, renddrawLines[0], binding));
            else
                foreach (DrawLine drawLine in renddrawLines)
                    drawLines.Add(drawLine);

            foreach (IDrawingObject one in draw.ChildDrawingObjects)
                Recursie(one, drawLines, drawtexts);
        }
        /// <summary>
        /// Собранные линии
        /// </summary>
        public List<ILine> DrawLines { get; private set; }
        /// <summary>
        /// Собранные текстовые блоки
        /// </summary>
        public List<Tuple<string, ILine, Binding>> DrawTexts { get; private set; }
    }
}
