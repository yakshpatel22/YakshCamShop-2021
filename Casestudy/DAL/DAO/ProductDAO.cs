using Casestudy.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
namespace Casestudy.DAL.DAO
{
    public class ProductDAO
    {
        private readonly AppDbContext _db;
        public ProductDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Products>> GetAllByBrand(int id)
        {
            return await _db.Products!.Where(item => item.Brands!.Id == id).ToListAsync();
        }
        public Products GetById(string id) //get the product by id
        {
            return _db.Products.Single(products => products.id == id);
        }
    }
}
