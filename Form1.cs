using System;
using System.Windows.Forms;

namespace Tron
{
    public partial class Form1 : Form
    {
        private Nodo[,] matriz;
        private int gridSize = 23; // Tamaño de la matriz 5x5

        public Form1()
        {
            InitializeComponent();
            CrearMatriz(gridSize, gridSize);
            DibujarMatriz();
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
            int casillaSize = 30;
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
    }
}
