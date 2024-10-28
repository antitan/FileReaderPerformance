Merci beaucoup pour ta reponse, j'ai détecté un nouveau probleme : une nouvelle exception est lancée lorsque la ligne var token = await tokenProvider.GetAccessTokenAsync() est appelée en parallele .
Certainement un probleme de concurrence de thread. Voici l'implementation :

using Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Ubp.Dione.Common.Netcore.Caching.Abstractions;

namespace Infrastructure.Services.DocumentProcessor
{
    public interface ITokenProvider
    {
        Task<string> GetAccessTokenAsync();
        Task ForceRefreshTokenAsync();
    }

    public class TokenProvider : ITokenProvider
    { 
        private readonly RestClient client; 
        private const string TokenCacheKey = "AccessToken";
        private const string CacheCategory = "JWT";
        private readonly short apiCallTimeoutInSeconds = 3000;
        private readonly ICacheRepository cacheRepository;
        private readonly ApplicationConfiguration applicationConfiguration;
        private readonly ILogger<IngestionService> logger;

        public TokenProvider(ILogger<IngestionService> logger, IOptions<ApplicationConfiguration> applicationConfiguration,ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
            this.applicationConfiguration = applicationConfiguration.Value;
            this.logger = logger;
            client = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(this.applicationConfiguration.ZitadelDomain, UriKind.Absolute),
                MaxTimeout = apiCallTimeoutInSeconds, 
                Proxy = new WebProxy(this.applicationConfiguration.UbpProxyUrl, true)
                {
                    Credentials = new NetworkCredential(this.applicationConfiguration.ProxyUser, this.applicationConfiguration.ProxyPassword)
                }
            });
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string? accessToken = null;
            try
            {  
                if (cacheRepository.TryGetValue(TokenCacheKey, out accessToken))
                {
                    return accessToken!;
                }
                accessToken = await FetchNewTokenAsync();
            }
            catch(Exception ex)
            {
                string msg = $"Error while requesting  new access token ";
                logger.LogError(ex, msg);
                throw new Exception(msg);
            }
            return accessToken;
        }

        public async Task ForceRefreshTokenAsync()
        {
            try
            {
                cacheRepository.Reset(new string[] { CacheCategory });
                 await FetchNewTokenAsync();
            }
            catch (Exception ex)
            {
                string msg = $"Error to refresh with new access token ";
                logger.LogError(ex, msg);
                throw new Exception(msg);
            }
        }


        private async Task<string> FetchNewTokenAsync()
        {
            var tokenResponse = await RequestNewTokenAsync();

            cacheRepository.Set(TokenCacheKey,
                                tokenResponse.accessToken, 
                                //we ensure that it didn't expires before...
                                TimeSpan.FromSeconds(tokenResponse.expiresIn!.Value! - 60),  
                                CacheCategory);

            return tokenResponse.accessToken!;
        }

        private async Task<(string? accessToken, int? expiresIn)> RequestNewTokenAsync()
        {
            string? accessToken = null;
            int? expiresIn = null;

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(applicationConfiguration.PrivateKey), out _);
            var securityKey = new RsaSecurityKey(rsa)
            {
                KeyId = applicationConfiguration.KeyId
            };
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload
            {
                { "iss", applicationConfiguration.UserId },
                { "sub", applicationConfiguration.UserId },
                { "aud", applicationConfiguration.ZitadelDomain },
                { "iat", DateTimeOffset.Now.ToUnixTimeSeconds() },
                { "exp", DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds() }
            };
            var jwtToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.WriteToken(jwtToken);
            var request = new RestRequest(applicationConfiguration.ZitadelTokenUrl, Method.Post);
            request.Timeout = 10000000;
            request.AddParameter("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer", ParameterType.GetOrPost);
            request.AddParameter("scope", $"openid profile email urn:zitadel:iam:user:resourceowner urn:zitadel:iam:org:projects:roles urn:zitadel:iam:org:project:id:{applicationConfiguration.ProjectId}:aud", ParameterType.GetOrPost);
            request.AddParameter("assertion", encodedJwt, ParameterType.GetOrPost);
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                accessToken = JsonDocument.Parse(response.Content!)?
                                                   .RootElement
                                                   .GetProperty("access_token")
                                                   .GetString();

                expiresIn = JsonDocument.Parse(response.Content!)?
                                                   .RootElement
                                                   .GetProperty("expires_in")
                                                   .GetInt32();
            }
            else
            {
                throw new Exception($"Error while requesting token: {response.StatusCode} - {response.Content}");
            }

            return (accessToken, expiresIn);
        }
    }

}


  Peux tu me donner une version qui supporte la concurrence des thread ?
