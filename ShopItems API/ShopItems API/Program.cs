using DataLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShopItems_API.DiContainer;
using System.Text;

namespace ShopItems_API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddAuthenticationServices(builder).AddDependencies();

            var app = builder.Build();

            app.MapControllers();
            app.MapGet("/", () => "Shop Items API\n\n\"/api/item\" - to see all of the items\n\"/login\" - to authorize");

            app.Run();
        }
    }
}
