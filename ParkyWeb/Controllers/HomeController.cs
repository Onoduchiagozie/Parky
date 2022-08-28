using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INationalRepository _npRepo;
    private readonly ITrailsRepository _trailsRepo;
    private readonly IAccountRepository _accountRepository;

    public HomeController(ILogger<HomeController> logger,IAccountRepository accountRepository,
        INationalRepository npRepo,ITrailsRepository trailsRepo)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _npRepo = npRepo;
        _trailsRepo = trailsRepo;
    }

    public async Task<IActionResult> Index()
    {
        IndexVm trailsPark = new IndexVm
        {
            NationParkIndex = await _npRepo.GetAllAsync(SD.NationalControllerUrl,HttpContext.Session.GetString("JWToken")),
            TrailsIndex = await _trailsRepo.GetAllAsync(SD.TrailAPIBaseUrl,HttpContext.Session.GetString("JWToken"))
        };
        return View(trailsPark);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        Users obj = new Users();
        return View(obj);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Users obj)
    {
        Users objUsers = await _accountRepository.LoginAsync(SD.AccountApiPath+"authenticate/",obj);
        if (objUsers.Token == null)
        {
            return View();
        }
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, objUsers.Username));
         identity.AddClaim(new Claim(ClaimTypes.Role, objUsers.Role));
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        
        HttpContext.Session.SetString("JWToken",objUsers.Token);
        TempData["alert"] = "Welcome  " + objUsers.Username;
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Users objToRegister)
    {
        bool objUsers = await _accountRepository.RegisterAsync(SD.AccountApiPath+"Register/",objToRegister);
        if (objUsers == false)
        {
            return View();
        }
        TempData["alert"] = "Registration Successfull";

        return RedirectToAction("Login");
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString("JWToken","");
        return RedirectToAction("Index");
    }
    
    [Route("/Home/AccessDenied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}