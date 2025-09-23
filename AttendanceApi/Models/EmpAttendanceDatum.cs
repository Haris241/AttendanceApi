using System;
using System.Collections.Generic;

namespace AttendanceApi.Models;

public partial class EmpAttendanceDatum
{
    public int? EmpId { get; set; }

    public string? EmployeeName { get; set; }

    public DateTime? UserDate { get; set; }

    public TimeOnly? Time { get; set; }

    public string? MachineName { get; set; }
}
