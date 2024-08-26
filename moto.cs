using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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
        public Direction Direccion { get; set; }
        public Nodo[,] Matriz { get; set; }
        public LinkedListNode<Casilla> Head { get; private set; }
        private Form1 form;
        private bool juegoTerminado;
        private static Random random = new Random(); // Instancia de Random para generar números aleatorios

        public Moto(Casilla posicionInicial, int tamañoEstela, Nodo[,] matriz, Form1 form)
        {
            // Asignar una velocidad aleatoria entre 1 y 10
            Velocidad = random.Next(1, 4); // 11 es excluyente, así que el rango es [1, 10]

            TamañoEstela = tamañoEstela;
            Combustible = 100;
            Items = new List<string>();
            Poderes = new List<string>();
            Estela = new LinkedList<Casilla>();
            Matriz = matriz;
            this.form = form; // Asignar el formulario
            juegoTerminado = false;

            // Inicializar la moto en la posición inicial
            Head = Estela.AddFirst(posicionInicial);
            Head.Value.BackColor = Color.Red; // Color de la moto
            Direccion = Direction.Right; // Dirección inicial
           
        }

        public void Mover()
        {
            if (juegoTerminado)
                return; // No hacer nada si el juego ya terminó

            // Mueve la moto en la dirección actual
            Casilla nuevaPosicion = ObtenerNuevaPosicion();

            if (nuevaPosicion is Pared)
            {
                form.DetenerJuego(); // Detener el juego
                juegoTerminado = true; // Actualizar el estado del juego
                return;
            }

            // Si la nueva posición es válida, actualizar la posición actual y estela
            if (nuevaPosicion != null)
            {
                // Añadir la nueva posición al frente de la lista
                Estela.AddFirst(nuevaPosicion);
                nuevaPosicion.BackColor = Color.Red; // Color de la moto

                // Actualizar el Head
                Head = Estela.First;

                // Eliminar el último nodo solo si la estela es más larga que el tamaño permitido
                while (Estela.Count > TamañoEstela)
                {
                    LinkedListNode<Casilla> nodoAntiguo = Estela.Last;
                    Estela.RemoveLast();
                    nodoAntiguo.Value.BackColor = Color.Black; // Restaurar el color anterior
                }

                // Cambiar el color de la estela (los nodos que no son la moto)
                foreach (var nodo in Estela)
                {
                    if (nodo != Head.Value)
                    {
                        nodo.BackColor = Color.Yellow; // Color de la estela
                    }
                }

                Combustible--; // Reducir el combustible
            }
        }


        private Casilla ObtenerNuevaPosicion()
        {
            int x = Head.Value.Left / 10; // Tamaño de la casilla (ajustar según sea necesario)
            int y = Head.Value.Top / 10;

            for (int i = 0; i < Velocidad; i++)
            {
                switch (Direccion)
                {
                    case Direction.Up: y--; break;
                    case Direction.Down: y++; break;
                    case Direction.Left: x--; break;
                    case Direction.Right: x++; break;
                }
                if (x < 0 || x >= Matriz.GetLength(1) || y < 0 || y >= Matriz.GetLength(0)) // Verificar límites del grid
                {
                    return null;
                }
                Casilla nuevaPosicion = Matriz[y, x].Casilla;
                if (nuevaPosicion is Pared)
                {
                    return nuevaPosicion;
                }
            }
    
            return Matriz[y, x].Casilla; // Obtener la nueva casilla
        }
    }
}



