using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using SistemaCadeteria.Repositorios;
using AutoMapper;
using NLog;

namespace SistemaCadeteria.Controllers;

public class PedidoController : Controller
{
    private readonly ILogger<PedidoController> _logger;
    private readonly IMapper _mapper;
    private readonly IPedidoRepository _pedidoRepositorio;
    private readonly IClienteRepository _clienteRepositorio;
    private readonly ICadeteRepository _cadeteRepositorio;
    private readonly Logger log;

    public PedidoController(ILogger<PedidoController> logger, IPedidoRepository pedidoRepositorio, IClienteRepository clienteRepositorio, ICadeteRepository cadeteRepositorio, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        this._pedidoRepositorio = pedidoRepositorio;
        this._clienteRepositorio = clienteRepositorio;
        this._cadeteRepositorio = cadeteRepositorio;
        this.log = LogManager.GetCurrentClassLogger();
    }

    public IActionResult Index()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    var pedidosVM = _mapper.Map<List<PedidoViewModel>>(_pedidoRepositorio.GetAll());
                    return View(pedidosVM);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult Crear()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult Crear(CrearPedidoViewModel pedido)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            Cliente auxCliente = _clienteRepositorio.GetByName(pedido.NombreCliente);
                            _pedidoRepositorio.Create(new(pedido.Observaciones, Convert.ToString((status)1), auxCliente));

                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            log.Info("\nNo se encontró ese cliente");
                            return RedirectToAction("NoExiste");
                            throw;
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult NoExiste()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult Asignar(int id)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    PedidoACadete pedCad = new(id, _cadeteRepositorio.GetAll());

                    return View(pedCad);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult Asignar(int IdPedido, int IdCadete)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    _pedidoRepositorio.AsignarCadete(IdPedido, IdCadete);

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult ModificarEstado(int idPedido)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                Pedido pedido = _pedidoRepositorio.GetById(idPedido);
                var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
                return View(pedidoVM);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult ModificarEstado(int idPedido, int Estado)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                _pedidoRepositorio.UpdateEstado(idPedido, Convert.ToString((status)Estado));

                return RedirectToAction("Index", "Cadete");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult CambiarCadete(int idPedido, int idCadete)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    CambiarCadete pedCad = new(idPedido, idCadete, _cadeteRepositorio.GetAll());

                    return View(pedCad);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult CambiarCadete(int IdPedido, int IdCadete, int IdCadeteACambiar)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    _pedidoRepositorio.CambiarCadete(IdPedido, IdCadeteACambiar);

                    return RedirectToAction("Index", "Cadete");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult Editar(int id)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    Pedido pedido = _pedidoRepositorio.GetById(id);
                    return View(new EditarPedidoViewModel(pedido.NroPedido, pedido.Observaciones));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public IActionResult Editar(EditarPedidoViewModel _pedido_)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        _pedidoRepositorio.UpdatePedido(new(_pedido_.NroPedido, _pedido_.Observaciones));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    public IActionResult Borrar(int id)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    _pedidoRepositorio.Delete(id);

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
