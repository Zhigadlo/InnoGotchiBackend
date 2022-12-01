using FluentValidation;
using InnoGotchi.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi.BLL.Validation
{
    public class PetDTOValidator : AbstractValidator<PetDTO>
    {
        public PetDTOValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().NotNull();
            RuleFor(p => p.Appearance).NotEmpty().NotNull();
            RuleFor(p => p.Farm).NotEmpty().NotNull();
            RuleFor(p => p.CreateTime).NotEmpty()
                                      .NotNull()
                                      .LessThan(p => p.DeadTime);
        }
    }
}
