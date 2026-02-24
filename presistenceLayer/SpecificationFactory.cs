using Domain;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistenceLayer
{
     public class SpecificationFactory
    {
        public static IQueryable<TEntity> CreateQure<TEntity,Tkey> ( IQueryable<TEntity> BaseQuery,ISpecificationDesignPattern<TEntity,Tkey> specification) where TEntity : BaseEntity<Tkey>
        {


            if(specification.Criterial is not null)
            {
              BaseQuery=   BaseQuery.Where(specification.Criterial);
            }


            if(specification.Include is not null && specification.Include.Any())
            {
                BaseQuery = specification.Include.Aggregate(BaseQuery, (curent, exp) => curent.Include(exp));
            }





            return BaseQuery;
        }

    }
}
