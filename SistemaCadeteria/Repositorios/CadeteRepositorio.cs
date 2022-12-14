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

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;
            SqliteCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT * FROM Cadete;";
            lector = command.ExecuteReader();
            while (lector.Read())
            {
                cadetes.Add(new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3])));
            }
            connection.Close();

            return (cadetes);
        }
        public CadeteViewModel GetById(int idCadete)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;

            SqliteCommand command1 = connection.CreateCommand();
            command1.CommandText = $"SELECT IdPedido FROM Pedido_Cadete WHERE IdCadete = '{idCadete}';";
            connection.Open();
            lector = command1.ExecuteReader();
            List<Int32> ids = new();
            while (lector.Read())
            {
                ids.Add(Convert.ToInt32(lector[0]));
            }
            connection.Close();

            List<PedidoViewModel> peds = new();
            foreach (var id in ids)
            {
                peds.Add(pedidoRepositorio.GetById(id));
            }

            CadeteViewModel cadete = new();
            SqliteCommand command2 = connection.CreateCommand();
            command2.CommandText = $"SELECT * FROM Cadete WHERE IdCadete = '{idCadete}';";
            connection.Open();
            lector = command2.ExecuteReader();
            while (lector.Read())
            {
                cadete = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]), peds);
            }
            connection.Close();

            return (cadete);
        }

        public CadeteViewModel GetByName(string nombreCadete)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteDataReader lector;

            CadeteViewModel cadete = new();
            SqliteCommand command2 = connection.CreateCommand();
            command2.CommandText = $"SELECT * FROM Cadete WHERE Nombre = '{nombreCadete}';";
            connection.Open();
            lector = command2.ExecuteReader();
            while (lector.Read())
            {
                cadete = new(Convert.ToInt32(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]), Convert.ToInt64(lector[3]));
            }
            connection.Close();

            SqliteCommand command1 = connection.CreateCommand();
            command1.CommandText = $"SELECT IdPedido FROM Pedido_Cadete WHERE IdCadete = '{cadete.Id}';";
            connection.Open();
            lector = command1.ExecuteReader();
            List<Int32> ids = new();
            while (lector.Read())
            {
                ids.Add(Convert.ToInt32(lector[0]));
            }
            connection.Close();

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
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Cadete (Nombre, Direccion, Telefono) VALUES ('{cadete.Nombre}', '{cadete.Direccion}', '{cadete.Telefono}');";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Update(CadeteViewModel cadete)
        {
            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Cadete SET Nombre = '{cadete.Nombre}', Direccion = '{cadete.Direccion}', Telefono = '{cadete.Telefono}' WHERE IdCadete = '{cadete.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Delete(int id)
        {

            SqliteConnection connection = new SqliteConnection(cadenaConexion);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Cadete WHERE IdCadete = '{id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}