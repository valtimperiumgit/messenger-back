using Messenger.ViewModels;

namespace Messenger.Database
{
    public class ClientRepository : IClientRepository
    {
        private readonly MessengerContext _context;

        public ClientRepository(MessengerContext context)
        {
            _context = context;
        }

        public async Task<Client> GetById(int id)
        {
            return _context.Clients.FirstOrDefault(c => c.Id == id);
        }

        public Client GetByPhone(string phone)
        {
            return _context.Clients.FirstOrDefault(c => c.Phone == phone);
        }

        public async Task<Chat?> GetChat(int id)
        {
            return _context.Chats.FirstOrDefault(chat => chat.IdChat == id);
        }

        public List<ClientViewModel> GetChatMembers(int id)
        {
            var chatMembers = _context.ChatMembers.Where(member => member.IdChat == id).ToList();
            var idClients = new List<int>();
            foreach(var member in chatMembers)
                idClients.Add(member.IdClient);

            var clients = _context.Clients.Where(member => idClients.Contains(member.Id)).ToList();
            List<ClientViewModel> model = new List<ClientViewModel>();

            foreach (var client in clients)
                model.Add(new ClientViewModel { client = client, avatars = _context.ClientAvatars.Where(c => c.Id == client.Id).ToList() });

            return model;
        }

        public List<ChatMessege> GetChatMesseges(int id)
        {
            return _context.ChatMesseges.Where(messege => messege.IdChat == id).ToList();
        }

        public List<Chat> GetChats(int id)
        {
            var user = _context.Clients.FirstOrDefault(user => user.Id == id);
            
            var chats = _context.ChatMembers.Where(chat => chat.IdClient == id).ToList();
            var chatsIds = new List<int>();
            foreach(var chat in chats)
            {
                chatsIds.Add(chat.IdChat);
            }

            return _context.Chats.Where(chat => chatsIds.Contains(chat.IdChat)).ToList();
        }

        public List<ChatsViewModel> GetChatsModel(List<Chat> chats, int idUser)
        {
            List<int> idsChats = new List<int>();
            foreach (var chat in chats)
            {
                idsChats.Add(chat.IdChat);
            }

            List<ChatsViewModel> model = new List<ChatsViewModel>();

            foreach (var chat in chats)
            {
                var members = GetChatMembers(chat.IdChat);
                var chatUser = members.FirstOrDefault(user => user.client.Id != idUser);
                var messages = GetChatMesseges(chat.IdChat);

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

        public void AddMessage(ChatMessege message)
        {
            _context.ChatMesseges.Add(message);
            _context.SaveChanges();
        }
    }
}
