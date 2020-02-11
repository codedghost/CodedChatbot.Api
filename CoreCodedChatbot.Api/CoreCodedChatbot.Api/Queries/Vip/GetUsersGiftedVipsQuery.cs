using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Queries.Vip
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