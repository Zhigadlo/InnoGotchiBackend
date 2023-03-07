using FluentValidation;
using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.Validation
{
    public class RequestValidator : AbstractValidator<ColoborationRequest>
    {
        public RequestValidator()
        {
            RuleFor(r => r.IsConfirmed).NotNull();
            RuleFor(r => r.Date).NotEmpty().NotNull();
            RuleFor(r => r.RequestOwnerId).NotEmpty().NotNull().NotEqual(r => r.RequestReceipientId);
            RuleFor(r => r.RequestReceipientId).NotEmpty().NotNull();
        }
    }
}
