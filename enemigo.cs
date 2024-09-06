namespace Tron
{
    public class Enemigo : Moto
    {
        private Direction direccionMovimiento; // Dirección en la que se moverá el enemigo

        public Enemigo(Casilla posicionInicial, int tamañoEstela, Nodo[,] matriz, Form1 form, Direction direccionMovimiento)
            : base(posicionInicial, tamañoEstela, matriz, form)
        {
            this.direccionMovimiento = direccionMovimiento; // Asignar la dirección inicial del enemigo
        }

        public override void Mover()
        {
            if (juegoTerminado)
                return; // No hacer nada si el juego ya terminó

            // Obtener la nueva posición en función de la dirección del enemigo
            Casilla nuevaPosicion = ObtenerNuevaPosicion();
            
            // Manejar el combustible como la moto normal
            contadorMovimiento++;
            if (contadorMovimiento >= 5)
            {
                Combustible--;
                contadorMovimiento = 0; // Reiniciar el contador
                form.CombustibleLabel.Text = $"Combustible: {Combustible}";
            }

            if (Combustible <= 0)
            {
                EliminarEnemigo(); // Llamar al método para eliminar el enemigo
                return;//logica para eliminar al enemigo
            }

            // Si la nueva posición es inválida (fuera de límites o colisión con pared)
            if (nuevaPosicion == null || nuevaPosicion is Pared)
            {
                EliminarEnemigo(); // Llamar al método para eliminar el enemigo
                return; //logica para eliminar al enemigo
                
            }

            // Si la nueva posición es un ítem, manejar la recogida del ítem
            if (nuevaPosicion is Item item)
            {
                nuevaPosicion.BackColor = Color.Black;
                item.Aplicar(this, Matriz); // Aplicar el efecto del ítem
                item.EliminarYRestaurarColor(form, Matriz); // Eliminar y restaurar el color del ítem
                nuevaPosicion = ObtenerNuevaPosicion(); // Avanzar a la siguiente posición después del ítem
                if (nuevaPosicion == null || nuevaPosicion is Pared)
                {
                    EliminarEnemigo(); // Llamar al método para eliminar el enemigo
                    return;
                }
            }

            // Mover el enemigo en la dirección actual
            Estela.AddFirst(nuevaPosicion);
            nuevaPosicion.BackColor = Color.Magenta; // Color del enemigo

            // Actualizar el Head
            Head = Estela.First;

            // Eliminar el último nodo solo si la estela es más larga que el tamaño permitido
            while (Estela.Count > TamañoEstela)
            {
                LinkedListNode<Casilla> nodoAntiguo = Estela.Last;
                Estela.RemoveLast();
                nodoAntiguo.Value.BackColor = Color.Black; // Restaurar el color anterior
            }

            // Cambiar el color de la estela (los nodos que no son el enemigo)
            foreach (var nodo in Estela)
            {
                if (nodo != Head.Value)
                {
                    nodo.BackColor = Color.White; // Color de la estela
                }
            }

            // El enemigo se mueve en la dirección establecida
            Direccion = direccionMovimiento;
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
        private void EliminarEnemigo()
        {
            // Restaurar el color de las casillas ocupadas por el enemigo
            foreach (var nodo in Estela)
            {
                nodo.BackColor = Color.Black; // Restaurar el color anterior
            }

            
            

            // Opcional: Si usas una lista de enemigos en Form1, también debes remover el enemigo de allí
        }
    }
}
