using Messenger.Database;
using Messenger.Helpers;
using Messenger.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("messages")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly JWTService _jwtService;

        public MessageController(IClientRepository repository, IChatRepository chatRepository, IMessageRepository messageRepository, JWTService jwtService)
        {
            _clientRepository = repository;
            _jwtService = jwtService;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost("messages")]
        public async Task<IActionResult> GetMessages([FromBody] MessagesLimit data)
        {
            var messages = await _messageRepository.GetMessagesByLimitAsync(data.idChat, data.limit, data.page);
            return Ok(messages);
        }

        [HttpPost("readMessages")]
        public async Task ReadMessages([FromBody] ChatInfo chat)
        {
            var jwtToken = _jwtService.Verify(chat.token);
            int idUser = int.Parse(jwtToken.Issuer);
            await _messageRepository.ReadMessagesAsync(chat.idChat, idUser);
        }
    }
}
