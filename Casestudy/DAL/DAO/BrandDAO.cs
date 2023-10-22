using Casestudy.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
namespace Casestudy.DAL.DAO
{
    public class BrandDAO
    {
        private readonly AppDbContext _db;
        public BrandDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Brands>> GetAll()
        {
            return await _db.Brands!.ToListAsync();
        }
    }
}
