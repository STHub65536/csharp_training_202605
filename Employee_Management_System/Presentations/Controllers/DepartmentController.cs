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

namespace Employee_Management_System.Presentations.Controllers;

[Route("Menu/Department")]
public class DepartmentController : Controller
{
    private DepartmentViewModelAdapter _adapter;
    private IDepartmentService _service;

    public DepartmentController(DepartmentViewModelAdapter adapter, IDepartmentService service)
    {
        _adapter = adapter;
        _service = service;
    }

    [HttpGet("List")]
    public IActionResult DepartmentList()
    {
        List<DepartmentViewModel> vmList = _service.GetDepartmentList().Select(d => _adapter.Convert(d)).ToList();
        return View(vmList);
    }

    [HttpPost("List")]
    public IActionResult DepartmentList(int deptNo)
    {
        if (!_service.HasEmployees(deptNo))
        {
            _service.DeleteDepartment(deptNo);
            return RedirectToAction();
        }
        else
        {
            ViewData["DeleteError"] = $"{_service.FindDepartment(deptNo)!.DeptName}には社員が所属しているため削除できません";
            
            List<DepartmentViewModel> vmList = _service.GetDepartmentList().Select(d => _adapter.Convert(d)).ToList();
            return View(vmList);
        }
    }

    [HttpGet("Register")]
    public IActionResult DepartmentRegister()
    {
        string? json = (string?)TempData["InitializeForm"];
        DepartmentViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = new DepartmentViewModel();
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<DepartmentViewModel>(json!);

        return View(vm);
    }

    [HttpPost("Register")]
    public IActionResult DepartmentRegister(DepartmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        bool isDifferent = _service.IsDepartmentDifferent(_adapter.Restore(vm));
        if (isDifferent)
        {
            TempData["DepartmentForm"]  = JsonSerializer.Serialize(vm);

            return RedirectToAction("Check");
        }
        else
        {
            ViewData["ExistingError"] = "入力された部署番号または部署名は既に存在しています";

            return View(vm);
        }
    }

    [HttpGet("Confirm")]
    public IActionResult Check()
    {
        string? json = (string?)TempData["DepartmentForm"];
        if (string.IsNullOrWhiteSpace(json))
        {
            return RedirectToAction("DepartmentRegister");
        }
        DepartmentViewModel vm = JsonSerializer.Deserialize<DepartmentViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Confirm")]
    public IActionResult Check(DepartmentViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            Department domain = _adapter.Restore(vm);
            _service.AddDepartment(domain);

            return RedirectToAction("DepartmentList");
        }
        else
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("DepartmentRegister");
        }
    }

    [HttpGet("Update")]
    public IActionResult DepartmentUpdate([FromQuery] int number)
    {
        Department domain = _service.FindDepartment(number)!;
        DepartmentViewModel vm = _adapter.Convert(domain);

        return View(vm);
    }

    [HttpPost("Update")]
    public IActionResult DepartmentUpdate(int number, DepartmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("DepartmentUpdate", vm);
        }
        Department domain = _adapter.Restore(vm);
        _service.UpdateDepartment(number, domain);

        return RedirectToAction("DepartmentList", "Department");
    }
}