﻿using System.Collections.Generic;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Repositories.Playlist
{
    [TestFixture]
    public class ClearRequestsRepositoryTests
    {
        private Mock<IChatbotContextFactory> _chatbotContextFactory;
        private Mock<IChatbotContext> _chatbotContext;

        private ClearRequestsRepository _subject;

        [SetUp]
        public void Setup()
        {
            _chatbotContextFactory = new Mock<IChatbotContextFactory>();
            _chatbotContext = new Mock<IChatbotContext>();
        }

        private void SetupSubject(Mock<DbSet<SongRequest>> songRequests)
        {
            _chatbotContext.Setup(s => s.SongRequests).Returns(songRequests.Object);

            _chatbotContextFactory.Setup(s => s.Create()).Returns(_chatbotContext.Object);

            _subject = new ClearRequestsRepository(_chatbotContextFactory.Object);
        }

        [Test, AutoData]
        public void EnsureMethodExitsWhen_NullItemsToRemove(List<SongRequest> dbList)
        {
            var dbSet = MockChatbotContextSetup.SetUpDbSetMock(dbList);

            SetupSubject(dbSet);

            _subject.ClearRequests(null);

            _chatbotContextFactory.Verify(s => s.Create(), Times.Never);
            _chatbotContext.Verify(s => s.SongRequests, Times.Never);
            _chatbotContext.Verify(s => s.SaveChanges(), Times.Never);
        }

        [Test, AutoData]
        public void EnsureMethodExitsWhen_EmptyListOfRequestsToRemove(List<SongRequest> dbList)
        {
            var dbSet = MockChatbotContextSetup.SetUpDbSetMock(dbList);

            SetupSubject(dbSet);

            _subject.ClearRequests(new List<BasicSongRequest>());

            _chatbotContextFactory.Verify(s => s.Create(), Times.Never);
            _chatbotContext.Verify(s => s.SongRequests, Times.Never);
            _chatbotContext.Verify(s => s.SaveChanges(), Times.Never);
        }
    }
}