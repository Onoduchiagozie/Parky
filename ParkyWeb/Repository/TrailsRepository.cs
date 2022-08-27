using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
namespace ParkyWeb.Repository;

public class TrailsRepository:Repository<Trails>,ITrailsRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public TrailsRepository(IHttpClientFactory clientFactory):base(clientFactory)
    {
        _clientFactory = clientFactory;
    }

}