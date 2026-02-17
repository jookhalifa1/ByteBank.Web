using Domain.Entity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract.Repositories
{
     public interface IRepository<TEntity,Tkey> where TEntity:BaseEntity<Tkey>
    {

        public Task<IEnumerable<TEntity>> GetAllAsync();

        public Task<TEntity> GetByIdAsync(Tkey id);

        public Task AddAsync(TEntity entity);

        public void Update(TEntity entity );

        public void Delete(TEntity entity);

       
    }
}
