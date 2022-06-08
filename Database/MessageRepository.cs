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

            if (allMessages.Count < limit && page == 1)
            {
                return allMessages;
            }

            if ((allMessages.Count - limit * page == 0))
            {
                var messagesOnLim = allMessages.GetRange(allMessages.Count - limit * page, limit);

                return messagesOnLim;
            }

            if ((allMessages.Count - limit * page < limit))
            {
                if (page == 1)
                {
                    var messagesAll = allMessages.GetRange(allMessages.Count - limit * page, limit);

                    return messagesAll;
                }
                var messagesLim = allMessages.GetRange(0, allMessages.Count - limit * page);
                return messagesLim;
            }

            if (allMessages.Count - limit * page < 0)
            {
                List<ChatMessege> mes = new List<ChatMessege>();
                return mes;
            }

            var messages = allMessages.GetRange(allMessages.Count - limit * page, limit);

            return messages;
        }
    }
}
