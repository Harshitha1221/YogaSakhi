using System.Threading.Tasks;
using YogaSakhi.Domain.Entities;

namespace YogaSakhi.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, string Message)> RegisterAsync(
            string email, 
            string password, 
            string fullName, 
            int age);

        Task<(bool Success, string Token, string Message)> LoginAsync(
            string email, 
            string password);

        Task<(bool Success, string Message)> RefreshTokenAsync(string token);
        Task<(bool Success, string Message)> VerifyEmailAsync(string email, string code);
        Task<User> GetUserByEmailAsync(string email);
        Task<string> GenerateJwtTokenAsync(User user);
    }
}
