using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Models
{
    public class DataBase
    {
        static public List<CadeteViewModel> Cadetes = InicializarCadete();
        static public int IdCadete { get; private set; }

        public DataBase() { }

        static private List<CadeteViewModel> InicializarCadete()
        {
            List<CadeteViewModel> cad = new();
            return cad;
        }

    }
}