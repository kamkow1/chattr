using System.Linq;
using chattr.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public UserController(AppDbContext ctx) => _ctx = ctx;

        [HttpPost]
        [Route("/api/user/register")]
        public IActionResult Register(string login, string password, string email)
        {
            if (!(_ctx.Users.Where(u => u.Login == login).FirstOrDefault() is null))
                return StatusCode(409);

            if (login is null || password is null || email is null)
            {
                return StatusCode(400);
            }
            else
            {
                User user = new()
                {
                    Login = login,
                    Password = password,
                    Email = email
                };

                _ctx.Users.Add(user);
                _ctx.SaveChanges();

                return StatusCode(200);
            }
        }
    }
}