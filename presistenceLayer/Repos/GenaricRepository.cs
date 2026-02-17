using Domain.Contract.Repositories;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using persistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistenceLayer.Repos
    {
        public class GenaricRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
        {
            private readonly StoreDbContext context;

            public GenaricRepository(StoreDbContext context )
            {
                this.context = context;
            }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var x = await context.Set<TEntity>().ToListAsync();
            return x;
        }


        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var x = await context.Set<TEntity>().FindAsync(id);
            return x;
        }
        public async Task AddAsync(TEntity entity)
            {
                 await context.Set<TEntity>().AddAsync(entity);
           
            }
        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);

        }

        public  void Delete(TEntity entity)
            {
                 context.Set<TEntity>().Remove(entity);
            }

         
          

          
        }
    }
