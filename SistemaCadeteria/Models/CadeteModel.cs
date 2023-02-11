namespace SistemaCadeteria.Models;

public class Cadete : Persona
{
    private List<Pedido> pedidos;

    public List<Pedido> Pedidos { get => pedidos; set => pedidos = value; }

    public Cadete() : base()
    {
        this.Pedidos = new List<Pedido>();
    }

    public Cadete(int i, string name, string direcc, long tel) : base(i, name, direcc, tel)
    {
        this.Pedidos = new List<Pedido>();
    }
    
    public Cadete(int i, string name, string direcc, long tel, List<Pedido> peds) : base(i, name, direcc, tel)
    {
       this.Pedidos = peds;
    }
}