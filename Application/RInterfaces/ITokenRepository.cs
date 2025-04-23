using Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.RInterfaces;

public interface ITokenRepository
{
    Task<RefreshToken> UpdateTokenAsync(Guid tokenId, string newToken);
    Task<RefreshToken> AddTokenAsync(RefreshToken token);
    Task<RefreshToken> GetTokenByIdAsync(Guid tokenId);
    Task<bool> ExistTokenById(Guid tokenId);
    Task<RefreshToken> UpdateTokenAsync(string newToken, Guid tokenId);
}