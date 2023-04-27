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
            var request=new  HttpRequestMessage(HttpMethod.Delete,url+Id);
            
            var client= _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        public async Task<IEnumerable<T>?> GetAllAsync(string url)
        {
            var request=new  HttpRequestMessage(HttpMethod.Get,url);
            
            var client= _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.OK)
            ?JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync())
            :null;
        }

        public async Task<T?> GetAsync(string url, int Id)
        {
            var request=new  HttpRequestMessage(HttpMethod.Get,url+Id);
            
            var client= _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.OK)
            ?JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())
            :null;
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate)
        {
            var request=new  HttpRequestMessage(HttpMethod.Patch,url);
            if (objectToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToUpdate), Encoding.UTF8, "application/json");
            }
            else{
                return false;
            }
            var client= _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return (response.StatusCode == System.Net.HttpStatusCode.NoContent);
        }
    }


}