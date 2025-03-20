using ShopItems_API.Models;

namespace ShopItems_API.Data
{
    public static class Users
    {
        public static List<User> GetUsers()
        {
            return new List<User>()
            {
                new User() { UserName = "Someone", Password = "passw", Role = "User" },
                new User() { UserName = "Honcharova", Password = "adminPassw", Role = "Admin" }
            };
        }
    }
}
