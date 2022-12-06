using FluentValidation;
using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.Validation
{
    public class PetValidator : AbstractValidator<Pet>
    {
        public PetValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull();
            RuleFor(p => p.Appearance).NotEmpty().NotNull();
            RuleFor(p => p.FarmId).NotEmpty().NotNull();
            RuleFor(p => p.CreateTime).NotEmpty()
                                      .NotNull();
        }
    }
}
