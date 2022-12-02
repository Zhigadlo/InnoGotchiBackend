using FluentValidation;
using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.Validation
{
    public class FarmValidator : AbstractValidator<Farm>
    {
        public FarmValidator()
        {
            RuleFor(f => f.Name).NotEmpty().NotNull();
            RuleFor(f => f.Owner).NotEmpty().NotNull();
        }
    }
}
