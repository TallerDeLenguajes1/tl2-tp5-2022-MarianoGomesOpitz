using System;
using System.Collections.Generic;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace SistemaCadeteria.Repositorios
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string cadenaConexion;

        public LoginRepository(IConexionRepository conexion)
        {
            this.cadenaConexion = conexion.GetConnectionString();
        }
        public int CantFilas(string user, string password)
        {
            int count = 0;
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString1 = $"SELECT COUNT(*) FROM Usuarios WHERE User ='{user}' AND Password = '{password}';";
                var command1 = new SqliteCommand(queryString1, connection);

                connection.Open();

                count = Convert.ToInt32(command1.ExecuteScalar());

                connection.Close();
            }

            return count;
        }
        public Usuario DatosUsuario(string user, string password)
        {
            Usuario us = null;
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string queryString2 = $"SELECT Name, User, Role FROM Usuarios WHERE User ='{user}' AND Password = '{password}';";
                var command2 = new SqliteCommand(queryString2, connection);

                connection.Open();

                using (var lector = command2.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        us = new(Convert.ToString(lector[0]), Convert.ToString(lector[1]), Convert.ToString(lector[2]));
                    }
                }

                connection.Close();
            }

            return us;
            }
        }
    }
