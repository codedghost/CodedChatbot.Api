using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class UpdateDonationVipsCommand : IUpdateDonationVipsCommand
{
    private readonly IConfigService _configService;
    private readonly IUpdateDonationVipsRepository _updateDonationVipsRepository;

    public UpdateDonationVipsCommand(
        IConfigService configService,
        IUpdateDonationVipsRepository updateDonationVipsRepository
    )
    {
        _configService = configService;
        _updateDonationVipsRepository = updateDonationVipsRepository;
    }

    public void Update(string username)
    {
    }
}