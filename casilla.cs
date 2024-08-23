using System.Drawing;
using System.Windows.Forms;

namespace Tron
{
    public class Casilla : Panel
    {
        public Casilla()
        {
            // Configura el panel para que tenga un borde y un tamaño específico
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.White; // Color de fondo predeterminado
        }
    }
}
