using System.Collections.Generic;
using System.Threading.Tasks;
using JTWAuthServer.Common;

namespace JTWAuthServer.Services {
    // ReSharper disable once InconsistentNaming
    public interface IJWTClientService {

        Task<JWTClient> GetClientByAppIdAsync(string appId);

        Task<JWTClient> GetClientByRefreshTokenAsync(string refreshToken);

        Task UpdateClientAsync(JWTClient client);

        Task CreateClientAsync(JWTClient client);

        Task<JWTClient> GetClientByAccessTokenAsync(string accessToken);

        Task<Paged<JWTClient>> GetPagedClientAsync(int pageSize, int pageIndex);
    }

}