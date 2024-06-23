using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.RequestModels.Ylyl;
using CoreCodedChatbot.ApiContract.ResponseModels.Ylyl;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IYlylService
{
    Task<YlylSessionResponse> ChangeSession(YlylSessionRequest request);
    Task SaveSubmission(YlylSubmissionRequest request);
    Task<YlylGetSubmissionsResponse> GetSubmissions(YlylGetSubmissionsRequest request);
    Task UpdateUsers(YlylUpdateUsersRequest request);
}