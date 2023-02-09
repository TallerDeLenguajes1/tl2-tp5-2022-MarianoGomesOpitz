namespace SistemaCadeteria.Models;

public class Cliente : Persona
{
    private string datosReferenciaDireccion;

    public string DatosReferenciaDireccion { get => datosReferenciaDireccion; set => datosReferenciaDireccion = value; }

    public Cliente() : base()
    { }
    
public Cliente(string name, string direcc, long tel, string datosRef) : base(name, direcc, tel)
        {
            this.DatosReferenciaDireccion = datosRef;
        }

        public Cliente(int i, string name, string direcc, long tel, string datosRef) : base(i, name, direcc, tel)
        {
            this.DatosReferenciaDireccion = datosRef;
        }
}