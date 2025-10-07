namespace AiAgent
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== AI Travel Planner (ReAct Agent) ===\n");

            // Get API keys from environment or user input
            var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var serpApiKey = Environment.GetEnvironmentVariable("SERPAPI_API_KEY");

            if (string.IsNullOrEmpty(openAiKey))
            {
                Console.Write("Enter OpenAI API Key: ");
                openAiKey = Console.ReadLine() ?? "";
            }

            if (string.IsNullOrEmpty(serpApiKey))
            {
                Console.Write("Enter SerpAPI Key: ");
                serpApiKey = Console.ReadLine() ?? "";
            }

            // Get travel details
            Console.Write("\nDestination: ");
            var destination = Console.ReadLine() ?? "Prague";

            Console.Write("Number of days: ");
            var daysInput = Console.ReadLine();
            var days = int.TryParse(daysInput, out var parsedDays) ? parsedDays : 3;

            Console.Write("Preferences (budget, interests, etc.): ");
            var preferences = Console.ReadLine() ?? "budget-friendly, cultural attractions";

            Console.WriteLine("\n🤖 Planning your trip... (this may take a minute)\n");

            try
            {
                var agent = new TravelAgent(openAiKey, serpApiKey, destination, days, preferences);
                var itinerary = await agent.PlanTripAsync();

                Console.WriteLine("\n\n=== FINAL ITINERARY ===\n");
                Console.WriteLine(itinerary);

                // Save to markdown file
                var fileName = $"trip_{destination.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.md";
                await File.WriteAllTextAsync(fileName, itinerary);

                Console.WriteLine($"\n✅ Itinerary saved to: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
