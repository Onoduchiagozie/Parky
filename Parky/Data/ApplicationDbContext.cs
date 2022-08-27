using Microsoft.EntityFrameworkCore;
using Parky.Models;

namespace Parky.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }
    public DbSet<NationPark> NationParks { get; set; }
    public DbSet<Trail> Trails { get; set; }
    public DbSet<Users> Users { get; set; }
}