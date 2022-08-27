using ParkyWeb.Models;

namespace ParkyWeb.Repository.IRepository;

public interface IAccountRepository:IRepository<Users>
{
    Task<Users> LoginAsync(string url, Users objToLogin);
    Task<bool> RegisterAsync(string url, Users objToRegister);
    
}