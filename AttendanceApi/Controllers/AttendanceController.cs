
using AttendanceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
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
                    else if (timeIn.Hour >= 7 && timeIn.Hour <= 9 && timeOut.Hour >= 20 && timeOut.Hour <= 23)
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
        [HttpGet]
        public IActionResult Fetch([FromQuery] DateOnly start, [FromQuery] DateOnly end)
        {
            try
            {
                var punches = db.Employees
                    .Where(e => e.UserDate >= start
                             && e.UserDate <= end.AddDays(1) 
                          )
                    .OrderBy(e => e.UserDate)
                    .ToList();

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
                    else if (timeIn.Hour >= 7 && timeIn.Hour <= 9 && timeOut.Hour >= 20 && timeOut.Hour <= 23)
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
//        select* from Employee where UserDate between '2025-08-01' and '2025-08-05'
//order by UserDate

      

    }
}
