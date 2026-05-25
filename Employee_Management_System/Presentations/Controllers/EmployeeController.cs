using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Services;
using Employee_Management_System.Presentations.ViewModels;
using Employee_Management_System.Presentations.ViewModels.Adapters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Management_System.Presentations.Controllers;
public class EmployeeController : Controller
{
    private EmployeeViewModelAdapter _employeeAdapter;
    private DepartmentViewModelAdapter _departmentAdapter;
    private IEmployeeService _employeeService;
    private IDepartmentService _departmentService;

    public EmployeeController(EmployeeViewModelAdapter employeeAdapter, DepartmentViewModelAdapter departmentAdapter, IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeAdapter = employeeAdapter;
        _departmentAdapter = departmentAdapter;
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    [HttpGet("List")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult EmployeeList()
    {
        List<Employee> domainList = _employeeService.GetEmployeeList();
        List<EmployeeViewModel> vmList = domainList.Select(d => _employeeAdapter.Convert(d)).ToList();
        return View(vmList);
    }

    [HttpPost("List")]
    public IActionResult EmployeeList(int empNo)
    {
        _employeeService.DeleteEmployee(empNo);
        return RedirectToAction();
    }

    [HttpGet("Register")]
    public IActionResult EmployeeRegister()
    {
        string? json = (string?)TempData["InitializeForm"];
        EmployeeViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = new EmployeeViewModel();
            SetListItem(vm, _departmentService, _departmentAdapter);
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<EmployeeViewModel>(json!);
        SetListItem(vm, _departmentService, _departmentAdapter);

        return View(vm);
    }

    [HttpPost("Register")]
    public IActionResult EmployeeRegister(EmployeeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            SetListItem(vm, _departmentService, _departmentAdapter);
            return View(vm);
        }

        Employee domain = _employeeAdapter.Restore(vm);
        bool isDifferent = _employeeService.IsEmployeeDifferent(domain);
        bool isCorrect = domain.CheckCorrectAge();
        if (isDifferent && isCorrect)
        {
            TempData["EmployeeForm"]  = JsonSerializer.Serialize(vm);
            Department? selectedDept = _departmentService.FindDepartment(domain.DeptNo != null? (int)domain.DeptNo : 0);
            TempData["DeptName"] = selectedDept != null? _departmentAdapter.Convert(selectedDept).DeptName : "無所属";

            return RedirectToAction("Check");
        }
        else
        {
            if (!isDifferent)
            {
                ViewData["ExistingError"] = "入力されたメールアドレスは既に存在しています";    
            }
            if (!isCorrect)
            {
                ViewData["AgeError"] = $"{Employee.MIN_AGE}～{Employee.MAX_AGE}歳までの方しか登録できません";
            }

            SetListItem(vm, _departmentService, _departmentAdapter);

            return View(vm);
        }
    }

    [HttpGet("Confirm")]
    public IActionResult Check()
    {
        string? json = (string?)TempData["EmployeeForm"];
        if (string.IsNullOrWhiteSpace(json))
        {
            return RedirectToAction("EmployeeRegister");
        }
        EmployeeViewModel vm = JsonSerializer.Deserialize<EmployeeViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Confirm")]
    public IActionResult Check(EmployeeViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            Employee domain = _employeeAdapter.Restore(vm);
            _employeeService.AddEmployee(domain);

            return RedirectToAction("EmployeeList");
        }
        else
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("EmployeeRegister");
        }
    }

    [HttpGet("Update")]
    public IActionResult EmployeeUpdate([FromQuery] int number)
    {
        Employee domain = _employeeService.FindEmployee(number);
        EmployeeViewModel vm = _employeeAdapter.Convert(domain);
        
        vm.DeptList.Add(new SelectListItem{Text="無所属", Value= "0"});
        List<DepartmentViewModel> departmentVM = _departmentService.GetDepartmentList().Select(d => _departmentAdapter.Convert(d)).ToList();
        departmentVM.ForEach(d => {
            string? deptNo = d.DeptNo?.ToString();
            vm.DeptList.Add(new SelectListItem{Text=d.DeptName, Value= deptNo ?? ""});
        });
        vm.ChangedDeptNo = vm.DeptNo;

        return View(vm);
    }

    [HttpPost("Update")]
    public IActionResult EmployeeUpdate(EmployeeViewModel vm)
    {
        Employee domain = _employeeAdapter.Restore(vm);
        _employeeService.UpdateEmployee(domain);

        return RedirectToAction("EmployeeList", "Employee");
    }

    public static void SetListItem(EmployeeViewModel vm, IDepartmentService _departmentService, DepartmentViewModelAdapter _departmentAdapter)
    {
        vm.DeptList.Add(new SelectListItem{Text="無所属", Value= "0"});
        List<DepartmentViewModel> departmentVM = _departmentService.GetDepartmentList().Select(d => _departmentAdapter.Convert(d)).ToList();
        departmentVM.ForEach(d => {
            string? deptNo = d.DeptNo?.ToString();
            vm.DeptList.Add(new SelectListItem{Text=d.DeptName, Value= deptNo ?? ""});
        });
    }
}