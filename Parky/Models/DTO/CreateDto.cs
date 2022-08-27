using System.ComponentModel.DataAnnotations;
using static Parky.Models.Trail;

namespace Parky.Models.DTO;
public class CreateDto
{
    [Required]
    public string Name { get; set; }
    public int Distance { get; set; }
    public DifficultyType Difficulty { get; set; }
    
    public int NationParkId { get; set; }
    public int Elevation { get; set; }

}