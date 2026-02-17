using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
     public class BaseEntity<TValue>
    {
        public TValue Id { get; set; } = default!;

    }
}
