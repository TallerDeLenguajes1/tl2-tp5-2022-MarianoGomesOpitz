using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.Repositorios;

namespace SistemaCadeteria.Controllers;

public class PedidoController : Controller
{
    private readonly ILogger<PedidoController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    private readonly IPedidoRepository pedidoRepositorio;
    private readonly IClienteRepository clienteRepositorio;
    private readonly ICadeteRepository cadeteRepositorio;

    public PedidoController(ILogger<PedidoController> logger)
    {
        _logger = logger;
        this.pedidoRepositorio = new PedidoRepository(connectionString);
        this.clienteRepositorio = new ClienteRepository(connectionString);
        this.cadeteRepositorio = new CadeteRepository(connectionString);
    }

    public IActionResult Index()
    {
        return View(pedidoRepositorio.GetAll());
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
            ClienteViewModel auxCliente = clienteRepositorio.GetByName(pedido.NombreCliente);
            pedidoRepositorio.Create(new(pedido.Observaciones, Convert.ToString((status)1), auxCliente));

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Asignar(int id)
    {
        PedidoACadete pedCad = new(id, cadeteRepositorio.GetAll());

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        pedidoRepositorio.AsignarCadete(IdPedido, IdCadete);

        return RedirectToAction("Index");
    }

    public IActionResult ModificarEstado(int idPedido)
    {
        return View(pedidoRepositorio.GetById(idPedido));
    }

    [HttpPost]
    public IActionResult ModificarEstado(int idPedido, int Estado)
    {
        pedidoRepositorio.UpdateEstado(idPedido, Convert.ToString((status)Estado));

        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        CambiarCadete pedCad = new(idPedido, idCadete, cadeteRepositorio.GetAll());

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int IdCadeteACambiar)
    {
        pedidoRepositorio.CambiarCadete(IdPedido, IdCadeteACambiar);

        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult Editar(int id)
    {
        PedidoViewModel _pedido_ = pedidoRepositorio.GetById(id);
        return View(new EditarPedidoViewModel(_pedido_.NroPedido, _pedido_.Observaciones));
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel _pedido_)
    {
        if (ModelState.IsValid)
        {
            pedidoRepositorio.UpdatePedido(new(_pedido_.NroPedido, _pedido_.Observaciones));

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Borrar(int id)
    {
        pedidoRepositorio.Delete(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
