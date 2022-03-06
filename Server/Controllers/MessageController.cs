using chattr.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<UserController> _logger;

        public MessageController(AppDbContext ctx, ILogger<UserController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/msg/get")]
        [Authorize]
        public IActionResult GetMessages([FromBody] Chat chat)
        {
            _logger.LogInformation($"dane z requesta: {JsonConvert.SerializeObject(chat)}");
            var messages = _ctx.Messages.Where(m => m.ChatId == chat.Id)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    m.ChatId,
                    m.ParentId,
                    m.SendDate,
                    m.UserId,
                    Username = _ctx.Users.FirstOrDefault(u => u.Id == m.UserId).Login
                })
                .ToList();

            if (!messages.Any())
                return NotFound("nie znaleziono wiadomości dla tego czatu");

            return Ok(messages);
        }

        [HttpPost]
        [Route("/api/msg/send")]
        [Authorize]
        public IActionResult SendMessage([FromBody] Message msg)
        {
            _logger.LogInformation($"nazwa usera {_ctx.Users.FirstOrDefault(u => u.Id == msg.UserId)?.Login}");
            
            Message message = new()
            { 
                Content = msg.Content,
                SendDate = DateTime.Now,
                ParentId = msg.ParentId,
                UserId = msg.UserId,
                ChatId = msg.ChatId,
                Username = _ctx.Users.FirstOrDefault(u => u.Id == msg.UserId)?.Login.ToString()
            };

            _ctx.Messages.Add(message);
            _ctx.SaveChanges();

            return StatusCode(200);
        }
    }
}
