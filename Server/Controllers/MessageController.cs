using chattr.Server.Helpers;
using chattr.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace chattr.Server.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<UserController> _logger;
        private readonly JWTHelper _jwtHelper;
        private readonly IConfiguration _config;

        public MessageController(AppDbContext ctx, ILogger<UserController> logger, JWTHelper jwtHelper, IConfiguration config)
        {
            _ctx = ctx;
            _logger = logger;
            _jwtHelper = jwtHelper;
            _config = config;
        }

        [HttpPost]
        [Route("/api/msg/get")]
        [Authorize]
        public IActionResult GetMessages([FromBody] Chat chat)
        {
            _logger.LogInformation($"dane z requesta: {JsonConvert.SerializeObject(chat)}");
            var messages = _ctx.Messages.Where(m => m.ChatId == chat.Id + 1).ToList();

            /*if (!messages.Any())
                return StatusCode(404);*/

            return Ok(messages);
        }

        [HttpPost]
        [Route("/api/msg/send")]
        [Authorize]
        public IActionResult SendMessage([FromBody] Message msg)
        {
            string token = HttpContext.Session.GetString("TOKEN");
            if (string.IsNullOrEmpty(token))
                return StatusCode(500);

            if (!_jwtHelper.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
                return StatusCode(401);

            Message message = new()
            { 
                Content = msg.Content,
                SendDate = DateTime.Now,
                ParentId = msg.ParentId,
                UserId = msg.UserId
            };

            _ctx.Messages.Add(message);
            _ctx.SaveChanges();

            return StatusCode(200);
        }
    }
}
