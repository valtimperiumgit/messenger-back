using Messenger.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database
{
    public class ChatRepository : IChatRepository
    {
        private readonly MessengerContext _context;
  
        private readonly IMessageRepository _messageRepository;

        public ChatRepository(MessengerContext context, IMessageRepository messageRepository)
        {
            _context = context;
            _messageRepository = messageRepository;
        }

        public async Task<Chat?> GetChatAsync(int id)
        {
            return await _context.Chats.AsNoTracking().
                FirstOrDefaultAsync(chat => chat.IdChat == id);
        }

        public async Task<List<ClientViewModel>> GetChatMembersAsync(int id)
        {

            var idsClients = await _context.ChatMembers.AsNoTracking()
                .Where(member => member.IdChat == id)
                .Select(member => member.IdClient)
                .ToListAsync();

            var clients = await _context.Clients.AsNoTracking().
                Where(member => idsClients.Contains(member.Id)).
                ToListAsync();

            List<ClientViewModel> model = new List<ClientViewModel>();

            foreach (var client in clients)
                model.Add(new ClientViewModel { client = client, 
                    avatars = await _context.ClientAvatars.AsNoTracking()
                    .Where(c => c.Id == client.Id)
                    .ToListAsync() });

            return model;
        }

        public async Task<List<Chat>> GetChatsAsync(int id)
        {
            var user = await _context.Clients.AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id);

            var chatsIds = await _context.ChatMembers.AsNoTracking()
                .Where(chat => chat.IdClient == id)
                .Select(chat => chat.IdChat)
                .ToListAsync();

            return await _context.Chats.AsNoTracking()
                .Where(chat => chatsIds.Contains(chat.IdChat))
                .ToListAsync();
        }

        public async Task<List<ChatsViewModel>> GetChatsModelAsync(List<Chat> chats, int idUser)
        {

            var chatsIds = chats.Select(chat => chat.IdChat).ToList();

            List<ChatsViewModel> model = new List<ChatsViewModel>();

            foreach (var chat in chats)
            {
                var members = await GetChatMembersAsync(chat.IdChat);
                var chatUser = members.FirstOrDefault(user => user.client.Id != idUser);
                var messages = await _messageRepository.GetMessagesByLimitAsync(chat.IdChat, 15, 1);
                var allChatMessagesCount = await _messageRepository.CountAllChatMessages(chat.IdChat);

                List<int> idsMessages = messages.Select(message => message.Id).ToList();

                if (idsMessages.Count != 0)
                {
                    var maxId = idsMessages.Max();
                    var lastMessage = messages
                        .FirstOrDefault(message => message.Id == maxId);

                    model.Add(new ChatsViewModel
                    {
                        chat = chat,
                        chatUser = chatUser,
                        lastMessage = lastMessage,
                        chatMessages = messages,
                        allMessagesCount = allChatMessagesCount
                    });
                }

                else
                {
                    model.Add(new ChatsViewModel { chat = chat, 
                        chatUser = chatUser });
                }

            }

            return model;

        }
    }
}
