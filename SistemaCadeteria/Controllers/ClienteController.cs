using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.Repositorios;

namespace SistemaCadeteria.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    private readonly IClienteRepository clienteRepository;

    public ClienteController(ILogger<ClienteController> logger)
    {
        _logger = logger;
        this.clienteRepository = new ClienteRepository(connectionString);
    }

    public IActionResult Index()
    {
        return View(clienteRepository.GetAll());
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearClienteViewModel _cliente_)
    {
        if (ModelState.IsValid)
        {
            clienteRepository.Create(new(_cliente_.Nombre, _cliente_.Direccion, _cliente_.Telefono, _cliente_.DatosReferenciaDireccion));

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Editar(int id)
    {
        ClienteViewModel _cliente_ = clienteRepository.GetById(id);
        return View(new EditarClienteViewModel(id, _cliente_.Nombre, _cliente_.Direccion, _cliente_.Telefono, _cliente_.DatosReferenciaDireccion));
    }

    [HttpPost]
    public IActionResult Editar(EditarClienteViewModel _cliente_)
    {
        if (ModelState.IsValid)
        {
            clienteRepository.Update(new(_cliente_.Id, _cliente_.Nombre, _cliente_.Direccion, _cliente_.Telefono, _cliente_.DatosReferenciaDireccion));

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Borrar(int id)
    {
        clienteRepository.Delete(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}