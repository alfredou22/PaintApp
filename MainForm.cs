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
        private Color colorSeleccionado = Color.Black;
        private Panel panelColores;

        enum Estado { Ninguno, Dibujando, Seleccionando }

        private readonly Color[] colores = new Color[]
        {
            Color.Black, Color.Gray, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Teal, Color.Blue, Color.Purple,
            Color.DarkGray, Color.Brown, Color.Pink, Color.Gold, Color.Khaki, Color.Lime, Color.LightGreen, Color.SkyBlue, Color.Plum
        };

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
                BackColor = Color.LightSteelBlue,
                ForeColor = Color.DarkSlateGray
            };
            cbFiguras.Items.AddRange(new string[] { "Selección", "Rectángulo", "Círculo", "Triángulo", "Línea" });
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
            eliminar.ForeColor = Color.DarkRed; //NO FUNCIONA
            eliminar.Click += (s, e) => { figuras.RemoveAll(f => f.Seleccionada); Invalidate(); };
            menuContextual.Items.Add(eliminar);

            //Panel de colores
            panelColores = new Panel
            {
                Width = 320,
                Height = 70,
                Top = 10,
                Left = this.ClientSize.Width - 340,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent
            };
            this.Controls.Add(panelColores);
            panelColores.Paint += PanelColores_Paint;
            panelColores.MouseClick += PanelColores_MouseClick;

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
                "Triángulo"  => new Triangulo(),
                "Círculo"    => new Circulo(),
                "Línea"      => new Linea(),
                _            => new Rectangulo(),
            };
            f.PuntoInicial = p; // Punto inicial es el punto donde se hace clic
            f.PuntoFinal = p; // Punto final inicial es el mismo punto  
            f.Color = colorSeleccionado; // Asignar el color seleccionado
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

        private void PanelColores_Paint(object sender, PaintEventArgs e)
        {
            int size = 24, margin = 8, cols = 9;
            for (int i = 0; i < colores.Length; i++)
            {
                int row = i / cols, col = i % cols;
                var rect = new Rectangle(col * (size + margin) + 8, row * (size + margin) + 8, size, size);
                using (var brush = new SolidBrush(colores[i]))
                    e.Graphics.FillEllipse(brush, rect);
                if (colores[i] == colorSeleccionado)
                    e.Graphics.DrawEllipse(new Pen(Color.Black, 3), rect);
                else
                    e.Graphics.DrawEllipse(Pens.DarkGray, rect);
            }
            // Texto
            var sf = new StringFormat { Alignment = StringAlignment.Center };
            e.Graphics.DrawString("Colors", this.Font, Brushes.Gray, panelColores.Width / 2, 2 * (size + margin) + 30, sf);
        }

        private void PanelColores_MouseClick(object sender, MouseEventArgs e)
        {
            int size = 24, margin = 8, cols = 9;
            for (int i = 0; i < colores.Length; i++)
            {
                int row = i / cols, col = i % cols;
                var rect = new Rectangle(col * (size + margin) + 8, row * (size + margin) + 8, size, size);
                if (rect.Contains(e.Location))
                {
                    colorSeleccionado = colores[i];
                    panelColores.Invalidate();
                    break;
                }
            }
        }
    }
}
