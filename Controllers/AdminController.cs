using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers;

public class AdminController : Controller
{
    private readonly CinemaDBContext _dbContext;
    public AdminController(CinemaDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ShowAllContentAdmins()
    {
        System.Console.WriteLine(_dbContext.Users);
        return View(await _dbContext.Users.Where(u => u.Role == "ContentAdmin").ToListAsync());
    }

    public IActionResult CreateContentAdmin()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CreateContentAdmin([Bind("Username,Password,Email,CreateTime,Role")] User user)
    {
        if (ModelState.IsValid)
        {
            user.Role = "ContentAdmin";
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> DeleteContentAdmin(int? id)
    {
        if (id == null || _dbContext.Users == null)
        {
            return NotFound();
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("DeleteContentAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_dbContext.Users == null)
        {
            return Problem("Entity set 'CinemaDBContext.Users'  is null.");
        }
        var user = await _dbContext.Users.FindAsync(id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
