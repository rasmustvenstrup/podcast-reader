using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Podcasts.Entities;
using Podcasts.Interfaces;
using Podcasts.Models;
using Podcasts.Services;
using StructureMap.AutoMocking.Moq;

namespace Podcasts.Tests.Unit.Services
{
    public class PodcastServiceTests
    {
        [Test]
        public async Task ShouldAddPodcast()
        {
            // Given
            var context = new Context<PodcastService>();
            string feedUrl = "http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500";
            PodcastService sut = context.CreateSut();

            // When
            await sut.AddPodcast(feedUrl);

            // Then
            Podcast addedPodcast = context.AddedPodcast;
            addedPodcast.Should().NotBeNull();
            addedPodcast.Should().NotBeNull();
            addedPodcast.Id.Should().NotBeEmpty();
            addedPodcast.Title.Should().Be("Harddisken");
            addedPodcast.Description.Should().Be("Harddisken er radioens teknologimagasin på P1. Vi forklarer de konkrete teknologiske udviklinger og deres konsekvenser for kulturen, samfundet og hverdagen.");
            addedPodcast.FeedUrl.Should().Be(feedUrl);
            addedPodcast.ImageUrl.Should().Be("http://www.dr.dk/mu/Asset?Id=5576dda66187a4061caf6d0e.jpg");
        }

        [Test]
        public void ShouldGetPodcasts()
        {
            // Given
            var context = new Context<PodcastService>();
            PodcastService sut = context.CreateSut();

            // When
            IReadOnlyCollection<PodcastModel> podcasts = sut.GetPodcasts();

            // Then
            podcasts.Should().HaveCount(2);
        }

        [Test]
        public async Task ShouldRemovePodcast()
        {
            // Given
            var context = new Context<PodcastService>();
            Guid podcastId = Guid.NewGuid();
            PodcastService sut = context.CreateSut();

            // When
            await sut.RemovePodcast(podcastId);

            // Then
            context.DeletedPodcastId.ShouldBeEquivalentTo(podcastId);
        }

        [Test]
        public void ShouldGetFeed()
        {
            // Given
            var context = new Context<PodcastService>();
            Guid podcastId = Guid.NewGuid();
            PodcastService sut = context.CreateSut();

            // When
            FeedModel feed = sut.GetFeed(podcastId);
            
            // Then
            feed.Should().NotBeNull();
        }
    }

    public class Context<T> where T : class
    {
        private readonly MoqAutoMocker<T> _moqAutoMocker;
        public Podcast AddedPodcast { get; set; }
        public Guid DeletedPodcastId { get; set; }

        public T CreateSut()
        {
            return _moqAutoMocker.ClassUnderTest;
        }

        public Context()
        {
            _moqAutoMocker = new MoqAutoMocker<T>();

            Podcast podcast = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");
            var podcasts = new ReadOnlyCollection<Podcast>(new List<Podcast> { podcast, podcast });
            var podcastsModels = new ReadOnlyCollection<PodcastModel>(new List<PodcastModel> { new PodcastModel(), new PodcastModel() });
            var feed = new FeedModel();

            Mock.Get(_moqAutoMocker.Get<IPodcastRepository>())
                .Setup(repository => repository.Add(It.IsAny<Podcast>()))
                .Returns(Task.FromResult(true))
                .Callback((Podcast p) => AddedPodcast = p);

            Mock.Get(_moqAutoMocker.Get<IPodcastRepository>())
                .Setup(repository => repository.Delete(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true))
                .Callback((Guid podcastId) => DeletedPodcastId = podcastId);

            Mock.Get(_moqAutoMocker.Get<IPodcastRepository>())
                .Setup(repository => repository.GetAll())
                .Returns(podcasts);

            Mock.Get(_moqAutoMocker.Get<IPodcastRepository>())
                .Setup(repository => repository.Get(It.IsAny<Guid>()))
                .Returns(podcast);

            Mock.Get(_moqAutoMocker.Get<IMapper>())
                .Setup(mapper => mapper.Map<IReadOnlyCollection<PodcastModel>>(It.IsAny<IReadOnlyCollection<Podcast>>()))
                .Returns(podcastsModels);

            Mock.Get(_moqAutoMocker.Get<IMapper>())
                .Setup(mapper => mapper.Map<FeedModel>(It.IsAny<SyndicationFeed>()))
                .Returns(feed);
        }
    }
}
