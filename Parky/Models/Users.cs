using System.ComponentModel.DataAnnotations.Schema;

namespace Parky.Models;

public class Users
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
    [NotMapped]
    public string? Token { get; set; }
}