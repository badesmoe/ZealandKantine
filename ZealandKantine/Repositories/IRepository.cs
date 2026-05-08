namespace ZealandKantine.Repositories
{
    public interface IRepository<T> where T : class
    {
       void Create(T entity);
       List<T> ReadAll();
       T Read(int id);
       void Update(T entity);
       void Delete(int id);
    }
}
