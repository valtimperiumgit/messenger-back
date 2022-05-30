using Messenger.ViewModels;

namespace Messenger.Database
{
    public interface IClientRepository
    {
        Client GetByPhone(string phone);
        Task<Client> GetById(int id);
        List<Chat> GetChats(int id);

        Task<Chat> GetChat(int id);

        List<ClientViewModel> GetChatMembers(int id);

        List<ChatMessege> GetChatMesseges(int id);

        List<ChatsViewModel> GetChatsModel(List<Chat> chats, int idUser);

        void AddMessage(ChatMessege message);
    }
}
