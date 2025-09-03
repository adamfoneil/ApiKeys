# API Keys Demo Application

**ALWAYS reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

API Keys Demo is a .NET 8.0 ASP.NET Core Blazor Server application that demonstrates secure API key management. It includes key generation, storage, and authentication mechanisms with a SQLite database backend.

## Working Effectively

### Bootstrap, Build, and Run
- Ensure .NET 8.0 SDK is installed: `dotnet --version` (requires 8.0.x)
- Restore dependencies: `dotnet restore` -- takes 15 seconds. NEVER CANCEL. Set timeout to 30+ minutes.
- Build solution: `dotnet build` -- takes 15 seconds. NEVER CANCEL. Set timeout to 30+ minutes.
- Run application: `cd ApiKeys.BlazorApp && dotnet run` -- starts in 10-15 seconds. Application runs on http://localhost:5292

### Known Build Limitations
- **No test projects exist** - this repository has no unit or integration tests
- **Authentication configuration issue** - secure API endpoints fail with "No authenticationScheme was specified" error
- Build succeeds completely with no warnings or errors

## Application Structure

### Key Projects
- **ApiKeys.Service** - Core library containing API key management logic and authorization handlers
- **ApiKeys.BlazorApp** - Blazor Server web application with UI and API controllers

### Important Files and Locations
- **Core Logic**: `ApiKeys.Service/ApiKeyManager.cs` (abstract base), `SqliteApiKeyManager.cs` (SQLite implementation)
- **Security**: `ApiKeys.Service/ApiKeyAuthorizationHandler.cs` (authorization policy implementation)
- **Web Application**: `ApiKeys.BlazorApp/Program.cs` (startup configuration)
- **API Controller**: `ApiKeys.BlazorApp/Controllers/DemoController.cs` (public and secure endpoints)
- **UI Pages**: 
  - `ApiKeys.BlazorApp/Components/Pages/ApiKeys.razor` (key management interface)
  - `ApiKeys.BlazorApp/Components/Pages/ApiDemo.razor` (API testing interface)
- **Database**: `ApiKeys.BlazorApp/apikeys.db` (auto-created SQLite database)

## Validation and Testing

### ALWAYS run these validation scenarios after making changes:

#### End-to-End API Key Management Flow
1. Navigate to http://localhost:5292/apikeys
2. Enter a key name in "Key Name" field
3. Click "Generate API Key" button
4. Verify key appears in green success alert with Base64 format
5. Verify key appears in "Existing API Keys" table
6. Test key deletion by clicking "Delete" button

#### API Endpoint Testing
1. Navigate to http://localhost:5292/api-demo
2. Click "Test Public Endpoint" - should return JSON with "This is public data" message
3. **Known Issue**: Click "Test Secure Endpoint" without API key - will show authentication error (this is expected)
4. **Authentication not fully configured** - secure endpoints fail due to missing default authentication scheme

#### Manual API Testing via curl
- Public endpoint: `curl http://localhost:5292/api/demo/public` - should return 200 OK with JSON
- Secure endpoint: `curl http://localhost:5292/api/demo/secure` - will return 500 error due to auth config issue

### Database Validation
- Database auto-creates as `ApiKeys.BlazorApp/apikeys.db` on first run
- Uses Entity Framework Core with SQLite provider
- Schema includes ApiKeys table with Id, Name (unique), Hash, and CreatedAt columns

## Development Workflow

### Making Changes
- **Always build before and after changes**: `dotnet build`
- **Always run application to test changes**: `cd ApiKeys.BlazorApp && dotnet run`
- **Always test the full API key generation flow** as outlined in validation scenarios
- **No linting or code formatting tools configured** - follow existing code style
- **No CI/CD pipelines exist** - manual testing required

### Common Tasks

#### Adding new API endpoints
1. Add methods to `ApiKeys.BlazorApp/Controllers/DemoController.cs`
2. Use `[Authorize(Policy = "ApiKey")]` attribute for secured endpoints
3. Test via the API Demo page at /api-demo

#### Modifying API key logic
1. Edit abstract methods in `ApiKeys.Service/ApiKeyManager.cs`
2. Implement in `ApiKeys.Service/SqliteApiKeyManager.cs`
3. Test via the API Keys management page at /apikeys

#### UI Changes
1. Edit Razor components in `ApiKeys.BlazorApp/Components/Pages/`
2. Uses Radzen UI component library - reference existing patterns
3. Always test in browser after changes

## Configuration Details

### Application Settings
- **Connection String**: Uses SQLite with "Data Source=apikeys.db"
- **Development Environment**: Logging level set to Information
- **Default Port**: 5292 (HTTP)

### Dependencies
- Microsoft.EntityFrameworkCore.Sqlite (9.0.8)
- Radzen.Blazor (7.3.3) - UI component library
- Microsoft.EntityFrameworkCore.Design (9.0.8)

## Troubleshooting

### Known Issues
- **Secure API Authentication**: Returns "No authenticationScheme was specified" error - this is a configuration limitation
- **No HTTPS redirect in development** - application runs on HTTP only
- **No test coverage** - manual testing required for all changes

### Common Problems
- **Database locked errors**: Stop the application before deleting apikeys.db file
- **Port conflicts**: Application uses port 5292 - ensure it's available
- **Build failures**: Run `dotnet restore` before `dotnet build`

## Repository Structure (ls -la output)
```
ApiKeys.BlazorApp/          # Web application project
ApiKeys.Service/            # Core service library  
ApiKeys.sln                 # Solution file
README.md                   # Project documentation
.gitignore                  # Git ignore rules
```

Application successfully tested and validated. Screenshot of working application available at: https://github.com/user-attachments/assets/18300459-2fbf-4fca-8f45-9489acc791d5