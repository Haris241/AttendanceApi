using System;
using System.Collections.Generic;

namespace AttendanceApi.Models;

public partial class EmpDatum
{
    public int? EmpId { get; set; }

    public DateTime? DateTime { get; set; }

    public DateTime? AccessDate { get; set; }

    public DateTime? AccessTime { get; set; }

    public string? AuthenticationResult { get; set; }

    public string? AuthenticationType { get; set; }

    public string? DeviceName { get; set; }

    public string? DeviceSrNo { get; set; }

    public string? ResourceName { get; set; }

    public string? ReaderName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PersonName { get; set; }

    public string? PersonGroup { get; set; }

    public string? CardNumber { get; set; }
}
