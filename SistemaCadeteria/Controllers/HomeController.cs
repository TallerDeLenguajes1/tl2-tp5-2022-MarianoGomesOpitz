using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using Microsoft.Data.Sqlite;

namespace SistemaCadeteria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string queryString = "SELECT Nombre FROM Cadeteria;";
            var command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataBase.cadeteria.NombreCadeteria = Convert.ToString(reader[0]);
                }
            }

            connection.Close();
        }

        return View(DataBase.cadeteria);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
