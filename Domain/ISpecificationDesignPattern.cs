using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
     public interface ISpecificationDesignPattern<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        ICollection<Expression<Func<TEntity, object>>> Include { get; }


        Expression<Func<TEntity,bool>> Criterial { get; }
    }
}
