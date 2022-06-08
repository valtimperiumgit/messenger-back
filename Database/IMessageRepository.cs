namespace Messenger.Database
{
    public interface IMessageRepository
    {
        Task<List<ChatMessege>> GetChatMessegesAsync(int id);

        Task AddMessageAsync(ChatMessege message);

        Task ReadMessagesAsync(int idChat, int idUser);

        Task<List<ChatMessege>> GetMessagesByLimitAsync(int idChat, int limit, int page);
    }
}
