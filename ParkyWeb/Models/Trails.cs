using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Parky.Models;
using Parky.Models.DTO;

namespace ParkyWeb.Models;

public class Trails
{
    public  int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Distance { get; set; }
    public int Elevation { get; set; }
    public enum DifficultyType
    {
        Easy,Moderate,Difficult,Expert
    } 
    public DifficultyType Difficulty { get; set; }
    
    public int NationParkId { get; set; }
    public NationParkDto? NationPark { get; set; }
}