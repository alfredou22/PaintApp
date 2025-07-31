using System;
using System.Drawing;

namespace PaintApp
{
    public class Circulo : Figura
    {
        public override void Dibujar(Graphics g)
        {
            var radio = Math.Min(
                Math.Abs(PuntoFinal.X - PuntoInicial.X),
                Math.Abs(PuntoFinal.Y - PuntoInicial.Y));
            var rect = new Rectangle(
                PuntoInicial.X - radio,
                PuntoInicial.Y - radio,
                radio * 2,
                radio * 2);

            using (var pen = new Pen(Seleccionada ? Color.Red : this.Color, 2))
                g.DrawEllipse(pen, rect);
        }

        public override bool Contiene(Point p)
        {
            var dx = p.X - PuntoInicial.X;
            var dy = p.Y - PuntoInicial.Y;
            var radio = Math.Min(
                Math.Abs(PuntoFinal.X - PuntoInicial.X),
                Math.Abs(PuntoFinal.Y - PuntoInicial.Y));
            return dx * dx + dy * dy <= radio * radio;
        }
    }
}
