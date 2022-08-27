using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Parky.Data;
using Parky.Models;
using Parky.Repository.IRepopsitory;
using Microsoft.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Parky.Repository;

public class UserRepository:IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly AppSettings _appSettings;

    public UserRepository(ApplicationDbContext context,IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }
    
    
    public bool IsUniqueUser(string username)
    {
        var user = _context.Users.SingleOrDefault(predicate: x => x.Username == username );
        if (user == null)
            return true;
        return false;
    }

    public Users Authenticate(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(predicate: x => x.Username == username && x.Password == password);
        if (user == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(s: _appSettings.Secret);
        if (user.Role != null)
        {
            if (user.Role != null)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims: new Claim[]
                    {
                        new Claim(type: ClaimTypes.Name, user.Id.ToString()),
                        new Claim(type: ClaimTypes.Role, user.Role.FirstOrDefault().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(value: 7),
                    SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key: key), algorithm: SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token: token);
            }
        }

        user.Password = "";

        return user;
    }

    public Users Register(string username, string password)
    {
        Users obj = new Users()
        {
            Username = username,
            Password = password,
            Role = "Admin"
        };
        _context.Users.Add(entity: obj);
        _context.SaveChanges();
        obj.Password = "";
        return obj;
    }
}