using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PaintApp
{
    public class MainForm : Form
    {
        private ComboBox cbFiguras;
        private Label lblCoords;
        private ContextMenuStrip menuContextual;
        private List<Figura> figuras = new List<Figura>();
        private Figura figuraActual;
        private Estado estadoActual = Estado.Ninguno;

        enum Estado { Ninguno, Dibujando, Seleccionando }

        public MainForm()
        {
            this.Text = "Mini Paint by Alfredou22";
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;

            // Personalización del fondo y bordes del formulario
            this.BackColor = Color.WhiteSmoke; // Cambia el color de fondo
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Bordes fijos
            this.MaximizeBox = true; // Deshabilita o no maximizar

            // Personalización del ComboBox
            cbFiguras = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.LightYellow,
                ForeColor = Color.DarkSlateGray
            };
            cbFiguras.Items.AddRange(new string[] { "Selección", "Rectángulo", "Línea", "Círculo", "Triángulo" });
            cbFiguras.SelectedIndex = 0;
            this.Controls.Add(cbFiguras);

            // Personalización del Label de coordenadas
            lblCoords = new Label
            {
                Location = new Point(200, 10),
                Width = 200,
                Font = new Font("Consolas", 10, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblCoords);

            // Personalización del menú contextual
            menuContextual = new ContextMenuStrip();
            var eliminar = new ToolStripMenuItem("Eliminar");
            eliminar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            eliminar.ForeColor = Color.Red;
            eliminar.Click += (s, e) => { figuras.RemoveAll(f => f.Seleccionada); Invalidate(); };
            menuContextual.Items.Add(eliminar);

            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;
            this.Paint += MainForm_Paint;
        }

        private Figura CrearFigura(string tipo, Point p)
        {
            Figura f = tipo switch
            {
                "Rectángulo" => new Rectangulo(),
                "Línea"       => new Linea(),
                "Círculo"     => new Circulo(),
                "Triángulo"    => new Triangulo(),
                _             => new Rectangulo(),
            };
            f.PuntoInicial = p;
            f.PuntoFinal = p;
            return f;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (cbFiguras.SelectedItem?.ToString() == "Selección")
            {
                // Desseleccionar todas las figuras
                foreach (var figura in figuras)
                    figura.Seleccionada = false;

                // Seleccionar figura bajo el cursor
                foreach (var figura in figuras)
                {
                    if (figura.Contiene(e.Location))
                    {
                        figura.Seleccionada = true;

                        // Construir detalles
                        string tipo = figura.GetType().Name;
                        string detalles = $"Tipo: {tipo}\n" +
                                          $"Punto Inicial: {figura.PuntoInicial}\n" +
                                          $"Punto Final: {figura.PuntoFinal}";

                        // Crear menú contextual dinámico
                        var menu = new ContextMenuStrip();
                        var detallesItem = new ToolStripMenuItem(detalles) { Enabled = false };
                        var eliminarItem = new ToolStripMenuItem("Eliminar");
                        eliminarItem.Click += (s, args) =>
                        {
                            figuras.Remove(figura);
                            Invalidate();
                        };
                        menu.Items.Add(detallesItem);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(eliminarItem);

                        // Mostrar menú contextual
                        menu.Show(this, e.Location);

                        Invalidate();
                        break;
                    }
                }
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                figuraActual = CrearFigura(cbFiguras.SelectedItem.ToString(), e.Location);
                estadoActual = Estado.Dibujando;
            }
            else if (e.Button == MouseButtons.Right)
            {
                foreach (var figura in figuras)
                {
                    if (figura.Contiene(e.Location))
                    {
                        figura.Seleccionada = true;
                        menuContextual.Show(this, e.Location);
                        break;
                    }
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            lblCoords.Text = $"X: {e.X}, Y: {e.Y}";
            if (estadoActual == Estado.Dibujando && figuraActual != null)
            {
                figuraActual.PuntoFinal = e.Location;
                Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (estadoActual == Estado.Dibujando && figuraActual != null)
            {
                figuraActual.PuntoFinal = e.Location;
                figuras.Add(figuraActual);
                figuraActual = null;
                estadoActual = Estado.Ninguno;
                Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            foreach (var figura in figuras)
                figura.Dibujar(e.Graphics);
            figuraActual?.Dibujar(e.Graphics);
        }
    }
}
