using FluentValidation;
using PodcastsWebApi.Models;


namespace PodcastsWebApi.ModelValidators
{
    public class EpisodesValidator : AbstractValidator<Episodes>
	{
		public EpisodesValidator()
		{
			RuleFor(x => x.Id).NotNull();


			// title url author numberofepisodes picture public List<Episodes> Episodes { get; set; }

		}
	}
}
