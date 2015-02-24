
namespace Get_A_Taxi.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext context;
        private IDbSet<T> set;
        public Repository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }
        public IQueryable<T> All()
        {
            return this.set;
        }
        public T Find(object id)
        {
            return this.set.Find(id);
        }
        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }
        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }
        public void Detach(T entity)
        {
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Detached;
        }
        public T Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
            return entity;
        }
        public T Delete(object id)
        {
            T entity = this.Find(id);
            this.Delete(entity);
            return entity;
        }
        public int SaveChanges()
        {
            try
            {

            return this.context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        int i = 5;
                    }
                }
                throw e;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }
            entry.State = state;
        }
        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return this.All().AsQueryable().Where(conditions);
        }
    }
}
