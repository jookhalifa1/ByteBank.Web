using Domain;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specificatinos
{
    public abstract class BaseSpecifications<TEntity, Tkey> : ISpecificationDesignPattern<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<TEntity, object>>> Include { get; } = [];

        public Expression<Func<TEntity, bool>> Criterial   {get;}


        protected BaseSpecifications(Expression<Func<TEntity, bool>>? expression )
        {
            Criterial=expression;
        }


        public void AddIclude(Expression<Func<TEntity, object>> expression)
        {
            Include.Add(expression);
        }


    }
}
