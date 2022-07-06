using Microsoft.AspNetCore.Mvc;
using payment.Data;
using payment.Models;
using System.Globalization;

namespace payment.Controllers
{
    public class PayrollController : Controller
    {
        private ApplicationDbContext _context;
        const string userID = "ID";
        const string Name = "_Name";
        const string status = "status";
        public PayrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.status = HttpContext.Session.GetInt32(status);
            if (ViewBag.status == 1)
            {
                return RedirectToAction("ManagerPage");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(Login login)
        {
            var result = from m in _context.Manager
                         join l in _context.Login on m.LoginID equals l.LoginID
                         select new
                         {
                             ID = m.Id,
                             Name = m.Name,
                             Age = m.Age,
                             Start_Job = m.JSD,
                             U_Name = l.username,
                             Password = l.password
                         };


            foreach (var item in result)
            {
                if (login.password == item.Password && login.username.ToLower() == item.U_Name.ToLower())
                {
                    TempData["success"] = "Login Successfully";
                    HttpContext.Session.SetString(userID, login.username.ToLower());
                    HttpContext.Session.SetString(Name, item.Name);
                    HttpContext.Session.SetInt32(status, 1);
                    return RedirectToAction("ManagerPage");
                }
                else
                {
                    TempData["error"] = "Login Failed";
                }
            }
            return View();
        }

        public IActionResult ManagerPage()
        {
            ViewBag.status = HttpContext.Session.GetInt32(status);
            ViewBag.Name = HttpContext.Session.GetString(Name).ToUpperInvariant();
            if (ViewBag.status == 1)
            {
                var cat1 = from e in _context.Employees
                           join a in _context.Attendance on e.AttendanceID equals a.Id
                           //where e.Category == 1
                           select new
                           {
                               eID = e.Id,
                               aID = a.Id,
                               eName = e.Name,
                               aCount = a.MonthlyCount,
                               eAge = e.Age,
                               ePay = e.Pay,
                               eStart_Job = e.JSD,
                               eCNIC = e.CNIC,
                               eGender = e.Gender,
                               eCategory = e.Category,
                               ePayment = e.Pay - ((30 - a.MonthlyCount) * 700)
                           };
                var cat2 = from e in _context.Employees
                           where e.Category == 2
                           select new
                           {
                               eID = e.Id,
                               eName = e.Name,
                               eAge = e.Age,
                               eStart_Job = e.JSD,
                               ePay = e.Pay,
                               eCNIC = e.CNIC,
                               eGender = e.Gender,
                               eCategory = e.Category,
                           };
                var cat3 = from e in _context.Employees
                           where e.Category == 3
                           select new
                           {
                               eID = e.Id,
                               eName = e.Name,
                               eAge = e.Age,
                               ePay = e.Pay,
                               eStart_Job = e.JSD,
                               eCNIC = e.CNIC,
                               eGender = e.Gender,
                               eCategory = e.Category,
                           };

                foreach (var item in cat1)
                {
                    ViewBag.cat1 = cat1;
                }foreach (var item in cat2)
                {
                    ViewBag.cat2 = cat2;
                }foreach (var item in cat3)
                {
                    ViewBag.cat3 = cat3;
                }
                return View();
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ManagerPage(Employees? emp)
        {
            if ((emp.Category == 1 || emp.Category == 2 || emp.Category == 3) && HttpContext.Session.GetInt32(status) == 1)
            {
                if (emp.Category == 1)
                {
                    Attendance atten = new Attendance();
                    atten.MonthlyCount = emp.ManagerID;
                    _context.Attendance.Add(atten);
                    _context.SaveChanges();
                    var lrow = from a in _context.Attendance orderby a.Id descending select a;
                    Attendance lastrow = atten;
                    foreach (var item in lrow)
                    {
                        lastrow = item;
                        break;
                    }
                    emp.ManagerID = 2;
                    emp.AttendanceID = lastrow.Id;
                    _context.Employees.Add(emp);
                    _context.SaveChanges();
                }
                else if (emp.Category == 2)
                {
                    emp.ManagerID = 2;
                    emp.AttendanceID = 1;
                    _context.Employees.Add(emp);
                    _context.SaveChanges();
                }
                else if (emp.Category == 3)
                {
                    emp.ManagerID = 2;
                    emp.AttendanceID = 1;
                    _context.Employees.Add(emp);
                    _context.SaveChanges();
                }
                return RedirectToAction("ManagerPage");

            }
            else
            {
                HttpContext.Session.SetInt32(status, 0);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (HttpContext.Session.GetInt32(status) == 1)
            {
                if (id.IndexOf(',') == -1)
                {
                    int e = int.Parse(id, NumberStyles.AllowLeadingSign);
                    ViewBag.check = 0;
                    var emp = _context.Employees.Find(e);
                    ViewBag.editID = emp.Id;
                    return View(emp);
                }
                else
                {
                    int e = int.Parse(id.Split(",")[0], NumberStyles.AllowLeadingSign);
                    int a = int.Parse(id.Split(",")[1], NumberStyles.AllowLeadingSign);
                    var emp = _context.Employees.Find(e);
                    var attend = _context.Attendance.Find(a);
                    ViewBag.editID = emp.Id;
                    emp.ManagerID = attend.MonthlyCount;
                    ViewBag.check = 1;
                    return View(emp);
                }
            }
            else
            {
                return RedirectToAction("Index");

            }
        }
        
        [HttpPost]
        public IActionResult Edit(Employees emp)
        {
            if (HttpContext.Session.GetInt32(status) == 1)
            {
                if(emp.Category == 1)
                {
                    var count = emp.ManagerID;
                    Attendance attend = new Attendance();
                    attend.MonthlyCount = count;
                    attend.Id = emp.AttendanceID;
                    _context.Attendance.Update(attend);
                    _context.SaveChanges();

                    emp.ManagerID = 2;
                    _context.Employees.Update(emp);
                    _context.SaveChanges();
                    TempData["success"] = "Record Update Done";
                    return RedirectToAction("ManagerPage");
                }
                else
                {
                    emp.ManagerID = 2;
                    emp.AttendanceID = 1;
                    _context.Employees.Update(emp);
                    _context.SaveChanges();
                    TempData["success"] = "Record Update Done";
                    return RedirectToAction("ManagerPage");
                }

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            if (HttpContext.Session.GetInt32(status) == 1)
            {
                if (id.IndexOf(',') == -1)
                {
                    int e = int.Parse(id, NumberStyles.AllowLeadingSign);
                    ViewBag.check = 0;
                    var emp = _context.Employees.Find(e);
                    ViewBag.deleteID = emp.Id;
                    return View(emp);
                }
                else
                {
                    int e = int.Parse(id.Split(",")[0], NumberStyles.AllowLeadingSign);
                    int a = int.Parse(id.Split(",")[1], NumberStyles.AllowLeadingSign);
                    var emp = _context.Employees.Find(e);
                    var attend = _context.Attendance.Find(a);
                    ViewBag.deleteID = emp.Id;
                    emp.ManagerID = attend.MonthlyCount;
                    ViewBag.check = 1;
                    return View(emp);
                }
            }
            else
            {
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public IActionResult Delete(Employees emp)
        {
            if (HttpContext.Session.GetInt32(status) == 1)
            {
                if (emp.Category == 1)
                {
                    var count = emp.ManagerID;
                    Attendance attend = new Attendance();

                    emp.ManagerID = 2;
                    _context.Employees.Remove(emp);
                    _context.SaveChanges();


                    attend.MonthlyCount = count;
                    attend.Id = emp.AttendanceID;
                    _context.Attendance.Remove(attend);
                    _context.SaveChanges();

                    TempData["success"] = "Record Delete Done";
                    return RedirectToAction("ManagerPage");

                }
                else
                {
                    emp.ManagerID = 2;
                    emp.AttendanceID = 1;
                    _context.Employees.Remove(emp);
                    _context.SaveChanges();
                    TempData["success"] = "Record Delete Done";
                    return RedirectToAction("ManagerPage");
                }

            }
            return RedirectToAction("Index");
        }
    }
}
