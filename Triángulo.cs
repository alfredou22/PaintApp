using System;
using System.Drawing;

namespace PaintApp
{
    public class Triangulo : Figura
    {
        public override void Dibujar(Graphics g)
        {
            // Define los tres puntos del triángulo usando PuntoInicial y PuntoFinal
            Point p1 = PuntoInicial;
            Point p2 = new Point(PuntoFinal.X, PuntoInicial.Y);
            Point p3 = new Point((PuntoInicial.X + PuntoFinal.X) / 2, PuntoFinal.Y);

            Point[] puntos = { p1, p2, p3 };

            if (Rellenar)
            {
                using (var brush = new SolidBrush(this.Color))
                    g.FillPolygon(brush, puntos);
            }

            using (var pen = new Pen(Seleccionada ? Color.Red : this.Color, 2))
                g.DrawPolygon(pen, puntos);
        }

        public override bool Contiene(Point p)
        {
            // Define los tres puntos del triángulo
            Point p1 = PuntoInicial;
            Point p2 = new Point(PuntoFinal.X, PuntoInicial.Y);
            Point p3 = new Point((PuntoInicial.X + PuntoFinal.X) / 2, PuntoFinal.Y);

            // Algoritmo de barycentric coordinates para verificar si el punto está dentro del triángulo
            float Area(Point a, Point b, Point c) =>
                Math.Abs((a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) / 2f);

            float areaTotal = Area(p1, p2, p3);
            float area1 = Area(p, p2, p3);
            float area2 = Area(p1, p, p3);
            float area3 = Area(p1, p2, p);

            return Math.Abs(areaTotal - (area1 + area2 + area3)) < 1.0f;
        }
    }
}