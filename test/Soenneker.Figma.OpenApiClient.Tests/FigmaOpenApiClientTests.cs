using Soenneker.Tests.HostedUnit;

namespace Soenneker.Figma.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class FigmaOpenApiClientTests : HostedUnitTest
{
    public FigmaOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
