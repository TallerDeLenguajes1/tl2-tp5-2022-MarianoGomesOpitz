using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Models
{
    public class DataBase
    {
        static public string nombreCadeteria = "";
        static public int IdCadete = InicializarIds();
        static public int IdPedido = InicializarIds();
        static public Cadeteria cadeteria = new();

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

    public class Cadeteria
    {
        public string NombreCadeteria { get; set; }
        public List<CadeteViewModel> Cadetes { get; set; }
        public List<PedidoViewModel> PedidosNoAsignados { get; set; }

        public Cadeteria()
        {
            this.NombreCadeteria = "";
            this.Cadetes = new();
            this.PedidosNoAsignados = new();
        }
    }
}