using Messenger.Database;
using Messenger.Helpers;
using Messenger.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("chats")]
    [ApiController]
    public class ChatsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly JWTService _jwtService;

        public ChatsController(IClientRepository repository, IChatRepository chatRepository, IMessageRepository messageRepository ,JWTService jwtService)
        {
            _clientRepository = repository;
            _jwtService = jwtService;
            _chatRepository = chatRepository;
        }

        [HttpPost("chats")]
        public async Task<JsonResult> UserChats([FromBody] string jwtToken)
        {

            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var chats = await _chatRepository.GetChatsAsync(idUser);

            var model = await _chatRepository.GetChatsModelAsync(chats, idUser);

            return Json(model);
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] int idChat)
        {
            var chat = await _chatRepository.GetChatAsync(idChat);
            var members = await _chatRepository.GetChatMembersAsync(idChat);
            var messeges = await _messageRepository.GetChatMessegesAsync(idChat);

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

            var chat = await _chatRepository.GetChatAsync(selectChat.idChat);

            var jwtToken = _jwtService.Verify(selectChat.token);
            int idUser = int.Parse(jwtToken.Issuer);

            var members = await _chatRepository.GetChatMembersAsync(selectChat.idChat);
            var messages = await _messageRepository.GetChatMessegesAsync(selectChat.idChat);

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
