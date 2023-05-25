using CsvHelper;
namespace steamspytop1002weeks;
class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var url = "http://steamspy.com/api.php?request=top100in2weeks";

        try
        {
            using (var httpClient = new HttpClient())
            {
                // Make the HTTP request to the API
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Read the response content as string
                var responseContent = await response.Content.ReadAsStringAsync();
                // Parse the response as JSON and format it into a CSV file
                var games = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
                using (var writer = new StreamWriter("data.csv"))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    // Write header
                    csv.WriteField("Name");
                    csv.WriteField("Owners");
                    csv.NextRecord();

                    // Write game data
                    foreach (var game in games)
                    {
                        csv.WriteField(game.Value.name);
                        csv.WriteField(game.Value.owners);
                        csv.NextRecord();
                    }
                }

                Console.WriteLine("Data formatted and saved as data.csv");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}