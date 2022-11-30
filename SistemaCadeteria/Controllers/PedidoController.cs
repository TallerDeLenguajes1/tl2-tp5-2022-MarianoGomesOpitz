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
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View(pedidoRepositorio.GetAll());
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult Crear()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View();
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult Crear(CrearPedidoViewModel pedido)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
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
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult Asignar(int id)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            PedidoACadete pedCad = new(id, cadeteRepositorio.GetAll());

            return View(pedCad);
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            pedidoRepositorio.AsignarCadete(IdPedido, IdCadete);

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult ModificarEstado(int idPedido)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View(pedidoRepositorio.GetById(idPedido));
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult ModificarEstado(int idPedido, int Estado)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            pedidoRepositorio.UpdateEstado(idPedido, Convert.ToString((status)Estado));

            return RedirectToAction("Index", "Cadete");
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            CambiarCadete pedCad = new(idPedido, idCadete, cadeteRepositorio.GetAll());

            return View(pedCad);
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int IdCadeteACambiar)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            pedidoRepositorio.CambiarCadete(IdPedido, IdCadeteACambiar);

            return RedirectToAction("Index", "Cadete");
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult Editar(int id)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            PedidoViewModel _pedido_ = pedidoRepositorio.GetById(id);
            return View(new EditarPedidoViewModel(_pedido_.NroPedido, _pedido_.Observaciones));
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel _pedido_)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
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
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult Borrar(int id)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            pedidoRepositorio.Delete(id);

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
