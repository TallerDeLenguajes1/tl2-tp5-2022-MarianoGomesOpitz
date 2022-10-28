using System.ComponentModel.DataAnnotations;

namespace SistemaCadeteria.Models;

public class Persona
{
    private int id;
    private string nombre, direccion, telefono;

    public int Id { get => id; set => id = value; }
    [Required][Display(Name = "Nombre completo")]
    public string Nombre { get => nombre; set => nombre = value; }
    [Required][Display(Name = "Dirección")]
    public string Direccion { get => direccion; set => direccion = value; }
    [Required][Phone][Display(Name = "Número de teléfono")]
    public string Telefono { get => telefono; set => telefono = value; }

    public Persona()
    { }

    public Persona(int i, string name, string direcc, string tel)
    {
        this.Id = i;
        this.Nombre = name;
        this.Direccion = direcc;
        this.Telefono = tel;
    }
}