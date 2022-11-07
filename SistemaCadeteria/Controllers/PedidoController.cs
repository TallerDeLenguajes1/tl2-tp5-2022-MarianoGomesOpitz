using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Controllers;

public class PedidoController : Controller
{
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(ILogger<PedidoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(DataBase.cadeteria.PedidosNoAsignados);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearPedidoViewModel pedido)
    {
        if (ModelState.IsValid)
        {
            DataBase.cadeteria.PedidosNoAsignados.Add(new PedidoViewModel(DataBase.IdPedido, pedido.Observaciones, 1, pedido.Nombre, pedido.Direccion, pedido.Telefono, pedido.DatosReferenciaDireccion));
            DataBase.IdPedido++;
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Asignar(int id)
    {
        PedidoACadete pedCad = new(id, DataBase.cadeteria.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        var CadeteAAsignar = DataBase.cadeteria.Cadetes.Find(a => a.Id == IdCadete);
        var PedidoAMover = DataBase.cadeteria.PedidosNoAsignados.Find(p => p.NroPedido == IdPedido);
        CadeteAAsignar.Pedidos.Add(PedidoAMover);
        DataBase.cadeteria.PedidosNoAsignados.Remove(PedidoAMover);

        return RedirectToAction("Index");
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        CambiarCadete pedCad = new(idPedido, idCadete, DataBase.cadeteria.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int idCadeteACambiar)
    {
        var cadeteOriginal = DataBase.cadeteria.Cadetes.Find(i => i.Id == IdCadete);
        var pedidoAMover = cadeteOriginal.Pedidos.Find(p => p.NroPedido == IdPedido);

        var cadeteACambiar = DataBase.cadeteria.Cadetes.Find(i => i.Id == idCadeteACambiar);

        cadeteACambiar.Pedidos.Add(pedidoAMover);
        cadeteOriginal.Pedidos.Remove(pedidoAMover);
        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult Editar(int id)
    {
        var pedido = DataBase.cadeteria.PedidosNoAsignados.Find(x => x.NroPedido == id);

        if (pedido == null)
        {
            foreach (var cadete in DataBase.cadeteria.Cadetes)
            {
                pedido = cadete.Pedidos.Find(l => l.NroPedido == id);

                if (pedido != null)
                {
                    break;
                }
            }
        }

        return View(new EditarPedidoViewModel(id, pedido.Costumer.Nombre, pedido.Costumer.Direccion, pedido.Costumer.Telefono, pedido.Costumer.DatosReferenciaDireccion, pedido.Observaciones));
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel pedidoRecibido)
    {
        if (ModelState.IsValid)
        {
            string controller = "";

            var pedidoAEditar = DataBase.cadeteria.PedidosNoAsignados.Find(y => y.NroPedido == pedidoRecibido.NroPedido);
            if (pedidoAEditar == null)
            {
                foreach (var cadete in DataBase.cadeteria.Cadetes)
                {
                    pedidoAEditar = cadete.Pedidos.Find(l => l.NroPedido == pedidoRecibido.NroPedido);
                    if (pedidoAEditar != null)
                    {
                        controller = "Cadete";
                        break;
                    }
                }
            }
            else
            {
                controller = "Pedido";
            }

            pedidoAEditar.Costumer.DatosReferenciaDireccion = pedidoRecibido.DatosReferenciaDireccion;
            pedidoAEditar.Costumer.Direccion = pedidoRecibido.Direccion;
            pedidoAEditar.Costumer.Nombre = pedidoRecibido.Nombre;
            pedidoAEditar.Costumer.Telefono = pedidoRecibido.Telefono;
            pedidoAEditar.Observaciones = pedidoRecibido.Observaciones;

            return RedirectToAction("Index", controller);
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Borrar(int id)
    {
        string controller = "";
        var pedidoABorrar = DataBase.cadeteria.PedidosNoAsignados.Find(z => z.NroPedido == id);
        if (pedidoABorrar == null)
        {
            foreach (var cadete in DataBase.cadeteria.Cadetes)
            {
                pedidoABorrar = cadete.Pedidos.Find(z => z.NroPedido == id);
                if (pedidoABorrar != null)
                {
                    cadete.Pedidos.Remove(pedidoABorrar);
                    controller = "Cadete";
                    break;
                }
            }
        }
        else
        {
            DataBase.cadeteria.PedidosNoAsignados.Remove(pedidoABorrar);
            controller = "Pedido";
        }


        return RedirectToAction("Index", controller);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
