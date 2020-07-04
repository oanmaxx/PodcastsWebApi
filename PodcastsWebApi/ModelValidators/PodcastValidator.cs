using FluentValidation;
using PodcastsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.ModelValidators
{
    public class PodcastValidator : AbstractValidator<Podcast>
    {
        public PodcastValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
