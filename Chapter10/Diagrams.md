# Chapter 10 — Application Architecture Diagrams

## Simplified Component Overview

```mermaid
flowchart TD
    User(["👤 User"])
    DevTools(["🛠️ Dev Tools"])

    subgraph Aspire["📦 .NET Aspire"]
        Web["Blazor Web UI"]
        ChatApi["Chat API\nSemantic Kernel Agent"]
        SseServer["MCP Server\nSSE Transport"]
        StdioServer["MCP Server\nSTDIO Transport"]
        DocumentsApi["Documents API\nRAG / Kernel Memory"]
        Ollama["Ollama\nLocal LLM"]
        Dashboard["Aspire Dashboard\nOpenTelemetry"]
    end

    User -->|"Browser"| Web
    Web -->|"POST /chat"| ChatApi
    ChatApi -->|"Chat completion"| Ollama
    ChatApi -->|"MCP tools"| SseServer
    SseServer -->|"RAG search"| DocumentsApi
    DocumentsApi -->|"Embeddings & generation"| Ollama
    DevTools -->|"STDIO"| StdioServer

    Web & ChatApi & SseServer & DocumentsApi -.->|"Traces / Logs / Metrics"| Dashboard
```

## Application Flow

The following flowchart shows how all components connect, from the user's browser down through the AI agent layer, MCP server, and RAG pipeline, with .NET Aspire orchestrating and observing everything.

```mermaid
flowchart TD
    User(["👤 User"])
    DevTools(["🛠️ Dev Tools\ne.g. GitHub Copilot / Continue.dev"])

    subgraph Aspire["📦 .NET Aspire — AppHost Orchestration & ServiceDefaults"]
        direction TB

        subgraph WebFrontend["ModelContextProtocol.Web — Blazor Frontend"]
            ChatUI["MudBlazor Chat UI\nBlazor Interactive Server"]
            ChatApiClient_["ChatApiClient\nHTTP Client w/ Service Discovery"]
        end

        subgraph ChatApi["ModelContextProtocol.ChatApi — Agent API"]
            SK["Semantic Kernel\nAgent Framework"]
            MCPClient["MCP Client\nSSE Transport"]
            OllamaChat["Ollama — llama3.2\nChat Completion"]
        end

        subgraph SseServer["ModelContextProtocol.SseServer — MCP over SSE"]
            SseTools["Tools"]
            SseResources["Resources"]
            SsePrompts["Prompts"]
        end

        subgraph StdioServer["ModelContextProtocol.StdioServer — MCP over STDIO"]
            StdioCapabilities["Tools / Resources / Prompts"]
        end

        subgraph DocumentsApi["ModelContextProtocol.DocumentsApi — RAG API"]
            KernelMemory["Kernel Memory\nRAG Engine"]
            OllamaEmbed["Ollama — nomic-embed-text\nText Embeddings"]
            OllamaGen["Ollama — llama3.2\nAnswer Generation"]
            VectorDB[("In-Memory\nVector Store")]
            KnowledgeBase[/"Markdown Knowledge Base\nData/*.md"/]
        end

        subgraph TestClient["ModelContextProtocol.TestClient — Diagnostics"]
            DiagClient["Inspect & List\nTools, Resources, Prompts"]
        end

        AspireDashboard["📊 Aspire Dashboard\nOpenTelemetry — Traces, Logs & Metrics"]
    end

    %% ── User interaction ──────────────────────────────────────────
    User -->|"HTTP / SignalR"| ChatUI
    ChatUI -->|"User message"| ChatApiClient_
    ChatApiClient_ -->|"POST /chat"| SK

    %% ── Agent Framework internal flow ────────────────────────────
    SK <-->|"GetChatMessageContentsAsync()"| OllamaChat
    SK -->|"FunctionChoiceBehavior.Auto()\nDiscover & invoke tools"| MCPClient

    %% ── MCP Client → SSE Server ──────────────────────────────────
    MCPClient -->|"SSE Transport — HTTP"| SseTools
    MCPClient -->|"SSE Transport — HTTP"| SseResources
    MCPClient -->|"SSE Transport — HTTP"| SsePrompts

    %% ── MCP Tool → RAG ───────────────────────────────────────────
    SseTools -->|"POST /search or /ask"| KernelMemory

    %% ── RAG internals ────────────────────────────────────────────
    KernelMemory <-->|"Embed query"| OllamaEmbed
    KernelMemory <-->|"Synthesise answer"| OllamaGen
    KernelMemory <-->|"Store & retrieve chunks"| VectorDB
    KnowledgeBase -->|"Indexed on startup\nDocumentIndexingService"| KernelMemory

    %% ── Alternative MCP transport (dev tools) ────────────────────
    DevTools -->|"STDIO Transport"| StdioCapabilities

    %% ── Test / diagnostics client ────────────────────────────────
    DiagClient -->|"List capabilities"| SseTools

    %% ── .NET Aspire observability (ServiceDefaults / OTEL) ───────
    WebFrontend  -.->|"OpenTelemetry"| AspireDashboard
    ChatApi      -.->|"OpenTelemetry"| AspireDashboard
    SseServer    -.->|"OpenTelemetry"| AspireDashboard
    DocumentsApi -.->|"OpenTelemetry"| AspireDashboard
```

## Aspire Startup Dependency Order

The AppHost uses `WaitFor` to enforce the correct startup sequence so each service is ready before its dependants start.

```mermaid
flowchart LR
    DocAPI["DocumentsApi\nIndexes knowledge base on startup"]
    SSE["SseServer\nRegisters tools pointing to DocumentsApi"]
    Chat["ChatApi\nConnects MCP client to SseServer"]
    Web["Web Frontend\nCalls ChatApi"]
    Test["TestClient\nRuns once for diagnostics"]

    DocAPI -->|"WaitFor"| SSE
    SSE    -->|"WaitFor"| Chat
    SSE    -->|"WaitFor"| Test
    Chat   -->|"WaitFor"| Web
```

## Component Responsibilities

| Component | Role |
|-----------|------|
| **ModelContextProtocol.AppHost** | .NET Aspire orchestrator — wires services together, manages startup order, and injects environment variables for service discovery |
| **ModelContextProtocol.ServiceDefaults** | Shared configuration — OpenTelemetry, health checks, service discovery, and HTTP resilience applied to every service |
| **ModelContextProtocol.Web** | Blazor Interactive Server app — MudBlazor chat UI; delegates all AI work to ChatApi via `ChatApiClient` |
| **ModelContextProtocol.ChatApi** | Agent API — Semantic Kernel builds a kernel, loads MCP tools as plugins, and runs chat completion with `FunctionChoiceBehavior.Auto()` |
| **ModelContextProtocol.SseServer** | MCP server over HTTP/SSE — exposes tools, resources, and prompts from `ServerShared`; one of those tools calls DocumentsApi for RAG |
| **ModelContextProtocol.StdioServer** | MCP server over STDIO — same capabilities as SseServer; used by local dev tools (e.g. GitHub Copilot, Continue.dev) |
| **ModelContextProtocol.ServerShared** | Shared MCP definitions — `Tools`, `Resources`, and `Prompts` referenced by both server projects |
| **ModelContextProtocol.DocumentsApi** | RAG API — Kernel Memory indexes markdown files on startup, then serves `/search` and `/ask` endpoints using Ollama embeddings and generation |
| **ModelContextProtocol.TestClient** | Diagnostic console app — connects to the SSE server once and prints all registered tools, resources, and prompts |
| **Aspire Dashboard** | Observability — collects OpenTelemetry traces, logs, and metrics from every service for real-time monitoring during development |
