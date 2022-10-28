namespace SistemaCadeteria.Models;

public class Cadete : Persona
{
    private List<Pedido> pedidos;

    public List<Pedido> Pedidos { get => pedidos; set => pedidos = value; }

    public Cadete() : base()
    {
        this.Pedidos = new List<Pedido>();
    }

    public Cadete(int i, string name, string direcc, string tel) : base(i, name, direcc, tel)
    {
        this.Pedidos = new List<Pedido>();
    }

    public int JornalACobrar()
    {
        int montoBase = 300, montoGanado = 0, pedidosEntregados = 0;

        foreach (var item in this.Pedidos)
        {
            if (item.Estado == (status)3)
            {
                (pedidosEntregados)++;
            }
        }

        montoGanado = pedidosEntregados * 300;
        return (montoGanado);
    }
}