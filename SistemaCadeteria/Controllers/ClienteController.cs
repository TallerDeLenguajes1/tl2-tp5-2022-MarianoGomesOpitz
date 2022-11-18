using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace SistemaCadeteria.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;

    public ClienteController(ILogger<ClienteController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(DataBase.cadeteria.Clientes);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearClienteViewModel cliente)
    {
        if (ModelState.IsValid)
        {

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Cliente (Nombre, Direccion, Telefono, DatosReferencia) VALUES ('{cliente.Nombre}', '{cliente.Direccion}', '{cliente.Telefono}', '{cliente.DatosReferenciaDireccion}');";

            connection.Open();
            command.ExecuteNonQuery();

            command.CommandText = $"SELECT * FROM Cliente WHERE Nombre = '{cliente.Nombre}' AND Direccion = '{cliente.Direccion}' AND Telefono = '{cliente.Telefono}' AND DatosReferencia = '{cliente.DatosReferenciaDireccion}';";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                DataBase.cadeteria.Clientes.Add(new ClienteViewModel(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4])));
            }
            connection.Close();


            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Editar(int id)
    {
        var cliente = DataBase.cadeteria.Clientes.Find(x => x.Id == id);
        return View(new EditarClienteViewModel(id, cliente.Nombre, cliente.Direccion, cliente.Telefono, cliente.DatosReferenciaDireccion));
    }

    [HttpPost]
    public IActionResult Editar(EditarClienteViewModel clienteRecibido)
    {
        if (ModelState.IsValid)
        {
            var clienteAEditar = DataBase.cadeteria.Clientes.Find(y => y.Id == clienteRecibido.Id);

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Cliente SET Nombre = '{clienteRecibido.Nombre}', Direccion = '{clienteRecibido.Direccion}', Telefono = '{clienteRecibido.Telefono}', DatosReferencia = '{clienteRecibido.DatosReferenciaDireccion}' WHERE IdCliente = '{clienteRecibido.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            clienteAEditar.Nombre = clienteRecibido.Nombre;
            clienteAEditar.Direccion = clienteRecibido.Direccion;
            clienteAEditar.Telefono = clienteRecibido.Telefono;
            clienteAEditar.DatosReferenciaDireccion = clienteRecibido.DatosReferenciaDireccion;



            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Borrar(int id)
    {
        var clienteABorrar = DataBase.cadeteria.Clientes.Find(z => z.Id == id);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM Cliente WHERE IdCliente = '{id}';";
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();

        DataBase.cadeteria.Clientes.Remove(clienteABorrar);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
