using ParkyWeb.Models;

namespace ParkyWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);
        Task<User> RegisterAsync(string url, User objToCreate);

    }
}
