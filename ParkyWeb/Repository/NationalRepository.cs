using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
namespace ParkyWeb.Repository;

public class NationalRepository:Repository<NationPark>,INationalRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public NationalRepository(IHttpClientFactory clientFactory):base(clientFactory)
    {
        _clientFactory = clientFactory;
    }

}