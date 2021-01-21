using EmployeeRegistration.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRegistration.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _db;

        public EmployeeController(EmployeeContext DB)
        {
            _db = DB;
        }
        public IActionResult EmployeeList()
        {
            try
            {
                //employee record from multiple table
                var EmpList = from EmployeeDataList in _db.tbl_Employee
                              join DepartmentDataList in _db.tbl_Department
                              on EmployeeDataList.dep_id equals DepartmentDataList.dept_id into Dep
                              from DepartmentDataList in Dep.DefaultIfEmpty()

                              select new Employee
                              {
                                  emp_id = EmployeeDataList.emp_id,
                                  emp_name = EmployeeDataList.emp_name,
                                  date_of_birth = EmployeeDataList.date_of_birth,
                                  date_of_join = EmployeeDataList.date_of_join,
                                  dep_id = EmployeeDataList.dep_id,
                                  Department = DepartmentDataList == null ? "" : DepartmentDataList.dept_name
                              };
                return View(EmpList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }

        }

        public IActionResult Create(Employee emp)
        {
            //for department dropDown
            DropDownForDepartment();
            return View(emp);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee emp)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    
                    if (emp.emp_id == 0)     //create new employee
                    {
                        var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
                        emp.created_by = remoteIpAddress.ToString();
                        emp.created_date = DateTime.Now.ToString();
                        _db.tbl_Employee.Add(emp);
                        await _db.SaveChangesAsync();
                    }
                         
                    else                     //update employee
                    {
                        //find employee by the employeeid and get created date and created by 
                        var EmpListv = _db.tbl_Employee
                                               .Where(p => p.emp_id == emp.emp_id)
                                               .Select(p => new { p.created_date, p.created_by });
                        foreach (var empValue in EmpListv)
                        {
                            emp.created_date = empValue.created_date;
                            emp.created_by = empValue.created_by;
                        }

                        var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
                        emp.modified_by = remoteIpAddress.ToString();
                        emp.modified_date = DateTime.Now.ToString();
                        _db.Entry(emp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                       
                        await _db.SaveChangesAsync();
                    }

                    return RedirectToAction("EmployeeList");
                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("EmployeeList");
            }

        }
        //department dropdown
        private void DropDownForDepartment()
        {
            try
            {
                List<Departments> DepList = new List<Departments>();
                DepList = _db.tbl_Department.ToList();
                DepList.Insert(0, new Departments { dept_id = 0, dept_name = "Please Select" });
                ViewBag.DepList = DepList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }

        }
        public async Task<IActionResult> Delete(int emp_id)
        {
            try
            {
                var deleteEmployee = await _db.tbl_Employee.FindAsync(emp_id);
                if (deleteEmployee != null)
                {
                    _db.tbl_Employee.Remove(deleteEmployee);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("EmployeeList");
            }
        }
    }
}

