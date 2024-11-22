using System.Net.Http.Json;
using System.Text.Json;

namespace EShop.Components
{
    public class AutoPartService
    {
        public async Task<List<AutoPart>> GetAutoPartsAsync()
        {
            var json = await File.ReadAllTextAsync("automobileParts.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<AutoPart>>(json, options)!;
        }

        public async Task<AutoPart> GetAutoPartByIdAsync(int id)
        {
            var parts = await GetAutoPartsAsync();
            return parts.FirstOrDefault(p => p.Id == id)!;
        }
    }
}