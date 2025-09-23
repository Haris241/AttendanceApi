using System;
using System.Collections.Generic;

namespace AttendanceApi.Models;

public partial class Tview
{
    public int? EmpId { get; set; }

    public string? EmployeeName { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? UserDate { get; set; }

    public TimeOnly? TimeIn { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public string? MachineName { get; set; }
}
