using System.Linq;
using chattr.Server.Helpers;
using chattr.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<UserController> _logger;
        private readonly JwtHelper _jwtHelper;

        public UserController(AppDbContext ctx, ILogger<UserController> logger, JwtHelper jwtHelper)
        {
            _ctx = ctx;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/users/register")]
        public IActionResult Register([FromBody] User user)
        {
            _logger.LogInformation($"metoda {System.Reflection.MethodBase.GetCurrentMethod()?.Name}, otrzymane dane: login - {user.Login}, hasło - {user.Password}, email - {user.Email}");

            if (!(_ctx.Users.Where(u => u.Login == user.Login).FirstOrDefault() is null))
                return StatusCode(409);

            if (user.Login is null || user.Password is null || user.Email is null)
                return StatusCode(400);

            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/users/auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            IActionResult response = Unauthorized();
            var foundUser = _ctx.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);

            if (foundUser is not null)
            {
                var tokenString = _jwtHelper.GenerateJsonWebToken();
                response = Ok(new { token = tokenString });
            }

            return response;
        }
        
        [Authorize]
        [HttpPost]
        [Route("/api/users/login")]
        public IActionResult Login([FromBody] User user)
        {
            var foundUser = _ctx.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);

            if (foundUser is null)
                return StatusCode(404);
            
            return Ok(foundUser);
        }
    }
}