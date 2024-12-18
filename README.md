# ShopItems-API

### About project
You can't make CRUD operations until you Authorithed. Only admin have access to POST, PUT and DELETE operations. 
Even if you simply want to GET information, you still have to be athorized, but you don't have to be an admin for that. 

### How it works with Postman
![video](./Resources/postman_API.gif)
### NuGet Packages
For JWT Bearer (Token Autherization):
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
- System.IdentityModel.Tokens.Jwt</br>

For MySQL Database:
- MySql.Data
- Dapper

In case you interested in data access realisation with help of EntityFramework, I have [REST API project with usage of this package](https://github.com/teafr/Books-API) 

### Additional technical Information
This is ASP.NET Core Empty project. 
You could guess that in this project I used Database to store all items and manipulate with them. I used my own Library, which I often connect to my projects. 
Class Libraries are really beneficial, because you can create it once and reuse it for different projects even if they have different project type.
