using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.Database.Context.Enums;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.Services.Identity;

namespace CoreCodedChatbot.ApiApplication.Repositories.Moderation
{
    public class TransferUserAccountRepository : ITransferUserAccountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public TransferUserAccountRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Transfer(string moderatorUsername, string oldUsername, string newUsername)
        {
            // All users should exist int the db at this point
            using (var context = _chatbotContextFactory.Create())
            {
                var modUser = context.Users.Find(moderatorUsername);

                var oldUser = context.Users.Find(oldUsername);
                var newUser = context.Users.Find(newUsername);

                if (modUser == null || oldUser == null || newUser == null)
                {
                    throw new Exception("One of the provided usernames does not exist");
                }

                TransferQuotes(context, oldUsername, newUsername);
                TransferSearchSynonymRequests(context, oldUsername, newUsername);
                TransferGuessingGameRecords(context, oldUsername, newUsername);
                TransferSongRequests(context, oldUsername, newUsername);
                TransferVipsAndBytes(context, oldUser, newUser);
                LogTransfer(context, moderatorUsername, oldUsername, newUsername);

                context.SaveChanges();
            }
        }

        private void TransferQuotes(IChatbotContext context, string oldUsername, string newUsername)
        {
            var quotes = context.Quotes.Where(q => q.CreatedBy == oldUsername);

            foreach (var quote in quotes)
            {
                quote.CreatedBy = newUsername;
            }
        }

        private void TransferSearchSynonymRequests(IChatbotContext context, string oldUsername, string newUsername)
        {
            var searchSynonymRequests = context.SearchSynonymRequests.Where(ssr => ssr.Username == oldUsername);

            foreach (var searchSynonymRequest in searchSynonymRequests)
            {
                searchSynonymRequest.Username = newUsername;
            }
        }

        private void TransferGuessingGameRecords(IChatbotContext context, string oldUsername, string newUsername)
        {
            var guessingGameRecords = context.SongPercentageGuesses.Where(spg => spg.Username == oldUsername);

            foreach (var guess in guessingGameRecords)
            {
                guess.Username = newUsername;
            }
        }

        private void TransferSongRequests(IChatbotContext context, string oldUsername, string newUsername)
        {
            var songRequests = context.SongRequests.Where(sr => sr.RequestUsername == oldUsername);

            foreach (var songRequest in songRequests)
            {
                songRequest.RequestUsername = newUsername;
            }
        }

        private void TransferVipsAndBytes(IChatbotContext context, User oldUsername, User newUsername)
        {
            newUsername.UsedVipRequests += oldUsername.UsedVipRequests;
            newUsername.UsedSuperVipRequests += oldUsername.UsedSuperVipRequests;
            newUsername.SentGiftVipRequests += oldUsername.SentGiftVipRequests;
            newUsername.ModGivenVipRequests += oldUsername.ModGivenVipRequests;
            newUsername.FollowVipRequest = oldUsername.FollowVipRequest;
            newUsername.SubVipRequests += oldUsername.SubVipRequests;
            newUsername.DonationOrBitsVipRequests = oldUsername.DonationOrBitsVipRequests;
            newUsername.ReceivedGiftVipRequests += oldUsername.ReceivedGiftVipRequests;
            newUsername.TokenVipRequests += oldUsername.TokenVipRequests;
            newUsername.TokenBytes += oldUsername.TokenBytes;
            newUsername.TotalBitsDropped = oldUsername.TotalBitsDropped;
            newUsername.TotalDonated += oldUsername.TotalDonated;
        }

        private void LogTransfer(IChatbotContext context, string moderatorUsername, string oldUsername, string newUsername)
        {
            var logRecord = new ModerationLog
            {
                Username = moderatorUsername,
                Action = ModerationAction.UsernameTransfer,
                ActionTakenTime = DateTime.UtcNow,
                ExtraInformation = $"{moderatorUsername} has transferred {oldUsername}'s account to {newUsername}"
            };

            context.ModerationLogs.Add(logRecord);
        }
    }
}