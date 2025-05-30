﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Orders.Frontend.Helpers;
using Orders.Frontend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Orders.Frontend.AuthenticationProviders
{
    public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IHttpClientFactory _httpClient;
        //private readonly HttpClient _httpClient;
        private readonly string _tokenKey;
        private readonly AuthenticationState _anonimous;

        public AuthenticationProviderJWT(IJSRuntime jSRuntime, IHttpClientFactory httpClient)
        {
            _jSRuntime = jSRuntime;
            _httpClient = httpClient;
            _tokenKey = "TOKEN_KEY";
            _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jSRuntime.GetLocalStorage(_tokenKey);
            if (token is null)
            {
                return _anonimous;
            }
            return BuildAuthenticationState(token.ToString()!);
        }


        public async Task<object> GetTokenAsync()
        {
            var token = await _jSRuntime.GetLocalStorage(_tokenKey);
            if( token is null)  return token!;
            
            return token.ToString()!;
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParseClaimsFromJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public IEnumerable<Claim> ParseClaimsFromJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var unserializedToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            return unserializedToken.Claims;
        }

        public async Task LoginAsync(string token)
        {
            await _jSRuntime.SetLocalStorage(_tokenKey, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task LogoutAsync()
        {
            await _jSRuntime.RemoveLocalStorage(_tokenKey);
            var cliente = _httpClient.CreateClient("BackEnd");
            cliente.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));
        }

    }
}
