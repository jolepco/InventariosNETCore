using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Orders.Frontend;
using Orders.Frontend.AuthenticationProviders;
using Orders.Frontend.Helpers;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Polly;
using Polly.Extensions.Http;
using System.Reflection;
using System.Security.Authentication;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var policy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .RetryAsync(10, (ex, count) =>
    {
        Console.WriteLine($"Polly: error during call, retry n. {count}");
    });

string cadena = $"{builder.Configuration["config:url"]}";

builder.Services.AddScoped<LoginHandler>();

builder.Services.AddHttpClient("BackEnd", client =>
{
    client.BaseAddress = new Uri(cadena);
})
    .AddHttpMessageHandler<LoginHandler>()
    .AddPolicyHandler(policy);


builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();

builder.Services.AddBlazoredModal();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

await builder.Build().RunAsync();
