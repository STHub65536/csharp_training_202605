using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Presentations.ViewModels.ViewModelAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Management_System.Presentations.ViewModels;
public class EmployeeViewModel
{
    [Display(Name = "社員番号")]
    public int? EmpNo { get; set; }

    [Display(Name = "社員名")]
    [Required(ErrorMessage = "{0}は入力必須です")]
    [StringLength(20, ErrorMessage = "{0}は{1}文字以内で入力してください")]
    public string EmpName { get; set; }

    [Display(Name = "生年月日")]
    [Required(ErrorMessage = "{0}は入力必須です")]
    [Age(15, 100, ErrorMessage = "15歳～100歳までの方しか登録できません")]
    public DateOnly Birthday { get; set; }

    [Display(Name = "メールアドレス")]
    [Required(ErrorMessage = "{0}は入力必須です")]
    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
    public string MailAddress { get; set; }

    public int? DeptNo { get; set; }

    public Department? Dept { get; set; }

    public int? ChangedDeptNo { get; set; }

    public List<SelectListItem> DeptList { get; set; } = new List<SelectListItem>();
}