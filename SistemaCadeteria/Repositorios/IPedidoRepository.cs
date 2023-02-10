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
        List<Pedido> GetAll();
        Pedido GetById(int idPedido);
        void Create(Pedido pedido);
        void AsignarCadete(int idPedido, int idCadete);
        void CambiarCadete(int idPedido, int idCadeteACambiar);
        void UpdatePedido(Pedido pedido);
        void UpdateEstado(int idPedido, string Estado);
        void Delete(int id);
        DataTable PedidosPorCliente();
        DataTable PedidosPorCadete();
    }
}