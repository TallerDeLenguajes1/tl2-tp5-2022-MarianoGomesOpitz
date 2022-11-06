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
        return View(DataBase.PedidosNoAsignados);
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
            DataBase.PedidosNoAsignados.Add(new PedidoViewModel(DataBase.IdPedido, pedido.Observaciones, 1, pedido.Nombre, pedido.Direccion, pedido.Telefono, pedido.DatosReferenciaDireccion));
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
        PedidoACadete pedCad = new(id, DataBase.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        var CadeteAAsignar = DataBase.Cadetes.Find(a => a.Id == IdCadete);
        var PedidoAMover = DataBase.PedidosNoAsignados.Find(p => p.NroPedido == IdPedido);
        CadeteAAsignar.Pedidos.Add(PedidoAMover);
        DataBase.PedidosNoAsignados.Remove(PedidoAMover);

        return RedirectToAction("Index");
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        CambiarCadete pedCad = new(idPedido, idCadete, DataBase.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int idCadeteACambiar)
    {
        var cadeteOriginal = DataBase.Cadetes.Find(i => i.Id == IdCadete);
        var pedidoAMover = cadeteOriginal.Pedidos.Find(p => p.NroPedido == IdPedido);

        var cadeteACambiar = DataBase.Cadetes.Find(i => i.Id == idCadeteACambiar);

        cadeteACambiar.Pedidos.Add(pedidoAMover);
        cadeteOriginal.Pedidos.Remove(pedidoAMover);
        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult Editar(int id)
    {
        var pedido = DataBase.PedidosNoAsignados.Find(x => x.NroPedido == id);

        if (pedido == null)
        {
            foreach (var cadete in DataBase.Cadetes)
            {
                pedido = cadete.Pedidos.Find(l => l.NroPedido == id);
            }
        }

        return View(new EditarPedidoViewModel(id, pedido.Costumer.Nombre, pedido.Costumer.Direccion, pedido.Costumer.Telefono, pedido.Costumer.DatosReferenciaDireccion, pedido.Observaciones));
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel pedidoRecibido)
    {
        if (ModelState.IsValid)
        {
            var pedidoAEditar = DataBase.PedidosNoAsignados.Find(y => y.NroPedido == pedidoRecibido.NroPedido);
            if (pedidoAEditar == null)
            {
                foreach (var cadete in DataBase.Cadetes)
                {
                    pedidoAEditar = cadete.Pedidos.Find(l => l.NroPedido == pedidoRecibido.NroPedido);
                    if (pedidoAEditar != null)
                    {
                        break;
                    }
                }
            }

            pedidoAEditar.Costumer.DatosReferenciaDireccion = pedidoRecibido.DatosReferenciaDireccion;
            pedidoAEditar.Costumer.Direccion = pedidoRecibido.Direccion;
            pedidoAEditar.Costumer.Nombre = pedidoRecibido.Nombre;
            pedidoAEditar.Costumer.Telefono = pedidoRecibido.Telefono;
            pedidoAEditar.Observaciones = pedidoRecibido.Observaciones;

            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Borrar(int id)
    {
        var pedidoABorrar = DataBase.PedidosNoAsignados.Find(z => z.NroPedido == id);
        if (pedidoABorrar == null)
        {
            foreach (var cadete in DataBase.Cadetes)
            {
                pedidoABorrar = cadete.Pedidos.Find(z => z.NroPedido == id);
                if (pedidoABorrar != null)
                {
                    cadete.Pedidos.Remove(pedidoABorrar);
                    break;
                }
            }
        }
        else
        {
            DataBase.PedidosNoAsignados.Remove(pedidoABorrar);
        }


        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
