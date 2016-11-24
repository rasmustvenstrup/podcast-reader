using System;
using System.ServiceModel.Syndication;
using System.Xml;
using FluentValidation;
using FluentValidation.Results;
using Website.Models;

namespace Website.Validators
{
    public class PodcastViewModelValidator : AbstractValidator<PodcastViewModel>
    {
        public PodcastViewModelValidator()
        {
            RuleFor(m => m.FeedUrl)
                .NotEmpty()
                .WithMessage("Indtast venligst en url.");

            When(m => !string.IsNullOrEmpty(m.FeedUrl), () =>
            {
                Custom(m =>
                {
                    try
                    {
                        using (XmlReader xmlReader = XmlReader.Create(m.FeedUrl))
                        {
                            SyndicationFeed.Load(xmlReader);
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        return new ValidationFailure("FeedUrl", "Den indtastede url er ikke gyldig.");
                    }
                });
            });
        }
    }
}