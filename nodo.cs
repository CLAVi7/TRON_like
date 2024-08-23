using System.Windows.Forms;

namespace Tron
{
    public class Nodo
    {
        public Nodo Arriba { get; set; }
        public Nodo Abajo { get; set; }
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }
        public Casilla Casilla { get; set; }

        public Nodo(Casilla casilla)
        {
            Casilla = casilla;
        }
    }
}

