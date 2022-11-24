using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace SistemaCadeteria.Controllers;

public class CadeteController : Controller
{
    private readonly ILogger<CadeteController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;

    public CadeteController(ILogger<CadeteController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(DataBase.cadeteria.Cadetes);
    }

    public IActionResult PedidosAsignados(int id)
    {
        return View(DataBase.cadeteria.Cadetes.Find(w => w.Id == id));
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

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Cadete (Nombre, Direccion, Telefono) VALUES ('{cadete.Nombre}', '{cadete.Direccion}', '{cadete.Telefono}');";

            connection.Open();
            command.ExecuteNonQuery();

            command.CommandText = $"SELECT * FROM Cadete WHERE Nombre = '{cadete.Nombre}' AND Direccion = '{cadete.Direccion}' AND Telefono = '{cadete.Telefono}';";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                DataBase.cadeteria.Cadetes.Add(new CadeteViewModel(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3])));
            }
            connection.Close();


            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Editar(int id)
    {
        var cadete = DataBase.cadeteria.Cadetes.Find(x => x.Id == id);
        return View(new EditarCadeteViewModel(id, cadete.Nombre, cadete.Direccion, cadete.Telefono));
    }

    [HttpPost]
    public IActionResult Editar(EditarCadeteViewModel cadeteRecibido)
    {
        if (ModelState.IsValid)
        {
            var cadeteAEditar = DataBase.cadeteria.Cadetes.Find(y => y.Id == cadeteRecibido.Id);

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Cadete SET Nombre = '{cadeteRecibido.Nombre}', Direccion = '{cadeteRecibido.Direccion}', Telefono = '{cadeteRecibido.Telefono}' WHERE IdCadete = '{cadeteRecibido.Id}';";
            connection.Open();
            command.ExecuteNonQuery();

            command.CommandText = $"SELECT Nombre, Direccion, Telefono FROM Cadete WHERE IdCadete = '{cadeteRecibido.Id}';";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                cadeteAEditar.Nombre = Convert.ToString(lector[0]);
                cadeteAEditar.Direccion = Convert.ToString(lector[1]);
                cadeteAEditar.Telefono = Convert.ToInt64(lector[2]);
            }
            connection.Close();



            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Borrar(int id)
    {
        var cadeteABorrar = DataBase.cadeteria.Cadetes.Find(z => z.Id == id);

        foreach (var pedido in cadeteABorrar.Pedidos)
        {
            pedido.Estado = Convert.ToString((status)1);
            DataBase.cadeteria.PedidosNoAsignados.Add(pedido);
        }

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM Cadete WHERE IdCadete = '{id}';";
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();

        DataBase.cadeteria.Cadetes.Remove(cadeteABorrar);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
