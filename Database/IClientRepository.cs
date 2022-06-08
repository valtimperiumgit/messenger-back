using Messenger.ViewModels;

namespace Messenger.Database
{
    public interface IClientRepository
    {
        Task<Client> GetByPhoneAsync(string phone);
        Task<Client> GetByIdAsync(int id);
    }
}
