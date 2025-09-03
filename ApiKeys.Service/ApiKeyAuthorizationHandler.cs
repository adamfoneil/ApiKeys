using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ApiKeys.Service;

public class ApiKeyRequirement : IAuthorizationRequirement { }

public class ApiKeyAuthorizationHandler(ApiKeyManager apiKeyManager, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<ApiKeyRequirement>
{
	private readonly ApiKeyManager _manager = apiKeyManager;
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		ApiKeyRequirement requirement)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null) return;

		if (httpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey) && !string.IsNullOrWhiteSpace(apiKey))
		{
			if (await _manager.ValidateAsync(apiKey!))
			{
				context.Succeed(requirement);
			}
		}
	}
}
