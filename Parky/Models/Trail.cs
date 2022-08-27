using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace Parky.Models;

public class Trail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Distance { get; set; }
    public enum DifficultyType
    {
        Easy,Moderate,Difficult,Expert
    } 
    public DifficultyType Difficulty { get; set; }
    public int NationParkId { get; set; }
    [ForeignKey("NationParkId")] 
    public NationPark NationPark { get; set;}
    public DateTime DateCreated { get; set; }
    public int Elevation { get; set; }

    } 