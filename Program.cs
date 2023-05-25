using CsvHelper;
namespace steamspytop1002weeks;
class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var date = DateTime.Now.ToString("MM-dd");
        string fileName = $"{date}top1002weeks.csv";
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
                var games = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Game>>(responseContent);

                using (var writer = new StreamWriter(fileName))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    // Write headers
                    Type type = typeof(Game);
                    var gameRefTypeMembers = type.GetProperties().ToList();
                    gameRefTypeMembers.ForEach(m => csv.WriteField(m.Name));
                    csv.NextRecord();

                    // Write game data
                    foreach (var game in games ?? new Dictionary<string, Game>())
                    {
                        csv.WriteField(game.Value.AppId);
                        csv.WriteField(game.Value.Name);
                        csv.WriteField(game.Value.Developer);
                        csv.WriteField(game.Value.Publisher);
                        csv.WriteField(game.Value.ScoreRank);
                        csv.WriteField(game.Value.Positive);
                        csv.WriteField(game.Value.Negative);
                        csv.WriteField(game.Value.Userscore);
                        csv.WriteField(game.Value.Owners);
                        csv.WriteField(game.Value.Average2Weeks);
                        csv.WriteField(game.Value.AverageForever);
                        csv.WriteField(game.Value.Median2Weeks);
                        csv.WriteField(game.Value.MedianForever);
                        csv.WriteField(game.Value.Price);
                        csv.WriteField(string.Format("{0:0.00}", game.Value.InitialPrice));
                        csv.WriteField(game.Value.Discount);
                        csv.WriteField(game.Value.Ccu);
                        csv.NextRecord();
                    }
                }

                Console.WriteLine($"Data formatted and saved as {fileName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
public class Game
{
    public int AppId { get; set; }
    public string? Name { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public string? ScoreRank { get; set; }
    public float Positive { get; set; }
    public float Negative { get; set; }
    public int Userscore { get; set; }
    public string? Owners { get; set; }
    public float AverageForever { get; set; }
    public int Average2Weeks { get; set; }
    public float MedianForever { get; set; }
    public int Median2Weeks { get; set; }
    public string? Price { get; set; }
    public string? InitialPrice { get; set; }
    public string? Discount { get; set; }
    public int Ccu { get; set; }
}