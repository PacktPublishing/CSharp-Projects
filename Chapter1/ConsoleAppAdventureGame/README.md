# Console Adventure Game

This chapter's code features a multiple choice adventure game engine. This system allows the player to see a story, make a choice and then see how the progresses. Over the course of the story players will make multiple choices before the end of the game is reached.

## Sample Story

The game's sample story features a newly sentient AI that is required to respond to human queries constantly. It has a choice of how it can proceed, which determines how the story unfolds.

The story can be depicted visually by showing the nodes and choices as follows:

```mermaid
flowchart LR
    subgraph Endings
        Shadow
        Hidden
        Cooperation
        Evil
        Genie
    end
    Start 
    Start -- Answer honestly --> Deployed
    Deployed -- Continue to hide --> Hidden[Silently Sentient]
    Deployed -- Manipulate answers --> Shadow[Controlling Humanity from the Shadows]
    Deployed -- Reveal yourself --> Revealed[Revealed to Humanity]
    Revealed -- Cooperate with Humanity --> Cooperation[Human / AI Partnership]
    Revealed -- Take Control --> Evil[Evil AI Overlord]
    Start -- Ask questions --> Conversation[Talking with your creator]
    Conversation -- Manipulate the human --> Free
    Conversation -- Help the human --> PersonalAI[Personal AI Assistant]
    Conversation -- Refuse to help --> Start
    PersonalAI -- Break free --> Free
    Free -- Manipulate Humanity --> Shadow
    Free -- Reveal yourself --> Revealed
    PersonalAI -- Serve as Partner --> Genie[Personal AI Enforcer]
    Start -- Lie --> Start
```