using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamLabs;

public class SaveStreamLabsDonationsRepository : ISaveStreamLabsDonationsRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ISetOrCreateSettingRepository _setOrCreateSettingRepository;

    public SaveStreamLabsDonationsRepository(
        IChatbotContextFactory chatbotContextFactory,
        ISetOrCreateSettingRepository setOrCreateSettingRepository)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _setOrCreateSettingRepository = setOrCreateSettingRepository;
    }

    public void Save(List<StreamLabsDonationIntermediate> donations)
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

        using (var context = _chatbotContextFactory.Create())
        {
            foreach (var donation in groupedDonations)
            {
                var user = context.GetOrCreateUser(donation.name);
                user.TotalDonated += donation.amount;
            }

            context.SaveChanges();
        }

        _setOrCreateSettingRepository.Set("LatestDonationId", latestDonationId.ToString());
    }
}