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
        List<Cliente> GetAll();
        Cliente GetById(int idCliente);
        Cliente GetByName(string nombreCliente);
        void Create(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(int id);
    }
}