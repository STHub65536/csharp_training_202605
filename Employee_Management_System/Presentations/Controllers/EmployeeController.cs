using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Services;
using Employee_Management_System.Presentations.ViewModels;
using Employee_Management_System.Presentations.ViewModels.Adapters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Management_System.Presentations.Controllers;
[Route("Menu/Employee")]
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
        EmployeeViewModel targetVM = _employeeAdapter.Convert(_employeeService.FindEmployee(empNo - 1000)!);
        _employeeService.DeleteEmployee(empNo);

        TempData["DeleteSuccess"] = $"{targetVM.EmpName}さん(社員番号:{empNo})の削除に成功しました";
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
        bool isCorrect;
        isCorrect = domain.CheckCorrectAge();
        if (isDifferent && isCorrect)
        {
            TempData["EmployeeForm"]  = JsonSerializer.Serialize(vm);
            Department? selectedDept = _departmentService.FindDepartment(domain.DeptNo != null? (int)domain.DeptNo : 0);
            TempData["DeptName"] = selectedDept != null? _departmentAdapter.Convert(selectedDept).DeptName : "無所属";

            return RedirectToAction("EmployeeCheck");
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

    [HttpGet("Register/Confirm")]
    public IActionResult EmployeeCheck()
    {
        string? json = (string?)TempData["EmployeeForm"];
        if (string.IsNullOrWhiteSpace(json))
        {
            return RedirectToAction("EmployeeRegister");
        }
        EmployeeViewModel vm = JsonSerializer.Deserialize<EmployeeViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Register/Confirm")]
    public IActionResult EmployeeCheck(EmployeeViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            Employee domain = _employeeAdapter.Restore(vm);
            _employeeService.AddEmployee(domain);
            int? empNo = _employeeService.GetEmployeeList().FirstOrDefault(e => e.MailAddress == vm.MailAddress).DeptNo;

            TempData["RegisterSuccess"] = $"{vm.EmpName}(社員番号:{empNo! + 1000})の登録に成功しました";
            return RedirectToAction("EmployeeList");
        }
        else
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("EmployeeRegister");
        }
    }

    [HttpGet("Update")]
    public IActionResult EmployeeUpdate(int number)
    {
        string? json = (string?)TempData["InitializeForm"];
        
        ViewData["Number"] = number;
        EmployeeViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = _employeeAdapter.Convert(_employeeService.FindEmployee(number - 1000)!);
            SetListItem(vm, _departmentService, _departmentAdapter);
            vm.ChangedDeptNo = vm.DeptNo == null? 0 : vm.DeptNo;
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<EmployeeViewModel>(json!);
        SetListItem(vm, _departmentService, _departmentAdapter);
        vm.ChangedDeptNo = vm.DeptNo == null? 0 : vm.DeptNo;
        return View(vm);
    }

    [HttpPost("Update")]
    public IActionResult EmployeeUpdate(int number, EmployeeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);

            if((ModelState["EmpName"]?.Errors.Count ?? 0) > 0)
            {
                TempData["NameError"] = ModelState["EmpName"]?.Errors[0].ErrorMessage;
            }
            if((ModelState["MailAddress"]?.Errors.Count ?? 0) > 0)
            {
                TempData["MailError"] = ModelState["MailAddress"]?.Errors[0].ErrorMessage;
            }

            return RedirectToAction("EmployeeUpdate", new{ number });
        }

        Employee domain = _employeeAdapter.Restore(vm);
        
        //重複チェック
        bool isDifferent = true;
        List<EmployeeViewModel> vmList = _employeeService.GetEmployeeList().Select(d => _employeeAdapter.Convert(d)).ToList();
        foreach(EmployeeViewModel viewModel in vmList)
        {
            Console.WriteLine("empno:"+viewModel.EmpNo);
            if(viewModel.EmpNo != number) //更新前データ以外の時に重複している値があればエラー
            {
                if(viewModel.MailAddress == vm.MailAddress)
                {
                    isDifferent = false;
                }
            }
        }
        
        bool isCorrect = domain.CheckCorrectAge();
        if (isDifferent && isCorrect)
        {
            TempData["EmployeeUpdateForm"]  = JsonSerializer.Serialize(vm);
            Department? selectedDept = _departmentService.FindDepartment(domain.DeptNo != null? (int)domain.DeptNo : 0);
            TempData["DeptName"] = selectedDept != null? _departmentAdapter.Convert(selectedDept).DeptName : "無所属";

            return RedirectToAction("EmployeeUpdateCheck", new { number });
        }
        else
        {
            if (!isDifferent)
            {
                TempData["ExistingError"] = "入力されたメールアドレスは既に存在しています";    
            }
            if (!isCorrect)
            {
                TempData["AgeError"] = $"{Employee.MIN_AGE}～{Employee.MAX_AGE}歳までの方しか登録できません";
            }

            TempData["InitializeForm"]  = JsonSerializer.Serialize(vm);

            return RedirectToAction("EmployeeUpdate", new { number });
        }
    }

    [HttpGet("Update/Confirm")]
    public IActionResult EmployeeUpdateCheck(int number)
    {
        string? json = (string?)TempData["EmployeeUpdateForm"];

        TempData.Keep("EmployeeUpdateForm");
        TempData.Keep("DeptName");
        ViewData["OriginNumber"] = number;

        EmployeeViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = new EmployeeViewModel();
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<EmployeeViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Update/Confirm")]
    public IActionResult EmployeeUpdateCheck(int number, string newDeptName, EmployeeViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            vm.EmpNo = number - 1000;

            Employee? originDomain = _employeeService.FindEmployee((int)vm.EmpNo)!;
            EmployeeViewModel originVM = _employeeAdapter.Convert(originDomain);

            Department? selectedDept = _departmentService.FindDepartment(originDomain.DeptNo ?? 0);
            string originDeptName = selectedDept != null? _departmentAdapter.Convert(selectedDept).DeptName : "無所属";
            
            Employee domain = _employeeAdapter.Restore(vm);
            _employeeService.UpdateEmployee(domain);

            TempData["UpdateSuccess"] = $"社員の更新に成功しました<br>社員番号:{originVM.EmpNo} , 社員名:{originVM.EmpName} → {vm.EmpName} , 所属部署:{originDeptName} → {newDeptName}<br>生年月日:{vm.Birthday.ToString("yyyy-MM-dd")}<br>メールアドレス:{originDomain.MailAddress} → {vm.MailAddress}";
            return RedirectToAction("EmployeeList");
        }
        else
        {
            vm.DeptNo = vm.ChangedDeptNo;
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("EmployeeUpdate", new{ number });
        }
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