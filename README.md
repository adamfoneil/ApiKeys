This is a small demo of how to manage keys for an API.
- secure storage via abstract class [ApiKeyManager.cs](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.Service/ApiKeyManager.cs). Implemented in this demo as [SqlLiteApiKeyManager.cs](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.Service/SqliteApiKeyManager.cs)
- an authorization handler [ApiKeyAuthorizationHandler](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.Service/ApiKeyAuthorizationHandler.cs)
- a minimal Blazor example for creating and deleting keys: [ApiKeys.razor](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.BlazorApp/Components/Pages/ApiKeys.razor)
- sample controller using public and secure endpoints: [DemoController.cs](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.BlazorApp/Controllers/DemoController.cs)
- a minimal testing UI [ApiDemo.razor](https://github.com/adamfoneil/ApiKeys/blob/master/ApiKeys.BlazorApp/Components/Pages/ApiDemo.razor)
