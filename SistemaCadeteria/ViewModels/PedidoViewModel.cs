using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SistemaCadeteria.Models;

namespace SistemaCadeteria.ViewModels
{
    public class PedidoViewModel
    {
        public int NroPedido { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public Cliente Costumer { get; set; }

        public PedidoViewModel() { }

        public PedidoViewModel(int nro, string obs, int est, string name, string direc, long tel, string datos)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
            this.Estado = Convert.ToString((status)est);
            this.Costumer = new(nro, name, direc, tel, datos);
        }
    }

    public class CrearPedidoViewModel
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

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Observaciones: ")]
        public string Observaciones { get; set; }
    }

    public class EditarPedidoViewModel
    {
        [Required]
        [NotNull]
        public int NroPedido { get; set; }

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

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Observaciones: ")]
        public string Observaciones { get; set; }

        public EditarPedidoViewModel() { }

        public EditarPedidoViewModel(int nro, string name, string direcc, long tel, string datos, string obs)
        {
            this.NroPedido = nro;
            this.Nombre = name;
            this.Direccion = direcc;
            this.Telefono = tel;
            this.DatosReferenciaDireccion = datos;
            this.Observaciones = obs;
        }
    }
}