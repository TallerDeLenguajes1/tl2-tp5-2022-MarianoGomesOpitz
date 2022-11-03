using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Models
{
    public class DataBase
    {
        static public int IdCadete = InicializarIds();
        static public int IdPedido = InicializarIds();
        static public List<CadeteViewModel> Cadetes = InicializarCadete();
        static public List<PedidoViewModel> PedidosNoAsignados = InicializarPedido();

        public DataBase() { }

        static private int InicializarIds()
        {
            return (1);
        }

        static private List<CadeteViewModel> InicializarCadete()
        {
            List<CadeteViewModel> cad = new();
            return cad;
        }

        static private List<PedidoViewModel> InicializarPedido()
        {
            List<PedidoViewModel> ped = new();
            return ped;
        }
    }
}