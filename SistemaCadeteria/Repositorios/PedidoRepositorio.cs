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

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command1 = connection.CreateCommand();
            connection.Open();
            command1.CommandText = $"SELECT * FROM Pedido;";
            lector = command1.ExecuteReader();
            while (lector.Read())
            {
                ClienteViewModel auxCliente = clienteRepository.GetById(Convert.ToInt32(lector[1]));
                pedidos.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[2]), Convert.ToString(lector[3]), auxCliente));
            }
            connection.Close();

            SqliteCommand command2 = connection.CreateCommand();
            connection.Open();
            command2.CommandText = $"SELECT IdPedido FROM Pedido_Cadete;";
            lector = command2.ExecuteReader();
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
            connection.Close();

            return (pedidos);
        }
        public PedidoViewModel GetById(int idPedido)
        {
            PedidoViewModel pedido = new();

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Pedido WHERE IdPedido = '{idPedido}';";
            connection.Open();
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                ClienteViewModel auxCliente = clienteRepository.GetById(Convert.ToInt32(lector[1]));
                pedido = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[2]), Convert.ToString(lector[3]), auxCliente);
            }
            connection.Close();

            return (pedido);
        }
        public void Create(PedidoViewModel pedido)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Pedido (IdCliente, Observaciones, Estado) VALUES ('{pedido.Costumer.Id}', '{pedido.Observaciones}', '{pedido.Estado}');";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void AsignarCadete(int idPedido, int idCadete)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Pedido_Cadete (IdPedido, IdCadete) VALUES ('{idPedido}', '{idCadete}');";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void CambiarCadete(int idPedido, int idCadeteACambiar)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Pedido_Cadete SET IdCadete = '{idCadeteACambiar}' WHERE IdPedido = '{idPedido}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdatePedido(PedidoViewModel pedido)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Pedido SET Observaciones = '{pedido.Observaciones}' WHERE IdPedido = '{pedido.NroPedido}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateEstado(int idPedido, string Estado)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Pedido SET Estado = '{Estado}' WHERE IdPedido = '{idPedido}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Delete(int idPedido)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Pedido WHERE IdPedido = '{idPedido}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public DataTable PedidosPorCliente()
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PedidosPorCliente;";
            DataTable tabla = new();
            connection.Open();
            tabla.Load(command.ExecuteReader());
            connection.Close();
            return (tabla);
        }

        public DataTable PedidosPorCadete()
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PedidosPorCadete;";
            DataTable tabla = new();
            connection.Open();
            tabla.Load(command.ExecuteReader());
            connection.Close();
            return (tabla);
        }
    }
}