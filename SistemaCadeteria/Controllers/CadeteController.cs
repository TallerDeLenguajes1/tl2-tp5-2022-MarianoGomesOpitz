using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Controllers;

public class CadeteController : Controller
{
    private readonly ILogger<CadeteController> _logger;

    static int id = 1;
    public CadeteController(ILogger<CadeteController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(DataBase.Cadetes);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearCadeteViewModel cadete)
    {
        if (ModelState.IsValid)
        {
            DataBase.Cadetes.Add(new CadeteViewModel(id, cadete.Nombre, cadete.Direccion, cadete.Telefono));
            id++;
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", new { error = "Algo ha fallado en la creación de un cadete" });
        }
    }

    public IActionResult Editar(int id)
    {
        var cadete = DataBase.Cadetes.Find(x => x.Id == id);
        return View(new EditarCadeteViewModel(id, cadete.Nombre, cadete.Direccion, cadete.Telefono));
    }

    [HttpPost]
    public IActionResult Editar(EditarCadeteViewModel cadeteRecibido)
    {
        if (ModelState.IsValid)
        {
            var cadeteAEditar = DataBase.Cadetes.Find(y => y.Id == cadeteRecibido.Id);
            cadeteAEditar.Nombre = cadeteRecibido.Nombre;
            cadeteAEditar.Direccion = cadeteRecibido.Direccion;
            cadeteAEditar.Telefono = cadeteRecibido.Telefono;

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", new { error = "Algo ha fallado en la edición del cadete" });
        }
    }

    public IActionResult Borrar(int id)
    {
        var cadeteABorrar = DataBase.Cadetes.Find(z => z.Id == id);
        DataBase.Cadetes.Remove(cadeteABorrar);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
