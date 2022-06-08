using Messenger.ViewModels;

namespace Messenger.Database
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetChatsAsync(int id);

        Task<Chat> GetChatAsync(int id);

        Task<List<ClientViewModel>> GetChatMembersAsync(int id);

        Task<List<ChatsViewModel>> GetChatsModelAsync(List<Chat> chats, int idUser);

    }
}
