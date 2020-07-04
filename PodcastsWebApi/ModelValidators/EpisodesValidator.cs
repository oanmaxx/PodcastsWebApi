using FluentValidation;
using PodcastsWebApi.Models;


namespace PodcastsWebApi.ModelValidators
{
    public class EpisodesValidator : AbstractValidator<Episodes>
	{
		public EpisodesValidator()
		{
			RuleFor(x => x.Id).NotNull();

		}
	}
}
