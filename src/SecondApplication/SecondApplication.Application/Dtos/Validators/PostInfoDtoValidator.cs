
using FluentValidation;

namespace SecondApplication.Application.Dtos.Validators;

public class PostInfoDtoValidator : AbstractValidator<PostInfoDto>
{
    public PostInfoDtoValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage("{Username} cannot be empty.")
            .NotNull().WithMessage("{Username} is required.");

        RuleFor(r => r.PostId)
          .NotEmpty().WithMessage("{Username} cannot be empty.")
          .NotNull().WithMessage("{Username} is required.");
    }
}
