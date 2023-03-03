using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using SistemaCadeteria.Repositorios;
using AutoMapper;
using NLog;

namespace SistemaCadeteria.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
    private readonly IMapper _mapper;
    private readonly IClienteRepository _clienteRepository;
    private readonly Logger log;

    public ClienteController(ILogger<ClienteController> logger, IClienteRepository clienteRepository, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        this._clienteRepository = clienteRepository;
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
                    var clientesVM = _mapper.Map<List<ClienteViewModel>>(_clienteRepository.GetAll());
                    return View(clientesVM);
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
            return RedirectToAction("Error");
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
            return RedirectToAction("Error");
            throw;
        }
    }

    [HttpPost]
    public IActionResult Crear(CrearClienteViewModel _cliente_)
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
                        var cliente = _mapper.Map<Cliente>(_cliente_);
                        _clienteRepository.Create(cliente);

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
            return RedirectToAction("Error");
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
                    Cliente cliente = _clienteRepository.GetById(id);
                    var clienteVM = _mapper.Map<EditarClienteViewModel>(cliente);
                    return View(clienteVM);

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
            return RedirectToAction("Error");
            throw;
        }
    }

    [HttpPost]
    public IActionResult Editar(EditarClienteViewModel _cliente_)
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
                        var cliente = _mapper.Map<Cliente>(_cliente_);
                        _clienteRepository.Update(cliente);

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
            return RedirectToAction("Error");
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
                    _clienteRepository.Delete(id);

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
            return RedirectToAction("Error");
            throw;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}