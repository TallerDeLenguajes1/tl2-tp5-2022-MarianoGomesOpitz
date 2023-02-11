namespace SistemaCadeteria.Models;

public class Usuario
{
    private string name, user, role;

    public string Name { get => name; set => name = value; }
    public string User { get => user; set => user = value; }
    public string Role { get => role; set => role = value; }

    public Usuario() { }

    public Usuario(string name_, string user_, string role_)
    {
        this.name = name_;
        this.user = user_;
        this.role = role_;
    }
}