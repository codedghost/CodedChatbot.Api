using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Queries.Vip
{
    public class UserHasVipsQuery : IUserHasVipsQuery
    {
        private readonly IUserHasVipsRepository _userHasVipsRepository;

        public UserHasVipsQuery(
            IUserHasVipsRepository userHasVipsRepository
            )
        {
            _userHasVipsRepository = userHasVipsRepository;
        }

        public bool UserHasVips(string username, int vips)
        {
            return _userHasVipsRepository.HasVips(username, vips);
        }
    }
}