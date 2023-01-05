using FluentValidation;
using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.Validation
{
    public class PictureValidator : AbstractValidator<Picture>
    {
        public PictureValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull();
            RuleFor(p => p.Image).NotEmpty().NotNull();
            RuleFor(p => p.Description).NotEmpty().NotNull();
        }
    }
}
