using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Presentations.ViewModels;
public class AdminViewModel
{
    [Display(Name = "ユーザーID")]
    public string UserId { get; set; } = "";

    [Display(Name = "パスワード")]
    [Required]
    public string Password { get; set; } = "";

    [Display(Name = "氏名")]
    public string UserName { get; set; } = "";
}