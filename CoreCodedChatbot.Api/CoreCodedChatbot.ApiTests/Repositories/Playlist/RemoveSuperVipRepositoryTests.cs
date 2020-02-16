using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Repositories.Playlist
{
    [TestFixture]
    public class RemoveSuperVipRepositoryTests
    {
        private Mock<IChatbotContextFactory> _chatbotContextFactory;
        private Mock<IChatbotContext> _context;
        private Mock<IConfigService> _configService;

        private Mock<DbSet<SongRequest>> _songRequests;
        private Mock<DbSet<User>> _users;

        private int _superVipCost = 50;

        private RemoveSuperVipRepository _subject;

        [SetUp]
        public void Setup()
        {
            _chatbotContextFactory = new Mock<IChatbotContextFactory>();
            _context = new Mock<IChatbotContext>();

            _configService = new Mock<IConfigService>();
            _configService.Setup(s => s.Get<int>("SuperVipCost")).Returns(_superVipCost);

            _songRequests = new Mock<DbSet<SongRequest>>();
            _users = new Mock<DbSet<User>>();
        }

        private void SetUsers(List<User> users)
        {
            _users.As<IQueryable<User>>().Setup(s => s.Provider).Returns(users.AsQueryable().Provider);
            _users.As<IQueryable<User>>().Setup(s => s.Expression).Returns(users.AsQueryable().Expression);
            _users.As<IQueryable<User>>().Setup(s => s.ElementType).Returns(users.AsQueryable().ElementType);
            _users.As<IQueryable<User>>().Setup(s => s.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
            _users.Setup(s => s.Find(It.IsAny<string>()))
                .Returns((object[] value) => users.SingleOrDefault(u => u.Username == (string)value[0]));
        }

        private void SetSongRequests(List<SongRequest> songRequests)
        {
            _songRequests.As<IQueryable<SongRequest>>().Setup(s => s.Provider).Returns(songRequests.AsQueryable().Provider);
            _songRequests.As<IQueryable<SongRequest>>().Setup(s => s.Expression).Returns(songRequests.AsQueryable().Expression);
            _songRequests.As<IQueryable<SongRequest>>().Setup(s => s.ElementType).Returns(songRequests.AsQueryable().ElementType);
            _songRequests.As<IQueryable<SongRequest>>().Setup(s => s.GetEnumerator()).Returns(songRequests.AsQueryable().GetEnumerator());
            _songRequests.Setup(s => s.Find(It.IsAny<int>())).Returns((object[] value) =>
                songRequests.SingleOrDefault(sr => sr.SongRequestId == (int)value[0]));
        }

        private void SetupSubject()
        {
            _context.Setup(s => s.SongRequests).Returns(_songRequests.Object);
            _context.Setup(s => s.Users).Returns(_users.Object);

            _chatbotContextFactory.Setup(s => s.Create()).Returns(_context.Object);
            _subject = new RemoveSuperVipRepository(_chatbotContextFactory.Object, _configService.Object);
        }

        [Test, AutoData]
        public void SaveChangesNotCalledWhen_UserHasNoSuperVip(List<SongRequest> requests, List<User> users)
        {
            var username = "TestUsername";

            SetUsers(users);
            SetSongRequests(requests);
            SetupSubject();

            _subject.Remove(username);

            _context.Verify(s => s.Users, Times.Never);
            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Never);
            _context.Verify(s => s.SaveChanges(), Times.Never);
        }

        [Test, AutoData]
        public void SaveChangesNotCalledWhen_UserHasAPlayedSuperVip(List<SongRequest> requests, List<User> users, SongRequest userRequest, User user)
        {
            userRequest.RequestUsername = user.Username;
            userRequest.Played = true;

            users.Add(user);
            requests.Add(userRequest);

            SetUsers(users);
            SetSongRequests(requests);
            SetupSubject();

            _subject.Remove(user.Username);

            _context.Verify(s => s.Users, Times.Never);
            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Never);
            _context.Verify(s => s.SaveChanges(), Times.Never);
        }

        [Test, AutoData]
        public void SaveChangesNotCalledWhen_UserOnlyHasNormalVipAndRegular(List<SongRequest> requests,
            List<User> users, SongRequest userVipRequest, SongRequest userRegularRequest, User user)
        {
            userVipRequest.RequestUsername = user.Username;
            userVipRequest.Played = false;
            userVipRequest.VipRequestTime = DateTime.Now;
            userVipRequest.SuperVipRequestTime = null;

            userRegularRequest.RequestUsername = user.Username;
            userVipRequest.Played = false;
            userRegularRequest.VipRequestTime = null;
            userRegularRequest.SuperVipRequestTime = null;

            requests.Add(userVipRequest);
            requests.Add(userRegularRequest);
            users.Add(user);

            SetUsers(users);
            SetSongRequests(requests);
            SetupSubject();

            _subject.Remove(user.Username);

            _context.Verify(s => s.Users, Times.Never);
            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Never);
            _context.Verify(s => s.SaveChanges(), Times.Never);
        }

        [Test, AutoData]
        public void SaveChangesNotCalledWhen_UserRecordDoesNotExist(List<SongRequest> requests, List<User> users, SongRequest songRequest, User user)
        {
            songRequest.RequestUsername = user.Username;
            songRequest.Played = false;
            songRequest.VipRequestTime = DateTime.Now;
            songRequest.SuperVipRequestTime = DateTime.Now;

            requests.Add(songRequest);

            SetUsers(users);
            SetSongRequests(requests);
            SetupSubject();

            _subject.Remove(user.Username);


            _context.Verify(s => s.Users, Times.Once);
            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Never);
            _context.Verify(s => s.SaveChanges(), Times.Never);
        }

        [Test, AutoData]
        public void SaveChangesCalledWhen_AllIsOk(List<SongRequest> requests, List<User> users, SongRequest userRequest,
            User user)
        {
            userRequest.RequestUsername = user.Username;
            userRequest.Played = false;
            userRequest.VipRequestTime = DateTime.Now;
            userRequest.SuperVipRequestTime = DateTime.Now;

            users.Add(user);
            requests.Add(userRequest);

            SetUsers(users);
            SetSongRequests(requests);
            SetupSubject();

            _subject.Remove(user.Username);

            _context.Verify(s => s.Users, Times.Once);
            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Once);
            _context.Verify(s => s.SaveChanges(), Times.Once);
        }
    }
}