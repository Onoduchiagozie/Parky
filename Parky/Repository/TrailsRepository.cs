using Microsoft.EntityFrameworkCore;
using Parky.Data;
using Parky.Models;
using Parky.Repository.IRepopsitory;

namespace Parky.Repository;

public class TrailsRepository:ITrailsRepository
{
    private readonly ApplicationDbContext _context;

    public TrailsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public ICollection<Trail> getTrails()
    {
        return _context.Trails.Include(c=>c.NationPark).OrderBy(a=>a.Name).ToList();
    }

    public ICollection<Trail> GetTrailsInNationalPark(int npId)
    {
        return _context.Trails.Include(c=>c.NationPark).Where(c=>c.NationParkId==npId).ToList();
    }

    public Trail getTrail(int nationparkId)
    {
        return _context.Trails.Include(c=>c.NationPark).FirstOrDefault(c => c.Id ==nationparkId);
    }

    public bool TrailExists(string name)
    {
        bool value = _context.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        return value;
    }

    public bool TrailExists(int id)
    {
        return _context.Trails.Any(a => a.Id==id);
        
    }

    public bool createTrail(Trail trail)
    {
        _context.Trails.Add(trail);
        return Save();
    }
    public bool updateTrail(Trail trail)
    {
        _context.Trails.Update(trail);
        return Save();
    }

    public bool deleteTrail(Trail trail)
    {
        _context.Trails.Remove(trail);
        return Save();
    }
    public bool Save()
    {
        return _context.SaveChanges() >= 0 ? true : false;
    }
}