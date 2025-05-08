
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Orders.Frontend.AuthenticationProviders;
using Orders.Shared.Entities;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Orders.Frontend.Services
{
    public class LoginHandler : DelegatingHandler
    {
        private readonly AuthenticationProviderJWT _authenticationProviderJWT;
        private NavigationManager _NavigationManager { get; set; } = null!;
        public LoginHandler(AuthenticationProviderJWT authenticationProviderJWT)
        {
            _authenticationProviderJWT = authenticationProviderJWT;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
                var token = await _authenticationProviderJWT.GetTokenAsync();

                if (token is not null)
                {
                    //var cliams = _authenticationProviderJWT.ParseClaimsFromJWT(token);
                    //foreach (var cliam in cliams)
                    //{
                    //    if (cliam.Type == "exp")
                    //    {
                    //        var expirationDateTime = DateTime.UnixEpoch.AddSeconds(-1 * double.Parse(cliam.Value));

                    //        if (expirationDateTime < DateTime.UtcNow)
                    //        {
                    //            await _authenticationProviderJWT.LogoutAsync();
                    //            _NavigationManager.NavigateTo($"/login");
                    //            return new HttpResponseMessage();
                    //        }
                    //    }
                    //}

                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.ToString());
                }
         
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
