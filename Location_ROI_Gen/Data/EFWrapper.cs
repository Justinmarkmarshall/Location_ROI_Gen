using Location_ROI_Gen.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Location_ROI_Gen.Data
{
    internal class EFWrapper : IEFWrapper
    {
        private readonly DataContext _context;

        public EFWrapper(DataContext context)
        {
            _context = context;
        }

        public async Task UpsertLocations(List<Location> locations)
        {
            try
            {
                await _context.AddRangeAsync(locations);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Succesfully wrote {locations.Count()} to the DB");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}, {ex.InnerException}");
            }
        }

        public async Task<List<Location>> GetFromDB()
        {
            return await _context.Location.ToListAsync();
        }
    }

    public interface IEFWrapper
    {
        Task UpsertLocations(List<Location> locations);
        Task<List<Location>> GetFromDB();
    }

}
