using System.Collections.Generic;
using System.Drawing;

namespace Tron
{
    public class Moto
    {
        public int Velocidad { get; set; }
        public int TamañoEstela { get; set; }
        public int Combustible { get; set; }
        public List<string> Items { get; set; }
        public List<string> Poderes { get; set; }
        public LinkedList<Casilla> Estela { get; set; }
        public Casilla PosicionActual { get; set; }
        public Direction Direccion { get; set; }
        public Nodo[,] Matriz { get; set; }
        private bool haMovido;

        public Moto(Casilla posicionInicial, int velocidad, int tamañoEstela, Nodo[,] matriz)
        {
            Velocidad = velocidad;
            TamañoEstela = tamañoEstela;
            Combustible = 100;
            Items = new List<string>();
            Poderes = new List<string>();
            Estela = new LinkedList<Casilla>();
            Matriz = matriz;

            // Inicializar la moto en la posición inicial
            PosicionActual = posicionInicial;
            PosicionActual.BackColor = Color.Red; // Color de la moto
            Direccion = Direction.Right; // Dirección inicial
            haMovido = false; // La moto aún no se ha movido
        }

        public void Mover()
        {
            if (Combustible <= 0)
                return; // No mover si no hay combustible

            // Mueve la moto en la dirección actual
            Casilla nuevaPosicion = ObtenerNuevaPosicion();

            // Si la nueva posición es válida, actualizar la posición actual y estela
            if (nuevaPosicion != null)
            {
                // Limpiar la posición actual de la moto
                PosicionActual.BackColor = Color.Black;

                // Solo añadir estela después del primer movimiento
                if (haMovido)
                {
                    AñadirEstela(nuevaPosicion);
                }

                // Mover la moto a la nueva posición
                PosicionActual = nuevaPosicion;
                PosicionActual.BackColor = Color.Red; // Color de la moto

                // Marcar que la moto se ha movido
                haMovido = true;

                Combustible--; // Reducir el combustible
            }
        }

        private void AñadirEstela(Casilla casilla)
        {
            if (Estela.Count >= TamañoEstela)
            {
                Casilla casillaAntigua = Estela.First.Value;
                Estela.RemoveFirst();
                casillaAntigua.BackColor = Color.Black; // Restaurar el color anterior
            }

            Estela.AddLast(casilla);
            casilla.BackColor = Color.Yellow; // Color de la estela
        }

        private Casilla ObtenerNuevaPosicion()
        {
            int x = PosicionActual.Left / 10; // Tamaño de la casilla (ajustar según sea necesario)
            int y = PosicionActual.Top / 10;

            switch (Direccion)
            {
                case Direction.Up: y--; break;
                case Direction.Down: y++; break;
                case Direction.Left: x--; break;
                case Direction.Right: x++; break;
            }

            // Verificar límites del grid
            if (x < 0 || x >= Matriz.GetLength(1) || y < 0 || y >= Matriz.GetLength(0))
            {
                // La moto ha salido del grid, puedes agregar lógica para manejar este caso (p.ej., detener el juego)
                return null;
            }

            return Matriz[y, x].Casilla; // Obtener la nueva casilla
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}

