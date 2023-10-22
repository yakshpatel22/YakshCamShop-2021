using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExercisesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        readonly AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Brands>>> Index()
        {
            BrandDAO dao = new(_db);
            List<Brands> allBrands = await dao.GetAll();
            return allBrands;
        }
    }
}
