namespace SistemaCadeteria.Models;

public class Pedido
{
    //numero, obs, cliente, estado
    private int nroPedido;
    private string observaciones;
    private string estado;
    private Cliente costumer;

    public int NroPedido { get => nroPedido; set => nroPedido = value; }
    public string Observaciones { get => observaciones; set => observaciones = value; }
    internal string Estado { get => estado; set => estado = value; }
    public Cliente Costumer { get => costumer; set => costumer = value; }

    public Pedido()
    {
    }

    public Pedido(string obs, string est, Cliente C)
        {
            this.Observaciones = obs;
            this.Estado = est;
            this.Costumer = C;
        }

        public Pedido(int nro, string obs, string est, Cliente C)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
            this.Estado = est;
            this.Costumer = C;
        }

        public Pedido(int nro, string obs)
        {
            this.NroPedido = nro;
            this.Observaciones = obs;
        }
}

enum status
{
    En_Preparación = 1,
    En_Camino = 2,
    Entregado = 3,
}