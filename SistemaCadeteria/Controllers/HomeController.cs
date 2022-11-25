using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.ViewModels;
using System.Data;
using System.Data.SqlClient;
using SistemaCadeteria.Repositorios;

namespace SistemaCadeteria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult PedidosPorCliente()
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM PedidosPorCliente;";
        DataTable tabla = new();
        connection.Open();
        tabla.Load(command.ExecuteReader());
        connection.Close();

        return View(tabla);
    }

    public IActionResult PedidosPorCadete()
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM PedidosPorCadete;";
        DataTable tabla = new();
        connection.Open();
        tabla.Load(command.ExecuteReader());
        connection.Close();

        return View(tabla);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
