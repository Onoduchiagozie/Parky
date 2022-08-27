using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;


namespace ParkyWeb.Controllers;

[Authorize]
public class NationalparksController : Controller
{
    private readonly INationalRepository _npRepo;

    public NationalparksController(INationalRepository npRepo)
    {
        _npRepo = npRepo;
    }
    
    // GET
    public IActionResult Index()
    {
        return View(new NationPark(){ });
    }
    [HttpGet]
    public async Task<IActionResult> GetAllNationalPark()
    {
        return Json(new { data = await _npRepo.GetAllAsync(SD.NationalControllerUrl,HttpContext.Session.GetString("JWToken")) });
    }
    
    
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult>Delete(int id)
    {
        var status = await _npRepo.DeleteAsync(SD.NationalControllerUrl, id,HttpContext.Session.GetString("JWToken"));
        if (status)
        {
            return Json(new { success= true, message="Delete Successfull" });
        }
        return Json(new { success= false, message="Delete Not Successfull" });

    }

    // LOADING CREAT/UPDATE PAGE
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upsert(int? id)
    {  
        NationPark obj = new NationPark(){};
        if (id == null)
        {
            return View(obj);
        }
        obj = await _npRepo.GetAsync(SD.NationalControllerUrl, id.GetValueOrDefault(),HttpContext.Session.GetString("JWToken"));
        if (obj == null)
        {
            return NotFound();
        }
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(NationPark obj)
    {
        if (ModelState.IsValid)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null;
                await using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
                obj.Picture = p1;
            }
            else
            {
                var objFromDb = await _npRepo.GetAsync(SD.NationalControllerUrl, obj.Id,HttpContext.Session.GetString("JWToken"));
                obj.Picture = objFromDb.Picture;
                
            }
            if (obj.Id == 0)
            {
                await _npRepo.CreateAsync(SD.NationalControllerUrl, obj,HttpContext.Session.GetString("JWToken"));
            }
            else
            {
                await _npRepo.UpdateAsync(SD.NationalControllerUrl+obj.Id, obj,HttpContext.Session.GetString("JWToken"));
            }
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(obj);
        }
    }
}