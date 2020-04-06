namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IUpdateDonationVipsRepository
    {
        void Update(string username, double bitsToVip, double donationAmountToVip);
    }
}