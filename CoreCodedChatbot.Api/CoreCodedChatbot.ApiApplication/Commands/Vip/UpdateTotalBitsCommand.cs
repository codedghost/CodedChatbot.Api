using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class UpdateTotalBitsCommand : IUpdateTotalBitsCommand
{
    private readonly IUpdateTotalBitsRepository _updateTotalBitsRepository;
    private readonly IUpdateDonationVipsCommand _updateDonationVipsCommand;

    public UpdateTotalBitsCommand(
        IUpdateTotalBitsRepository updateTotalBitsRepository,
        IUpdateDonationVipsCommand updateDonationVipsCommand
    )
    {
        _updateTotalBitsRepository = updateTotalBitsRepository;
        _updateDonationVipsCommand = updateDonationVipsCommand;
    }

    public void Update(string username, int totalBits)
    {

        _updateTotalBitsRepository.Update(username, totalBits);

        _updateDonationVipsCommand.Update(username);
    }
}