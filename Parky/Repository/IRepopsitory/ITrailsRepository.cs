using Parky.Models;

namespace Parky.Repository.IRepopsitory;

public interface ITrailsRepository
{
    ICollection<Trail> getTrails();
    ICollection<Trail> GetTrailsInNationalPark(int npId);
    Trail getTrail(int TrailId);
    bool TrailExists(string name);
    bool TrailExists(int name);
    bool createTrail(Trail trail);
    bool updateTrail(Trail trail);
    bool deleteTrail(Trail trail);
    bool Save();
}