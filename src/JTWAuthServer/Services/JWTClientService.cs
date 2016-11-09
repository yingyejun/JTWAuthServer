using System;
using System.Threading.Tasks;
using JTWAuthServer.Data;
using Microsoft.EntityFrameworkCore;

namespace JTWAuthServer.Services {
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class JWTClientService : IJWTClientService {
        private readonly JWTAuthDbContext _context;

        public JWTClientService(JWTAuthDbContext context) {
            _context = context;
        }

        public async Task<JWTClient> GetClientByAppIdAsync(string appId) {
            return await _context.Set<JWTClient>().FirstOrDefaultAsync(t => t.AppId == appId);
        }

        public async Task<JWTClient> GetClientByRefreshTokenAsync(string refreshToken) {
            return await _context.Set<JWTClient>().FirstOrDefaultAsync(t => t.LastRefreshToken == refreshToken);
        }

        public async Task<JWTClient> GetClientByAccessTokenAsync(string accessToken) {
            return await _context.Set<JWTClient>().FirstOrDefaultAsync(t => t.LastAccessToken == accessToken);
        }

        public async Task UpdateClientAsync(JWTClient client) {
            _context.Set<JWTClient>().Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task CreateClientAsync(JWTClient client) {
            _context.Set<JWTClient>().Add(client);
            await _context.SaveChangesAsync();
        }
    }
}