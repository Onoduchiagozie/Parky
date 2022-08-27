using Parky.Models;

namespace Parky.Repository.IRepopsitory;

public interface INationalParkRepository
{
    ICollection<NationPark> getNationParks();
    NationPark getNationPark(int nationparkId);
    bool NationParkExists(string name);
    bool NationParkExists(int name);
    bool createNationPark(NationPark nationPark);
    bool updateNationPark(NationPark nationPark);
    bool deleteNationPark(NationPark nationPark);
    bool Save();
}