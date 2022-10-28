using System.ComponentModel.DataAnnotations;

namespace SistemaCadeteria.Models;

public class Cliente : Persona
{
    private string datosReferenciaDireccion;

    [StringLength(50)][Display(Name = "Datos de referencia de la dirección")]
    public string DatosReferenciaDireccion { get => datosReferenciaDireccion; set => datosReferenciaDireccion = value; }

    public Cliente() : base()
    { }

    public Cliente(int i, string name, string direcc, string tel, string datosRef) : base(i, name, direcc, tel)
    {
        this.DatosReferenciaDireccion = datosRef;
    }
}