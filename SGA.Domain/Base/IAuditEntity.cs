using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Base
{
    public interface IAuditEntity
    {
       public DateTime CreatedAt { get; set; }
      public string CreatedBy { get; set; }
      public DateTime? ModifiedAt { get; set; }
      public string ModifiedBy { get; set; }
       public bool IsDeleted { get; set; }
      public DateTime? DeletedAt { get; set; }
       public string DeletedBy { get; set; }
       
    }
}
