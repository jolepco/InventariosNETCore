
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Blazored.Modal.Services;
using Orders.Frontend.Pages.Auth;

namespace Orders.Frontend.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;
        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            var claims = authenticationState.User.Claims.ToList();
            var photoClaim = claims.FirstOrDefault(x => x.Type == "Photo");
            if (photoClaim is not null)
            {
                photoUser = photoClaim.Value;
            }
        }
        private void ShowModal()
        {
            Modal.Show<Login>();
        }

    }
}

