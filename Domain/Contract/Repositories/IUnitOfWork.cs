using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract.Repositories
{
     public interface IUnitOfWork
    {

        IRepository<TEntity, TKey> GetRepo<TEntity, TKey>()where TEntity:BaseEntity<TKey> ;


        Task<int> SaveChangeRepoAsync();
    }
}
