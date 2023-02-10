namespace SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

public class PedidoACadete
{
    public int Id { get; set; }
    public List<Cadete> Cadetes { get; set; }

    public PedidoACadete() { }

    public PedidoACadete(int i, List<Cadete> c)
    {
        this.Id = i;
        this.Cadetes = c;
    }
}