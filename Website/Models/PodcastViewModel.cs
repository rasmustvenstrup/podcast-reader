using System.Collections.Generic;
using FluentValidation.Attributes;
using Podcasts.Models;
using Website.Validators;

namespace Website.Models
{
    [Validator(typeof(PodcastViewModelValidator))]
    public class PodcastViewModel
    {
        public string FeedUrl { get; set; }
        public IReadOnlyCollection<PodcastModel> Podcasts { get; set; }
    }
}