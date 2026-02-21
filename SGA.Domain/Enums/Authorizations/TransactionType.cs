using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Enums.Authorizations
{
    public enum TransactionType
    {
        Charge = 1,
        Recharge = 2,
        Refund = 3,
        Adjustment = 4
    }
}
