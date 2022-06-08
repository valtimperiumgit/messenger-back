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

    }
}
