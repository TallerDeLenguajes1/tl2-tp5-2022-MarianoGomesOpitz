namespace SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

public class CambiarCadete
{
    public int IdPedido { get; set; }
    public int IdCadete { get; set; }
    public List<Cadete> Cadetes { get; set; }

    public CambiarCadete() { }

    public CambiarCadete(int iP, int iC, List<Cadete> c)
    {
        this.IdPedido = iP;
        this.IdCadete = iC;
        this.Cadetes = c;
    }
}