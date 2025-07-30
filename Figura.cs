using System.Drawing;

namespace PaintApp
{
    public abstract class Figura
    {
        public Point PuntoInicial { get; set; }
        public Point PuntoFinal  { get; set; }
        public bool Seleccionada { get; set; } = false;

        public abstract void Dibujar(Graphics g);
        public abstract bool Contiene(Point p);
    }
}
