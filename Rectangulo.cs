using System;
using System.Drawing;

namespace PaintApp
{
    public class Rectangulo : Figura
    {
        public override void Dibujar(Graphics g)
        {
            var rect = new Rectangle(
                Math.Min(PuntoInicial.X, PuntoFinal.X),
                Math.Min(PuntoInicial.Y, PuntoFinal.Y),
                Math.Abs(PuntoFinal.X - PuntoInicial.X),
                Math.Abs(PuntoFinal.Y - PuntoInicial.Y));

            using (var pen = new Pen(Seleccionada ? Color.Red : this.Color, 2))
                g.DrawRectangle(pen, rect);
        }

        public override bool Contiene(Point p)
        {
            var rect = new Rectangle(
                Math.Min(PuntoInicial.X, PuntoFinal.X),
                Math.Min(PuntoInicial.Y, PuntoFinal.Y),
                Math.Abs(PuntoFinal.X - PuntoInicial.X),
                Math.Abs(PuntoFinal.Y - PuntoInicial.Y));
            return rect.Contains(p);
        }
    }
}
