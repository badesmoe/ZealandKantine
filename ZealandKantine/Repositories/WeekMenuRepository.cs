namespace ZealandKantine.Repositories
{
    public class WeekMenuRepository
    {
        public void Create(Models.WeekMenu weekMenu)
        {
            Console.WriteLine($"Creating WeekMenu: Week {weekMenu.WeekNumber}, Year {weekMenu.Year}");
        }
    }
}
