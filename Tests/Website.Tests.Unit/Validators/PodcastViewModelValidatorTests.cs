using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using Website.Models;
using Website.Validators;

namespace Website.Tests.Unit.Validators
{
    public class PodcastViewModelValidatorTests
    {
        [TestCase("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500", 0)]
        public void ShouldReturnValidModelState(string feedUrl, int expectedErrors)
        {
            // Given
            var podcastViewModel = new PodcastViewModel { FeedUrl = feedUrl };
            var sut = new PodcastViewModelValidator();

            // When
            ValidationResult validationResult = sut.Validate(podcastViewModel);

            // Then
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().HaveCount(expectedErrors);
        }

        [TestCase(null, 1, "Indtast venligst en url.")]
        [TestCase("", 1, "Indtast venligst en url.")]
        [TestCase("http://www.dr.dk", 1, "Den indtastede url er ikke gyldig.")]
        public void ShouldReturnInvalidModelState(string feedUrl, int expectedErrors, string expectedErrorMessage)
        {
            // Given
            var podcastViewModel = new PodcastViewModel { FeedUrl = feedUrl };
            var sut = new PodcastViewModelValidator();

            // When
            ValidationResult validationResult = sut.Validate(podcastViewModel);

            // Then
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(expectedErrors);
            validationResult.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}
