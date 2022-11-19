using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace SistemaCadeteria.Controllers;

public class PedidoController : Controller
{
    private readonly ILogger<PedidoController> _logger;

    static string connectionString = "Data Source=DB/PedidosDB.db;Cache=Shared";
    SqliteConnection connection = new SqliteConnection(connectionString);
    SqliteDataReader lector;

    public PedidoController(ILogger<PedidoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(DataBase.cadeteria.PedidosNoAsignados);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(CrearPedidoViewModel pedido)
    {

        if (ModelState.IsValid)
        {
            SqliteCommand command = connection.CreateCommand();

            ClienteViewModel cliente = DataBase.cadeteria.Clientes.Find(i => i.Nombre == pedido.NombreCliente);

            command.CommandText = $"INSERT INTO Pedido (IdCliente, Observaciones, Estado) VALUES ('{cliente.Id}', '{pedido.Observaciones}', '{Convert.ToString((status)1)}');";

            connection.Open();
            command.ExecuteNonQuery();

            command.CommandText = $"SELECT * FROM Pedido WHERE IdCliente = '{cliente.Id}' AND Observaciones = '{pedido.Observaciones}' AND Estado = '{Convert.ToString((status)1)}';";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                var cost = DataBase.cadeteria.Clientes.Find(c => c.Id == Convert.ToInt32(lector[1]));
                DataBase.cadeteria.PedidosNoAsignados.Add(new PedidoViewModel(Convert.ToInt32(lector[0]), Convert.ToString(lector[2]), Convert.ToString(lector[3]), cost));
            }
            connection.Close();

            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Asignar(int id)
    {
        PedidoACadete pedCad = new(id, DataBase.cadeteria.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO Pedido_Cadete (IdPedido, IdCadete) VALUES ('{IdPedido}', '{IdCadete}');";

        connection.Open();
        command.ExecuteNonQuery();

        command.CommandText = $"SELECT IdPedido, IdCadete FROM Pedido_Cadete WHERE IdPedido = '{IdPedido}' AND IdCadete = '{IdCadete}';";
        lector = command.ExecuteReader();
        while (lector.Read())
        {
            var PedidoAMover = DataBase.cadeteria.PedidosNoAsignados.Find(p => p.NroPedido == Convert.ToInt32(lector[0]));
            var CadeteAAsignar = DataBase.cadeteria.Cadetes.Find(a => a.Id == Convert.ToInt32(lector[1]));
            CadeteAAsignar.Pedidos.Add(PedidoAMover);
            DataBase.cadeteria.PedidosNoAsignados.Remove(PedidoAMover);
        }
        connection.Close();


        return RedirectToAction("Index");
    }

    public IActionResult ModificarEstado(int idPedido)
    {
        PedidoViewModel pedido = null;

        SqliteCommand command = connection.CreateCommand();

        command.CommandText = $"SELECT IdPedido, IdCadete FROM Pedido_Cadete WHERE IdPedido = '{idPedido}';";
        connection.Open();
        lector = command.ExecuteReader();
        while (lector.Read())
        {
            CadeteViewModel cadete = DataBase.cadeteria.Cadetes.Find(c => c.Id == Convert.ToInt32(lector[1]));
            pedido = cadete.Pedidos.Find(p => p.NroPedido == Convert.ToInt32(lector[0]));
        }
        connection.Close();

        return View(pedido);
    }

    [HttpPost]
    public IActionResult ModificarEstado(int idPedido, int Estado)
    {
        PedidoViewModel pedido = null;
        foreach (var cadete in DataBase.cadeteria.Cadetes)
        {
            pedido = cadete.Pedidos.Find(p => p.NroPedido == idPedido);

            if (pedido != null)
            {
                break;
            }
        }

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE Pedido SET Estado = '{Convert.ToString((status)Estado)}' WHERE IdPedido = '{idPedido}';";
        connection.Open();
        command.ExecuteNonQuery();

        command.CommandText = $"SELECT Estado FROM Pedido WHERE IdPedido = '{idPedido}';";
        connection.Open();
        lector = command.ExecuteReader();
        while (lector.Read())
        {

            pedido.Estado = Convert.ToString(lector[0]);
        }
        connection.Close();

        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        CambiarCadete pedCad = new(idPedido, idCadete, DataBase.cadeteria.Cadetes);

        return View(pedCad);
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int IdCadeteACambiar)
    {
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = $"UPDATE Pedido_Cadete SET IdCadete = '{IdCadeteACambiar}' WHERE IdPedido = '{IdPedido}';";

        connection.Open();
        command.ExecuteNonQuery();

        command.CommandText = $"SELECT IdPedido, IdCadete FROM Pedido_Cadete WHERE IdPedido = '{IdPedido}' AND IdCadete = '{IdCadeteACambiar}';";
        lector = command.ExecuteReader();
        while (lector.Read())
        {
            var cadeteOriginal = DataBase.cadeteria.Cadetes.Find(i => i.Id == IdCadete);
            var pedidoAMover = cadeteOriginal.Pedidos.Find(p => p.NroPedido == Convert.ToInt32(lector[0]));
            var cadeteACambiar = DataBase.cadeteria.Cadetes.Find(i => i.Id == Convert.ToInt32(lector[1]));
            cadeteACambiar.Pedidos.Add(pedidoAMover);
            cadeteOriginal.Pedidos.Remove(pedidoAMover);
        }
        connection.Close();

        return RedirectToAction("Index", "Cadete");
    }

    public IActionResult Editar(int id)
    {
        var pedido = DataBase.cadeteria.PedidosNoAsignados.Find(x => x.NroPedido == id);

        if (pedido == null)
        {
            foreach (var cadete in DataBase.cadeteria.Cadetes)
            {
                pedido = cadete.Pedidos.Find(l => l.NroPedido == id);

                if (pedido != null)
                {
                    break;
                }
            }
        }

        return View(new EditarPedidoViewModel(id, pedido.Observaciones));
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel pedidoRecibido)
    {
        if (ModelState.IsValid)
        {
            string controller = "";

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Pedido SET Observaciones = '{pedidoRecibido.Observaciones}' WHERE IdPedido = '{pedidoRecibido.NroPedido}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            var pedidoAEditar = DataBase.cadeteria.PedidosNoAsignados.Find(y => y.NroPedido == pedidoRecibido.NroPedido);
            if (pedidoAEditar == null)
            {
                foreach (var cadete in DataBase.cadeteria.Cadetes)
                {
                    pedidoAEditar = cadete.Pedidos.Find(l => l.NroPedido == pedidoRecibido.NroPedido);
                    if (pedidoAEditar != null)
                    {
                        controller = "Cadete";
                        break;
                    }
                }
            }
            else
            {
                controller = "Pedido";
            }

            command.CommandText = $"SELECT Observaciones FROM Pedido WHERE IdPedido = '{pedidoRecibido.NroPedido}';";
            connection.Open();
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                pedidoAEditar.Observaciones = Convert.ToString(lector[0]);
            }
            connection.Close();

            return RedirectToAction("Index", controller);
        }
        else
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Borrar(int id)
    {
        string controller = "";

        SqliteCommand command1 = connection.CreateCommand();
        command1.CommandText = $"DELETE FROM Pedido_Cadete WHERE IdPedido = '{id}';";
        connection.Open();
        command1.ExecuteNonQuery();
        connection.Close();

        SqliteCommand command2 = connection.CreateCommand();
        command2.CommandText = $"DELETE FROM Pedido WHERE IdPedido = '{id}';";
        connection.Open();
        command2.ExecuteNonQuery();
        connection.Close();

        var pedidoABorrar = DataBase.cadeteria.PedidosNoAsignados.Find(z => z.NroPedido == id);
        if (pedidoABorrar == null)
        {
            foreach (var cadete in DataBase.cadeteria.Cadetes)
            {
                pedidoABorrar = cadete.Pedidos.Find(z => z.NroPedido == id);
                if (pedidoABorrar != null)
                {
                    cadete.Pedidos.Remove(pedidoABorrar);
                    controller = "Cadete";
                    break;
                }
            }
        }
        else
        {
            DataBase.cadeteria.PedidosNoAsignados.Remove(pedidoABorrar);
            controller = "Pedido";
        }


        return RedirectToAction("Index", controller);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
