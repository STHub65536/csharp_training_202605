using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Presentations.ViewModels;
public class DepartmentViewModel
{
    [Display(Name = "部署番号")]
    [Required(ErrorMessage = "{0}は入力必須です")]
    [Range(100, 999, ErrorMessage = "{0}は{1}～{2}までの数字で入力して下さい")]
    public int? DeptNo { get; set; } = null;

    [Display(Name = "部署名")]
    [Required(ErrorMessage = "{0}は入力必須です")]
    [MaxLength(20, ErrorMessage = "{0}は{1}文字以内で入力してください")]
    [RegularExpression(@"^[ぁ-んァ-ヶｱ-ﾝﾞﾟ一-龠ー]+部$", ErrorMessage = "正しい形式(例: 総務部)で入力してください")]
    public string DeptName { get; set; } = "";
}