using Messenger.Database;
using Messenger.Helpers;
using Messenger.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IClientRepository _repository;
        private readonly JWTService _jwtService;

        public ChatHub(IClientRepository repository, JWTService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task SelectChat(int idChat, string token)
        {
            var chat = await _repository.GetChatAsync(idChat);

            var jwtToken = _jwtService.Verify(token);
            int idUser = int.Parse(jwtToken.Issuer);

            var members = await _repository.GetChatMembersAsync(idChat);
            var messages = await _repository.GetChatMessegesAsync(idChat);

            var chatUser = members.FirstOrDefault(member => member.client.Id != idUser);

            ChatViewModel model = new ChatViewModel
            {
                chat = chat,
                members = members,
                chatMesseges = messages,
                chatUser = chatUser
            };

            await Clients.All.SendAsync("ReceiveMessage", model);
        }

        public async Task SendMessage(string message, int idChat, string token)
        {
            var chat = await _repository.GetChatAsync(idChat);

            var jwtToken = _jwtService.Verify(token);
            int idUser = int.Parse(jwtToken.Issuer);
            var user = await _repository.GetByIdAsync(idUser);

            ChatMessege newMessage = new ChatMessege
            {
                IdChat = idChat,
                Datetime = DateTime.Now,
                IdClient = idUser,
                Body = message,
                Viewed = false
            };

            await _repository.AddMessageAsync(newMessage);

            await Clients.All.SendAsync("ReceiveMessages", newMessage);
        }
    }
}
