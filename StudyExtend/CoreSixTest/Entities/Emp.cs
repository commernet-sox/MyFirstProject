using FluentValidation;
using System.ComponentModel.DataAnnotations;
namespace CoreSixTest.Entities
{
    public class Emp
    {
        [Compare("Id1",ErrorMessage ="Id值不一样")]
        public string Id { get; set; }
        [StringLength(1)]
        public string Id1 { get; set; }
        public string Name { get; set; }
        
    }
    public class EmpValidator : AbstractValidator<Emp>
    {
        public EmpValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithMessage("姓名不能为空").Length(2, 20).WithMessage("姓名长度输入错误");
        }
    }
}
