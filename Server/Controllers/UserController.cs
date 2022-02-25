using System.Linq;
using chattr.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<UserController> _logger;

        public UserController(AppDbContext ctx, ILogger<UserController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/user/register")]
        public IActionResult Register([FromBody] User user)
        {
            _logger.LogInformation($"otrzymane dane: login - {user.Login}, hasło - {user.Password}, email - {user.Email}");

            if (!(_ctx.Users.Where(u => u.Login == user.Login).FirstOrDefault() is null))
                return StatusCode(409);

            if (user.Login is null || user.Password is null || user.Email is null)
            {
                return StatusCode(400);
            }
            else
            {
                User userToAdd = new()
                {
                    Login = user.Login,
                    Password = user.Password,
                    Email = user.Email
                };

                _ctx.Users.Add(user);
                _ctx.SaveChanges();

                return StatusCode(201);
            }
        }
    }
}