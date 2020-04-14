using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChatCommand
{
    public class AddChatCommandRepository : IAddChatCommandRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public AddChatCommandRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Add(List<string> keywords, string informationText, string helpText, string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                if (context.InfoCommandKeywords.Any(ik => keywords.Contains(ik.InfoCommandKeywordText)))
                {
                    throw new Exception("A command with one or more of the provided aliases already exists");
                }

                var infoCommand = new InfoCommand
                {
                    InfoText = "{0}" + informationText,
                    InfoHelpText = "Hey @{0}! " + helpText,
                    AddedByUser = username
                };

                context.InfoCommands.Add(infoCommand);
                context.SaveChanges();

                var infoCommandKeywords = keywords.Select(k => new InfoCommandKeyword
                {
                    InfoCommandId = infoCommand.InfoCommandId,
                    InfoCommandKeywordText = k
                });

                context.InfoCommandKeywords.AddRange(infoCommandKeywords);
                context.SaveChanges();
            }
        }
    }
}