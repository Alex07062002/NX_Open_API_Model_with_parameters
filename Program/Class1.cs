using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NXOpen;
using NXOpen.UF;
using System.Collections;

namespace Program
{
    public class Program
    {
        
        private static UFSession theUfSession;
        public static Program theProgram;
        public static bool isDisposeCalled;

        public static double height;//Высота, мм
        public static double Radius;//радиус, мм
        public static double step;//шагрезьбы, мм

        public Program()
        {
            try
            {
                theUfSession = UFSession.GetUFSession();
                isDisposeCalled = false;


            }
            catch (NXOpen.NXException ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }

        public static int Main(string[] args)
        {
            Form1 f = new Form1();
            f.ShowDialog();

            try
            {
                theProgram = new Program();
                Tag UFPart1;
                string name1 = "bolt";
                int units1 = 1;
                theUfSession.Part.New(name1, units1, out UFPart1);

/******************Создаём тело вращения, позже на него будет добавлена резьба******************/
                // Объявляем 4 прямые для описания тела вращения
                UFCurve.Line segment0 = new UFCurve.Line();
                UFCurve.Line segment1 = new UFCurve.Line();
                UFCurve.Line segment2 = new UFCurve.Line();
                UFCurve.Line segment3 = new UFCurve.Line();

                // задаём параметры прямых (start_point - начальная точка (x,y,z), end_point - конечная точка (x,y,z))
                segment0.start_point = new double[3];
                segment0.start_point[0] = 0.00;
                segment0.start_point[1] = 0.00;
                segment0.start_point[2] = 0.00;
                segment0.end_point = new double[3];
                segment0.end_point[0] = 0.00;
                segment0.end_point[1] = 0.00;
                segment0.end_point[2] = height;

                segment1.start_point = new double[3];
                segment1.start_point[0] = 0.00;
                segment1.start_point[1] = 0.00;
                segment1.start_point[2] = height;
                segment1.end_point = new double[3];
                segment1.end_point[0] = Radius;
                segment1.end_point[1] = 0.00;
                segment1.end_point[2] = height;

                segment2.start_point = new double[3];
                segment2.start_point[0] = Radius;
                segment2.start_point[1] = 0.00;
                segment2.start_point[2] = height;
                segment2.end_point = new double[3];
                segment2.end_point[0] = Radius;
                segment2.end_point[1] = 0.00;
                segment2.end_point[2] = 0.00;

                segment3.start_point = new double[3];
                segment3.start_point[0] = Radius;
                segment3.start_point[1] = 0.00;
                segment3.start_point[2] = 0.00;
                segment3.end_point = new double[3];
                segment3.end_point[0] = 0.00;
                segment3.end_point[1] = 0.00;
                segment3.end_point[2] = 0.00;

                //Объявляем массив тэгов - объектов, которыми оперирует NX
                Tag[] BoltArray = new Tag[5];

                //Записываем прямые в массив тэгов
                theUfSession.Curve.CreateLine(ref segment0, out BoltArray[0]);
                theUfSession.Curve.CreateLine(ref segment1, out BoltArray[1]);
                theUfSession.Curve.CreateLine(ref segment2, out BoltArray[2]);
                theUfSession.Curve.CreateLine(ref segment3, out BoltArray[3]);
                
                //указываем начальную точку, относительно которой будет происходить построение
                // по умолчанию (0,0,0)
                double[] ref_pt1 = new double[3];
                ref_pt1[0] = 0.00;
                ref_pt1[1] = 0.00;
                ref_pt1[2] = 0.00;

                // указываем вектор, относительно которого происходит вращение
                double[] direction1 = { 0.00, 0.00, 1.00 };

                // диапозон вращения
                string[] limit1 = { "0", "360" };

                Tag[] features1;

                // операция вращения
                theUfSession.Modl.CreateRevolved(BoltArray, limit1, ref_pt1,
                direction1, FeatureSigns.Nullsign, out features1);

                Tag[] features2;

/******************Создаём тело вращения (головка болта)******************/
                // Объявляем 4 прямые для описания тела вращения
                UFCurve.Line segment4 = new UFCurve.Line();
                UFCurve.Line segment5 = new UFCurve.Line();
                UFCurve.Line segment6 = new UFCurve.Line();
                UFCurve.Line segment7 = new UFCurve.Line();

                // задаём параметры прямых (start_point - начальная точка (x,y,z), end_point - конечная точка (x,y,z))
                /* Radius * 1.8 - радиус головки относительно основного диаметра, рассчитано согласно госту 7798 (не менее), т.е. 
                /при расхождении значений коэффициент был округлён в большую сторону*/
                /*height+(Radius*1.7) - высота головки, рассчитана аналогично радиусу.*/
                segment4.start_point = new double[3];
                segment4.start_point[0] = 0.00;
                segment4.start_point[1] = 0.00;
                segment4.start_point[2] = height;
                segment4.end_point = new double[3];
                segment4.end_point[0] = Radius * 1.8;           
                segment4.end_point[1] = 0.00;
                segment4.end_point[2] = height;

                segment5.start_point = new double[3];
                segment5.start_point[0] = Radius * 1.8;
                segment5.start_point[1] = 0.00;
                segment5.start_point[2] = height;
                segment5.end_point = new double[3];
                segment5.end_point[0] = Radius * 1.8;
                segment5.end_point[1] = 0.00;
                segment5.end_point[2] = height+(Radius*1.7);

                segment6.start_point = new double[3];
                segment6.start_point[0] = Radius * 1.8;
                segment6.start_point[1] = 0.00;
                segment6.start_point[2] = height + (Radius * 1.7);
                segment6.end_point = new double[3];
                segment6.end_point[0] = 0.00;
                segment6.end_point[1] = 0.00;
                segment6.end_point[2] = height + (Radius * 1.7);

                segment7.start_point = new double[3];
                segment7.start_point[0] = 0.00;
                segment7.start_point[1] = 0.00;
                segment7.start_point[2] = height + (Radius * 1.7);
                segment7.end_point = new double[3];
                segment7.end_point[0] = 0.00;
                segment7.end_point[1] = 0.00;
                segment7.end_point[2] = height;

                //Объявляем массив тэгов - объектов, которыми оперирует NX
                Tag[] BoltArray1 = new Tag[5];

                //Записываем прямые в массив тэгов
                theUfSession.Curve.CreateLine(ref segment4, out BoltArray1[0]);
                theUfSession.Curve.CreateLine(ref segment5, out BoltArray1[1]);
                theUfSession.Curve.CreateLine(ref segment6, out BoltArray1[2]);
                theUfSession.Curve.CreateLine(ref segment7, out BoltArray1[3]);

                //указываем начальную точку, относительно которой будет происходить построение
                // z = height, т.к. головка стыкуется к телу с резьбой
                double[] ref_pt2 = new double[3];
                ref_pt1[0] = 0.00;
                ref_pt1[1] = 0.00;
                ref_pt1[2] = height;

                // указываем вектор, относительно которого происходит вращение
                double[] direction2 = { 0.00, 0.00, 1.00 };

                //диапозон вращения
                string[] limit2 = { "0", "360" };

                // операция вращения
                theUfSession.Modl.CreateRevolved(BoltArray1, limit2, ref_pt2,
                direction2, FeatureSigns.Positive, out features2);

                Tag feat1 = features2[0];
                Tag cyl_tag, obj_id_camf, blend1;
                Tag[] Edge_array_cyl, list1, list2;
                int ecount;

                // вычисляем число рёбер в детали
                theUfSession.Modl.AskFeatBody(feat1, out cyl_tag);
                theUfSession.Modl.AskBodyEdges(cyl_tag, out
                Edge_array_cyl);
                theUfSession.Modl.AskListCount(Edge_array_cyl, out
                ecount);

                //создаём листы для сглаживания
                ArrayList arr_list2 = new ArrayList();

                // добавляем ребро для сглаживания
                for (int ii = 0; ii < ecount; ii++)
                {
                    Tag edge;
                    theUfSession.Modl.AskListItem(Edge_array_cyl, ii, out edge);
                    if (ii == 2)
                    {
                        arr_list2.Add(edge);
                    }
                }

                // конвертируем в лист тэгов
                list2 = (Tag[])arr_list2.ToArray(typeof(Tag));


                // скругление
                /*Аргументами для скругления являются:
                   1. “3” – радиус;
                   2. list2 – массив ребер, на которых необходимо выполнить
                    скругление;
                   3. allow_smooth - Smooth overflow/prevent flag: 0 = Allow this
                   type of blend; 1 = Prevent this type of blend;
                   4. allow_smooth - Cliffedge overflow/prevent flag: 0 = Allow
                   this type of blend; 1 = Prevent this type of blend;
                   5. allow_notch - Notch overflow/prevent flag: 0 = Allow this
                   type of blend; 1 = Prevent this type of blend;
                   6. vrb_tol - Variable radius blend tolerance.
                 */
                int allow_smooth = 0;
                int allow_cliff = 0;
                int allow_notch = 0;
                double vrb_tol = 0.0;

                string rad1 = Convert.ToInt32(Radius).ToString();
                theUfSession.Modl.CreateBlend(rad1, list2, allow_smooth,
                allow_cliff, allow_notch, vrb_tol, out blend1);

/*********************** Добавление символической резьбы ************************************/
                Tag feat = features1[0];
                Tag[] FeatFaces;
                int FacesCount, FaceType, FaceNormDir;
                Tag face, s_face, c_face, feature_eid;

                double[] point = new double[3];
                double[] dir = new double[3];
                double[] box = new double[6];
                double radius, rad;

                s_face = new Tag();
                c_face = new Tag();

                theUfSession.Modl.AskFeatFaces(feat, out FeatFaces);
                theUfSession.Modl.AskListCount(FeatFaces, out
               FacesCount);

                UFModl.SymbThreadData thread = new UFModl.SymbThreadData();
                for (int i = 0; i < FacesCount; i++)
                {
                    theUfSession.Modl.AskListItem(FeatFaces, i, out face);
                    theUfSession.Modl.AskFaceData(face, out FaceType,
                   point, dir, box, out radius, out rad, out FaceNormDir);
                    if (FaceType == 22)
                    {
                        s_face = face;
                    }
                    if (FaceType == 16)
                    {
                        c_face = face;
                     }
                }
                double[] thread_direction = { 0.00, 0.00, 1.00 };
                // направление резьбы
                thread.cyl_face = c_face;
                thread.start_face = s_face;
                thread.axis_direction = thread_direction;
                thread.rotation = 1;
                thread.num_starts = 1;
                thread.length = Convert.ToString(height);// длина резьбы
                thread.form = "Metric";// метрическая система
                thread.major_dia = Convert.ToString(2 * Radius);
                //внешний диаметр резьбы
                thread.minor_dia = Convert.ToString((2 * Radius) -1);
                //внутренний диаметр резьбы
                thread.tapped_dia = Convert.ToString(2 *
                Radius);//диметр резьбы
                thread.pitch = Convert.ToString(step);//шаг резьбы
                thread.angle = "60";//угол резьбы
                try
                {
                    // создание символичной резьбы
                    theUfSession.Modl.CreateSymbThread(ref thread, out
                    feat);
                }
                catch (NXOpen.NXException ex)
                {
                    UI.GetUI().NXMessageBox.Show("Message",
                    NXMessageBox.DialogType.Error, ex.Message);
                }

/******************************************************* Создание паза под отвёртку ***************************************/
                // создание прямых
                UFCurve.Line segment8 = new UFCurve.Line();
                UFCurve.Line segment9 = new UFCurve.Line();
                UFCurve.Line segment10 = new UFCurve.Line();
                UFCurve.Line segment11 = new UFCurve.Line();

                // задаём параметры прямых
                segment8.start_point = new double[3];
                segment8.start_point[0] = Radius * 1.8;
                segment8.start_point[1] = Radius * 0.2;
                segment8.start_point[2] = height + (Radius * 1.7);
                segment8.end_point = new double[3];
                segment8.end_point[0] = Radius * 1.8;
                segment8.end_point[1] = -(Radius * 0.2);
                segment8.end_point[2] = height + (Radius * 1.7);

                segment9.start_point = new double[3];
                segment9.start_point[0] = Radius * 1.8;
                segment9.start_point[1] = -(Radius * 0.2);
                segment9.start_point[2] = height + (Radius * 1.7);
                segment9.end_point = new double[3];
                segment9.end_point[0] = -(Radius * 1.8);
                segment9.end_point[1] = -(Radius * 0.2);
                segment9.end_point[2] = height + (Radius * 1.7);

                segment10.start_point = new double[3];
                segment10.start_point[0] = -(Radius * 1.8);
                segment10.start_point[1] = -(Radius * 0.2);
                segment10.start_point[2] = height + (Radius * 1.7);
                segment10.end_point = new double[3];
                segment10.end_point[0] = -(Radius * 1.8);
                segment10.end_point[1] = Radius * 0.2;
                segment10.end_point[2] = height + (Radius * 1.7);

                segment11.start_point = new double[3];
                segment11.start_point[0] = -(Radius * 1.8);
                segment11.start_point[1] = Radius * 0.2;
                segment11.start_point[2] = height + (Radius * 1.7);
                segment11.end_point = new double[3];
                segment11.end_point[0] = Radius * 1.8;
                segment11.end_point[1] = Radius * 0.2;
                segment11.end_point[2] = height + (Radius * 1.7);

                // создаём массив тэгов и добавляем прямые
                Tag[] BoltArray2 = new Tag[5];

                theUfSession.Curve.CreateLine(ref segment8, out BoltArray2[0]);
                theUfSession.Curve.CreateLine(ref segment9, out BoltArray2[1]);
                theUfSession.Curve.CreateLine(ref segment10, out BoltArray2[2]);
                theUfSession.Curve.CreateLine(ref segment11, out BoltArray2[3]);

                // проводим операцию вычитания
                string glub = Convert.ToInt32(height * 0.09).ToString();
                string taper_angle = "0.0";
                string[] limit3 = { "0", glub };
                double[] ref_pt3 = new double[3];
                double[] direction3 = { 0.0, 0.0, -1.0 };
                theUfSession.Modl.CreateExtruded(BoltArray2, taper_angle,
                limit3, ref_pt3, direction3, FeatureSigns.Negative, out features2);

            }
            catch (NXOpen.NXException ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
                return 0;
        }

    }
}
