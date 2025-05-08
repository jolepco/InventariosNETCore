using Microsoft.AspNetCore.Identity;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitOfWork.Implementations;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;
using System.Text.Json.Serialization;

namespace Orders.Backend.Data
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {


            services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

            services.AddScoped<IStatesRepository, StatesRepository>();
            services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();

            services.AddScoped<ICitiesRepository, CitiesRepository>();
            services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();

            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICategoriesUnitOfWork, CategoriesUnitOfWork>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

            //agregar la inyeccion para cargar archivos
            services.AddScoped<IFileStorage, FileStorage>();

            //servicio de correo
            services.AddScoped<IMailHelper, MailHelper>();

            //Para evitar la redundancia ciclica en la respuesta de los JSON vamos a agregar la siguiente configuración
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            

            //agrega data a la base de datos
            services.AddTransient<SeedDb>();
            //configuracion de contraseña

            services.AddIdentity<User, IdentityRole>(x =>
            {
                //validar correo
                x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                x.SignIn.RequireConfirmedEmail = true;
                // anterior proceso
                x.User.RequireUniqueEmail = true;
                x.Password.RequireDigit = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
                //seguridad al sistema
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                x.Lockout.MaxFailedAccessAttempts = 3;
                x.Lockout.AllowedForNewUsers = true;

            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();



            return services;
        }
    }
}
