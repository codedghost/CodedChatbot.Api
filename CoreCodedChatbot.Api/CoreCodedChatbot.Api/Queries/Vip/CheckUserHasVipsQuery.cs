using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Queries.Vip
{
    public class CheckUserHasVipsQuery : ICheckUserHasVipsQuery
    {
        private readonly IGetUsersVipCountRepository _getUsersVipCountRepository;

        public CheckUserHasVipsQuery(
            IGetUsersVipCountRepository getUsersVipCountRepository
            )
        {
            _getUsersVipCountRepository = getUsersVipCountRepository;
        }

        public bool CheckUserHasVips(string username, int vipsToCheck)
        {
            var userVips = _getUsersVipCountRepository.GetVips(username);

            return userVips >= vipsToCheck;
        }
    }
}