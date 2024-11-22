using System.Text.Json;

namespace IntegrationTests;

public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHelloWorld()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", content);
    }

    [Fact]
    public async Task Get_DaysBetweenDates()
    {
        var response = await _client.GetAsync("/DaysBetweenDates?date1=2023-01-01&date2=2023-01-10");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("9", content);
    }

    [Fact]
    public async Task Get_ValidatePhoneNumber()
    {
        var response = await _client.GetAsync("/validatephonenumber?phoneNumber=%2B1234567890");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("true", content);
    }

    [Fact]
    public async Task Get_ValidateSpanishDNI()
    {
        var response = await _client.GetAsync("/validatespanishdni?dni=12345678Z");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"valid\"", content);
    }

    [Fact]
    public async Task Get_ReturnColorCode()
    {
        var response = await _client.GetAsync("/returncolorcode?color=red");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"#FF0000\"", content);
    }

    [Fact]
    public async Task Get_TellMeAJoke()
    {
        var response = await _client.GetAsync("/tellmeajoke");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Get_MoviesByTitle()
    {
        var response = await _client.GetAsync("/moviesbytitle?title=Inception");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Get_ParseURL()
    {
        var response = await _client.GetAsync("/parseurl?someurl=https%3A%2F%2Fwww.example.com");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"host\":\"www.example.com\"", content);
    }

    [Fact]
    public async Task Get_ListFiles()
    {
        var response = await _client.GetAsync("/listfiles");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Get_CalculateMemoryConsumption()
    {
        var response = await _client.GetAsync("/calculatememoryconsumption");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Get_RandomEuropeanCountry()
    {
        var countries = new[]
        {
            new { Name = "Albania", ISOCode = "AL" },
            new { Name = "Andorra", ISOCode = "AD" },
            new { Name = "Armenia", ISOCode = "AM" },
            new { Name = "Austria", ISOCode = "AT" },
            new { Name = "Azerbaijan", ISOCode = "AZ" },
            new { Name = "Belarus", ISOCode = "BY" },
            new { Name = "Belgium", ISOCode = "BE" },
            new { Name = "Bosnia and Herzegovina", ISOCode = "BA" },
            new { Name = "Bulgaria", ISOCode = "BG" },
            new { Name = "Croatia", ISOCode = "HR" },
            new { Name = "Cyprus", ISOCode = "CY" },
            new { Name = "Czech Republic", ISOCode = "CZ" },
            new { Name = "Denmark", ISOCode = "DK" },
            new { Name = "Estonia", ISOCode = "EE" },
            new { Name = "Finland", ISOCode = "FI" },
            new { Name = "France", ISOCode = "FR" },
            new { Name = "Georgia", ISOCode = "GE" },
            new { Name = "Germany", ISOCode = "DE" },
            new { Name = "Greece", ISOCode = "GR" },
            new { Name = "Hungary", ISOCode = "HU" },
            new { Name = "Iceland", ISOCode = "IS" },
            new { Name = "Ireland", ISOCode = "IE" },
            new { Name = "Italy", ISOCode = "IT" },
            new { Name = "Kazakhstan", ISOCode = "KZ" },
            new { Name = "Kosovo", ISOCode = "XK" },
            new { Name = "Latvia", ISOCode = "LV" },
            new { Name = "Liechtenstein", ISOCode = "LI" },
            new { Name = "Lithuania", ISOCode = "LT" },
            new { Name = "Luxembourg", ISOCode = "LU" },
            new { Name = "Malta", ISOCode = "MT" },
            new { Name = "Moldova", ISOCode = "MD" },
            new { Name = "Monaco", ISOCode = "MC" },
            new { Name = "Montenegro", ISOCode = "ME" },
            new { Name = "Netherlands", ISOCode = "NL" },
            new { Name = "North Macedonia", ISOCode = "MK" },
            new { Name = "Norway", ISOCode = "NO" },
            new { Name = "Poland", ISOCode = "PL" },
            new { Name = "Portugal", ISOCode = "PT" },
            new { Name = "Romania", ISOCode = "RO" },
            new { Name = "Russia", ISOCode = "RU" },
            new { Name = "San Marino", ISOCode = "SM" },
            new { Name = "Serbia", ISOCode = "RS" },
            new { Name = "Slovakia", ISOCode = "SK" },
            new { Name = "Slovenia", ISOCode = "SI" },
            new { Name = "Spain", ISOCode = "ES" },
            new { Name = "Sweden", ISOCode = "SE" },
            new { Name = "Switzerland", ISOCode = "CH" },
            new { Name = "Turkey", ISOCode = "TR" },
            new { Name = "Ukraine", ISOCode = "UA" },
            new { Name = "United Kingdom", ISOCode = "GB" },
            new { Name = "Vatican City", ISOCode = "VA" }
        };

        var response = await _client.GetAsync("/randomeuropeancountry");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var countryResponse = JsonSerializer.Deserialize<Country>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(countryResponse);
        var country = countries.FirstOrDefault(c => c.Name == countryResponse.Name || c.ISOCode == countryResponse.ISOCode);
        Assert.NotNull(country);
    }
        public record Country(string Name, string ISOCode);
}
