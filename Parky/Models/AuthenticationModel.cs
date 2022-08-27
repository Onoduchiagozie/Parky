using System.ComponentModel.DataAnnotations;

namespace Parky.Models;

public class AuthenticationModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}