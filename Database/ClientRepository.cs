using Messenger.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database
{
    public class ClientRepository : IClientRepository
    {
        private readonly MessengerContext _context;

        public ClientRepository(MessengerContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetByPhoneAsync(string phone)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task<Chat?> GetChatAsync(int id)
        {
            return await _context.Chats.FirstOrDefaultAsync(chat => chat.IdChat == id);
        }

        public async Task<List<ClientViewModel>> GetChatMembersAsync(int id)
        {
            var chatMembers = await _context.ChatMembers.Where(member => member.IdChat == id).ToListAsync();
            var idClients = new List<int>();
            foreach(var member in chatMembers)
                idClients.Add(member.IdClient);

            var clients = await _context.Clients.Where(member => idClients.Contains(member.Id)).ToListAsync();
            List<ClientViewModel> model = new List<ClientViewModel>();

            foreach (var client in clients)
                model.Add(new ClientViewModel { client = client, avatars = await _context.ClientAvatars.Where(c => c.Id == client.Id).ToListAsync() });

            return model;
        }

        public async Task<List<ChatMessege>> GetChatMessegesAsync(int id)
        {
            return await _context.ChatMesseges.Where(messege => messege.IdChat == id).ToListAsync();
        }

        public async Task<List<Chat>> GetChatsAsync(int id)
        {
            var user = await _context.Clients.FirstOrDefaultAsync(user => user.Id == id);
            
            var chats = await _context.ChatMembers.Where(chat => chat.IdClient == id).ToListAsync();
            var chatsIds = new List<int>();
            foreach(var chat in chats)
            {
                chatsIds.Add(chat.IdChat);
            }

            return await _context.Chats.Where(chat => chatsIds.Contains(chat.IdChat)).ToListAsync();
        }

        public async Task<List<ChatsViewModel>> GetChatsModelAsync(List<Chat> chats, int idUser)
        {
            List<int> idsChats = new List<int>();
            foreach (var chat in chats)
            {
                idsChats.Add(chat.IdChat);
            }

            List<ChatsViewModel> model = new List<ChatsViewModel>();

            foreach (var chat in chats)
            {
                var members = await GetChatMembersAsync(chat.IdChat);
                var chatUser = members.FirstOrDefault(user => user.client.Id != idUser);
                var messages = await GetChatMessegesAsync(chat.IdChat);

                List<int> idsMessages = new List<int>();
                foreach(var message in messages)
                {
                    idsMessages.Add(message.Id);
                }

                if(idsMessages.Count != 0)
                {
                    var maxId = idsMessages.Max();
                    var lastMessage = messages.FirstOrDefault(message => message.Id == maxId);
                    model.Add(new ChatsViewModel { chat = chat, chatUser = chatUser, lastMessage = lastMessage, chatMessages = messages});
                }

                else
                {
                    model.Add(new ChatsViewModel { chat = chat, chatUser = chatUser});
                }
                
            }

            return model;

        }

        public async Task AddMessageAsync(ChatMessege message)
        {
            _context.ChatMesseges.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task ReadMessagesAsync(int idChat, int idUser)
        {
            var messages = await _context.ChatMesseges.Where(message => message.IdClient != idUser && message.IdChat == idChat).ToListAsync();

            foreach(var message in messages)
            {
                message.Viewed = true;
            };

            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessege>> GetMessagesByLimitAsync(int idChat, int limit, int page)
        {
            var allMessages = await _context.ChatMesseges.Where(message =>
            message.IdChat == idChat).ToListAsync();

            if(allMessages.Count < limit && page == 1)
            {
                return allMessages;
            }

            if(allMessages.Count - limit * page < 0)
            {
                List<ChatMessege> mes = new List<ChatMessege>();
                return mes;
            }
            
            if(allMessages.Count - limit * page < limit)
            {
                var messagesLim = allMessages.GetRange(0, allMessages.Count - limit * page);
                return messagesLim;
            }
            
            var messages = allMessages.GetRange(allMessages.Count - limit * page, limit);

            return messages;
        }
    }
}
