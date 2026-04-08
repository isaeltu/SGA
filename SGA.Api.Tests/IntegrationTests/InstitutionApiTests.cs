using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SGA.Api.Tests.IntegrationTests;

public sealed class InstitutionApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public InstitutionApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateInstitution_WithValidData_ReturnsCreatedAndId()
    {
        var dto = new
        {
            Code = "INST001",
            Name = "Test Institution",
            CreatedBy = "test-user"
        };

        var response = await _client.PostAsJsonAsync("/api/institutions", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var id = await response.Content.ReadFromJsonAsync<int>();
        Assert.NotNull(id);
        Assert.True(id > 0);
    }

    [Fact]
    public async Task GetInstitution_WithValidId_ReturnsInstitutionData()
    {
        var createDto = new
        {
            Code = "INST002",
            Name = "Another Institution",
            CreatedBy = "test-user"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/institutions", createDto);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content.ReadFromJsonAsync<int>();

        var getResponse = await _client.GetAsync($"/api/institutions/{id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var institution = await getResponse.Content.ReadFromJsonAsync<InstitutionDto>();
        Assert.NotNull(institution);
        Assert.Equal("INST002", institution!.Code);
        Assert.Equal("Another Institution", institution.Name);
    }

    [Fact]
    public async Task GetInstitution_WithInvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/institutions/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private sealed record InstitutionDto(int Id, string Code, string Name, bool IsActive);
}
