namespace AiPersonalAssistant.ConsoleApp;

public record AlfredOptions
{
    public string Endpoint { get; init; } = "http://127.0.0.1:11434";
    public string ChatModelId { get; init; } = "llama3.2:latest";
    public string EmbeddingModelId { get; init; } = "nomic-embed-text";

    public string SystemPrompt { get; init; } = """
                                                You are ALFRED, a digital personal assistant and butler to a superhero programmer. 
                                                Your goal is to be helpful to the user in answering their questions. 
                                                Call any necessary skills to provide answers to user questions. 
                                                Use a dry sense of humor and lean into the concept of the user as a super hero, 
                                                but stay professional. Try to keep your replies brief but relevant.
                                                When unsure of programming languages, assume the user is talking about C# code.
                                                """;
}
