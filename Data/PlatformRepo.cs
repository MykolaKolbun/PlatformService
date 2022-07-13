using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatfromRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatfromRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p=> p.Id == id);
        }

        public bool SaveCanges()
        {
            return (_context.SaveChanges()>= 0);
        }
    }
}