using System.Text;
using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        public Repository(IHttpClientFactory clientFactory)
        { 
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateAsync(string url, T objectToCreate)
        {
            var request=new  HttpRequestMessage(HttpMethod.Post,url);
            if (objectToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToCreate), Encoding.UTF8, "application/json");
            }
            else{
                return false;
            }
            var client= _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        public async Task<bool> DeleteAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>?> GetAllAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }


}