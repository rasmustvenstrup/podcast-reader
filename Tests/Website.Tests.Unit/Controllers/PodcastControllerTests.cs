using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Podcasts.Interfaces;
using Podcasts.Models;
using StructureMap.AutoMocking.Moq;
using Website.Controllers;
using Website.Models;

namespace Website.Tests.Unit.Controllers
{
    public class PodcastControllerTests
    {
        [Test]
        public void ShouldGetIndex()
        {
            // Given
            var context = new Context<PodcastController>();
            PodcastController sut = context.CreateSut();

            // When
            ViewResult result = sut.Index() as ViewResult;

            // Then
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<PodcastViewModel>();
            PodcastViewModel model = (PodcastViewModel) result.Model;
            model.FeedUrl.Should().BeNull();
            model.Podcasts.Should().HaveCount(2);
        }

        [Test]
        public void ShouldGetFeed()
        {
            // Given
            var context = new Context<PodcastController>();
            Guid podcastId = Guid.NewGuid();
            PodcastController sut = context.CreateSut();

            // When
            ViewResult result = sut.Feed(podcastId) as ViewResult;

            // Then
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<FeedModel>();
            FeedModel model = (FeedModel)result.Model;
            model.Should().NotBeNull();
        }
    }

    public class Context<T> where T : class
    {
        private readonly MoqAutoMocker<T> _moqAutoMocker;

        public T CreateSut()
        {
            return _moqAutoMocker.ClassUnderTest;
        }

        public Context()
        {
            _moqAutoMocker = new MoqAutoMocker<T>();

            var podcastsModels = new ReadOnlyCollection<PodcastModel>(new List<PodcastModel> { new PodcastModel(), new PodcastModel() });
            var feed = new FeedModel();

            Mock.Get(_moqAutoMocker.Get<IPodcastService>())
                .Setup(service => service.GetPodcasts())
                .Returns(podcastsModels);

            Mock.Get(_moqAutoMocker.Get<IPodcastService>())
                .Setup(service => service.GetFeed(It.IsAny<Guid>()))
                .Returns(feed);

            Mock.Get(_moqAutoMocker.Get<IPodcastService>())
                .Setup(service => service.AddPodcast(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
        }
    }
}
