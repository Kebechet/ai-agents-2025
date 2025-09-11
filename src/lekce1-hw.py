# Assignment: https://drive.google.com/file/d/1MQHMLCMFPblhWGdcprtMOd9tZ5pj3YAS/view

# Zadání:
# Napiš Python skript, který zavolá LLM API, použije nástroj (např. výpočetní funkci) a
# vrátí odpověď zpět LLM.

# Forma odevzdání:
# Vypracovaný úkol odevzdejte ve formě zdrojového kódu. Projekt ideálně nahrajte na
# Github a odevzdejte link do Github repositáře. Link odevzdejte v Google Classroom.

import os
import json
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

# Initialize OpenAI client
client = OpenAI(
    api_key=os.environ.get("OPENAI_API_KEY"),
)

# Function Implementations
def calculate_bmi(heightMeters: float, weightKg: float) -> float:
    return weightKg / (heightMeters * heightMeters)

# Define custom tools
tools = [
    {
        "type": "function",
        "function": {
            "name": "calculate_bmi",
            "description": "Use this function to get the BMI value based on height and weight",
            "parameters": {
                "type": "object",
                "properties": {
                    "weightKg": {
                        "type": "number",
                        "description": "Weight value in kilograms",
                    },
                    "heightMeters": {
                        "type": "number",
                        "description": "height value in meters",
                    }
                },
                "required": ["weightKg","heightMeters"]
            },
        }
    }
]

available_functions = {
    "calculate_bmi": calculate_bmi,
}

# Function to process messages and handle function calls
def get_completion_from_messages(messages, model="gpt-4o"):
    response = client.chat.completions.create(
        model=model,
        messages=messages,
        tools=tools,  # Custom tools
        tool_choice="auto"  # Allow AI to decide if a tool should be called
    )

    response_message = response.choices[0].message

    print("First response:", response_message)

    if response_message.tool_calls:
        # Find the tool call content
        tool_call = response_message.tool_calls[0]

        # Extract tool name and arguments
        function_name = tool_call.function.name
        function_args = json.loads(tool_call.function.arguments)
        tool_id = tool_call.id

        # Call the function
        function_to_call = available_functions[function_name]
        function_response = function_to_call(**function_args)

        print("Func response:", function_response)

        messages.append({
            "role": "assistant",
            "tool_calls": [
                {
                    "id": tool_id,
                    "type": "function",
                    "function": {
                        "name": function_name,
                        "arguments": json.dumps(function_args),
                    }
                }
            ]
        })
        messages.append({
            "role": "tool",
            "tool_call_id": tool_id,
            "name": function_name,
            "content": json.dumps(function_response),
        })

        # Second call to get final response based on function output
        second_response = client.chat.completions.create(
            model=model,
            messages=messages,
            tools=tools,
            tool_choice="auto"
        )
        final_answer = second_response.choices[0].message

        # print("Second response:", final_answer)
        return final_answer

    return "No relevant function call found."


# Example usage
messages = [
    {"role": "system", "content": "You are a helpful AI assistant with focus on healthy lifestyle"},
    {"role": "user", "content": "What is my BMI when my weight is 91kg and I have 180cm ?"},
]

response = get_completion_from_messages(messages)
# print("--- Full response: ---")
# pprint(response)
print("--- Response text: ---")
print(response.content)


