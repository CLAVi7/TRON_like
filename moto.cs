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
        public Queue<Item> Items { get; private set; } = new Queue<Item>();
        private bool aplicandoItem;
        private System.Windows.Forms.Timer itemTimer;
        public List<string> Poderes { get; set; }
        public LinkedList<Casilla> Estela { get; set; }
        public Direction Direccion { get; set; }
        public Nodo[,] Matriz { get; set; }
        public LinkedListNode<Casilla> Head { get; private set; }
        private Form1 form;
        private bool juegoTerminado;
        private static Random random = new Random(); // Instancia de Random para generar números aleatorios
        private int contadorMovimiento;

        public Moto(Casilla posicionInicial, int tamañoEstela, Nodo[,] matriz, Form1 form)
        {
            // Asignar una velocidad aleatoria entre 1 y 10
            Velocidad = random.Next(1, 2); // 11 es excluyente, así que el rango es [1, 10]
            contadorMovimiento = 0;
            TamañoEstela = tamañoEstela;
            Combustible = 100;
            
            Poderes = new List<string>();
            Estela = new LinkedList<Casilla>();
            Matriz = matriz;
            this.form = form; // Asignar el formulario
            juegoTerminado = false;

            // Inicializar la moto en la posición inicial
            Head = Estela.AddFirst(posicionInicial);
            Head.Value.BackColor = Color.Red; // Color de la moto
            Direccion = Direction.Right; // Dirección inicial
            itemTimer = new System.Windows.Forms.Timer();
            itemTimer.Interval = 1000; // 1 segundo de cooldown
            itemTimer.Tick += AplicarSiguienteItem;
           
        }

        public void Mover()
        {
            if (juegoTerminado)
                return; // No hacer nada si el juego ya terminó

            // Obtener la nueva posición en función de la dirección
            Casilla nuevaPosicion = ObtenerNuevaPosicion();

            // Si la nueva posición es inválida (fuera de límites o colisión con pared)
            if (nuevaPosicion == null || nuevaPosicion is Pared)
            {
                form.DetenerJuego(); // Detener el juego
                juegoTerminado = true; // Actualizar el estado del juego
                return;
            }

            // Si la nueva posición es un ítem, manejar la recogida del ítem
            if (nuevaPosicion is Item item)
            {
                nuevaPosicion.BackColor = Color.Black;
                item.Aplicar(this); // Aplicar el efecto del ítem
                // Limpiar la casilla del grid donde estaba el ítem
                 // Restaurar el color original de la casilla

                // Avanzar a la siguiente posición después del ítem
                nuevaPosicion = ObtenerNuevaPosicion();
                if (nuevaPosicion == null || nuevaPosicion is Pared)
                {
                    form.DetenerJuego(); // Detener el juego si la siguiente posición es inválida
                    juegoTerminado = true; // Actualizar el estado del juego
                    return;
                }
            }

            // Mueve la moto en la dirección actual
            // Añadir la nueva posición al frente de la lista de la estela
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

            // Aplicar el ítem si hay alguno en la lista
            if (Items.Count > 0)
            {
                Item itemEnCola = Items.Dequeue(); // Renombrar aquí para evitar conflicto
                itemEnCola.Aplicar(this);
            }
        }



        private Casilla ObtenerNuevaPosicion()
        {
            int x = Head.Value.Left / 10; // Tamaño de la casilla (ajustar según sea necesario)
            int y = Head.Value.Top / 10;

            // Calcular la nueva posición en función de la dirección y la velocidad
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
            
            if (nuevaPosicion is Item)
            {
            return nuevaPosicion;
            }
            return nuevaPosicion;
        }

        public void AgregarItem(Item item)
            {
                Items.Enqueue(item);
                if (!aplicandoItem)
                {
                    aplicandoItem = true;
                    itemTimer.Start();
                }
            }

        private void AplicarSiguienteItem(object sender, EventArgs e)
            {
                if (Items.Count > 0)
                {
                    Item item = Items.Dequeue();
                    item.Aplicar(this);

                    // Actualiza el label o realiza otras acciones necesarias

                    if (Items.Count == 0)
                    {
                        itemTimer.Stop();
                        aplicandoItem = false;
                    }
                }
            }
        }
}


