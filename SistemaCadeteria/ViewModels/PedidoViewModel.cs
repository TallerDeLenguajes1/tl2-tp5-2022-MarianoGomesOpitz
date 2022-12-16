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
        public ClienteViewModel Costumer { get; set; }

        public PedidoViewModel() { }

        public PedidoViewModel(string obs, string est, ClienteViewModel C)
        {
            this.Observaciones = obs;
            this.Estado = est;
            this.Costumer = C;
        }

        public PedidoViewModel(int nro, string obs, string est, ClienteViewModel C)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
            this.Estado = est;
            this.Costumer = C;
        }

        public PedidoViewModel(int nro, string obs)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
        }
    }

    public class CrearPedidoViewModel
    {
        [Required]
        [DisplayName("Nombre del cliente: ")]
        public string NombreCliente { get; set; }

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

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Observaciones: ")]
        public string Observaciones { get; set; }

        public EditarPedidoViewModel() { }

        public EditarPedidoViewModel(int nro, string obs)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
        }
    }

    public class MapperPedidoViewModel
    {
        public List<PedidoViewModel> GetPedidoViewModel(List<Pedido> pedidos)
        {
            List<PedidoViewModel> pedViewModels = new();

            foreach (var pedido in pedidos)
            {
                PedidoViewModel pedViewModel = new();
                pedViewModel.NroPedido = pedido.NroPedido;
                pedViewModel.Observaciones = pedido.Observaciones;
                pedViewModel.Estado = pedido.Estado;
                pedViewModel.Costumer.Id = pedido.Costumer.Id;
                pedViewModel.Costumer.Nombre = pedido.Costumer.Nombre;
                pedViewModel.Costumer.Direccion = pedido.Costumer.Direccion;
                pedViewModel.Costumer.Telefono = pedido.Costumer.Telefono;
                pedViewModel.Costumer.DatosReferenciaDireccion = pedido.Costumer.DatosReferenciaDireccion;
                pedViewModels.Add(pedViewModel);
            }

            return pedViewModels;
        }

    }
}

enum status
{
    En_Preparación = 1,
    En_Camino = 2,
    Entregado = 3,
}