using System;
using System.Collections.Generic;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace SistemaCadeteria.Repositorios
{
    public interface ILoginRepository
    {
        int CantFilas(string user, string password);
        Usuario DatosUsuario(string user, string password);
    }
}