using System.Text;
using Newtonsoft.Json;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository;

public class AccountRepository:Repository<Users>,IAccountRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public AccountRepository(IHttpClientFactory clientFactory):base(clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<Users> LoginAsync(string url, Users objToLogin)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (objToLogin != null)
        {
            request.Content = new StringContent(
                JsonConvert.SerializeObject(objToLogin),
                Encoding.UTF8,
                "application/json");
        }
        else
        {
            return new Users();
        }
        var client = _clientFactory.CreateClient();
        HttpResponseMessage responseMessage = await client.SendAsync(request);
        if (responseMessage.StatusCode ==System.Net.HttpStatusCode.OK)
        {
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Users>(jsonString);
        }
        else
        {
            return new Users();
        }
    }

    public async Task<bool> RegisterAsync(string url, Users objToRegister)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (objToRegister != null)
        {
            request.Content = new StringContent(
                JsonConvert.SerializeObject(objToRegister),
                Encoding.UTF8,
                "application/json");
        }
        else
        {
            return false;
        }
        var client = _clientFactory.CreateClient();
        HttpResponseMessage responseMessage = await client.SendAsync(request);
        if (responseMessage.StatusCode ==System.Net.HttpStatusCode.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}