using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using SGA.Web.Models;
using SGA.Web.Services;
using Xunit;

namespace SGA.Web.Tests.Services;

public sealed class SgaApiClientTests
{
    [Fact]
    public async Task CreateInstitutionAsync_WhenApiReturnsCreated_ReturnsId()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("123", Encoding.UTF8, "application/json")
            });

        var client = new SgaApiClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7200/")
        }, NullLogger<SgaApiClient>.Instance);

        var id = await client.CreateInstitutionAsync(new CreateInstitutionRequest("INST001", "Inst", "tester"), CancellationToken.None);

        Assert.Equal(123, id);
    }

    [Fact]
    public async Task CreateInstitutionAsync_WhenApiReturnsError_ThrowsWithStatusCode()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("invalid payload")
            });

        var client = new SgaApiClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7200/")
        }, NullLogger<SgaApiClient>.Instance);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.CreateInstitutionAsync(new CreateInstitutionRequest("", "Inst", "tester"), CancellationToken.None));

        Assert.Contains("API error 400", ex.Message);
    }

    [Fact]
    public async Task PortalLoginAsync_WhenApiReturnsUnauthorized_ThrowsWithStatusCode()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("unauthorized")
            });

        var client = new SgaApiClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7200/")
        }, NullLogger<SgaApiClient>.Instance);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.PortalLoginAsync(new PortalLoginRequest("admin@demo.com"), CancellationToken.None));

        Assert.Contains("API error 401", ex.Message);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

        public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseFactory(request));
        }
    }
}
