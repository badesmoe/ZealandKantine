using System.Data;
using ZealandKantine.Models;

namespace ZealandKantine.Repositories
{
    public class DailySpecialRepository
    {
        private readonly CafeZea _dbContext;

        public DailySpecialRepository(CafeZea dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(DailySpecial special)
        {
            _dbContext.DailySpecials.Add(special);

            _dbContext.SaveChanges();
        }
        
    }



}

