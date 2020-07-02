using FluentValidation;
using PodcastsWebApi.Models;

namespace PodcastsWebApi.ModelValidators
{
    public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			//Id validation
			RuleFor(x => x.Id).NotNull();

			//FirstName Validation
			RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("The First Name must contain at least 2 characters.");
			RuleFor(x => x.FirstName).Matches("^[A-Za-z0-9]").WithMessage("Only A-Z, a-z, 0-9 allowed in FirstName.");


			//LastName Validation
			RuleFor(x => x.LastName).MinimumLength(2).WithMessage("The Last Name must contain at least 2 characters.");
			RuleFor(x => x.LastName).Matches("^[A-Za-z0-9]").WithMessage("Only A-Z, a-z, 0-9 allowed in LastName.");

			// UserName  
			RuleFor(u => u.UserName).MinimumLength(5).WithMessage("Username must be at least 5 characters.");
			
			//Password 
			
			
			//EmailAddress Validation
			RuleFor(e => e.EmailAddress).Matches(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$").WithMessage("The Email Address is invalid.");
		}
	}
}
