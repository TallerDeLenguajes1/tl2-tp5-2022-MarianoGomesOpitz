using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SistemaCadeteria.Models;

namespace SistemaCadeteria.ViewModels
{
    public class CadeteViewModel : Persona
    {
        public List<PedidoViewModel> Pedidos { get; set; }

        public CadeteViewModel() : base()
        {

        }
        public CadeteViewModel(int i, string name, string direcc, long tel) : base(i, name, direcc, tel)
        {
            this.Id = i;
            this.Nombre = name;
            this.Direccion = direcc;
            this.Telefono = tel;
            this.Pedidos = new List<PedidoViewModel>();
        }
    }

    public class CrearCadeteViewModel
    {
        [Required]
        [DisplayName("Nombre: ")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Dirección: ")]
        public string Direccion { get; set; }

        [Required]
        [DisplayName("Teléfono: ")]
        public long Telefono { get; set; }
    }

    public class EditarCadeteViewModel
    {
        [Required]
        [NotNull]
        public int Id { get; set; }

        [Required]
        [DisplayName("Nombre: ")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Teléfono: ")]
        public long Telefono { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Dirección: ")]
        public string Direccion { get; set; }

        public EditarCadeteViewModel() { }
        public EditarCadeteViewModel(int i, string name, string direcc, long tel)
        {
            this.Id = i;
            this.Nombre = name;
            this.Direccion = direcc;
            this.Telefono = tel;
        }
    }
}
