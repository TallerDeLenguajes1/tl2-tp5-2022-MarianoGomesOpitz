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

            DataBase.PedidosNoAsignados.Add(new PedidoViewModel(DataBase.IdPedido, pedido.Observaciones, 1, pedido.Nombre, pedido.Direccion, pedido.Telefono, pedido.DatosReferenciaDireccion));
            DataBase.IdPedido++;
            return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        var pedido = DataBase.PedidosNoAsignados.Find(x => x.NroPedido == id);
        return View(new EditarPedidoViewModel(id, pedido.Costumer.Nombre, pedido.Costumer.Direccion, pedido.Costumer.Telefono, pedido.Costumer.DatosReferenciaDireccion, pedido.Observaciones));
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel pedidoRecibido)
    {
        if (ModelState.IsValid)
        {
            var pedidoAEditar = DataBase.PedidosNoAsignados.Find(y => y.NroPedido == pedidoRecibido.NroPedido);
            pedidoAEditar.Costumer.DatosReferenciaDireccion = pedidoRecibido.DatosReferenciaDireccion;
            pedidoAEditar.Costumer.Direccion = pedidoRecibido.Direccion;
            pedidoAEditar.Costumer.Nombre = pedidoRecibido.Nombre;
            pedidoAEditar.Costumer.Telefono = pedidoRecibido.Telefono;
            pedidoAEditar.Observaciones = pedidoRecibido.Observaciones;

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Borrar(int id)
    {
        var pedidoABorrar = DataBase.PedidosNoAsignados.Find(z => z.NroPedido == id);
        DataBase.PedidosNoAsignados.Remove(pedidoABorrar);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
