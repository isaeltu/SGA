using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int? DepartmentId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string? Position { get; set; }

    public DateTime HireDate { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Person Person { get; set; } = null!;
}
