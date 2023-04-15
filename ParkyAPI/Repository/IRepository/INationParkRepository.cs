using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNatinalPark(NationalPark nationalPark);
        bool UpdateNatinalPark(NationalPark nationalPark);
        bool DeleteNatinalPark(int id);
        bool Save();
    }
}
