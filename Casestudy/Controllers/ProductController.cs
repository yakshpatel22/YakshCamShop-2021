using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        [Route("{brandid}")]
        public async Task<ActionResult<List<Products>>> Index(int brandid)
        {
            ProductDAO dao = new(_db);
            List<Products> itemsForBrands = await dao.GetAllByBrand(brandid);
            return itemsForBrands;
        }
    }
}
