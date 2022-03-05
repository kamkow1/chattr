using chattr.Server.Helpers;
using chattr.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<ChatController> _logger;
        private readonly JWTHelper _jwtHelper;
        private readonly IConfiguration _config;

        public ChatController(AppDbContext ctx, ILogger<ChatController> logger, JWTHelper jwtHelper, IConfiguration config)
        {
            _ctx = ctx;
            _logger = logger;
            _jwtHelper = jwtHelper;
            _config = config;
        }

        [HttpPost]
        [Route("/api/chats/create")]
        [Authorize]
        public IActionResult CreateChat([FromBody] Chat chat)
        {
            Chat newChat = new()
            {
                Topic = chat.Topic,
                Description = chat.Description,
                CreationDate = DateTime.Now
            };

            _ctx.Chats.Add(newChat);
            _ctx.SaveChanges();

            return StatusCode(200);
        }

        [HttpPost]
        [Route("/api/chats/get")]
        [Authorize]
        public IActionResult GetChatsForUser([FromBody] User user)
        {
            List<Chat> chats = _ctx.Chats.Where(c => c.Members.Contains(_ctx.Users.Find(user.Id))).ToList();
            _logger.LogInformation(JsonConvert.SerializeObject(chats));

            return Ok(chats);
        }
    }
}
