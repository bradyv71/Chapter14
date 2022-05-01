using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClassSchedule.Models;

namespace ClassSchedule.Controllers
{
    public class HomeController : Controller
    {
        private IClassScheduleUnitOfWork data { get; set; }
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(IClassScheduleUnitOfWork classScheduleUnitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            data = classScheduleUnitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public ViewResult Index(int id)
        {
            // if day id passed to action method, store in session
            if (id > 0) {
                _httpContextAccessor.HttpContext.Session.SetInt32("dayid", id);
            }

            // options for Days query
            var dayOptions = new QueryOptions<Day> { 
                OrderBy = d => d.DayId
            };

            // options for Classes query
            var classOptions = new QueryOptions<Class> {
                Includes = "Teacher, Day"
            };

            // order by day if no day id. Otherwise, filter by day and order by time.
            if (id == 0) {
                classOptions.OrderBy = c => c.DayId;
            }
            else {
                classOptions.Where = c => c.DayId == id;
                classOptions.OrderBy = c => c.MilitaryTime;
            }

            // execute queries
            ViewBag.Days = data.Days.List(dayOptions);
            return View(data.Classes.List(classOptions));
        }
    }
}
