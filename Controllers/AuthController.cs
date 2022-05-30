using Messenger.Database;
using Messenger.Helpers;
using Messenger.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IClientRepository _repository;
        private readonly JWTService _jwtService;

        public AuthController(IClientRepository repository, JWTService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }


        [HttpGet("start")]
        public IActionResult Start()
        {
            return Ok("success");
        }
       
        [HttpPost("login")]
        public JsonResult Login([FromBody] string phone)
        {
            
            Client client = _repository.GetByPhone(phone);
            if (client == null)
                return Json("User not found");

            string token = _jwtService.Generate(client.Id);
            

            return Json(token);
        }

        
        [HttpPost("user")]
        public IActionResult User([FromBody] string jwtToken)
        {   
            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var user = _repository.GetById(idUser);
            return Ok(user);
        }

        [HttpPost("chats")]
        public IActionResult UserChats([FromBody] string jwtToken)
        {

            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var chats = _repository.GetChats(idUser);

            var model = _repository.GetChatsModel(chats, idUser);

            return Ok(model);
        }

      
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] int idChat)
        {
            var chat = await _repository.GetChat(idChat);
            var members = _repository.GetChatMembers(idChat);
            var messeges = _repository.GetChatMesseges(idChat);

            ChatViewModel model = new ChatViewModel
            {
                chat = chat,
                members = members,
                chatMesseges = messeges
            };

            return Ok(model);
        }

        [HttpPost("selectChat")]
        public async Task<IActionResult> SelectChat([FromBody] ChatInfo selectChat)
        {
            
            var chat = await _repository.GetChat(selectChat.idChat);

            var jwtToken = _jwtService.Verify(selectChat.token);
            int idUser = int.Parse(jwtToken.Issuer);

            var members = _repository.GetChatMembers(selectChat.idChat);
            var messages = _repository.GetChatMesseges(selectChat.idChat);

            var chatUser = members.FirstOrDefault(member => member.client.Id != idUser);

            ChatViewModel model = new ChatViewModel
            {   
        
                chat = chat,
                members = members,
                chatMesseges = messages,
                chatUser = chatUser
            };

            return Ok(model);
        }
    }
}
