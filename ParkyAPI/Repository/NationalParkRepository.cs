using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NationalParkRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public bool CreateNatinalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNatinalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark? GetNationalPark(int nationalParkId)
        {
            return _dbContext.NationalParks.FirstOrDefault(a => a.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _dbContext.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return _dbContext.NationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _dbContext.NationalParks.Any(a => a.Id == id);
        }

        // Take care of this approach, it is good.
        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNatinalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}