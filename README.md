[![](https://img.shields.io/nuget/v/soenneker.figma.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.figma.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.figma.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.figma.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.figma.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.figma.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.figma.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.figma.openapiclient/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Figma.OpenApiClient
A Kiota-generated .NET client for Figma's REST API.

## Installation

```bash
dotnet add package Soenneker.Figma.OpenApiClient
```

## Recommended setup

For dependency injection, authenticated transport, and client reuse, install the companion utility:

```bash
dotnet add package Soenneker.Figma.OpenApiClientUtil
```

```csharp
using Soenneker.Figma.OpenApiClientUtil.Registrars;

services.AddFigmaOpenApiClientUtilAsScoped();
```

Configure `Figma:ApiKey`, inject `IFigmaOpenApiClientUtil`, and call `Get()` inside the scope. The utility is scoped, while its HTTP client dependency remains singleton so disposing one utility scope does not discard the long-lived transport.

## Direct construction

```csharp
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Figma.OpenApiClient;
using Soenneker.Figma.OpenApiClient.Models;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.figma.com")
};
httpClient.DefaultRequestHeaders.Add("X-Figma-Token", figmaToken);

var adapter = new HttpClientRequestAdapter(
    new AnonymousAuthenticationProvider(),
    httpClient: httpClient);
var figma = new FigmaOpenApiClient(adapter);

GetFileResponseResponse? file = await figma.V1.Files[fileKey].GetAsync(
    cancellationToken: cancellationToken);
```

`AnonymousAuthenticationProvider` is appropriate here because the dedicated `HttpClient` already carries Figma's authentication header. Do not put a Figma token on a shared client that can send default headers to unrelated hosts.

## Navigating the client

The request builders mirror Figma's URL hierarchy. API v1 endpoints are under `figma.V1`; v2 endpoints such as webhooks are under `figma.V2`. Item request builders use indexers for path parameters, as in `figma.V1.Files[fileKey]`.

Endpoint methods accept a request-configuration callback for query parameters, headers, and Kiota middleware options, followed by a cancellation token. Response values may be nullable when the OpenAPI description permits an empty response.

## Generated-code boundaries

Public request-builder names and model shapes follow the source OpenAPI description and can change when the client is regenerated. Kiota maps documented service errors to generated error models; transport, authentication, and serialization failures surface as HTTP or Kiota exceptions.

Files under `src/Soenneker.Figma.OpenApiClient` are generated. Keep application-specific behavior in a separate project or the companion utility rather than editing generated files.
