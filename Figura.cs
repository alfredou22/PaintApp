using System.Drawing;

namespace PaintApp
{
    public abstract class Figura
    {
        public Point PuntoInicial { get; set; }
        public Point PuntoFinal  { get; set; }
        public bool Seleccionada { get; set; } = false; // propiedad para indicar si la figura está seleccionada

        public Color Color {get; set; } = Color.Black; //propiedad para el selector de colores

        public bool Rellenar { get; set; } = false; // propiedad para el relleno de figuras

        public abstract void Dibujar(Graphics g);
        public abstract bool Contiene(Point p);
    }
}
