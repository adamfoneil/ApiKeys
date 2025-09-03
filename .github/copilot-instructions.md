# API Key Management System

The API Key Management System is a .NET 8.0 Blazor Server application that provides secure API key generation, validation, and management capabilities. The system consists of a core service library and a web application with both UI and REST API endpoints.

**ALWAYS reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Bootstrap and Build
- Bootstrap the repository:
  - `cd /path/to/repository`
  - `dotnet restore` -- takes 5-10 seconds for NuGet package downloads. NEVER CANCEL. Set timeout to 30+ seconds.
  - `dotnet build` -- takes 2-20 seconds depending on cache state. NEVER CANCEL. Set timeout to 60+ seconds.
- **CRITICAL**: Always use adequate timeouts for build commands. Initial builds can take up to 20 seconds.

### Run the Application
- **ALWAYS run the bootstrapping steps first** before attempting to run the application.
- Development server: `cd ApiKeys.BlazorApp && dotnet run`
- The application starts on `http://localhost:5000` by default
- Database (SQLite) is automatically created on first run in the project directory
- **Startup time**: 3-5 seconds. NEVER CANCEL startup processes.

### Test the Application
- **No unit tests exist** - this is a demonstration application
- Manual testing is required for validation
- Build validation: `dotnet build` (must succeed with at most warnings about Radzen package versions)

## Validation

### Manual Testing Requirements
After making ANY changes to the codebase, **ALWAYS perform these validation steps**:

1. **Build Validation**: 
   - Run `dotnet build` and ensure it completes successfully
   - Expect 2 warnings about Radzen.Blazor package versions (this is normal)
   - Build should complete in 2-20 seconds

2. **Application Startup Test**:
   - Navigate to `ApiKeys.BlazorApp` directory
   - Run `dotnet run`
   - Wait for "Now listening on: http://localhost:5000" message
   - Application should start within 5 seconds

3. **API Endpoint Validation**:
   - Test public endpoint: `curl http://localhost:5000/api/demo/public`
   - Should return JSON with message "This is public data - no API key required"
   - Test secure endpoint without key: `curl http://localhost:5000/api/demo/secure`
   - Should return HTTP 500 (authorization failure)

4. **Web UI Validation**:
   - Access home page: `curl http://localhost:5000/`
   - Should return HTML starting with `<!DOCTYPE html>`
   - Access API keys page: `curl http://localhost:5000/apikeys`
   - Should return HTML with API key management interface

5. **Database Creation Test**:
   - Remove any existing `apikeys.db*` files
   - Start the application
   - Verify SQLite database files are created automatically
   - Check logs for Entity Framework database creation messages

### NEVER CANCEL Operations
- **Build operations**: Can take 20+ seconds on first run. NEVER CANCEL.
- **NuGet restore**: Can take 10+ seconds. NEVER CANCEL.
- **Application startup**: Takes 3-5 seconds. NEVER CANCEL.
- **Database creation**: Automatic on startup, takes 1-2 seconds. NEVER CANCEL.

## Architecture Overview

### Projects Structure
- **ApiKeys.Service**: Core library containing API key management logic
  - `ApiKeyManager.cs`: Abstract base class for API key operations
  - `SqliteApiKeyManager.cs`: SQLite-specific implementation
  - `ApiKeyAuthorizationHandler.cs`: ASP.NET Core authorization handler
- **ApiKeys.BlazorApp**: Blazor Server web application
  - `Program.cs`: Application configuration and startup
  - `Components/`: Blazor components for UI
  - `Controllers/`: REST API controllers
  - `wwwroot/`: Static web assets

### Key Dependencies
- **.NET 8.0**: Target framework
- **Entity Framework Core 8.0.8**: Data access
- **SQLite**: Database provider
- **Radzen Blazor 5.6.0**: UI component library
- **ASP.NET Core**: Web framework

### Database
- **SQLite database** automatically created on application startup
- Database file: `apikeys.db` (with associated `-shm` and `-wal` files)
- **Schema**: Single `ApiKeys` table with Id, Name, Hash, CreatedAt columns
- **IMPORTANT**: Database files should NOT be committed to source control

## Common Tasks

### Adding New Features
1. **Always build first**: `dotnet build` to ensure current state is valid
2. **Make minimal changes**: Focus on single feature at a time
3. **Test immediately**: Run validation steps after each change
4. **Database migrations**: Not implemented - schema changes require recreating database

### Troubleshooting Build Issues
- **Package restore errors**: Run `dotnet restore` in repository root
- **Build failures**: Check .NET 8.0 SDK is installed with `dotnet --version`
- **Radzen warnings**: Normal - package manager resolves to compatible version
- **Missing database**: Application creates it automatically on startup

### Adding API Endpoints
1. Add new controller method in `ApiKeys.BlazorApp/Controllers/`
2. Use `[Authorize(Policy = "ApiKey")]` for secured endpoints
3. Test with `curl` after starting application
4. Public endpoints need no authorization attribute

### Modifying Blazor UI
1. Edit `.razor` files in `ApiKeys.BlazorApp/Components/`
2. Use Radzen components for consistency
3. Add `@rendermode InteractiveServer` for stateful components
4. Test by accessing pages in browser

## File Locations

### Frequently Modified Files
- `ApiKeys.Service/SqliteApiKeyManager.cs`: Core API key management logic
- `ApiKeys.BlazorApp/Program.cs`: Application configuration
- `ApiKeys.BlazorApp/Components/Pages/ApiKeys.razor`: Main UI page
- `ApiKeys.BlazorApp/Controllers/DemoController.cs`: API endpoints

### Configuration Files
- `ApiKeys.BlazorApp/appsettings.json`: Application settings
- `ApiKeys.BlazorApp/appsettings.Development.json`: Development overrides
- `ApiKeys.sln`: Solution file for both projects

### Build Artifacts (DO NOT MODIFY)
- `bin/`, `obj/`: Build output directories
- `apikeys.db*`: SQLite database files (auto-generated)

## Expected Command Outputs

### Successful Build
```
dotnet build
Build succeeded.
    2 Warning(s)
    0 Error(s)
Time Elapsed 00:00:02.03
```

### Application Startup
```
dotnet run
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### API Test (Public Endpoint)
```
curl http://localhost:5000/api/demo/public
{"message":"This is public data - no API key required","timestamp":"2025-09-03T11:15:53.5330287Z","data":["Item 1","Item 2","Item 3"]}
```

## Critical Reminders

- **ALWAYS test your changes** using the manual validation steps
- **NEVER commit database files** (they are auto-generated)
- **Set appropriate timeouts** (60+ seconds) for build operations
- **Use minimal changes** - modify only what's necessary for the task
- **Database is auto-created** - no manual setup required
- **Default port is 5000** - application binds to http://localhost:5000