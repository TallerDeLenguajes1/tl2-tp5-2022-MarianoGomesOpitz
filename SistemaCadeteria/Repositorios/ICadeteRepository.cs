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
        List<Cadete> GetAll();
        Cadete GetById(int idCadete);
        Cadete GetByName(string nombreCadete);
        void Create(Cadete cadete);
        void Update(Cadete cadete);
        void Delete(int id);
    }
}