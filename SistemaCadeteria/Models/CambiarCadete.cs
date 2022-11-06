namespace SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

public class CambiarCadete
{
    public int IdPedido { get; set; }
    public int IdCadete { get; set; }
    public List<CadeteViewModel> Cadetes { get; set; }

    public CambiarCadete() { }

    public CambiarCadete(int iP, int iC, List<CadeteViewModel> c)
    {
        this.IdPedido = iP;
        this.IdCadete = iC;
        this.Cadetes = c;
    }
}