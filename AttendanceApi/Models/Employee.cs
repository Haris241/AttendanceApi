using System;
using System.Collections.Generic;

namespace AttendanceApi.Models;

public partial class Employee
{
    public int? EmpId { get; set; }

    public string? EmployeeName { get; set; }

    public DateOnly? Date { get; set; }

    public DateOnly? UserDate { get; set; }

    public TimeOnly? TimeIn { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public string? MachineName { get; set; }
}
