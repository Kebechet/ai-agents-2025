using OpenAI.Chat;

namespace AiAgent;

public class TravelAgent
{
    private readonly ChatClient _chatClient;
    private readonly SerpApiService _serpApi;
    private readonly string _destination;
    private readonly int _days;
    private readonly string _preferences;

    public TravelAgent(string openAiKey, string serpApiKey, string destination, int days, string preferences)
    {
        _destination = destination;
        _days = days;
        _preferences = preferences;
        _serpApi = new SerpApiService(serpApiKey);

        var client = new OpenAI.OpenAIClient(openAiKey);
        _chatClient = client.GetChatClient("gpt-4");
    }

    public async Task<string> PlanTripAsync()
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(@"You are a travel planning assistant using ReAct (Reasoning and Acting) methodology.

For each step:
1. THOUGHT: Think about what information you need
2. ACTION: Decide what search to perform (be specific about search query)
3. Wait for OBSERVATION with search results
4. Repeat until you have enough information

When you have gathered all necessary information, respond with FINAL ANSWER followed by the complete travel plan in markdown format.")
        };

        var initialPrompt = $@"Plan a {_days}-day trip to {_destination}.
Preferences: {_preferences}

Use ReAct methodology to gather information about:
1. Top attractions in {_destination}
2. Best time to visit and weather in {_destination}
3. Local cuisine and restaurants in {_destination}
4. Transportation options in {_destination}
5. Travel tips for {_destination}

After gathering information, create a day-by-day itinerary in markdown format.";

        messages.Add(new UserChatMessage(initialPrompt));

        var maxIterations = 10;
        var iteration = 0;

        while (iteration < maxIterations)
        {
            iteration++;
            Console.WriteLine($"\n=== Iteration {iteration} ===");

            var response = await _chatClient.CompleteChatAsync(messages);
            var responseText = response.Value.Content[0].Text;

            Console.WriteLine($"Agent: {responseText}");
            messages.Add(new AssistantChatMessage(responseText));

            // Check if agent has final answer
            if (responseText.Contains("FINAL ANSWER", StringComparison.OrdinalIgnoreCase))
            {
                return ExtractFinalAnswer(responseText);
            }

            // Extract action from response
            var action = ExtractAction(responseText);
            if (!string.IsNullOrEmpty(action))
            {
                Console.WriteLine($"\nExecuting search: {action}");
                var searchResult = await _serpApi.SearchAsync(action);

                Console.WriteLine($"Search results: {searchResult.Substring(0, Math.Min(200, searchResult.Length))}...");

                var observation = $"OBSERVATION: {searchResult}";
                messages.Add(new UserChatMessage(observation));
            }
            else
            {
                // If no clear action, prompt agent to continue
                messages.Add(new UserChatMessage(
                    "Please provide your next THOUGHT and ACTION, or provide the FINAL ANSWER if you have enough information."));
            }
        }

        return "Travel planning reached maximum iterations. Please try again with a more specific query.";
    }

    private string ExtractAction(string response)
    {
        var lines = response.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("ACTION:", StringComparison.OrdinalIgnoreCase))
            {
                return line.Split("ACTION:", StringSplitOptions.None)[1].Trim();
            }
            if (line.Contains("Search:", StringComparison.OrdinalIgnoreCase))
            {
                return line.Split("Search:", StringSplitOptions.None)[1].Trim();
            }
        }
        return string.Empty;
    }

    private string ExtractFinalAnswer(string response)
    {
        var finalAnswerIndex = response.IndexOf("FINAL ANSWER", StringComparison.OrdinalIgnoreCase);
        if (finalAnswerIndex >= 0)
        {
            return response.Substring(finalAnswerIndex + "FINAL ANSWER".Length).Trim().TrimStart(':').Trim();
        }
        return response;
    }
}
