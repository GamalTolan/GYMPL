using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GYMDAL.Data.DataSeed
{
    public static class GymDataSeed
    {
        public static bool DataSeed(GymDbContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    var categories = GetDataFromJson<Category>("categories.json");
                    context.Categories.AddRange(categories);
                }

                if (!context.Plans.Any())
                {
                    var plans = GetDataFromJson<Plan>("plans.json");
                    context.Plans.AddRange(plans);
                }

                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while seeding the database: {ex.Message}", ex);
            }

        }
        private static List<T> GetDataFromJson<T>(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file '{fileName}' was not found in the 'wwwroot\\Files' directory.");
            }
            var jsonData = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(jsonData, options);
        }
    }
}
