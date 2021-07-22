using Core;
using Core.KipDescription;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Core.DrawAssembler
{
    public class Constructor
    {
        Core.MaterialReferences.MaterialReferences _MaterialReferences = null;
        Core.KipiaInfo _Kipianfo;
        IEnumerable<ConnectionWireSchemeComponent> _Cables;
        public Constructor(Core.MaterialReferences.MaterialReferences matref, Core.KipiaInfo kipinfo, IEnumerable<ConnectionWireSchemeComponent> cables)
        {
            _MaterialReferences = matref;
            _Cables = cables;
            _Kipianfo = kipinfo;
        }

        /// <summary>
        /// Создает выноски схемы
        /// </summary>
        /// <param name="exhoustObjects"></param>
        /// <param name="splitters"></param>
        private   void createSplitters(List<Core.IDrawingObject> exhoustObjects, out List<Core.IDrawingObject> splitters, out List<int[]> spl)
        {
            spl = new List<int[]>();
            splitters = new List<IDrawingObject>();

            for (int i = 0; i < exhoustObjects.Count; i += 1)
            {
                IDrawingObject Line = new DrawingObjects.Line(Orien.Vertical, Core.Binding.DownCentr, Binding.UpCenter, (exhoustObjects.Count / 2 - Math.Abs(exhoustObjects.Count / 2 - i)) * 10 + 60);
                IDrawingObject Line2 = new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.UpCenter, Math.Abs(exhoustObjects.Count / 2 - i) * 50);
                spl.Add(new int[] { i, Math.Abs(exhoustObjects.Count / 2 - i) * 50 });
                IDrawingObject Line3 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, Math.Abs(exhoustObjects.Count / 2 - i) * 10 + 15);
                if (i < exhoustObjects.Count / 2)
                {
                    Line2.BindingThis = Binding.RightCenter;
                    Line3.BindingParent = Binding.LeftCenter;
                }
                else
                {
                    Line2.BindingThis = Binding.LeftCenter;
                    Line3.BindingParent = Binding.RightCenter;
                }
                Line.AddChild(Line2);
                Line2.AddChild(Line3);
                splitters.Add(Line3);
                exhoustObjects[i].AddChild(Line);
            }
        }

        List<string> wires(uint i)
        {
            List<string> r = new List<string>();
            for (uint ii = 0; ii < i; ii++)
                r.Add(ii.ToString());
            return r;
        }
        /// <summary>
        /// Создает клеммы схемы
        /// </summary>
        private   void createClems(IEnumerable<ConnectionWireSchemeComponent> components, out IDrawingObject grid, out List<IDrawingObject> exhoustObjects, out List<int[]> cll)
        {
            cll = new List<int[]>();
            //грид для клемм 
            grid = new DrawingObjects.Grid(Orien.Horizontal, Binding.DownCentr, Binding.UpCenter);

            //список для исходящих элементов
            exhoustObjects = new List<IDrawingObject>();
            //счетчик выводов с каждой клемы
            int countLine = 0;
            //сборка клемм
            int i = 0;
            if (components != null)
                foreach (ConnectionWireSchemeComponent cwsc in components)
                {
                    bool first = true;
                    List<string> cleems = wires( _MaterialReferences.GetCableWires( cwsc.CableParametrs));
                    cll.Add(new int[] { i, cleems.Count });
                    i += 1;
                    foreach (string str in cleems)
                    {
                        countLine += 1;
                        IDrawingObject extLine = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter);
                        extLine.AddChild(new DrawingObjects.Text(Orien.Vertical, countLine.ToString(), Binding.Center, Binding.RightCenter));
                        if (first)
                        {
                            IDrawingObject Line = new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.LeftCenter, 30 * (cleems.Count - 1));
                            extLine.AddChild(Line);
                            exhoustObjects.Add(Line);
                            IDrawingObject ce = new DrawingObjects.CableExhoust(Binding.DownCentr, Binding.UpCenter);
                            Line.AddChild(ce);
                            IDrawingObject notice = new DrawingObjects.Note(Binding.Center, Binding.LeftDown);
                            ce.AddChild(notice);
                            if (countLine == 1)
                            {
                                notice.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Кабельный ввод", Binding.LeftDown, Binding.LeftDown));
                                notice.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Комплект прибора", Binding.LeftDown, Binding.LeftUp));
                            }
                            else
                                notice.AddChild(new DrawingObjects.Text(Orien.Horizontal, "                 ", Binding.LeftDown, Binding.LeftDown));
                            first = false;
                        }

                        IDrawingObject clem = new DrawingObjects.Square(Binding.Center, Binding.Center);
                        clem.AddChild(new DrawingObjects.Text(Orien.Horizontal, str, Binding.Center, Binding.Center));
                        clem.AddChild(extLine);
                        grid.AddChild(clem);
                    }
                }

            //делаем две клемы
            IDrawingObject clem1 = new DrawingObjects.Square(Binding.Center, Binding.Center);
            clem1.AddChild(new DrawingObjects.Text(Orien.Horizontal, "+", Binding.Center, Binding.Center));

            IDrawingObject clem2 = new DrawingObjects.Square(Binding.Center, Binding.Center);
            clem2.AddChild(new DrawingObjects.Text(Orien.Horizontal, "-", Binding.Center, Binding.Center));

            //суем клеммы в грид
            grid.AddChild(clem1);
            grid.AddChild(clem2);

            //вывод для первой клемы 
            IDrawingObject extclemm1 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter);
            extclemm1.AddChild(new DrawingObjects.Text(Orien.Vertical, (countLine + 1).ToString(), Binding.LeftCenter, Binding.RightCenter));
            //для втрой клеммы
            IDrawingObject extclemm2 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter);
            extclemm2.AddChild(new DrawingObjects.Text(Orien.Vertical, (countLine + 2).ToString(), Binding.LeftCenter, Binding.RightCenter));

            //Подключаем выводы к каждой клемме
            clem1.AddChild(extclemm1);
            clem2.AddChild(extclemm2);

            //соединяем клеммы 
            IDrawingObject line = new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.LeftCenter);
            IDrawingObject ce1 = new DrawingObjects.CableExhoust(Binding.DownCentr, Binding.UpCenter);
            line.AddChild(ce1);
            extclemm1.AddChild(line);
            exhoustObjects.Add(line);
            IDrawingObject notice1 = new DrawingObjects.Note(Binding.Center, Binding.LeftDown);
            if (components == null || components.Count() == 0)
            {
                notice1.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Кабельный ввод", Binding.LeftDown, Binding.LeftDown));
                notice1.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Комплект прибора", Binding.LeftDown, Binding.LeftUp));
            }
            else
                notice1.AddChild(new DrawingObjects.Text(Orien.Horizontal, "                 ", Binding.LeftDown, Binding.LeftDown));
            ce1.AddChild(notice1);
            cll.Add(new int[] { i, 2 });
        }
        /// <summary>
        /// Добавляет основную схему
        /// </summary>
        /// <param name="splitters"></param>
        private void createSchem(List<IDrawingObject> splitters, List<ConnectionWireSchemeComponent> components, out List<int[]> grl)
        {
            grl = new List<int[]>();
            for (int i = 0; i < splitters.Count(); i += 1)
            {
                IDrawingObject AdresOrCablebox;

                bool isgrl = false;
                int ii = i + 1;

                string kb;
                string Mr1;
                string T1;
                string Mr2;
                string Sz1;
                string Sz2;
                string signal;
                if (components.Count > i)
                {
                    kb = components[i].CableParametrs;

                    //if (components[i].MetalHoseDiametr > 0)
                        Mr1 = components[i].MetalHoseDiametr.ToString();
                    //else
                    //    Mr1 = null;

                    //if (components[i].MetalHoseDiametr1 > 0)
                        Mr2 = components[i].MetalHoseDiametr1.ToString();
                    //else
                    //    Mr2 = null;

                    Sz1 = components[i].ConnectionParametrsPipeBox;
                    Sz2 = components[i].ConnectionParametrsMetalHose;

                    //if (components[i].ProtectedPipeDiametr.Diameter > 0)
                    T1 = components[i].ProtectedPipeDiametr;//.Diameter;//.ToString();
                    //else
                    //    T1 = null;
                    signal = TypeParser.enumToString<SignalType>(components[i].SignalTypeRegarding);

                    if (components[i].CableBox == CableBoxType.Box)
                    {
                        AdresOrCablebox = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 190);

                        IDrawingObject cd = new DrawingObjects.CableDuct(Binding.DownCentr, Binding.UpCenter);
                        IDrawingObject text = new DrawingObjects.Text(Orien.Horizontal, "K" + ii.ToString(), Binding.Center, Binding.Center);

                        IDrawingObject rect = new DrawingObjects.Square(Binding.UpCenter, Binding.DownCentr, 25, 6);
                        cd.AddChild(rect);

                        IDrawingObject note = new DrawingObjects.Note(Binding.UpCenter, Binding.LeftUp);
                        rect.AddChild(note);

                        note.AddChild(new DrawingObjects.Text(Orien.Horizontal, "C" + ii.ToString() + ",Г" + ii.ToString(), Binding.LeftUp, Binding.LeftDown));

                        AdresOrCablebox.AddChild(cd);
                        cd.ChildDrawingObjects[0].AddChild(text);
                    }
                    else
                    {
                        AdresOrCablebox = new DrawingObjects.Rectangle(Binding.DownCentr, Binding.UpCenter);
                        AdresOrCablebox.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Адрес", Binding.Center, Binding.Center));
                    }
                }
                else
                {
                    kb = "Кб";
                    Mr1 = "Мр";
                    T1 = "T";
                    Mr2 = "Мр";
                    Sz1 = "С";
                    Sz2 = "С";
                    signal = "4...20мА Exia";
                      AdresOrCablebox = new DrawingObjects.Rectangle(Binding.DownCentr, Binding.UpCenter);
                      AdresOrCablebox.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Адрес", Binding.Center, Binding.Center));

                }

                //прогоняем кабель вниз
                IDrawingObject longLine = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 50);
                splitters[i].AddChild(longLine);

                //Блок отвечает за первую выноску
                if (Mr1 != "" && Mr1 != null)
                {
                    IDrawingObject note = new DrawingObjects.Note(Binding.UpCenter, Binding.LeftCenter);
                    note.AddChild(new DrawingObjects.Text(Orien.Horizontal, kb + ii.ToString(), Binding.LeftUp, Binding.LeftDown));
                    note.AddChild(new DrawingObjects.Text(Orien.Horizontal, Mr1, Binding.LeftUp, Binding.LeftUp));
                    longLine.AddChild(note);
                }//Блок отвечает за первую выноску

                //Рисовательный объект первого соеденителя
                IDrawingObject connector;

                //Блок отвечает за создание первого соеденителя и его линии заземления, если он не нужен то он просто линия
                if (((T1 != "" && T1 != null) || (Mr2 != "" && Mr2 != null)) && (Mr1 != "" && Mr1 != null))
                {
                    connector = new DrawingObjects.Connector(Binding.DownCentr, Binding.UpCenter);

                    //выноска для соеденителя
                    IDrawingObject note1 = new DrawingObjects.Note(Binding.UpCenter, Binding.LeftUp);
                    connector.AddChild(note1);
                    note1.AddChild(new DrawingObjects.Text(Orien.Horizontal, Sz1 + ii.ToString(), Binding.LeftUp, Binding.LeftDown));
                    //Уходит влево и вниз
                    IDrawingObject leftdown = new DrawingObjects.Line(Orien.Horizontal, Binding.LeftCenter, Binding.RightCenter, 50, DrawLine.Type.GOST2__303___5);
                    IDrawingObject dl1 = new DrawingObjects.Line(Orien.Vertical, Binding.LeftCenter, Binding.UpCenter, 315 + 50, DrawLine.Type.GOST2__303___5);
                    leftdown.AddChild(dl1);
                    connector.AddChild(leftdown);

                    grl.Add(new int[] { i, 50 + 9 });
                    isgrl = true;
                }
                else
                {
                    connector = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 10);
                }
                longLine.AddChild(connector);
                //Блок отвечает за создание первого соеденителя и его линии заземления, если он не нужен то он просто линия

                //уходит вниз
                IDrawingObject down = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 20);
                //овал номер кабеля кажется
                IDrawingObject number = new DrawingObjects.Oval(Binding.DownCentr, Binding.UpCenter);
                number.AddChild(new DrawingObjects.Text(Orien.Horizontal, ii.ToString(), Binding.Center, Binding.Center));
                number.AddChild(new DrawingObjects.Text(Orien.Horizontal, signal, Binding.UpCenter, Binding.LeftDown));
                connector.AddChild(down);
                down.AddChild(number);


                IDrawingObject line2 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 100);
                number.AddChild(line2);
                //Этот блок отвечает за создание второй выноски
                if (T1 != "" && T1 != null)
                {
                    IDrawingObject note2 = new DrawingObjects.Note(Binding.Center, Binding.LeftCenter);
                    note2.AddChild(new DrawingObjects.Text(Orien.Horizontal, kb + ii.ToString(), Binding.LeftUp, Binding.LeftDown));
                    note2.AddChild(new DrawingObjects.Text(Orien.Horizontal, T1, Binding.LeftUp, Binding.LeftUp));
                    line2.AddChild(note2);
                }
                //Этот блок отвечает за создание второй выноски

                //это соеденитель 2
                IDrawingObject connector2;

                //Блок отвечает за создание второго соеденителя и его линии заземления, если он не нужен то он просто линия
                if ((T1 != "" && T1 != null) && (Mr2 != "" && Mr2 != null))
                {
                    //это соеденитель 2
                    connector2 = new DrawingObjects.Connector(Binding.DownCentr, Binding.UpCenter);

                    IDrawingObject note4 = new DrawingObjects.Note(Binding.UpCenter, Binding.LeftUp);
                    note4.AddChild(new DrawingObjects.Text(Orien.Horizontal, Sz2 + ii.ToString(), Binding.LeftUp, Binding.LeftDown));
                    connector2.AddChild(note4);
                    //еще влево и вниз
                    IDrawingObject leftdown2 = new DrawingObjects.Line(Orien.Horizontal, Binding.LeftCenter, Binding.RightCenter, 40, DrawLine.Type.GOST2__303___5);
                    IDrawingObject dl2 = new DrawingObjects.Line(Orien.Vertical, Binding.LeftCenter, Binding.UpCenter, 155 + 50, DrawLine.Type.GOST2__303___5);
                    leftdown2.AddChild(dl2);
                    connector2.AddChild(leftdown2);
                    if (!isgrl)
                        grl.Add(new int[] { i, 40 + 9 });
                    isgrl = true;
                }
                else
                {
                    connector2 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 10);
                }
                //Блок отвечает за создание второго соеденителя и его линии заземления, если он не нужен то он просто линия
                line2.AddChild(connector2);

                //и еще вниз
                IDrawingObject line3 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 70);
                connector2.AddChild(line3);

                //бокс для адреса
               /* IDrawingObject box = new DrawingObjects.Rectangle(Binding.DownCentr, Binding.UpCenter);
                box.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Адрес", Binding.Center, Binding.Center));*/

                line3.AddChild(AdresOrCablebox);

                //блок отвечает за создание третьей выноски
                if (Mr2 != "" && Mr2 != null)
                {
                    IDrawingObject note3 = new DrawingObjects.Note(Binding.Center, Binding.LeftCenter);
                    note3.AddChild(new DrawingObjects.Text(Orien.Horizontal, kb + ii.ToString(), Binding.LeftUp, Binding.LeftDown));
                    note3.AddChild(new DrawingObjects.Text(Orien.Horizontal, Mr2, Binding.LeftUp, Binding.LeftUp));
                    line3.AddChild(note3);
                }
                //блок отвечает за создание третьей выноски       
                if (!isgrl)
                    grl.Add(new int[] { i, 0 });
            }
        }
        
        /// <summary>
        /// Make grounding
        /// </summary>
        /// <param name="drawobj"></param>
        /// <param name="components"></param>
        /// <param name="isHetBox"></param>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="g"></param>
        /// <param name="ground"></param>
        private void createGrounding(IDrawingObject drawobj, List<ConnectionWireSchemeComponent> components, bool isHetBox, List<int[]> c, List<int[]> s, List<int[]> g, string ground)
        {
            int extcount = components.Count() + 1;
            int cleems = 2;
            foreach (ConnectionWireSchemeComponent con in components)
            {
                //List<string> str = _MaterialReferences.GetCableWires( con.CableParametrs);
                cleems += (int)_MaterialReferences.GetCableWires(con.CableParametrs);
            }
            int splitCountR = components.Count() / 2;

            int r = cleems * 30 / 2 + splitCountR * 50;
            int d = 60 + extcount / 2 * 10 + 15;

            IDrawingObject left = new DrawingObjects.Line(Orien.Horizontal, Binding.RightCenter, Binding.LeftCenter, r - 5, DrawLine.Type.GOST2__303___5);
            IDrawingObject down = new DrawingObjects.Line(Orien.Vertical, Binding.RightCenter, Binding.UpCenter, d + 445, DrawLine.Type.GOST2__303___5);
            left.AddChild(down);
            drawobj.AddChild(left);


            int indexcreatgrounline = -1;
            if (g.Where(ind => ind[1] != 0).Count() != 0)
                indexcreatgrounline = g.Where(ind => ind[1] != 0).First()[0];
            if (indexcreatgrounline != -1)
            {
                int lenght = c.Sum(cl => cl[1]) * 15;
                for (int i = 0; i <= indexcreatgrounline; i += 1)
                {
                    if (i != indexcreatgrounline)
                    {
                        lenght -= c.Where(cl => cl[0] == indexcreatgrounline).First()[1] * 30;
                    }
                    else
                        lenght -= c.Where(cl => cl[0] == indexcreatgrounline).First()[1] * 15;
                }

                lenght += g.Where(ind => ind[0] == indexcreatgrounline).First()[1];

                if (indexcreatgrounline <= (s.Count - 1) / 2)
                    lenght += s.Where(ind => ind[0] == indexcreatgrounline).First()[1];
                else
                    lenght -= s.Where(ind => ind[0] == indexcreatgrounline).First()[1];

                IDrawingObject down1 = new DrawingObjects.Line(Orien.Vertical, Binding.DownCentr, Binding.UpCenter, 50);
                down.AddChild(down1);
                IDrawingObject line = (new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.RightCenter, lenght + r - 5 + 25 + 10, DrawLine.Type.GOST2__303___5));
                down1.AddChild(line);
                down1.AddChild(new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.LeftCenter, 10, DrawLine.Type.GOST2__303___5));

                down.AddChild(new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.RightCenter, lenght + r - 5 + 25));
                down.AddChild(new DrawingObjects.Text(Orien.Horizontal, "Пр 1м", Binding.DownCentr, Binding.RightDown));
                down.AddChild(new DrawingObjects.Text(Orien.Horizontal, "П 2м", Binding.DownCentr, Binding.RightUp));
                line.AddChild(new DrawingObjects.Text(Orien.Horizontal, ground, Binding.Center, Binding.UpCenter));
            }


            if (isHetBox)
            {
                int firstclem = 0;
                if (components.Count > 1)
                    firstclem = (int)_MaterialReferences.GetCableWires( components[0].CableParametrs);
                int relu = firstclem * 15;
                IDrawingObject u = new DrawingObjects.Line(Orien.Vertical, Binding.RightCenter, Binding.DownCentr, 70);
                IDrawingObject l = new DrawingObjects.Line(Orien.Horizontal, Binding.UpCenter, Binding.RightCenter, 25 + r - 5 + cleems * 30 / 2 - firstclem * 30 + relu + 120);
                u.AddChild(l);
                IDrawingObject dw = new DrawingObjects.Line(Orien.Vertical, Binding.LeftCenter, Binding.UpCenter, 200);
                l.AddChild(dw);
                dw.AddChild(new DrawingObjects.Line(Orien.Horizontal, Binding.DownCentr, Binding.LeftCenter, 25 + r - 5 + cleems * 30 / 2 - firstclem * 30 + relu + 120));

                left.AddChild(u);
                IDrawingObject note = new DrawingObjects.Note(Binding.Center, Binding.RightUp);
                note.AddChild(new DrawingObjects.Text(Orien.Horizontal, "ТЧ (Термочехол)", Binding.LeftDown, Binding.RightDown));
                note.AddChild(new DrawingObjects.Text(Orien.Horizontal, "комплект прибора", Binding.LeftDown, Binding.RightUp));
                l.AddChild(note);

            }
        }
        /// <summary>
        /// Самый главный метод вот
        /// </summary>
        /// <param name="kipiaInfo"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public IDrawingObject ConstructorSchem(string ground)
        {
            //Создаем овал с текстом это прибор начало схемы самый верх
            IDrawingObject drawingobj = new DrawingObjects.Oval(Binding.Center, Binding.Center);
            drawingobj.AddChild(new DrawingObjects.Text(Orien.Horizontal, _Kipianfo.SpecificationComponent.DesignationKip, Binding.Center, Binding.Center));

          

            IDrawingObject g;
            List<IDrawingObject> exhoustObjects;
            List<int[]> cll;
            createClems(_Cables, out g, out exhoustObjects, out cll);
            drawingobj.AddChild(g);
            //делаем выноски
            List<IDrawingObject> Splitters;
            List<int[]> spl;
            createSplitters(exhoustObjects, out Splitters, out spl);
            //делаем основную схему
            List<int[]> grl;
            createSchem(Splitters, _Cables.ToList(), out grl);
            //Добавляем заземление
            createGrounding(drawingobj, _Cables.ToList(), _Kipianfo.ConnectionWireSchemeComponent.ThermalCover != HeaterType.None  , cll, spl, grl, ground);

            return drawingobj;
        }
    }
}
