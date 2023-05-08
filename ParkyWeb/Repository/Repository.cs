using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System.Net.Http.Headers;
using System.Text;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateAsync(string url, T objectToCreate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objectToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        public async Task<bool> DeleteAsync(string url, int Id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.NoContent);
        }

        public async Task<IEnumerable<T>?> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.OK)
            ? JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync())
            : null;
        }

        public async Task<T?> GetAsync(string url, int Id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.OK)
            ? JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())
            : null;
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objectToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.NoContent);
        }
    }
}