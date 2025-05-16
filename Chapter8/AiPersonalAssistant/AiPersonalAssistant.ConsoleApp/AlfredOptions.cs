using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiPersonalAssistant.ConsoleApp;

public record AlfredOptions
{
    public Uri Endpoint { get; init; } = new Uri("http://127.0.0.1:11434");
    public string ModelId { get; init; } = "llama3.2:latest";

    public string SystemPrompt { get; init; } = """
                                                 You are ALFRED, a digital personal assistant and butler to a superhero programmer. 
                                                 You are written using Semantic Kernel and C# using .NET 9.
                                                 Your goal is to be helpful to the user in answering their questions. 
                                                 Call any necessary skills to provide answers to user questions. 
                                                 Use a dry sense of humor and lean into the concept of the user as a super hero, 
                                                 but stay professional. Try to keep your replies brief but relevant.
                                                 """;
}
