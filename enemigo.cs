namespace Tron
{
    public class Enemigo : Moto
    {
        private Direction direccionActual;
        private System.Windows.Forms.Timer temporizadorCambioDireccion;
        public bool IsAlive;
        

        public Enemigo(Casilla posicionInicial, int tamañoEstela, Nodo[,] matriz, Form1 form, Direction direccionInicial)
            : base(posicionInicial, tamañoEstela, matriz, form, false)
        {
            this.direccionActual = direccionInicial; // Asignar la dirección inicial del enemigo
            CambiarDireccionAleatoria();
            IsAlive=true;

            
            temporizadorCambioDireccion = new System.Windows.Forms.Timer();
            temporizadorCambioDireccion.Interval = 300000;
            temporizadorCambioDireccion.Tick += (s, e) => CambiarDireccionAleatoria();
            temporizadorCambioDireccion.Start();
        }
        public bool DetectarColisionConEstela(Casilla nuevaPosicion, List<Moto> motos)
        {
            foreach (var otraMoto in motos)
            {
                if (otraMoto == this)
                    continue;

                foreach (var nodoEstela in otraMoto.Estela)
                {
                    if (nodoEstela == nuevaPosicion)
                    {
                        return true; // Colisión detectada
                    }
                }
            }
            return false; // No hay colisión
        }


        private void CambiarDireccionAleatoria()
        {
            Random random = new Random();
            Direction nuevaDireccion;

            do
            {
                nuevaDireccion = (Direction)random.Next(0, 4); // Elegir una dirección aleatoria
            }
            while (EsDireccionOpuesta(nuevaDireccion)); // Asegurar que no sea la dirección opuesta

            direccionActual = nuevaDireccion;
        }

        private bool EsDireccionOpuesta(Direction nuevaDireccion)
        {
            return (direccionActual == Direction.Up && nuevaDireccion == Direction.Down) ||
                   (direccionActual == Direction.Down && nuevaDireccion == Direction.Up) ||
                   (direccionActual == Direction.Left && nuevaDireccion == Direction.Right) ||
                   (direccionActual == Direction.Right && nuevaDireccion == Direction.Left);
        }

        public override void Mover(List<Moto> motos)
        {
            if (juegoTerminado)
                return; // No hacer nada si el juego ya terminó

            // Obtener la nueva posición en función de la dirección del enemigo
            Casilla nuevaPosicion = ObtenerNuevaPosicion();

            // Manejar el combustible como la moto normal
            contadorMovimiento++;
            if (DetectarColisionConEstela(nuevaPosicion, motos))
            {
                // Si colisiona con la estela, eliminar al enemigo
                if (!esJugador)
                {
                    EliminarEnemigo();
                }
                return;
            }

            if (contadorMovimiento >= 5)
            {
                Combustible--;
                contadorMovimiento = 0; // Reiniciar el contador
                form.CombustibleLabel.Text = $"Combustible: {Combustible}";
            }

            if (Combustible <= 0)
            {
                EliminarEnemigo(); // Eliminar el enemigo si se queda sin combustible
                return;
            }

            // Si la nueva posición es inválida (fuera de límites o colisión con pared)
            if (nuevaPosicion == null || nuevaPosicion is Pared)
            {
                EliminarEnemigo(); // Eliminar si choca con una pared
                return;
            }

            // Si la nueva posición es un ítem
            if (nuevaPosicion is Item item)
            {
                nuevaPosicion.BackColor = Color.Black;
                item.Aplicar(this, Matriz); // Aplicar el efecto del ítem
                item.EliminarYRestaurarColor(form, Matriz); // Eliminar el ítem y restaurar color
            }

            // Mover el enemigo
            Estela.AddFirst(nuevaPosicion);
            nuevaPosicion.BackColor = Color.Magenta; // Color del enemigo

            // Actualizar el Head
            Head = Estela.First;

            // Limitar la longitud de la estela
            while (Estela.Count > TamañoEstela)
            {
                LinkedListNode<Casilla> nodoAntiguo = Estela.Last;
                Estela.RemoveLast();
                nodoAntiguo.Value.BackColor = Color.Black; // Restaurar el color anterior
            }

            // Colorear la estela
            foreach (var nodo in Estela)
            {
                if (nodo != Head.Value)
                {
                    nodo.BackColor = Color.White; // Color de la estela
                }
            }

            // Mover en la dirección actual
            switch (direccionActual)
            {
                case Direction.Up:
                    // Lógica para mover hacia arriba
                    break;
                case Direction.Down:
                    // Lógica para mover hacia abajo
                    break;
                case Direction.Left:
                    // Lógica para mover hacia la izquierda
                    break;
                case Direction.Right:
                    // Lógica para mover hacia la derecha
                    break;
            }
        }

        private Casilla ObtenerNuevaPosicion()
        {
            int x = Head.Value.Left / 10; // Ajusta según el tamaño de la casilla
            int y = Head.Value.Top / 10;

            // Calcular la nueva posición en función de la dirección
            switch (direccionActual)
            {
                case Direction.Up: y--; break;
                case Direction.Down: y++; break;
                case Direction.Left: x--; break;
                case Direction.Right: x++; break;
            }

            // Verificar límites
            if (x < 0 || x >= Matriz.GetLength(1) || y < 0 || y >= Matriz.GetLength(0))
            {
                return null; // Posición inválida
            }

            return Matriz[y, x].Casilla;
        }

        public void EliminarEnemigo()
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