namespace Get_A_Taxi.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        T Find(object id);
        void Add(T entity);
        void Update(T entity);
        void Detach(T entity);
        T Delete(T entity);
        T Delete(object id);
        int SaveChanges();
        IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions);
    }
}
