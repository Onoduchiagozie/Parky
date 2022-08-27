using Parky.Models;

namespace Parky.Repository.IRepopsitory;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Users Authenticate(string username, string password);
    Users Register(string username, string password);
}