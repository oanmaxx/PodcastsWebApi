using FluentValidation;
using PodcastsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.ModelValidators
{
    public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			RuleFor(x => x.Id).NotNull();
		}
	}
}
