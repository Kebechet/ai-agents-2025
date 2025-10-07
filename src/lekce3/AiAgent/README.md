# AI Travel Planner - ReAct Agent

A travel planning assistant powered by AutoGen and OpenAI that uses the ReAct (Reasoning and Acting) methodology to create personalized travel itineraries.

## Features

- **ReAct Agent Pattern**: The agent thinks, acts, and observes iteratively to gather information
- **SerpAPI Integration**: Real-time web search for travel information
- **OpenAI GPT-4**: Intelligent reasoning and planning
- **Markdown Output**: Generates formatted travel itineraries

## Architecture

The project implements a ReAct agent that:
1. **THOUGHT**: Reasons about what information is needed
2. **ACTION**: Performs web searches via SerpAPI
3. **OBSERVATION**: Analyzes search results
4. **Repeats** until enough information is gathered
5. **FINAL ANSWER**: Generates a complete travel itinerary

## Prerequisites

- .NET 8.0 SDK
- OpenAI API Key ([Get one here](https://platform.openai.com/api-keys))
- SerpAPI Key ([Get one here](https://serpapi.com/))

## Setup

1. Clone the repository
2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. (Optional) Set environment variables for API keys:
   ```bash
   set OPENAI_API_KEY=your_openai_key_here
   set SERPAPI_API_KEY=your_serpapi_key_here
   ```
   Or enter them when prompted during runtime.

## Usage

Run the application:
```bash
dotnet run
```

You'll be prompted to enter:
- Destination (e.g., "Prague", "Tokyo", "Paris")
- Number of days
- Preferences (e.g., "budget-friendly, cultural attractions, food lover")

The agent will:
1. Search for attractions, restaurants, weather, transportation, and tips
2. Reason about the information gathered
3. Create a day-by-day itinerary
4. Save it as a markdown file (e.g., `trip_Prague_20250107_143022.md`)

## Example

```
=== AI Travel Planner (ReAct Agent) ===

Destination: Prague
Number of days: 3
Preferences (budget, interests, etc.): budget-friendly, cultural attractions

🤖 Planning your trip... (this may take a minute)

=== Iteration 1 ===
Agent: THOUGHT: I need to gather information about Prague attractions...
ACTION: top attractions in Prague

Executing search: top attractions in Prague
...

✅ Itinerary saved to: trip_Prague_20250107_143022.md
```

## Project Structure

```
AiAgent/
├── Program.cs           # Main entry point
├── TravelAgent.cs       # ReAct agent implementation
├── SerpApiService.cs    # SerpAPI integration
├── AiAgent.csproj       # Project configuration
└── README.md            # This file
```

## Technologies Used

- **OpenAI 2.1.0**: Official OpenAI SDK for .NET
- **System.Text.Json**: Built-in JSON serialization
- **OpenAI GPT-4**: Language model
- **SerpAPI**: Web search API
- **.NET 8.0**: Runtime

## Assignment Requirements

This project fulfills the AI Agents course assignment:
- ✅ **Agent Pattern**: ReAct (Reasoning and Acting)
- ✅ **Framework**: Custom implementation using OpenAI SDK
- ✅ **Tools**: SerpAPI for web search
- ✅ **LLM**: OpenAI GPT-4
- ✅ **Output**: Markdown file with travel itinerary

## License

MIT
