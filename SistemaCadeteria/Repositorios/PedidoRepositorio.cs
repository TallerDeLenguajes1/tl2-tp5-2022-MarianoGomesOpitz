using System;
using System.Collections.Generic;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SqlClient;

namespace SistemaCadeteria.Repositorios
{
    public interface IPedidoRepository
    {
        public List<PedidoViewModel> GetAll();
        public PedidoViewModel GetById(int idPedido);
        public void Create(PedidoViewModel pedido);
        public void AsignarCadete(int idPedido, int idCadete);
        public void CambiarCadete(int idPedido, int idCadeteACambiar);
        public void UpdatePedido(PedidoViewModel pedido);
        public void UpdateEstado(int idPedido, string Estado);
        public void Delete(int id);
        public DataTable PedidosPorCliente();
        public DataTable PedidosPorCadete();
    }

    public class PedidoRepository : IPedidoRepository
    {
        private readonly string cadenaConexion;
        private readonly IClienteRepository clienteRepository;

        public PedidoRepository(string _cadenaConexion_)
        {
            this.cadenaConexion = _cadenaConexion_;
            this.clienteRepository = new ClienteRepository(_cadenaConexion_);
        }

        public List<PedidoViewModel> GetAll()
        {
            List<PedidoViewModel> pedidos = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString1 = $"SELECT * FROM Pedido;";
                var command1 = new SqliteCommand(queryString1, connection);

                connection.Open();

                using (var lector = command1.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ClienteViewModel auxCliente = clienteRepository.GetById(Convert.ToInt32(lector[1]));
                        pedidos.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[2]), Convert.ToString(lector[3]), auxCliente));
                    }
                }

                connection.Close();

                string queryString2 = $"SELECT IdPedido FROM Pedido_Cadete;";
                var command2 = new SqliteCommand(queryString2, connection);

                connection.Open();

                using (var lector = command2.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        for (int i = pedidos.Count() - 1; i >= 0; i--)
                        {
                            if (pedidos[i].NroPedido == Convert.ToInt32(lector[0]))
                            {
                                pedidos.RemoveAt(i);
                            }
                        }
                    }
                }

                connection.Close();
            }

            return (pedidos);
        }

        public PedidoViewModel GetById(int idPedido)
        {
            PedidoViewModel pedido = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Pedido WHERE IdPedido = '{idPedido}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ClienteViewModel auxCliente = clienteRepository.GetById(Convert.ToInt32(lector[1]));
                        pedido = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[2]), Convert.ToString(lector[3]), auxCliente);
                    }
                }

                connection.Close();
            }

            return (pedido);
        }

        public void Create(PedidoViewModel pedido)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"INSERT INTO Pedido (IdCliente, Observaciones, Estado) VALUES ('{pedido.Costumer.Id}', '{pedido.Observaciones}', '{pedido.Estado}');";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AsignarCadete(int idPedido, int idCadete)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"INSERT INTO Pedido_Cadete (IdPedido, IdCadete) VALUES ('{idPedido}', '{idCadete}');";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void CambiarCadete(int idPedido, int idCadeteACambiar)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"UPDATE Pedido_Cadete SET IdCadete = '{idCadeteACambiar}' WHERE IdPedido = '{idPedido}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void UpdatePedido(PedidoViewModel pedido)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"UPDATE Pedido SET Observaciones = '{pedido.Observaciones}' WHERE IdPedido = '{pedido.NroPedido}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void UpdateEstado(int idPedido, string Estado)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"UPDATE Pedido SET Estado = '{Estado}' WHERE IdPedido = '{idPedido}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int idPedido)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"DELETE FROM Pedido WHERE IdPedido = '{idPedido}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public DataTable PedidosPorCliente()
        {
            DataTable tabla = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = "SELECT * FROM PedidosPorCliente;";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                tabla.Load(command.ExecuteReader());

                connection.Close();
            }

            return (tabla);
        }

        public DataTable PedidosPorCadete()
        {
            DataTable tabla = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = "SELECT * FROM PedidosPorCadete;";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                tabla.Load(command.ExecuteReader());

                connection.Close();
            }

            return (tabla);
        }
    }
}