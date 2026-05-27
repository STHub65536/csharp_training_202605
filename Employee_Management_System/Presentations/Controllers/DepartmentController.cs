using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            DepartmentViewModel targetVM = _adapter.Convert(_service.FindDepartment(deptNo)!);
            _service.DeleteDepartment(deptNo);

            TempData["DeleteSuccess"] = $"{targetVM.DeptName}(部署番号:{targetVM.DeptNo})の削除に成功しました";
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

            return RedirectToAction("DepartmentCheck");
        }
        else
        {
            ViewData["ExistingError"] = "入力された部署番号または部署名は既に存在しています";

            return View(vm);
        }
    }

    [HttpGet("Register/Confirm")]
    public IActionResult DepartmentCheck()
    {
        string? json = (string?)TempData["DepartmentForm"];
        if (string.IsNullOrWhiteSpace(json))
        {
            return RedirectToAction("DepartmentRegister");
        }
        DepartmentViewModel vm = JsonSerializer.Deserialize<DepartmentViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Register/Confirm")]
    public IActionResult DepartmentCheck(DepartmentViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            Department domain = _adapter.Restore(vm);
            _service.AddDepartment(domain);

            TempData["RegisterSuccess"] = $"{vm.DeptName}(部署番号:{vm.DeptNo})の登録に成功しました";
            return RedirectToAction("DepartmentList");
        }
        else
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("DepartmentRegister");
        }
    }

    [HttpGet("Update")]
    public IActionResult DepartmentUpdate(int number)
    {
        string? json = (string?)TempData["InitializeForm"];
        
        ViewData["Number"] = number;
        DepartmentViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = _adapter.Convert(_service.FindDepartment(number)!);
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<DepartmentViewModel>(json!);

        return View(vm);
    }

    [HttpPost("Update")]
    public IActionResult DepartmentUpdate(int number, DepartmentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);

            if((ModelState["DeptNo"]?.Errors.Count ?? 0) > 0)
            {
                TempData["NumberError"] = ModelState["DeptNo"]?.Errors[0].ErrorMessage;
            }
            if((ModelState["DeptName"]?.Errors.Count ?? 0) > 0)
            {
                TempData["NameError"] = ModelState["DeptName"]?.Errors[0].ErrorMessage;
            }
            
            return RedirectToAction("DepartmentUpdate", new{ number });
        }

        //重複チェック
        List<DepartmentViewModel> vmList = _service.GetDepartmentList().Select(d => _adapter.Convert(d)).ToList();
        foreach(DepartmentViewModel viewModel in vmList)
        {
            if(viewModel.DeptNo != number) //更新前データ以外の時に重複している値があればエラー
            {
                if(viewModel.DeptNo == vm.DeptNo || viewModel.DeptName == vm.DeptName)
                {
                    TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
                    TempData["ExistingError"] = "更新しようとしている部署番号もしくは部署名は既に存在しています";

                    return RedirectToAction("DepartmentUpdate", new{ number });
                }
            }
        }
        
        TempData["DepartmentUpdateForm"] = JsonSerializer.Serialize(vm);

        return RedirectToAction("DepartmentUpdateCheck", new{ number });
    }

    [HttpGet("Update/Confirm")]
    public IActionResult DepartmentUpdateCheck(int number)
    {
        string? json = (string?)TempData["DepartmentUpdateForm"];

        TempData.Keep("DepartmentUpdateForm");
        ViewData["OriginNumber"] = number;

        DepartmentViewModel vm;
        if (string.IsNullOrWhiteSpace(json))
        {
            vm = new DepartmentViewModel();
            return View(vm);
        }
        vm = JsonSerializer.Deserialize<DepartmentViewModel>(json!);
        return View(vm);
    }

    [HttpPost("Update/Confirm")]
    public IActionResult DepartmentUpdateCheck(int number, DepartmentViewModel vm, int isRegister)
    {
        if(isRegister == 1)
        {
            DepartmentViewModel originVM = _adapter.Convert(_service.FindDepartment(number)!);
            Department domain = _adapter.Restore(vm);
            _service.UpdateDepartment(number, domain);

            TempData["UpdateSuccess"] = $"部署の更新に成功しました<br>部署番号:{originVM.DeptNo} → {vm.DeptNo} , 部署名:{originVM.DeptName} → {vm.DeptName}";
            return RedirectToAction("DepartmentList");
        }
        else
        {
            TempData["InitializeForm"] = JsonSerializer.Serialize(vm);
            
            return RedirectToAction("DepartmentUpdate", new{ number });
        }
    }
}