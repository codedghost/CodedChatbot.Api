using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public interface IStreamLabsService
    {
        void Initialise();
    }

    public class StreamLabsService : IStreamLabsService
    {
        private readonly IGetRecentDonationsQuery _getRecentDonationsQuery;
        private readonly ISaveStreamLabsDonationsRepository _saveStreamLabsDonationsRepository;
        private readonly IUpdateDonationVipsCommand _updateDonationVipsCommand;
        private readonly ILogger<IStreamLabsService> _logger;
        private Timer CheckLatestDonationsTimer { get; set; }

        private const int MaxRetries = 2;

        public StreamLabsService(
            IGetRecentDonationsQuery getRecentDonationsQuery,
            ISaveStreamLabsDonationsRepository saveStreamLabsDonationsRepository,
            IUpdateDonationVipsCommand updateDonationVipsCommand,
            ILogger<IStreamLabsService> logger
        )
        {
            _getRecentDonationsQuery = getRecentDonationsQuery;
            _saveStreamLabsDonationsRepository = saveStreamLabsDonationsRepository;
            _updateDonationVipsCommand = updateDonationVipsCommand;
            _logger = logger;
        }

        public void Initialise()
        {
            CheckLatestDonationsTimer = new Timer(e =>
                {
                    CheckLatestDonations();
                },
                null, 
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));
        }

        private async void CheckLatestDonations()
        {
            try
            {
                var attempts = 0;

                List<StreamLabsDonationIntermediate> donations = null;

                while (donations == null)
                {
                    donations = await _getRecentDonationsQuery.Get();
                    attempts++;

                    if (attempts >= MaxRetries) break;
                }

                if (donations == null || !donations.Any()) return;

                _saveStreamLabsDonationsRepository.Save(donations);

                foreach (var user in donations.Select(d => d.Name.ToLower()).Distinct())
                {
                    _updateDonationVipsCommand.Update(user);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when checking for latest StreamLabs donations");
            }
        }
    }
}