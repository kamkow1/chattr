using chattr.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<ChatController> _logger;

        public ChatController(AppDbContext ctx, ILogger<ChatController> logger)
        {
            _ctx = ctx;
            _logger = logger;
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

        [HttpGet]
        [Route("/api/chats/get")]
        [Authorize]
        public IActionResult GetChatsForUser()
        {
            //List<Chat> chats = _ctx.Chats.Where(c => c.Members.Contains(_ctx.Users.Find(user.Id))).ToList();
            List<Chat> chats = _ctx.Chats.ToList();
            _logger.LogInformation(JsonConvert.SerializeObject(chats));

            return Ok(chats);
        }
    }
}
