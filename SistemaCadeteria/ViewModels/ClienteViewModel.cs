using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SistemaCadeteria.Models;

namespace SistemaCadeteria.ViewModels
{
    public class ClienteViewModel : Persona
    {
        public string DatosReferenciaDireccion { get; set; }

        public ClienteViewModel() : base()
        { }

        public ClienteViewModel(string name, string direcc, long tel, string datosRef) : base(name, direcc, tel)
        {
            this.DatosReferenciaDireccion = datosRef;
        }

        public ClienteViewModel(int i, string name, string direcc, long tel, string datosRef) : base(i, name, direcc, tel)
        {
            this.DatosReferenciaDireccion = datosRef;
        }
    }

    public class CrearClienteViewModel
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

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Datos de referencia de la dirección: ")]
        public string DatosReferenciaDireccion { get; set; }
    }

    public class EditarClienteViewModel
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

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Datos de referencia de la dirección: ")]
        public string DatosReferenciaDireccion { get; set; }

        public EditarClienteViewModel() { }
        public EditarClienteViewModel(int i, string name, string direcc, long tel, string datos)
        {
            this.Id = i;
            this.Nombre = name;
            this.Direccion = direcc;
            this.Telefono = tel;
            this.DatosReferenciaDireccion = datos;
        }
    }
}