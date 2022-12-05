using FluentValidation;
using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.Validation
{
    public class RequestValidator : AbstractValidator<ColoborationRequest>
    {
        public RequestValidator()
        {
            RuleFor(r => r.IsConfirmed).NotEmpty().NotNull();
            RuleFor(r => r.RequestOwner).NotEmpty().NotNull();
            RuleFor(r => r.RequestReceipient).NotEmpty().NotNull();
            RuleFor(r => r.Date).NotEmpty().NotNull();
            RuleFor(r => r.RequestOwnerId).NotEmpty().NotNull();
            RuleFor(r => r.RequestReceipientId).NotEmpty().NotNull();
        }
    }
}
