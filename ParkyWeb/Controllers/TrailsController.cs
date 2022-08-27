using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers;

public class TrailsController : Controller
{
    private readonly INationalRepository _npRepo;
    private readonly ITrailsRepository _trails;

    public TrailsController(INationalRepository npRepo,ITrailsRepository trails)
    {
        _npRepo = npRepo;
        _trails = trails;
    }
    
    // GET
    public IActionResult Index()
    {
        return View(new Trails(){ });
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrails()
    {
        return Json(new { data = await _trails.GetAllAsync(SD.TrailAPIBaseUrl,HttpContext.Session.GetString("JWToken")) });
    }
    
    [HttpDelete]
    public async Task<IActionResult>Delete(int id)
    {
        var status = await _trails.DeleteAsync(SD.TrailAPIBaseUrl, id,HttpContext.Session.GetString("JWToken"));
        if (status)
        {
            return Json(new { success= true, message="Delete Successfull" });
        }
        return Json(new { success= false, message="Delete Not Successfull" });

    }
    
    // LOADING CREAT/UPDATE PAGE
    public async Task<IActionResult> Upsert(int? id)
    {
        IEnumerable<NationPark> npList = await _npRepo.GetAllAsync(SD.NationalControllerUrl,HttpContext.Session.GetString("JWToken"));
        TrailsVM objVM = new TrailsVM {NationParkList = npList,Trails = new Trails()};
        
        // ALWAYS TRUE FOR CREATING 
        if (id == null)
        {
            return View(objVM);
        }
        // GET VALUES TO POPULATE FORM FOR EDITING trails
        objVM.Trails = await _trails.GetAsync(SD.TrailAPIBaseUrl, id.GetValueOrDefault(),HttpContext.Session.GetString("JWToken"));
        if (objVM.Trails == null)
        {
            return NotFound();
        }
        return View(objVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(TrailsVM obj)
    {
        if (ModelState.IsValid)
        {
           if (obj.Trails.Id == 0)
            {
                await _trails.CreateAsync(SD.TrailAPIBaseUrl,obj.Trails,HttpContext.Session.GetString("JWToken"));
            }
            else
            {
                await _trails.UpdateAsync(SD.TrailAPIBaseUrl+obj.Trails.Id, obj.Trails,HttpContext.Session.GetString("JWToken"));
            }
            return RedirectToAction(nameof(Index));
        }
        else
        {
             var npList = await _npRepo.GetAllAsync(SD.NationalControllerUrl,HttpContext.Session.GetString("JWToken"));
            TrailsVM objVM = new TrailsVM()
            {
                NationParkList = npList,
                Trails = obj.Trails
                
            };
            return View(objVM);
        }
    }
}