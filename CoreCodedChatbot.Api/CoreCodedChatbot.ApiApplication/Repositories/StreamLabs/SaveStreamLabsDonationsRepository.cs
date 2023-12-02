using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamLabs;

public class SaveStreamLabsDonationsRepository : BaseRepository<User>
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public SaveStreamLabsDonationsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task Save(List<StreamLabsDonationIntermediate> donations)
    {
        var latestDonationId = donations.OrderByDescending(d => d.CreateAt).FirstOrDefault()?.DonationId ?? 0;
        if (latestDonationId == 0) return;

        var groupedDonations = donations
            .OrderByDescending(d => d.CreateAt)
            .GroupBy(d => d.Name.ToLower())
            .Select(d => new
            {
                name = d.First().Name.ToLower(),
                amount = (int) Math.Round(d.Sum(rec => rec.Amount) * 100)
            });

        foreach (var donation in groupedDonations)
        {
            var user = Context.GetOrCreateUser(donation.name);
            user.TotalDonated += donation.amount;

            await Context.SaveChangesAsync();
        }

        using (var setOrCreateSettingRepository = new SetOrCreateSettingRepository(_chatbotContextFactory))
        {
            await setOrCreateSettingRepository.Set("LatestDonationId", latestDonationId.ToString());
        }
    }
}