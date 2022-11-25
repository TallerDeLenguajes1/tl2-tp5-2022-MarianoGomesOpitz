﻿using System.Diagnostics;
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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string user = HttpContext.Session.GetString("User");
        if (!(string.IsNullOrEmpty(user)))
        {
            return View(model: user);
        }
        else
        {
            return RedirectToAction("Login");
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
        SqliteCommand command = connection.CreateCommand();
        connection.Open();
        command.CommandText = $"SELECT COUNT(*) FROM Usuarios WHERE User LIKE '{user}' AND Password = '{password}';";
        count = Convert.ToInt32(command.ExecuteScalar());
        connection.Close();

        if (count > 0)
        {
            HttpContext.Session.SetString("User", user);
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
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
