using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark? GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNatinalPark(NationalPark nationalPark);
        bool UpdateNatinalPark(NationalPark nationalPark);
        bool DeleteNatinalPark(NationalPark nationalPark);
        bool Save();
    }
}
