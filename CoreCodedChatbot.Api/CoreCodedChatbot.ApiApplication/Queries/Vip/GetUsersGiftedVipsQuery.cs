using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Queries.Vip
{
    public class GetUsersGiftedVipsQuery : IGetUsersGiftedVipsQuery
    {
        private readonly IGetUsersGiftedVipsRepository _getUsersGiftedVipsRepository;

        public GetUsersGiftedVipsQuery(
            IGetUsersGiftedVipsRepository getUsersGiftedVipsRepository
        )
        {
            _getUsersGiftedVipsRepository = getUsersGiftedVipsRepository;
        }

        public int GetUsersGiftedVips(string username)
        {
            return _getUsersGiftedVipsRepository.GetUsersGiftedVips(username);
        }
    }
}