# LFC Website — OpenCode Instructions

## Build & run
This is **legacy .NET Framework 4.5** (ASP.NET MVC 5), NOT .NET Core.
- `dotnet` CLI will not work. Use MSBuild or Visual Studio.
- Solution file: `LFC.sln`. Single project: `LFC\LFC.csproj`.
- Configurations: Debug, Production, Release.

### Linux build (Mono)
- Requires: `sudo pacman -S mono-msbuild mono-msbuild-sdkresolver lftp`
- NuGet restore: `mono /tmp/nuget.exe restore LFC.sln` (nuget.exe at `/tmp/nuget.exe`)
- Build: `msbuild LFC.sln /p:Configuration=Production`
- Deploy to FTP server: `./deploy.sh` (set `FTP_PASSWORD` env var first)
- Packages folder: `../packages` relative to `LFC/\` (already restored)

## Architecture
- Entry points: `Global.asax.cs`, `Startup.cs`, `App_Start\RouteConfig.cs`
- DAL: `LFC\DAL\LFCContext.cs` (EF6 Code First)
- Controllers: `LFC\Controllers\`
- Models: `LFC\Models\`
- Views: `LFC\Views\`
- Migrations: `LFC\Migrations\` (many historical migrations)

## Constraints
- `Web.config` contains hardcoded production DB credentials and SendGrid SMTP password. **Never commit these files to any repo.**
- No automated test framework exists.
- NuGet uses old-style `packages.config` (not PackageReference).
