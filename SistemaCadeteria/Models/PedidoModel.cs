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
        this.NroPedido = 0;
        this.Observaciones = "";
        this.Estado = Convert.ToString((status)1);
        this.Costumer = new Cliente();
    }

    public Pedido(int i, string obs, int est, Cliente cos)
    {
        this.NroPedido = i;

        this.Observaciones = obs;

        this.Estado = Convert.ToString((status)est);

        this.Costumer = cos;
    }
}