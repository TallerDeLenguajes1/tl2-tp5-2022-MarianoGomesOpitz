using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using Microsoft.Data.Sqlite;
using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;

    static bool noLeido = true;

    public IActionResult Index()
    {
        if (noLeido)
        {
            SqliteCommand command1 = connection.CreateCommand();

            connection.Open();
            command1.CommandText = "SELECT Nombre FROM Cadeteria;";
            lector = command1.ExecuteReader();
            while (lector.Read())
            {
                DataBase.cadeteria.NombreCadeteria = Convert.ToString(lector[0]);
            }
            connection.Close();

            SqliteCommand command2 = connection.CreateCommand();

            connection.Open();
            command2.CommandText = $"SELECT * FROM Cliente;";
            lector = command2.ExecuteReader();
            while (lector.Read())
            {
                DataBase.cadeteria.Clientes.Add(new ClienteViewModel(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4])));
            }
            connection.Close();
            
            SqliteCommand command3 = connection.CreateCommand();

            connection.Open();
            command3.CommandText = $"SELECT * FROM Cadete;";
            lector = command3.ExecuteReader();
            while (lector.Read())
            {
                DataBase.cadeteria.Cadetes.Add(new CadeteViewModel(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3])));
            }
            connection.Close();

            noLeido = false;
        }

        return View(DataBase.cadeteria);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
