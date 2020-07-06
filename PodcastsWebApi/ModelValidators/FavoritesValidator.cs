using FluentValidation;
using PodcastsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.ModelValidators
{
    public class FavoritesValidator : AbstractValidator<Favorites>
    {
        public FavoritesValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Podcast).NotNull();
            RuleFor(x => x.Email).NotNull();
        }
    }
}
