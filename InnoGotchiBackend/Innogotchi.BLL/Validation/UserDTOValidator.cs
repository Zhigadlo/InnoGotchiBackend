using FluentValidation;
using InnoGotchi.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi.BLL.Validation
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(u => u.Email).NotEmpty()
                                 .NotNull()
                                 .EmailAddress();
            RuleFor(u => u.Password).NotEmpty()
                                    .NotNull()
                                    .MinimumLength(8);
            RuleFor(u => u.FirstName).NotEmpty().NotNull();
            RuleFor(u => u.LastName).NotEmpty().NotNull();

        }
    }
}
