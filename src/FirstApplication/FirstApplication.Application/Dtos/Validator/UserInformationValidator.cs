using FluentValidation;

namespace FirstApplication.Application.Dtos.Validator;

public class UserInformationValidator : AbstractValidator<UserInfoDto>
{
    public UserInformationValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("{UserId} cannot be empty.")
            .NotNull().WithMessage("{UserId} is required.");

    }
   
}
