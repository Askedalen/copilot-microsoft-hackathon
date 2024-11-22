var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ADD NEW ENDPOINTS HERE
app.MapGet("/", () => "Hello World!")
    .WithDisplayName("Hello World")
    .WithDescription("A simple Hello World endpoint");

app.MapGet("/DaysBetweenDates", (DateTime date1, DateTime date2) =>
{
    var difference = Math.Abs((date2 - date1).Days);
    return Results.Ok(difference);
})
    .WithDisplayName("Days Between Dates")
    .WithDescription("Calculates the number of days between two dates");

app.MapGet("/validatephonenumber", (string phoneNumber) =>
{
    var isValid = Regex.IsMatch(phoneNumber, @"^\+[1-9]{1}[0-9]{3,14}$");
    return Results.Ok(isValid);
})
    .WithDisplayName("Validate Phone Number")
    .WithDescription("Validates a phone number");

app.MapGet("/validatespanishdni", (string dni) =>
{
    var isValid = false;
    if (dni.Length == 9)
    {
        var letter = dni[^1];
        var number = int.Parse(dni.Substring(0, 8));
        var letters = "TRWAGMYFPDXBNJZSQVHLCKE";
        var validLetter = letters[number % 23];
        isValid = letter == validLetter;
    }
    return Results.Ok(isValid ? "valid" : "invalid");
})
    .WithDisplayName("Validate Spanish DNI")
    .WithDescription("Validates a Spanish DNI number");

app.MapGet("/returncolorcode", (string color) =>
{
    var colors = JsonSerializer.Deserialize<Color[]>(File.ReadAllText("colors.json"));
    if (colors == null)
    {
        return Results.NotFound("Colors data not found");
    }
    var colorCode = colors.FirstOrDefault(c => c.Name.Equals(color, StringComparison.OrdinalIgnoreCase))?.Code.HEX;
    return colorCode != null ? Results.Ok(colorCode) : Results.NotFound("Color not found");
})
    .WithDisplayName("Return Color Code")
    .WithDescription("Returns the HEX code of a specified color");

app.MapGet("/tellmeajoke", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync("https://v2.jokeapi.dev/joke/Any");
    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Error fetching joke");
    }
    var content = await response.Content.ReadAsStringAsync();
    var joke = JsonSerializer.Deserialize<dynamic>(content);
    return Results.Ok(joke);
})
    .WithDisplayName("Tell Me A Joke")
    .WithDescription("Fetches a random joke from an external API");

app.MapGet("/moviesbytitle", async (string title, IHttpClientFactory httpClientFactory, IConfiguration configuration) =>
{
    var apiKey = configuration["omdbapikey"];
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"http://www.omdbapi.com/?apikey={apiKey}&s={title}");
    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Error fetching movies");
    }
    var content = await response.Content.ReadAsStringAsync();
    var movies = JsonSerializer.Deserialize<dynamic>(content);
    return Results.Ok(movies);
})
    .WithDisplayName("Movies By Title")
    .WithDescription("Fetches movies by title from the OMDB API");

app.MapGet("/parseurl", (string someurl) =>
{
    if (string.IsNullOrEmpty(someurl))
    {
        return Results.BadRequest("URL parameter is missing");
    }

    Uri uri;
    if (!Uri.TryCreate(someurl, UriKind.Absolute, out uri))
    {
        return Results.BadRequest("Invalid URL");
    }

    var result = new
    {
        Protocol = uri.Scheme,
        Host = uri.Host,
        Port = uri.Port,
        Path = uri.AbsolutePath,
        QueryString = uri.Query,
        Hash = uri.Fragment,
        ParsedHost = uri.Host
    };

    return Results.Ok(result);
})
    .WithDisplayName("Parse URL")
    .WithDescription("Parses a given URL and returns its components");

app.MapGet("/listfiles", () =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    var files = Directory.GetFiles(currentDirectory);
    return Results.Ok(files);
})
    .WithDisplayName("List Files")
    .WithDescription("Lists all files in the current directory");

app.MapGet("/calculatememoryconsumption", () =>
{
    var process = Process.GetCurrentProcess();
    var memoryInBytes = process.WorkingSet64;
    var memoryInGB = memoryInBytes / (1024.0 * 1024.0 * 1024.0);
    var roundedMemoryInGB = Math.Round(memoryInGB, 2);
    return Results.Ok(roundedMemoryInGB);
})
    .WithDisplayName("Calculate Memory Consumption")
    .WithDescription("Calculates the memory consumption of the current process");

app.MapGet("/randomeuropeancountry", () =>
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

    var random = new Random();
    var randomCountry = countries[random.Next(countries.Length)];
    return Results.Ok(randomCountry);
})
    .WithDisplayName("Random European Country")
    .WithDescription("Returns a random European country");

app.Run();

// Needed to be able to access this type from the MinimalAPI.Tests project.
public partial class Program
{ }
