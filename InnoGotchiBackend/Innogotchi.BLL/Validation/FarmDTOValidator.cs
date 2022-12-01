using FluentValidation;
using InnoGotchi.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi.BLL.Validation
{
    public class FarmDTOValidator : AbstractValidator<FarmDTO>
    {
        public FarmDTOValidator() 
        {
            RuleFor(f => f.Name).NotEmpty().NotNull();
            RuleFor(f => f.Owner).NotEmpty().NotNull();
        }
    }
}
