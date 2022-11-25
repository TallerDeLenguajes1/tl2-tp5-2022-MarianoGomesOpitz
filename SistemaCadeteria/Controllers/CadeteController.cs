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
        return View(cadeteRepository.GetAll());
    }

    public IActionResult PedidosAsignados(int id)
    {
        return View(cadeteRepository.GetById(id));
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearCadeteViewModel _cadete_)
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

    public IActionResult Editar(int id)
    {
        CadeteViewModel cadete = cadeteRepository.GetById(id);
        return View(new EditarCadeteViewModel(id, cadete.Nombre, cadete.Direccion, cadete.Telefono));
    }

    [HttpPost]
    public IActionResult Editar(EditarCadeteViewModel _cadete_)
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

    public IActionResult Borrar(int id)
    {
        cadeteRepository.Delete(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}