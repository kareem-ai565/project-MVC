namespace project.Repository
{
    public interface IRepository<T>
    {
        public List<T> GetAll(string? include);
        public T GetById(int id);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public void save();

    }
}
