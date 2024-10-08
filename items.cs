namespace Tron
{
public abstract class Item : Casilla
{
    public abstract void Aplicar(Moto moto, Nodo[,] matriz);
    public Color Color { get; set; }
    public bool Usado { get; set; } = false;
    public Form1 form;
    public int X {get; set;}
    public int Y { get; set; }

    public Item(Form1 form)
    {
        this.form = form;
    }

    public void EliminarYRestaurarColor(Form1 form, Nodo[,] matriz)
    {
        if (Usado)
        {
            // Eliminar el ítem de los controles del formulario
            form.Controls.Remove(this);

            // Restaurar el color de la casilla en la matriz
            if (X >= 0 && Y >= 0 && X < matriz.GetLength(1) && Y < matriz.GetLength(0))
            {
                Casilla casilla = matriz[Y, X].Casilla;
                casilla.BackColor = Color.Black; // Restaurar el color de la casilla a negro
                matriz[Y, X].Casilla = casilla; // Actualizar la matriz
            }
            form.Invalidate(); // Esto hace que el formulario se redibuje
        }
    }
}

public class CeldaCombustible : Item
{
    public int Cantidad { get; set; }
    private static Random random = new Random();

    public CeldaCombustible(Form1 form) : base(form) { }

    public override void Aplicar(Moto moto, Nodo[,] matriz)
    {
        Cantidad = random.Next(10, 30);
        moto.Combustible += Cantidad;
        Usado = true;
        EliminarYRestaurarColor(moto.form, matriz);
    }
}

public class CrecimientoEstela : Item
{
    public int Incremento { get; set; }
    private static Random random = new Random();

    public CrecimientoEstela(Form1 form) : base(form) { }

    public override void Aplicar(Moto moto, Nodo[,] matriz)
    {
        Incremento = random.Next(2, 11);
        moto.TamañoEstela += Incremento;
        Usado = true;
        EliminarYRestaurarColor(moto.form, matriz);
    }
}

public class Bomba : Item
{
    public Bomba(Form1 form) : base(form) { }

    public override void Aplicar(Moto moto, Nodo[,] matriz)
    {
        if (moto.esJugador){
            form.DetenerJuego();
            Usado = true;
            EliminarYRestaurarColor(moto.form, matriz);

        }
     
    }
}
}