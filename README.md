# DatingApp

A full-stack dating web application. The backend is an ASP.NET Core 8 Web API; the
frontend is an Angular 21 single-page app. Users can register, set up a profile with
photos, browse and filter other members, like them, and exchange messages in real time.

This started as my build of Neil Cummings' course "Build an app with ASP.NET Core and
Angular from scratch", which I then extended and customized.

## Features

- Authentication and authorization using ASP.NET Core Identity with JWT bearer tokens and roles (Member, Admin)
- Member profiles with multiple photo uploads
- Photo hosting on Cloudinary, including selecting a main photo
- Member browsing with age/gender/activity filters, sorting, and server-side pagination
- Likes, with "liked" and "liked by" lists
- Private messaging and online-presence tracking over SignalR
- Admin area for role assignment and photo moderation

## Tech stack

Backend (`/API`)
- ASP.NET Core 8 Web API
- Entity Framework Core 8 with SQL Server
- ASP.NET Core Identity, JWT bearer authentication
- SignalR
- CloudinaryDotNet

Frontend (`/client`)
- Angular 21 (standalone components, SSR-ready)
- Tailwind CSS 4 and DaisyUI
- RxJS, @microsoft/signalr

Infrastructure
- SQL Server via Docker Compose
- GitHub Actions deploying to Azure App Service

## Getting started

Prerequisites:
- .NET 8 SDK
- Node.js 22+ and the Angular CLI (`npm i -g @angular/cli`)
- Docker, for SQL Server
- A Cloudinary account, for photo uploads

1. Start the database:
   ```bash
   docker compose up -d
   ```

2. Configure the API. Copy the example settings and fill in your own values:
   ```bash
   cp API/appsettings.Development.json.example API/appsettings.Development.json
   ```
   Set the database password and a long random `TokenKey` in
   `API/appsettings.Development.json`, and add your Cloudinary credentials to
   `API/appsettings.json`. `appsettings.Development.json` is gitignored, so real
   secrets stay out of version control.

3. Run the API:
   ```bash
   cd API
   dotnet run
   ```
   Migrations are applied and sample data is seeded on startup.

4. Run the client:
   ```bash
   cd client
   npm install
   ng serve
   ```
   The app is available at http://localhost:4200.

## License

[MIT](LICENSE)
