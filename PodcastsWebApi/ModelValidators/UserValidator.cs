﻿using FluentValidation;
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
			RuleFor(f => f.FirstName).NotEmpty().WithMessage("First Name must not be empty.");
			RuleFor(f => f.FirstName).MinimumLength(2).WithMessage("The length of First Name must be at least 2 characters.");
			//^\w-'+('- \w+)*$   ????
			RuleFor(f=> f.FirstName).Matches(@"^\w+( \w+)*$").WithMessage("Only A-Z, a-z, 0-9 and _ allowed in FirstName.");

			//LastName Validation
			RuleFor(l => l.LastName).NotEmpty().WithMessage("Last Name must not be empty.");
			RuleFor(l => l.LastName).MinimumLength(2).WithMessage("The length of Last Name must be at least 2 characters.");
			RuleFor(l => l.LastName).Matches(@"^\w+( \w+)*$").WithMessage("Only A-Z, a-z, 0-9 and _ allowed in LastName.");

			// UserName 
			RuleFor(u => u.UserName).NotEmpty().WithMessage("Username must not be emtpy.");
			RuleFor(u => u.UserName).MinimumLength(5).WithMessage("Username must be at least 5 characters.");
			RuleFor(u => u.UserName).NotEqual(User => User.FirstName).WithMessage("Username should not be equal to FirstName.");
			RuleFor(u => u.UserName).NotEqual(User => User.LastName).WithMessage("Username should not be equal to LastName.");

			//Password 
			//save the password into db?
			RuleFor(p => p.Password).NotEmpty().WithMessage("Password must not be empty");
			RuleFor(p => p.Password).MinimumLength(8).WithMessage("The length of Password must be at least 8 characters.");
			//confirmed password? 

			//EmailAddress Validation
			//The email really should be validated in some other way, such as through an email confirmation flow where an email is actually sent.
			//see https://github.com/dotnet/runtime/issues/27592
			RuleFor(e => e.EmailAddress).NotEmpty().WithMessage("Email Address must not be empty.");
			RuleFor(e => e.EmailAddress).Matches(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$").WithMessage("Email Address is not a valid email address.");
		}
	}
}
