using System;
using System.Windows.Forms;

namespace Tron
{
    public partial class Form1 : Form
    {
        private Nodo[,] matriz;
        private Moto moto;
        private int gridSize = 65; // Tamaño de la matriz 23x23

        public Form1()
        {
            InitializeComponent();
            CrearMatriz(gridSize, gridSize);
            DibujarMatriz();

            // Inicializa la moto en una posición específica
            Casilla posicionInicial = matriz[gridSize / 2, gridSize / 2].Casilla; // Posición central
            moto = new Moto(posicionInicial, velocidad: 1, tamañoEstela: 3, matriz);

            // Configura la dirección inicial de la moto (por ejemplo, hacia la derecha)
            moto.Direccion = Direction.Right;

            // Configura el Timer (usa el definido en el designer)
            timer.Interval = 100; // Intervalo en milisegundos (ajusta según sea necesario)
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void CrearMatriz(int filas, int columnas)
        {
            matriz = new Nodo[filas, columnas];

            // Crear nodos y conectarlos
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Casilla casilla;
                    if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)
                    {
                        casilla = new Pared(); // Bordes son paredes
                    }
                    else
                    {
                        casilla = new Libre(); // Interior es libre
                    }

                    matriz[i, j] = new Nodo(casilla);

                    if (i > 0)
                    {
                        matriz[i, j].Arriba = matriz[i - 1, j];
                        matriz[i - 1, j].Abajo = matriz[i, j];
                    }

                    if (j > 0)
                    {
                        matriz[i, j].Izquierda = matriz[i, j - 1];
                        matriz[i, j - 1].Derecha = matriz[i, j];
                    }
                }
            }
        }

        private void DibujarMatriz()
        {
            int casillaSize = 10; // Ajusta el tamaño si es necesario
            this.ClientSize = new System.Drawing.Size(gridSize * casillaSize, gridSize * casillaSize);

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Casilla casilla = matriz[i, j].Casilla;
                    casilla.Width = casillaSize;
                    casilla.Height = casillaSize;
                    casilla.Left = j * casillaSize;
                    casilla.Top = i * casillaSize;
                    this.Controls.Add(casilla);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            moto.Mover();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (moto.Direccion != Direction.Down) moto.Direccion = Direction.Up;
                    break;
                case Keys.Down:
                    if (moto.Direccion != Direction.Up) moto.Direccion = Direction.Down;
                    break;
                case Keys.Left:
                    if (moto.Direccion != Direction.Right) moto.Direccion = Direction.Left;
                    break;
                case Keys.Right:
                    if (moto.Direccion != Direction.Left) moto.Direccion = Direction.Right;
                    break;
            }
        }
    }
}
