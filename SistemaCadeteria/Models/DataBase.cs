using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Models
{
    public class DataBase
    {
        static public Cadeteria cadeteria = new();

        public DataBase() { }
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