using Domain.Contract.Repositories;
using Domain.Entity;
using persistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistenceLayer.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;

        public UnitOfWork( StoreDbContext context)
        {
            this.context = context;
        }

        private readonly Dictionary<Type,object> repoDic = new Dictionary<Type,object>();   
        
        public IRepository<TEntity, TKey> GetRepo<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
           var EntityType= typeof(TEntity);
            if(repoDic.TryGetValue(EntityType, out var repo))
            {
                return (IRepository<TEntity, TKey>)repo;
            }
            var newrepo = new GenaricRepository<TEntity, TKey>(context);

            repoDic[EntityType] = newrepo;
            return newrepo;
        }

        public async Task<int> SaveChangeRepoAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
