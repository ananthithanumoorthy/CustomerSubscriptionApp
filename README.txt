# AS.UG.Portal.Web

Simple ASP.NET Core web app for Customer Subscription test project.

## Features
- Login with roles (Admin/User) using cookie auth
- Password reset (token via IDataProtector; dev email prints to console)
- Customer Subscription page (view/filter/add/update/delete)
- APIs for all subscription operations and count updates
- SQLite by default; switch to SQL Server by changing connection string

## Run locally
1. Ensure .NET 7 SDK installed.
2. From project folder:
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet run
3. Open `https://localhost:5001` and login:
   admin@local.test / Admin@123

## Notes
- Admin role required for create/update/delete subscription and count updates.
- For production, replace ConsoleEmailSender with real SMTP/email provider and secure data protection keys.

