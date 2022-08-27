using System.ComponentModel.DataAnnotations;
using static Parky.Models.Trail;

namespace Parky.Models.DTO;
public class UpdateDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Distance { get; set; }
    public DifficultyType Difficulty { get; set; }
    [Required]
    public int? NationParkId { get; set; }
    public int Elevation { get; set; }

}