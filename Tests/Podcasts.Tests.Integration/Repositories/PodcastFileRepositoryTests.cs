using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Podcasts.Entities;
using Podcasts.Repositories;
using StructureMap;

namespace Podcasts.Tests.Integration.Repositories
{
    public class PodcastFileRepositoryTests
    {
        [Test]
        public async Task ShouldAddPodcast()
        {
            using (var context = new Context<PodcastFileRepository>())
            {
                // Given
                Podcast podcast = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");
                var sut = context.CreateSut();

                // When
                await sut.Add(podcast);

                // Then
                var podcastFileRepository = new PodcastFileRepository();
                Podcast podcastFromRepository = podcastFileRepository.Get(podcast.Id);
                podcastFromRepository.ShouldBeEquivalentTo(podcast);
                podcastFromRepository.Should().NotBeSameAs(podcast);
            }
        }

        [Test]
        public async Task ShouldGetPodcast()
        {
            using (var context = new Context<PodcastFileRepository>())
            {
                // Given
                Podcast podcast = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");
                await context.GivenPodcastsExist(podcast);
                var sut = context.CreateSut();

                // When
                Podcast podcastFromRepository = sut.Get(podcast.Id);

                // Then
                podcastFromRepository.Should().NotBeNull();
                podcastFromRepository.ShouldBeEquivalentTo(podcast);
                podcastFromRepository.Should().NotBeSameAs(podcast);
            }
        }

        [Test]
        public async Task ShouldGetAllPodcasts()
        {
            using (var context = new Context<PodcastFileRepository>())
            {
                // Given
                await context.Given2PodcastsExist();
                var sut = context.CreateSut();

                // When
                IReadOnlyCollection<Podcast> podcasts = sut.GetAll();

                // Then
                podcasts.Should().HaveCount(2);
            }
        }

        [Test]
        public async Task ShouldDeletePodcast()
        {
            using (var context = new Context<PodcastFileRepository>())
            {
                // Given
                Podcast podcast = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");
                await context.GivenPodcastsExist(podcast);
                var sut = context.CreateSut();

                // When
                await sut.Delete(podcast.Id);

                // Then
                var podcastFileRepository = new PodcastFileRepository();
                Action action = () => podcastFileRepository.Get(podcast.Id);
                action.ShouldThrow<InvalidOperationException>();
            }
        }
    }

    public class Context<T> : IDisposable where T : class 
    {
        private T _sut;
        private Container _container;

        public T CreateSut()
        {
            _container = new Container();
            _sut = _container.GetInstance<T>();
            return _sut;
        }

        public void Dispose()
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(T).GetField("_filepath", bindFlags);
            if (field != null)
            {
                string filePath = (string)field.GetValue(_sut);
                File.Delete(filePath);
            }
        }

        public async Task GivenPodcastsExist(Podcast podcast)
        {
            var podcastFileRepository = new PodcastFileRepository();
            await podcastFileRepository.Add(podcast);
        }

        public async Task Given2PodcastsExist()
        {
            var podcastFileRepository = new PodcastFileRepository();
            Podcast podcast1 = Podcast.Create("http://www.dr.dk/mu/Feed/harddisken?format=podcast&limit=500");
            Podcast podcast2 = Podcast.Create("http://www.dr.dk/mu/Feed/troldspejlet?format=podcast&limit=500");
            await podcastFileRepository.Add(podcast1);
            await podcastFileRepository.Add(podcast2);
        }
    }
}
