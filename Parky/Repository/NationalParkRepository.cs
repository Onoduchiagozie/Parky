using System.Reflection.Metadata.Ecma335;
using Parky.Data;
using Parky.Models;
using Parky.Repository.IRepopsitory;

namespace Parky.Repository;

public class NationalParkRepository:INationalParkRepository
{
    private readonly ApplicationDbContext _context;

    public NationalParkRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public ICollection<NationPark> getNationParks()
    {
        return _context.NationParks.OrderBy(a=>a.Name).ToList();
    }

    public NationPark getNationPark(int nationparkId)
    {
        return _context.NationParks.FirstOrDefault(c => c.Id ==nationparkId);
    }
    public bool NationParkExists(string name)
    {
        bool value = _context.NationParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        return value;
    }
    public bool NationParkExists(int id)
    {
        return _context.NationParks.Any(a => a.Id==id);
        
    }
    public bool createNationPark(NationPark nationPark)
    {
        _context.NationParks.Add(nationPark);
        return Save();
    }

    public bool updateNationPark(NationPark nationPark)
    {
        _context.NationParks.Update(nationPark);
        return Save();
    }

    public bool deleteNationPark(NationPark nationPark)
    {
        _context.NationParks.Remove(nationPark);
        return Save();
    }
    public bool Save()
    {
        return _context.SaveChanges() >= 0 ? true : false;
    }
}