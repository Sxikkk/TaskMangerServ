using Application.RInterfaces;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class TokenRepository: ITokenRepository
{
    private readonly AppDbContext _context;
    
    public TokenRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<RefreshToken> UpdateTokenAsync(Guid tokenId, string newToken)
    {
        try
        {
            if (!await ExistTokenById(tokenId))
                throw new NullReferenceException("Token not found");

            var token = await GetTokenByIdAsync(tokenId);
            token.Token = newToken;
            await _context.SaveChangesAsync();
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<RefreshToken> AddTokenAsync(RefreshToken token)
    {
        if (await ExistTokenById(token.Id))
        {
            var existingToken = await GetTokenByIdAsync(token.Id);
            _context.RefreshTokens.Remove(existingToken);
            await _context.SaveChangesAsync();
        }

        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken> GetTokenByIdAsync(Guid tokenId)
    {
        if (!await ExistTokenById(tokenId))
            throw new NullReferenceException("Token not found");
        return (await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Id == tokenId))!;
    }

    public async Task<bool> ExistTokenById(Guid tokenId)
    {
        return await _context.RefreshTokens.AnyAsync(t => t.Id == tokenId);
    }

    public async Task<RefreshToken> UpdateTokenAsync(string newToken, Guid tokenId)
    {
        var token = await GetTokenByIdAsync(tokenId);
        token.Token = newToken;
        await _context.SaveChangesAsync();
        return token;
    }
}