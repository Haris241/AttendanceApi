
using AttendanceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase

    {
        private readonly ApplicationContext db;
        public AttendanceController(ApplicationContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public IActionResult FetchJson([FromBody]List<Employee> punches)
        {
            try
            {

                foreach (var punch in punches)
                {
                    // Skip if TimeIn or TimeOut is null
                    if (!punch.TimeIn.HasValue || !punch.TimeOut.HasValue || !punch.UserDate.HasValue)
                        continue;

                    var timeIn = punch.TimeIn.Value;
                    var timeOut = punch.TimeOut.Value;
                    var userDate = punch.UserDate.Value;

                    if (timeIn.Hour >= 0 && timeIn.Hour <= 6)
                    {
                        punch.TimeIn = timeOut;
                        var nextDayPunch = punches.FirstOrDefault(p => p.UserDate == userDate.AddDays(1) && p.EmpId == punch.EmpId);

                        if (nextDayPunch != null && nextDayPunch.TimeIn.HasValue)
                        {
                            punch.TimeOut = nextDayPunch.TimeIn;
                        }
                    }
                    else if (timeIn.Hour >= 7 && timeIn.Hour <= 9 && timeOut.Hour >= 21 && timeOut.Hour <= 23)
                    {
                        punch.TimeIn = timeOut;
                        var nextDayPunch = punches.FirstOrDefault(p => p.UserDate == userDate.AddDays(1) && p.EmpId == punch.EmpId);

                        if (nextDayPunch != null && nextDayPunch.TimeIn.HasValue)
                        {
                            punch.TimeOut = nextDayPunch.TimeIn;
                        }

                    }
                    else
                    {
                        punch.TimeIn = timeIn;
                        punch.TimeOut = timeOut;
                    }
                }

                return Ok(new { success = true, records = punches });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("Utility")]
        public IActionResult Fetch([FromQuery] DateOnly start, [FromQuery] DateOnly end)
        {
            try
            {
                var connectionString = db.Database.GetConnectionString();

               
                var createTableSql = @"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
        CREATE TABLE [dbo].[Employees] (
            [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            [EmpId] INT NULL,
            [EmployeeName] NVARCHAR(255) NULL,
            [Date] DATE NULL,
            [UserDate] DATE NULL,
            [TimeIn] TIME NULL,
            [TimeOut] TIME NULL,
            [MachineName] NVARCHAR(255) NULL
        );";

               
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
              


                // Fetch TaigaTesting records in the date range
                var punches = db.TaigaTestings
                    .Where(e => e.UserDate >= start && e.UserDate <= end)
                    .OrderBy(e => e.UserDate)
                    .ToList();

                foreach (var punch in punches)
                {
                    // Skip if required fields are missing
                    if (!punch.TimeIn.HasValue || !punch.TimeOut.HasValue || !punch.UserDate.HasValue)
                        continue;

                    var timeIn = punch.TimeIn.Value;
                    var timeOut = punch.TimeOut.Value;
                    var userDate = punch.UserDate.Value;

                    // Initialize corrected time values
                    var correctedTimeIn = timeIn;
                    var correctedTimeOut = timeOut;

                    // Apply shift correction logic
                    if (timeIn.Hour >= 0 && timeIn.Hour <= 6)
                    {
                        correctedTimeIn = timeOut;

                        var nextDayPunch = punches.FirstOrDefault(p =>
                            p.UserDate == userDate.AddDays(1) && p.EmpId == punch.EmpId);

                        if (nextDayPunch != null && nextDayPunch.TimeIn.HasValue)
                        {
                            correctedTimeOut = nextDayPunch.TimeIn.Value;
                        }
                    }
                    else if (timeIn.Hour >= 7 && timeIn.Hour <= 9 && timeOut.Hour >= 21 && timeOut.Hour <= 23)
                    {
                        correctedTimeIn = timeOut;

                        var nextDayPunch = punches.FirstOrDefault(p =>
                            p.UserDate == userDate.AddDays(1) && p.EmpId == punch.EmpId);

                        if (nextDayPunch != null && nextDayPunch.TimeIn.HasValue)
                        {
                            correctedTimeOut = nextDayPunch.TimeIn.Value;
                        }
                    }

                    // Create a new Employee record with corrected times
                    var employee = new Employee
                    {
                        EmpId = punch.EmpId,
                        EmployeeName = punch.EmployeeName,
                        Date = punch.Date,
                        UserDate = punch.UserDate,
                        TimeIn = correctedTimeIn,
                        TimeOut = correctedTimeOut,
                        MachineName = punch.MachineName
                    };

                    db.Employees.Add(employee);
                }

                db.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = "Corrected data inserted into Employee table."
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Error occurred.",
                    error = ex.Message
                });
            }
        }

        //        select* from Employee where UserDate between '2025-08-01' and '2025-08-05'
        //order by UserDate



    }
}
