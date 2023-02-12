using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.ViewModels;
using System.Data;
using System.Data.SqlClient;
using SistemaCadeteria.Repositorios;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using NLog;

namespace SistemaCadeteria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    SqliteDataReader lector;
    private readonly IPedidoRepository _pedidoRepositorio;
    private readonly ILoginRepository _loginRepositorio;
    private readonly Logger log;

    public HomeController(ILogger<HomeController> logger, IPedidoRepository pedidoRepository, ILoginRepository loginRepositorio)
    {
        _logger = logger;
        this._pedidoRepositorio = pedidoRepository;
        this._loginRepositorio = loginRepositorio;
        this.log = LogManager.GetCurrentClassLogger();
    }

    public IActionResult Index()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                Usuario us = new();
                us.User = HttpContext.Session.GetString("User");
                us.Name = HttpContext.Session.GetString("Name");
                us.Role = HttpContext.Session.GetString("Role");
                return View(us);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult Login()
    {
        try
        {
            Usuario us = new();
            us.User = HttpContext.Session.GetString("User");
            us.Name = HttpContext.Session.GetString("Name");
            us.Role = HttpContext.Session.GetString("Role");
            return View(us);
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult IniciarSesion(string user, string password)
    {
        try
        {
            int count = _loginRepositorio.CantFilas(user, password);

            if (count == 1)
            {
                Usuario us = _loginRepositorio.DatosUsuario(user, password);
                HttpContext.Session.SetString("Name", us.Name);
                HttpContext.Session.SetString("User", us.User);
                HttpContext.Session.SetString("Role", us.Role);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("DatosIncorrectos");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult DatosIncorrectos()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(user))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult CerrarSesion()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult PedidosPorCliente()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    DataTable tabla = _pedidoRepositorio.PedidosPorCliente();
                    return View(tabla);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult PedidosPorCadete()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    DataTable tabla = _pedidoRepositorio.PedidosPorCadete();
                    return View(tabla);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
