using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;
using SistemaCadeteria.Repositorios;
using AutoMapper;
using NLog;

namespace SistemaCadeteria.Controllers;

public class CadeteController : Controller
{
    private readonly ILogger<CadeteController> _logger;
    private readonly IMapper _mapper;

    private readonly ICadeteRepository _cadeteRepository;
    private readonly Logger log;

    public CadeteController(ILogger<CadeteController> logger, IMapper mapper, ICadeteRepository cadeteRepository)
    {
        this._logger = logger;
        this._mapper = mapper;
        this._cadeteRepository = cadeteRepository;
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
                    var cadetesVM = _mapper.Map<List<CadeteViewModel>>(_cadeteRepository.GetAll());
                    return View(cadetesVM);
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

    public IActionResult CadeteNombre()
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                string name = HttpContext.Session.GetString("Name");
                if (rol == "Cadete")
                {
                    var cadeteVM = _mapper.Map<CadeteViewModel>(_cadeteRepository.GetByName(name));
                    return View(cadeteVM);
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

    public IActionResult PedidosAsignados(int id)
    {
        try
        {
            string user = HttpContext.Session.GetString("User");
            if (!(string.IsNullOrEmpty(user)))
            {
                string rol = HttpContext.Session.GetString("Role");
                if (rol == "Admin")
                {
                    var cadeteVM = _mapper.Map<CadeteViewModel>(_cadeteRepository.GetById(id));
                    return View(cadeteVM);
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
    public IActionResult Crear(CrearCadeteViewModel _cadete_)
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
                        var cadete = _mapper.Map<Cadete>(_cadete_);
                        _cadeteRepository.Create(cadete);

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
                    Cadete cadete = _cadeteRepository.GetById(id);
                    var cadeteVM = _mapper.Map<EditarCadeteViewModel>(cadete);
                    return View(cadeteVM);
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
    public IActionResult Editar(EditarCadeteViewModel _cadete_)
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
                        var cadete = _mapper.Map<Cadete>(_cadete_);
                        _cadeteRepository.Update(cadete);

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
                    _cadeteRepository.Delete(id);

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