using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrailRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public bool CreateTrail(Trail trail)
        {
            _dbContext.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _dbContext.Trails.Remove(trail);
            return Save();
        }

        public Trail? GetTrail(int trailId)
        {
            //return _dbContext.Trails.FirstOrDefault(a => a.Id == trailId);
            return _dbContext.Trails.Include(u => u.NationalPark).FirstOrDefault(a => a.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            //return _dbContext.Trails.OrderBy(a => a.Name).ToList();
            return _dbContext.Trails.Include(u => u.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _dbContext.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _dbContext.Trails.Any(a => a.Id == id);
        }

        // Take care of this approach, it is good.
        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _dbContext.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            //without "Include",NationalPark object will be null in the return object
            return _dbContext.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
            //return _dbContext.Trails.Where(c => c.NationalParkId == npId).ToList();
        }
    }
}