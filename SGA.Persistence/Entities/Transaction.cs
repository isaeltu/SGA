using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int AuthorizationId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal BalanceAfter { get; set; }

    public int? ReservationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Authorization Authorization { get; set; } = null!;

    public virtual Reservation? Reservation { get; set; }
}
