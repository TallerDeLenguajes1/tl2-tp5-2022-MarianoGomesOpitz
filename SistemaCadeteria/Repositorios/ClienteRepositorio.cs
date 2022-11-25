using System;
using System.Collections.Generic;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace SistemaCadeteria.Repositorios
{
    public interface IClienteRepository
    {
        public List<ClienteViewModel> GetAll();
        public ClienteViewModel GetById(int idCliente);
        public ClienteViewModel GetByName(string nombreCliente);
        public void Create(ClienteViewModel cliente);
        public void Update(ClienteViewModel cliente);
        public void Delete(int id);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly string cadenaConexion;

        public ClienteRepository(string _cadenaConexion_)
        {
            this.cadenaConexion = _cadenaConexion_;
        }

        public List<ClienteViewModel> GetAll()
        {
            List<ClienteViewModel> clientes = new();

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT * FROM Cliente;";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                clientes.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4])));
            }
            connection.Close();

            return (clientes);
        }
        public ClienteViewModel GetById(int idCliente)
        {
            ClienteViewModel cliente = new();

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Cliente WHERE IdCliente = '{idCliente}';";
            connection.Open();
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                cliente = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4]));
            }
            connection.Close();

            return (cliente);
        }
        public ClienteViewModel GetByName(string nombreCliente)
        {
            ClienteViewModel cliente = new();

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Cliente WHERE Nombre = '{nombreCliente}';";
            connection.Open();
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                cliente = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4]));
            }
            connection.Close();

            return (cliente);
        }
        public void Create(ClienteViewModel cliente)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Cliente (Nombre, Direccion, Telefono, DatosReferencia) VALUES ('{cliente.Nombre}', '{cliente.Direccion}', '{cliente.Telefono}', '{cliente.DatosReferenciaDireccion}');";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Update(ClienteViewModel cliente)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Cliente SET Nombre = '{cliente.Nombre}', Direccion = '{cliente.Direccion}', Telefono = '{cliente.Telefono}', DatosReferencia = '{cliente.DatosReferenciaDireccion}' WHERE IdCliente = '{cliente.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Delete(int idCliente)
        {

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Cliente WHERE IdCliente = '{idCliente}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}