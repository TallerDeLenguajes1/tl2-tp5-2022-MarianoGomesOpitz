using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Models
{
    public class DataBase
    {
        static public int IdCliente = InicializarIds();
        static public int IdCadete = InicializarIds();
        static public int IdPedido = InicializarIds();
        static public Cadeteria cadeteria = new();

        public DataBase() { }

        static private int InicializarIds()
        {
            return (1);
        }
    }

    public class Cadeteria
    {
        public string NombreCadeteria { get; set; }
        public List<ClienteViewModel> Clientes { get; set; }
        public List<CadeteViewModel> Cadetes { get; set; }
        public List<PedidoViewModel> PedidosNoAsignados { get; set; }

        public Cadeteria()
        {
            this.NombreCadeteria = "";
            this.Clientes = new();
            this.Cadetes = new();
            this.PedidosNoAsignados = new();
        }
    }
}