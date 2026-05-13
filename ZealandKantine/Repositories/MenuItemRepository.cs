using Microsoft.EntityFrameworkCore;
using System.Data;
using ZealandKantine.Interfaces;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class MenuItemRepository : IRepository<MenuItem>
    {
        private readonly CafeZea _dbContext;

        public MenuItemRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(MenuItem entity)
        {
            _dbContext.MenuItems.Add(entity);
            _dbContext.SaveChanges();
        }
        public List<MenuItem> ReadAll()
        {
            return _dbContext.MenuItems.ToList();
        }
        public MenuItem? Read(int id)
        {
            return _dbContext.MenuItems.Find(id);
        }
        public void Update(MenuItem entity)
        {
            var existing = _dbContext.MenuItems.Find(entity.Id);
            if (existing == null) return;
            existing.Name = entity.Name;
            existing.Price = entity.Price;
            existing.Category = entity.Category;
            existing.IsActive = entity.IsActive;
            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var entity = _dbContext.MenuItems.Find(id);
            if (entity == null) return;
            _dbContext.MenuItems.Remove(entity);
            _dbContext.SaveChanges();
        }
        public List<MenuItem> GetAllActive()
        {
            var order = new List<MenuCategory>
            {
                MenuCategory.Morgenmad,
                MenuCategory.Frokost,
                MenuCategory.Drikkevarer
            };

            return _dbContext.MenuItems
                .Where(m => m.IsActive)
                .AsEnumerable()
                .OrderBy(m => order.IndexOf(m.Category))
                .ToList();
        }
        public DailySpecial? GetTodaysDailySpecial()
        {
            //var today = DateTime.Today;
            //var tomorrow = today.AddDays(1);

            //return _dbContext.DailySpecials
            //    .FirstOrDefault(ds =>
            //        ds.Date >= today &&
            //        ds.Date < tomorrow &&
            //        ds.IsActive);

            var today = DateOnly.FromDateTime(DateTime.Today);
            return _dbContext.MenuDays
                .Where(md => md.Date == today)
                .Include(md => md.DailySpecials)
                .SelectMany(md => md.DailySpecials)
                .Where(ds => ds.IsActive)
                .FirstOrDefault();
        }

        public IEnumerable<DailySpecial> GetAllDailySpecials()
        {
            return _dbContext.DailySpecials.ToList();
        }

    }
}
