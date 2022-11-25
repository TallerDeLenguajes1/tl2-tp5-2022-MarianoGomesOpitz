namespace SistemaCadeteria.Models;

public class Persona
{
    private int id;
    private string nombre, direccion;
    private long telefono;

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }
    public long Telefono { get => telefono; set => telefono = value; }

    public Persona()
    { }
    
    public Persona(string name, string direcc, long tel)
    {
        this.Nombre = name;
        this.Direccion = direcc;
        this.Telefono = tel;
    }

    public Persona(int i, string name, string direcc, long tel)
    {
        this.Id = i;
        this.Nombre = name;
        this.Direccion = direcc;
        this.Telefono = tel;
    }
}