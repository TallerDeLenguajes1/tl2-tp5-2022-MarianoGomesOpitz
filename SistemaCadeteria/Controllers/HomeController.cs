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

namespace SistemaCadeteria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;
    private readonly IPedidoRepository pedidoRepositorio;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        this.pedidoRepositorio = new PedidoRepository(connectionString);
    }

    public IActionResult Index()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            List<String> modelo = new();
            modelo.Add(HttpContext.Session.GetString("User"));
            modelo.Add(HttpContext.Session.GetString("Name"));
            modelo.Add(HttpContext.Session.GetString("Role"));
            return View(model: modelo);
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

    public IActionResult Login()
    {
        return View(model: HttpContext.Session.GetString("User"));
    }

    [HttpPost]
    public IActionResult IniciarSesion(string user, string password)
    {
        int count = 0;
        SqliteConnection connection = new SqliteConnection(connectionString);
        SqliteCommand command1 = connection.CreateCommand();
        connection.Open();
        command1.CommandText = $"SELECT COUNT(*) FROM Usuarios WHERE User ='{user}' AND Password = '{password}';";
        count = Convert.ToInt32(command1.ExecuteScalar());
        connection.Close();

        if (count == 1)
        {
            SqliteCommand command2 = connection.CreateCommand();
            command2.CommandText = $"SELECT Name, User, Role FROM Usuarios WHERE User ='{user}' AND Password = '{password}';";
            connection.Open();
            lector = command2.ExecuteReader();
            while (lector.Read())
            {
                HttpContext.Session.SetString("Name", Convert.ToString(lector[0]));
                HttpContext.Session.SetString("User", Convert.ToString(lector[1]));
                HttpContext.Session.SetString("Role", Convert.ToString(lector[2]));
            }
            connection.Close();

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("DatosIncorrectos");
        }
    }

    public IActionResult DatosIncorrectos()
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

    public IActionResult CerrarSesion()
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

    public IActionResult PedidosPorCliente()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            string rol = HttpContext.Session.GetString("Role");
            if (rol == "Admin")
            {
                DataTable tabla = pedidoRepositorio.PedidosPorCliente();
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

    public IActionResult PedidosPorCadete()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            string rol = HttpContext.Session.GetString("Role");
            if (rol == "Admin")
            {
                DataTable tabla = pedidoRepositorio.PedidosPorCadete();
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
