using Casestudy.DAL.DomainClasses;
using System.Text.Json;
namespace Casestudy.DAL
{
    public class DataUtility
    {
        private readonly AppDbContext _db;
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }
       /* private async Task<bool> LoadBrands(dynamic jsonObjectArray)
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands?.RemoveRange(_db.Brands);
                await _db.SaveChangesAsync();
                List<String> allCategories = new();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("BRAND", out JsonElement menuItemJson))
                    {
                        allCategories.Add(ItemJson.GetString()!);
                    }
                }
                IEnumerable<String> categories = allCategories.Distinct<String>();
                foreach (string catname in categories)
                {
                    Brands cat = new();
                    cat.Name = catname;
                    await _db.Brands!.AddAsync(cat);
                    await _db.SaveChangesAsync();
                }
                loadedBrands = true;
            }*/
    }
}
