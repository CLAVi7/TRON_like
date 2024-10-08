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
        public LinkedListNode<Casilla> Head { get; set; }
        public Form1 form;
        public bool juegoTerminado;
        private static Random random = new Random(); // Instancia de Random para generar números aleatorios
        public int contadorMovimiento;
        public bool esJugador;
        public bool IsAlive;

        public Moto(Casilla posicionInicial, int tamañoEstela, Nodo[,] matriz, Form1 form, bool esJugador)
        {
            // Asignar una velocidad aleatoria entre 1 y 10
            Velocidad = random.Next(1, 11); // 11 es excluyente, así que el rango es [1, 10]
            contadorMovimiento = 0;
            TamañoEstela = tamañoEstela;
            Combustible = 100;
            IsAlive=true;
            
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
        public bool DetectarColisionConEstela(Casilla nuevaPosicion, List<Moto> motos)
        {
            // Verificar colisión con la propia estela
            foreach (var nodoEstela in this.Estela)
            {
                if (nodoEstela == nuevaPosicion)
                {   
                    return true; // Colisión con la propia estela detectada
                }
            }

            // Verificar colisión con la cabeza o la estela de otras motos
            foreach (var otraMoto in motos)
            {
                

                // Colisión con la cabeza de otra moto
                if (otraMoto.Head.Value == nuevaPosicion)
                {
                    
                    return true; // Colisión detectada con la cabeza de otra moto
                }

                // Colisión con la estela de otra moto
                foreach (var nodoEstela in otraMoto.Estela)
                {
                    if (nodoEstela == nuevaPosicion)
                    {
                        
                        return true; // Colisión detectada con la estela de otra moto
                    }
                }
            }
            return false; // No hay colisión
        }


        public virtual void Mover(List<Moto> motos)
        {
            if (juegoTerminado)
                return; // No hacer nada si el juego ya terminó

            // Obtener la nueva posición en función de la dirección
            Casilla nuevaPosicion = ObtenerNuevaPosicion();
            contadorMovimiento++;
            if (contadorMovimiento >= 5)
            {
                Combustible--;
                contadorMovimiento = 0; // Reiniciar el contador
                form.CombustibleLabel.Text = $"Combustible: {Combustible}";
            }

            if (Combustible <= 0)
            {
                form.DetenerJuego(); // Detener el juego
                juegoTerminado = true; // Actualizar el estado del juego
                return;
            }
            if (DetectarColisionConEstela(nuevaPosicion, motos))
            {
                form.DetenerJuego();
                juegoTerminado = true; // Actualizar el estado del juego
                return;
            }

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
                AgregarItem(item);  // Aplicar el efecto del ítem
                // Limpiar la casilla del grid donde estaba el ítem
                 // Restaurar el color original de la casilla
                item.EliminarYRestaurarColor(form, Matriz);
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
                itemEnCola.Aplicar(this, Matriz);
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
                    item.Aplicar(this, Matriz);

                    // Actualiza el label o realiza otras acciones necesarias

                    if (Items.Count == 0)
                    {
                        itemTimer.Stop();
                        aplicandoItem = false;
                    }
                }
            }
        public void EliminarEnemigo2()
        {
            
            IsAlive=false;
            // Eliminar el enemigo de la lista de enemigos en el formulario
            if (form != null)
            {
                form.EliminarEnemigo(this);
            }

            // Eliminar el enemigo del grid
            if (Head != null)
            {
                Head.Value.BackColor = Color.Black; // Restaurar el color de la casilla en el grid
            }

            // Limpiar la estela
            while (Estela.Count > 0)
            {
                var nodoAntiguo = Estela.Last;
                Estela.RemoveLast();
                nodoAntiguo.Value.BackColor = Color.Black; // Restaurar el color de la casilla en el grid
            }
            
            // Eliminar el enemigo de la memoria
            Estela = null;
            Head = null;
            form = null;
        }
        }
        
        
        
}

