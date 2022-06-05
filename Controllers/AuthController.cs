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
        public async Task<JsonResult> Login([FromBody] string phone)
        {
            
            Client client = await _repository.GetByPhoneAsync(phone);
            if (client == null)
                return Json("User not found");

            string token = _jwtService.Generate(client.Id);
            

            return Json(token);
        }

        
        [HttpPost("user")]
        public async Task<IActionResult> User([FromBody] string jwtToken)
        {   
            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var user = await _repository.GetByIdAsync(idUser);
            return Ok(user);
        }

        [HttpPost("chats")]
        public async Task<IActionResult> UserChats([FromBody] string jwtToken)
        {

            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var chats = await _repository.GetChatsAsync(idUser);

            var model = await _repository.GetChatsModelAsync(chats, idUser);

            return Ok(model);
        }

      
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] int idChat)
        {
            var chat = await _repository.GetChatAsync(idChat);
            var members = await _repository.GetChatMembersAsync(idChat);
            var messeges = await _repository.GetChatMessegesAsync(idChat);

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
            
            var chat = await _repository.GetChatAsync(selectChat.idChat);

            var jwtToken = _jwtService.Verify(selectChat.token);
            int idUser = int.Parse(jwtToken.Issuer);

            var members = await _repository.GetChatMembersAsync(selectChat.idChat);
            var messages = await _repository.GetChatMessegesAsync(selectChat.idChat);

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

        [HttpPost("readMessages")]
        public async Task ReadMessages([FromBody] ChatInfo chat)
        {
            var jwtToken = _jwtService.Verify(chat.token);
            int idUser = int.Parse(jwtToken.Issuer);
            await _repository.ReadMessagesAsync(chat.idChat, idUser);
        }

        [HttpPost("messages")]
        public async Task<IActionResult> GetMessages([FromBody] MessagesLimit data)
        {

            var messages = await _repository.GetMessagesByLimitAsync(data.idChat, data.limit, data.page);
            return Ok(messages);
        }

    }
}
