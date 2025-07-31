using System;
using System.Drawing;

namespace PaintApp
{
    public class Linea : Figura
    {
        public override void Dibujar(Graphics g)
        {
            using (var pen = new Pen(Seleccionada ? Color.Red : this.Color, 2))
                g.DrawLine(pen, PuntoInicial, PuntoFinal);
        }

        public override bool Contiene(Point p)
        {
            float dx = PuntoFinal.X - PuntoInicial.X;
            float dy = PuntoFinal.Y - PuntoInicial.Y;
            float lengthSq = dx * dx + dy * dy;
            if (lengthSq == 0) return false;
            float t = ((p.X - PuntoInicial.X) * dx + (p.Y - PuntoInicial.Y) * dy) / lengthSq;
            if (t < 0 || t > 1) return false;
            float projX = PuntoInicial.X + t * dx;
            float projY = PuntoInicial.Y + t * dy;
            float distancia = MathF.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
            return distancia <= 5;
        }
    }
}
