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
        private readonly JWTHelper _jwtHelper;

        public UserController(AppDbContext ctx, ILogger<UserController> logger, JWTHelper jwtHelper)
        {
            _ctx = ctx;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        [Route("/api/user/register")]
        public IActionResult Register([FromBody] User user)
        {
            _logger.LogInformation($"metoda {System.Reflection.MethodBase.GetCurrentMethod().Name}, otrzymane dane: login - {user.Login}, hasło - {user.Password}, email - {user.Email}");

            if (!(_ctx.Users.Where(u => u.Login == user.Login).FirstOrDefault() is null))
                return StatusCode(409);

            if (user.Login is null || user.Password is null || user.Email is null)
                return StatusCode(400);

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

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/user/login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation($"surowy request: {HttpContext.Request.Body.ToString()}");

            IActionResult response = Unauthorized();

            _logger.LogInformation($"metoda { System.Reflection.MethodBase.GetCurrentMethod().Name}, otrzymane dane: login - {user.Login}, hasło - {user.Password}, email - {user.Email}");

            if (_ctx.Users.Where(u => u.Login == user.Login).FirstOrDefault() is null)
                return StatusCode(404);

            User foundUser = _ctx.Users.Where(u => u.Login == user.Password && u.Password == user.Password).FirstOrDefault();

            if (foundUser is null)
                return StatusCode(404);

            var authenticatedUser = AuthenticateUser(user, foundUser);

            string tokenString = _jwtHelper.GenerateJsonWebToken(user);
            response = Ok(new { token = tokenString });

            return response;
        }

        private static User AuthenticateUser(User userToAuth, User foundUser)
        {
            User user = null;

            if (userToAuth.Login == foundUser.Login)
            {
                user = foundUser;
            }

            return user;
        }
    }
}