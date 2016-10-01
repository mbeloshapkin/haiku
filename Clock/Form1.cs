using System;
using System.Drawing;
using System.Windows.Forms;

namespace Clock {
    public partial class Form1 : Form {
        SizeF c;  // Ось стрелок. 
        // Координаты всех стрелок ровно в полночь. С наступающим НОВЫМ ГОДОМ!
        PointF p0; // Короткий конец всех стрелок
        PointF pH; // Длинный конец часовой стрелки
        PointF pM; // Длинный конец минутной и секундной стрелок
        PointF pD0, pD1; // деление циферблата
        DateTime theTime = DateTime.Now;

        public Form1() {
            InitializeComponent();
            // Вычисляем координаты стрелок и делений в вертикальном положении            
            c = new SizeF(ClientSize.Width / 2f, ClientSize.Height / 2f);
            float r = Math.Min(ClientSize.Width, ClientSize.Height) / 2.5f; // Радиус циферблата чуть меньше половины формы
            p0 = new PointF(0, -r * .2f); pH = new PointF(0, r * .5f); 
            pM = new PointF(0, r * .9f);
            pD0 = new PointF(0, .8f * r); pD1 = new PointF(0, r);
            timer1.Start();            
        }

        // Рисование ручкой pen чёрточки от p0 до p1, развёрнутой относительно оси стрелок на угол a  
        void DrawDash(Graphics g, Pen pen, PointF p0, PointF p1, float a) {
            // Преобразования такие, потому что грёбаная ось Y GDI направлена вниз. А не потому, что им так положено.
            float a1 = -(float)Math.Cos(a), b1 = (float)Math.Sin(a),
                a2 = -(float)Math.Sin(a), b2 = -(float)Math.Cos(a);
            g.DrawLine(pen,
                new PointF(p0.X * a1 + p0.Y * b1, p0.X * a2 + p0.Y * b2) + c,
                new PointF(p1.X * a1 + p1.Y * b1, p1.X * a2 + p1.Y * b2) + c);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            // Рисуем циферблат
            // 12 часов пожирнее
            DrawDash(e.Graphics, new Pen(Brushes.DarkGray, 10), pD0, pD1, 0);
            // Остальной циферблат. Я сократил дроби 2п/12 и 2п/60, ну вы поняли. Это же минималистичный код.
            for (int ct = 1; ct < 12; ct++)
                DrawDash(e.Graphics, new Pen(Brushes.DarkGray, 3), pD0, pD1, ct * (float)Math.PI / 6f);
            // Часовая стрелка. Танцуем с бубном, чтобы она не перескакивала по делениям а аналогово крутилась            
            DrawDash(e.Graphics, new Pen(Brushes.Black, 4), p0, pH, 
                (theTime.Hour * 60f + theTime.Minute) * (float)Math.PI / 360f);
            // Остальные стрелки. Фиг с ними, пусть перескакивают.
            DrawDash(e.Graphics, new Pen(Brushes.Black, 2), p0, pM, theTime.Minute * (float)Math.PI / 30f);
            DrawDash(e.Graphics, new Pen(Brushes.Red, 1), p0, pM, theTime.Second * (float)Math.PI / 30f); // красненькая!
        }

        private void timer1_Tick(object sender, EventArgs e) {
            DateTime now = DateTime.Now;
            if (now.Second != theTime.Second) { theTime = now; Invalidate(); }            
        }
    }
}
