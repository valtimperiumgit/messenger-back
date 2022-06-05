using Messenger.ViewModels;

namespace Messenger.Database
{
    public interface IClientRepository
    {
        Task<Client> GetByPhoneAsync(string phone);
        Task<Client> GetByIdAsync(int id);
        Task<List<Chat>> GetChatsAsync(int id);

        Task<Chat> GetChatAsync(int id);

        Task<List<ClientViewModel>> GetChatMembersAsync(int id);

        Task<List<ChatMessege>> GetChatMessegesAsync(int id);

        Task<List<ChatsViewModel>> GetChatsModelAsync(List<Chat> chats, int idUser);

        Task AddMessageAsync(ChatMessege message);

        Task ReadMessagesAsync(int idChat, int idUser);

        Task<List<ChatMessege>> GetMessagesByLimitAsync(int idChat, int limit, int page);
    }
}
