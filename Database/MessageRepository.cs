using Microsoft.EntityFrameworkCore;

namespace Messenger.Database
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessengerContext _context;
       
        public MessageRepository(MessengerContext context)
        {
            _context = context;
            
        }

        public async Task<List<ChatMessege>> GetChatMessegesAsync(int id)
        {
            return await _context.ChatMesseges.AsNoTracking()
                .Where(messege => messege.IdChat == id)
                .ToListAsync();
        }

        public async Task AddMessageAsync(ChatMessege message)
        {
            _context.ChatMesseges.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task ReadMessagesAsync(int idChat, int idUser)
        {
            var messages = await _context.ChatMesseges
                .Where(message => message.IdClient != idUser && message.IdChat == idChat)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.Viewed = true;
            };

            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessege>> GetMessagesByLimitAsync(int idChat, int limit, int page)
        {
            var allMessages = await _context.ChatMesseges.AsNoTracking()
                .Where(message => message.IdChat == idChat)
                .ToListAsync();

            List<ChatMessege> messagesForReturn = new List<ChatMessege>();

            if (allMessages.Count < limit)
                messagesForReturn = allMessages;

            else if (allMessages.Count > limit * page)
                messagesForReturn = allMessages.GetRange(allMessages.Count - (limit * page), limit);

            else if (allMessages.Count < limit * page)
                messagesForReturn = allMessages.GetRange(0, allMessages.Count - limit * (page - 1));

            return messagesForReturn;
        }

        public async Task<int> CountAllChatMessages(int idChat)
        {
            return await _context.ChatMesseges.AsNoTracking()
                .Where(message => message.IdChat == idChat)
                .CountAsync();
        }
    }
}
