using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<User> LoginAsync(string url, User objToCreate)
        {
            throw new NotImplementedException();
        }

        public Task<User> RegisterAsync(string url, User objToCreate)
        {
            throw new NotImplementedException();
        }
    }
}
