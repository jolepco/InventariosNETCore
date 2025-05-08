using Microsoft.EntityFrameworkCore;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesFullAsync();
            // await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Jose", "Puerto", "jolepco@yopgmail.com", "3156680751", "Calle Luna Calle Sol", UserType.SuperAdmin);

        }


        private async Task CheckCountriesFullAsync()
        {
            if (!_context.Countries.Any())
            {
                var countriesStatesCitiesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesStatesCitiesSQLScript);
            }
        }


        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.SuperAdmin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Moniquirá");
                city ??= await _context.Cities.FirstOrDefaultAsync();


                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    //City = _context.Cities.FirstOrDefault(),
                    CityId = city.Id, 
                    UserType = userType,
                    
                };

                try
                {
                    await _usersUnitOfWork.AddUserAsync(user, "123456");
                    await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                    var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                    await _usersUnitOfWork.ConfirmEmailAsync(user, token);

                }
                catch (Exception ex)
                {

                }
              
            }

            return user;
        }

        private async Task CheckCountriesAsync()
        {
            await Task.Delay(1000);
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    Code = "169",
                    ListStates = new List<State>()
            {
                new State()
                {
                    Name = "Antioquia",
                    Code="777",
                    ListCities = new List<City>() {
                        new() { Name = "Medellín" , Code="123"},
                        new() { Name = "Itagüí" , Code="124"},
                        new() { Name = "Envigado", Code="125" },
                        new() { Name = "Bello" , Code="126"},
                        new() { Name = "Rionegro" , Code="127"},
                    }
                },
                new State()
                {
                    Name = "Bogotá",
                    Code ="145",
                    ListCities = new List<City>() {
                        new City() { Name = "Usaquen" , Code="112" },
                        new City() { Name = "Champinero" , Code="123"},
                        new City() { Name = "Santa fe" , Code="125" },
                        new City() { Name = "Useme" , Code="188"},
                        new City() { Name = "Bosa" , Code="256"},
                    }
                },
            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    Code = "001",
                    ListStates = new List<State>()
            {
                new State()
                {
                    Name = "Florida",
                    Code="111",
                    ListCities = new List<City>() {
                        new City() { Name = "Orlando" , Code="325" },
                        new City() { Name = "Miami" , Code="378"},
                        new City() { Name = "Tampa" , Code="821"},
                        new City() { Name = "Fort Lauderdale" , Code="987" },
                        new City() { Name = "Key West" , Code="658" },
                    }
                },
                new State()
                {
                    Name = "Texas",
                    Code ="888",
                    ListCities = new List<City>() {
                        new City() { Name = "Houston" , Code="912" },
                        new City() { Name = "San Antonio" , Code="913" },
                        new City() { Name = "Dallas" , Code="914"},
                        new City() { Name = "Austin" , Code="913" },
                        new City() { Name = "El Paso" , Code = "918"},
                    }
                },
            }
                });
            }

        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Calzado", Code = "CAL" });
                _context.Categories.Add(new Category { Name = "Tecnología", Code = "TEC" });
                _context.Categories.Add(new Category { Name = "Apple", Code = "APL" });
                _context.Categories.Add(new Category { Name = "Ciencia", Code = "CIE" });
                _context.Categories.Add(new Category { Name = "Hardware", Code = "HAR" });
                _context.Categories.Add(new Category { Name = "Software", Code = "SOF" });
                _context.Categories.Add(new Category { Name = "Programacion", Code = "PRO" });
                _context.Categories.Add(new Category { Name = "Televisión", Code = "TEL" });
                _context.Categories.Add(new Category { Name = "Consmeticos", Code = "COS" });
                _context.Categories.Add(new Category { Name = "Medicina", Code = "MED" });
                _context.Categories.Add(new Category { Name = "Filosofía", Code = "FIL" });
                _context.Categories.Add(new Category { Name = "OSIO", Code = "OSI" });
            }

            await _context.SaveChangesAsync();
        }



    }

}
