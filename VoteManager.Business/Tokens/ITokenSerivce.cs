using System.Threading.Tasks;
using VoteManager.Models.Tokens;

namespace VoteManager.Business.Tokens
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(TokenRequest request);
    }
}