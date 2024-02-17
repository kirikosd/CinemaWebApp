using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CinemaApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Start()
        {
            return View("~/Views/_StartPage.cshtml");
        }

        private readonly CinemaDBContext _dbContext;
        public AccountController(CinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(string username, string password, string email)
        {
            //using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                Role = "Customer",
                CreateTime = DateTime.UtcNow
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return View("~/Views/Customer/Index.cshtml");
        }
        [HttpGet("login")]
        public async Task<ActionResult<User>> Login(string username, string password)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return Unauthorized("User not Found");
            }
            if (user != null)
            {
                if (user.Password == password)
                {
                    string pathh;
                    if (user.Role == "Admin")
                    {
                        pathh = "~/Views/Admin/Index.cshtml";
                    }
                    else if (user.Role == "ContentAdmin"){
                        pathh = "~/Views/ContentAdmin/Index.cshtml";
                    }
                    else
                    {
                        pathh = "~/Views/Customer/Index.cshtml";
                    }
                    return View(pathh);
                }
                else
                {
                    return Unauthorized("Wrong Password");
                }
            }
            return Ok();
        }
    }
}

