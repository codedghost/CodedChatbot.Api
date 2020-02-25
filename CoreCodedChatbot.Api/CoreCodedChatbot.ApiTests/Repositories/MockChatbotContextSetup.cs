using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CoreCodedChatbot.ApiTests.Repositories
{
    internal static class MockChatbotContextSetup
    {
        internal static Mock<DbSet<T>> SetUpDbSetMock<T>(List<T> data) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(s => s.Provider).Returns(data.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(s => s.Expression).Returns(data.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(s => s.ElementType).Returns(data.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(s => s.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            switch (data)
            {
                case List<InfoCommand> infoCommands:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => infoCommands.SingleOrDefault(u => u.InfoCommandId == (int)value[0]) as T);
                    break;
                case List<InfoCommandKeyword> infoCommandKeywords:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => infoCommandKeywords.SingleOrDefault(u => u.InfoCommandKeywordText == (string)value[0]) as T);
                    break;
                case List<LogEntry> logEntries:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => logEntries.SingleOrDefault(u => u.ID == (int)value[0]) as T);
                    break;
                case List<Quote> quotes:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => quotes.SingleOrDefault(u => u.QuoteId == (int)value[0]) as T);
                    break;
                case List<Setting> settings:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => settings.SingleOrDefault(u => u.SettingId == (int)value[0]) as T);
                    break;
                case List<Song> songs:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => songs.SingleOrDefault(u => u.SongId == (int)value[0]) as T);
                    break;
                case List<SongGuessingRecord> songGuessingRecords:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => songGuessingRecords.SingleOrDefault(u => u.SongGuessingRecordId == (int)value[0]) as T);
                    break;
                case List<SongPercentageGuess> songPercentageGuesses:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => songPercentageGuesses.SingleOrDefault(u => u.SongPercentageGuessId == (int)value[0]) as T);
                    break;
                case List<SongRequest> songRequests:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => songRequests.SingleOrDefault(u => u.SongRequestId == (int)value[0]) as T);
                    break;
                case List<StreamStatus> streamStatuses:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => streamStatuses.SingleOrDefault(u => u.StreamStatusId == (int)value[0]) as T);
                    break;
                case List<User> users:
                    dbSet.Setup(s => s.Find(It.IsAny<string>()))
                        .Returns((object[] value) => users.SingleOrDefault(u => u.Username == (string)value[0]) as T);
                    break;
                default:
                    throw new Exception(
                        "Unknown database type. Please update the MockChatbotContextSetup to include this data type");
            }

            return dbSet;
        }
    }
}