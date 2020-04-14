using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.Database.Context.Enums;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;
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
                context.TransferUser(moderatorUsername, oldUsername, newUsername);

                context.SaveChanges();
            }
        }
    }
}