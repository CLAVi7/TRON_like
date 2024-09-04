using System;
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

    public Form1()
    {
    InitializeComponent();
    
    // Configura el tamaño del formulario
    // Ajusta el tamaño según tus necesidades

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

    // Configura la dirección inicial de la moto (por ejemplo, hacia la derecha)
    moto.Direccion = Direction.Right;

    
    

    // Configura el Timer 
    
    timer.Interval = 600 / moto.Velocidad; // Intervalo en milisegundos (ajusta según sea necesario)
    timer.Tick += Timer_Tick;
    timer.Start();
    this.ClientSize = new System.Drawing.Size(1200, 700);
        
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (!juegoTerminado)
        {
            moto.Mover();
            
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

    // Método para detener el juego
    public void DetenerJuego()
    {
        juegoTerminado = true;
        timer.Stop(); // Detener el temporizador
        MessageBox.Show("¡Perdiste!", "Fin del Juego", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
    }
}
}