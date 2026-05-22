using System;
using System.Collections.Generic;
using System.Linq;
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
        List<Department> domainList = _service.GetDepartmentList();
        //domainList.ForEach(d => Console.WriteLine("fffff"+d.DeptName));
        
        List<DepartmentViewModel> vmList = domainList.Select(d => _adapter.Convert(d)).ToList();
        return View(vmList);
    }

    [HttpPost("List")]
    public IActionResult DepartmentList(int id)
    {
        _service.DeleteDepartment(id);
        return RedirectToAction();
    }

    [HttpGet("Register")]
    public IActionResult DepartmentRegister()
    {
        return View();
    }

    [HttpPost("Register")]
    public IActionResult DepartmentRegister(DepartmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("DepartmentRegister", vm);
        }
        
        Department domain = _adapter.Restore(vm);
        
        if(_service.FindDepartment((int)vm.DeptNo!) != null) // ViewModelでのバリデーションでvm.DeptNoの非nullはチェック済み
        {
            ViewData["ExistingNoError"] = "この部署番号は既に使用されています";
            return View("DepartmentRegister", vm);
        }

        _service.AddDepartment(domain);
        return RedirectToAction("DepartmentList", "Department");
    }

    [HttpGet("Update")]
    public IActionResult DepartmentUpdate([FromQuery] int id)
    {
        Department domain = _service.FindDepartment(id)!;
        DepartmentViewModel vm = _adapter.Convert(domain);

        return View(vm);
    }

    [HttpPost("Update")]
    public IActionResult DepartmentUpdate(int id, DepartmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("DepartmentUpdate", vm);
        }
        Department domain = _adapter.Restore(vm);
        _service.UpdateDepartment(id, domain);

        return RedirectToAction("DepartmentList", "Department");
    }
}