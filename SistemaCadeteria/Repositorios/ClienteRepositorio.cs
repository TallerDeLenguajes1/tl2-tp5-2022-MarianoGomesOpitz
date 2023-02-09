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
        public List<Cliente> GetAll();
        public Cliente GetById(int idCliente);
        public Cliente GetByName(string nombreCliente);
        public void Create(Cliente cliente);
        public void Update(Cliente cliente);
        public void Delete(int id);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly string cadenaConexion;

        public ClienteRepository(string _cadenaConexion_)
        {
            this.cadenaConexion = _cadenaConexion_;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> clientes = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Cliente;";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        clientes.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4])));
                    }
                }

                connection.Close();
            }

            return (clientes);
        }

        public Cliente GetById(int idCliente)
        {
            Cliente cliente = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Cliente WHERE IdCliente = '{idCliente}';";
                var command1 = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command1.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        cliente = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4]));
                    }
                }

                connection.Close();
            }

            return (cliente);
        }

        public Cliente GetByName(string nombreCliente)
        {
            Cliente cliente = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Cliente WHERE Nombre = '{nombreCliente}';";
                var command1 = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command1.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        cliente = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), Convert.ToString(lector[4]));
                    }
                }

                connection.Close();
            }

            return (cliente);
        }

        public void Create(Cliente cliente)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"INSERT INTO Cliente (Nombre, Direccion, Telefono, DatosReferencia) VALUES ('{cliente.Nombre}', '{cliente.Direccion}', '{cliente.Telefono}', '{cliente.DatosReferenciaDireccion}');";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(Cliente cliente)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"UPDATE Cliente SET Nombre = '{cliente.Nombre}', Direccion = '{cliente.Direccion}', Telefono = '{cliente.Telefono}', DatosReferencia = '{cliente.DatosReferenciaDireccion}' WHERE IdCliente = '{cliente.Id}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int idCliente)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"DELETE FROM Cliente WHERE IdCliente = '{idCliente}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}