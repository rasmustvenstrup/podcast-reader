using System;
using System.ServiceModel.Syndication;
using FluentAssertions;
using NUnit.Framework;
using Podcasts.Entities;

namespace Podcasts.Tests.Unit.Entities
{
    public class PodcastTests
    {
        [Test]
        public void ShouldCreatePodcast()
        {
            // Given
            string feedUrl = "http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500";

            // When
            Podcast sut = Podcast.Create(feedUrl);

            // Then
            sut.Should().NotBeNull();
            sut.Id.Should().NotBeEmpty();
            sut.Title.Should().Be("Harddisken");
            sut.Description.Should().Be("Harddisken er radioens teknologimagasin på P1. Vi forklarer de konkrete teknologiske udviklinger og deres konsekvenser for kulturen, samfundet og hverdagen.");
            sut.FeedUrl.Should().Be(feedUrl);
            sut.ImageUrl.Should().Be("http://www.dr.dk/mu/Asset?Id=5576dda66187a4061caf6d0e.jpg");
        }

        [TestCase(null, "Argument cannot be null or empty.\r\nParameternavn: feedUrl")]
        [TestCase("", "Argument cannot be null or empty.\r\nParameternavn: feedUrl")]
        [TestCase("http://www.dr.dk", "Argument is not a valid RSS feed url.\r\nParameternavn: feedUrl")]
        public void ShouldThrowGivenFeedUrlIsInvalid(string feedUrl, string exceptionMessage)
        {
            // When
            Action action = () => Podcast.Create(feedUrl);

            // Then
            action.ShouldThrow<ArgumentException>().WithMessage(exceptionMessage);
        }

        [Test]
        public void ShouldGetFeed()
        {
            // Given
            Podcast sut = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");

            // When
            SyndicationFeed feed = sut.GetFeed();

            // Then
            feed.Should().NotBeNull();
            feed.Title.Text.Should().Be("Harddisken");
            feed.Description.Text.Should().Be("Harddisken er radioens teknologimagasin på P1. Vi forklarer de konkrete teknologiske udviklinger og deres konsekvenser for kulturen, samfundet og hverdagen.");
            feed.ImageUrl.AbsoluteUri.Should().Be("http://www.dr.dk/mu/Asset?Id=5576dda66187a4061caf6d0e.jpg");
            feed.Items.Should().HaveCount(151);
        }
    }
}
