using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.Repositorios;

namespace SistemaCadeteria.Controllers;

public class CadeteController : Controller
{
    private readonly ILogger<CadeteController> _logger;
    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";

    private readonly ICadeteRepository cadeteRepository;

    public CadeteController(ILogger<CadeteController> logger)
    {
        _logger = logger;
        this.cadeteRepository = new CadeteRepository(connectionString);
    }

    public IActionResult Index()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View(cadeteRepository.GetAll());
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult PedidosAsignados(int id)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View(cadeteRepository.GetById(id));
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
    public IActionResult Crear(CrearCadeteViewModel _cadete_)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            if (ModelState.IsValid)
            {
                cadeteRepository.Create(new(_cadete_.Nombre, _cadete_.Direccion, _cadete_.Telefono));

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

    public IActionResult Editar(int id)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            CadeteViewModel cadete = cadeteRepository.GetById(id);
            return View(new EditarCadeteViewModel(id, cadete.Nombre, cadete.Direccion, cadete.Telefono));
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    [HttpPost]
    public IActionResult Editar(EditarCadeteViewModel _cadete_)
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            if (ModelState.IsValid)
            {
                cadeteRepository.Update(new(_cadete_.Id, _cadete_.Nombre, _cadete_.Direccion, _cadete_.Telefono));

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
            cadeteRepository.Delete(id);

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