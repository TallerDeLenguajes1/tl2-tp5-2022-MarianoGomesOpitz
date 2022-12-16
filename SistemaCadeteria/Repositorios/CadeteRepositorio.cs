using System;
using System.Collections.Generic;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace SistemaCadeteria.Repositorios
{
    public interface ICadeteRepository
    {
        public List<CadeteViewModel> GetAll();
        public CadeteViewModel GetById(int idCadete);
        public CadeteViewModel GetByName(string nombreCadete);
        public void Create(CadeteViewModel cadete);
        public void Update(CadeteViewModel cadete);
        public void Delete(int id);
    }

    public class CadeteRepository : ICadeteRepository
    {
        private readonly string cadenaConexion;
        private readonly IPedidoRepository pedidoRepositorio;

        public CadeteRepository(string _cadenaConexion_)
        {
            this.cadenaConexion = _cadenaConexion_;
            this.pedidoRepositorio = new PedidoRepository(_cadenaConexion_);
        }

        public List<CadeteViewModel> GetAll()
        {
            List<CadeteViewModel> cadetes = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Cadete;";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        cadetes.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3])));
                    }
                }

                connection.Close();
            }

            return (cadetes);
        }

        public CadeteViewModel GetById(int idCadete)
        {
            List<Int32> ids = new();
            CadeteViewModel cadete = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString1 = $"SELECT IdPedido FROM Pedido_Cadete WHERE IdCadete = '{idCadete}';";
                var command1 = new SqliteCommand(queryString1, connection);

                connection.Open();

                using (var lector = command1.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ids.Add(Convert.ToInt32(lector[0]));
                    }
                }

                connection.Close();

                List<PedidoViewModel> peds = new();
                foreach (var id in ids)
                {
                    peds.Add(pedidoRepositorio.GetById(id));
                }

                string queryString2 = $"SELECT * FROM Cadete WHERE IdCadete = '{idCadete}';";
                var command2 = new SqliteCommand(queryString2, connection);

                connection.Open();

                using (var lector = command2.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        cadete = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), peds);
                    }
                }

                connection.Close();
            }

            return (cadete);
        }

        public CadeteViewModel GetByName(string nombreCadete)
        {
            CadeteViewModel cadete = new();
            List<Int32> ids = new();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"SELECT * FROM Cadete WHERE Nombre = '{nombreCadete}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                using (var lector = command.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        cadete = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]));
                    }
                }

                connection.Close();

                string queryString2 = $"SELECT IdPedido FROM Pedido_Cadete WHERE IdCadete = '{cadete.Id}';";
                var command2 = new SqliteCommand(queryString2, connection);

                connection.Open();

                using (var lector = command2.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ids.Add(Convert.ToInt32(lector[0]));
                    }
                }

                connection.Close();


            }
            List<PedidoViewModel> peds = new();
            foreach (var id in ids)
            {
                peds.Add(pedidoRepositorio.GetById(id));
            }

            cadete.Pedidos = peds;

            return (cadete);
        }

        public void Create(CadeteViewModel cadete)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"INSERT INTO Cadete (Nombre, Direccion, Telefono) VALUES ('{cadete.Nombre}', '{cadete.Direccion}', '{cadete.Telefono}');";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(CadeteViewModel cadete)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"UPDATE Cadete SET Nombre = '{cadete.Nombre}', Direccion = '{cadete.Direccion}', Telefono = '{cadete.Telefono}' WHERE IdCadete = '{cadete.Id}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString = $"DELETE FROM Cadete WHERE IdCadete = '{id}';";
                var command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}