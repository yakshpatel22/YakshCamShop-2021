using Casestudy.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Casestudy.DAL.DAO
{
    public class BranchDAO
    {
        private AppDbContext _db;
        public BranchDAO(AppDbContext context)
        {
            _db = context;
        }
        public List<Branch> GetThreeClosetStores(float? lat, float? lng) //finds the nearest 3 stores
        {
            List<Branch> branchDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Branches.FromSqlRaw("dbo.pGetThreeClosestStores @lat, @lng", latParam, lngParam);
                branchDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return branchDetails;
        }
    }
}
