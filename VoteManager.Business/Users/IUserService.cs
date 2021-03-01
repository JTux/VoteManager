using System.Threading.Tasks;
using VoteManager.Models.Users;

namespace VoteManager.Business.Users
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegister model);
        Task<bool> RegisterUserAsync(UserRegister model, string role);
    }
}