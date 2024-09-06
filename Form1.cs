using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tron
{
    public partial class Form1 : Form
    {
        public Label CombustibleLabel { get; private set; }
        private Nodo[,] matriz;
        private Moto moto;
        private int gridSize = 65; // Tamaño de la matriz 65x65
        private bool juegoTerminado = false; // Variable para controlar el estado del juego
        private System.Windows.Forms.Timer timer; // Temporizador para el movimiento de la moto
        private System.Windows.Forms.Timer itemTimer; // Temporizador para la generación de items
        private Queue<Item> items = new Queue<Item>(); // Cola de items
        private Random random = new Random();
        private Enemigo enemigo;

        public Form1()
        {
            InitializeComponent();

            // Inicializa y agrega el Label
            CombustibleLabel = new Label();
            CombustibleLabel.AutoSize = true;
            CombustibleLabel.Location = new System.Drawing.Point(1000, 10); // Ajusta la posición según sea necesario
            CombustibleLabel.Text = "Combustible: 100"; // Texto inicial
            this.Controls.Add(CombustibleLabel);

            CrearMatriz(gridSize, gridSize);
            DibujarMatriz();

            // Inicializa la moto en una posición específica
            Casilla posicionInicial = matriz[4, 6].Casilla; // Posición central
            moto = new Moto(posicionInicial, tamañoEstela: 4, matriz, this); // Pasar 'this'

            Casilla posicionInicialE = matriz[20, 20].Casilla;
            enemigo = new Enemigo(posicionInicialE, tamañoEstela: 4, matriz, this, Direction.Right);


            // Configura la dirección inicial de la moto (por ejemplo, hacia la derecha)
            moto.Direccion = Direction.Right;

            // Configura el Timer para el movimiento de la moto
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 300 / moto.Velocidad; // Intervalo en milisegundos (ajusta según sea necesario)
            timer.Tick += Timer_Tick;
            timer.Start();

            // Configura el Timer para generar items
            itemTimer = new System.Windows.Forms.Timer();
            itemTimer.Interval = 5000; // Intervalo en milisegundos (5 segundos)
            itemTimer.Tick += GenerarItemAleatorio;
            itemTimer.Start();

            this.ClientSize = new System.Drawing.Size(1200, 700);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!juegoTerminado)
            {
                moto.Mover();
                enemigo.Mover();
            }
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
            int casillaSize = 10;
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!juegoTerminado) // Solo permitir cambios de dirección si el juego no ha terminado
            {
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

        private void GenerarItemAleatorio(object sender, EventArgs e)
        {
            // Lista de items disponibles
            List<Item> itemsDisponibles = new List<Item>
            {
                new CeldaCombustible(),
                new CrecimientoEstela(),
                new Bomba()
            };

            // Selecciona una posición aleatoria en el grid
            int x = random.Next(1, gridSize - 1); // Evita los bordes
            int y = random.Next(1, gridSize - 1);

            // Selecciona un item aleatorio
            Item itemAleatorio = itemsDisponibles[random.Next(itemsDisponibles.Count)];

            // Coloca el item en el grid y añade a la cola
            ColocarItemEnGrid(x, y, itemAleatorio);
            items.Enqueue(itemAleatorio);
            
        }

        private void ColocarItemEnGrid(int x, int y, Item item)
        {
            // Primero limpia la casilla donde se va a colocar el item, para asegurarse de que no esté oculta
            if (matriz[x, y].Casilla is Libre)
            {
                item.X = x;
                item.Y = y;

                item.Color = GetColorParaItem(item);
                matriz[x, y].Casilla.BackColor = item.Color; 
                // Coloca el item en la posición (x, y) del grid
                matriz[x, y].Casilla = item;
                item.Width = 10; 
                item.Height = 10;
                item.Left = x * 10;
                item.Top = y * 10;
                this.Controls.Add(item);
                
                this.Invalidate(); 
            }
            
        }


        private Color GetColorParaItem(Item item)
        {
            if (item is CeldaCombustible) return Color.Green;
            if (item is CrecimientoEstela) return Color.Cyan;
            if (item is Bomba) return Color.Red;
            return Color.Transparent;
        }
        

        
        // Método para detener el juego
        public void DetenerJuego()
        {
            juegoTerminado = true;
            timer.Stop(); // Detener el temporizador del movimiento
            itemTimer.Stop(); // Detener el temporizador de items
            MessageBox.Show("¡Perdiste!", "Fin del Juego", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
    }
}
