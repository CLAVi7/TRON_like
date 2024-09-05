namespace Tron
{
public abstract class Item : Casilla
{
    public abstract void Aplicar(Moto moto);
    public Color Color { get; set; }
    public bool Usado { get; set; } = false;

        public void ActualizarColorSiUsado()
        {
            if (Usado)
            {
                this.Color = Color.Black; // Cambia el color a negro si el item ha sido usado
                // Aquí debes actualizar el color de la casilla en el grid
                this.BackColor = Color.Black; // Si la clase Casilla tiene un atributo que representa el color en el grid
            }
        }
}

public class CeldaCombustible : Item
{
    public int Cantidad { get; set; }
    private static Random random = new Random();
    
    public override void Aplicar(Moto moto)
    {
        Cantidad = random.Next(40, 100);
        moto.Combustible += Cantidad;
        Usado=true;
        ActualizarColorSiUsado();
        
    }
}

public class CrecimientoEstela : Item
{
    public int Incremento { get; set; }
    private static Random random = new Random();

    public override void Aplicar(Moto moto)
    {
        Incremento = random.Next(2, 11);
        moto.TamañoEstela += Incremento;
        Usado=true;
        ActualizarColorSiUsado();
    }
}

public class Bomba : Item
{
    public override void Aplicar(Moto moto)
    {
        // Lógica para explosion
        Usado=true;
        ActualizarColorSiUsado();
    }
}
}