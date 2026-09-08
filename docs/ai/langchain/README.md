LangChain is a framework for building applications powered by Large Language Models (LLMs) such as GPT.

In simple terms, an LLM can answer questions, but real applications often also need to:

- Read documents or databases
- Remember earlier messages
- Call APIs and other tools
- Follow a sequence of steps
- Retrieve relevant information before answering

LangChain provides reusable components to connect these capabilities.

### Main concepts

- **Model:** The LLM that understands and generates text.
- **Prompt:** Instructions and input given to the model.
- **Chain:** A sequence of operations, such as retrieving data and then generating an answer.
- **Retriever:** Finds relevant information from documents or a vector database.
- **Memory:** Stores conversation context. In newer designs, this is often implemented through message history or persisted state.
- **Tools:** External functions the model can call, such as search, calculators, APIs, or databases.
- **Agent:** Uses the LLM to decide which tools to call and in what order.
- **Output parser:** Converts the model’s response into a structured format.

### Simple example

For a chatbot that answers questions about company documents:

1. The user asks a question.
2. LangChain finds relevant sections in the documents.
3. It combines those sections with the question.
4. It sends everything to the LLM.
5. The LLM produces an evidence-based answer.

This pattern is called **Retrieval-Augmented Generation**, or **RAG**.

### Interview-ready answer

> LangChain is an open-source framework for developing LLM-powered applications. It provides components for connecting language models with prompts, documents, retrievers, databases, APIs, memory, and tools. It is commonly used to build RAG systems, chatbots, and AI agents. LangChain does not train the LLM; it orchestrates the model and the surrounding application components.

### Common interview follow-ups

**Why use LangChain instead of calling an LLM API directly?**  
For a simple prompt, a direct API call is often enough. LangChain becomes useful when the application needs retrieval, tool calling, state management, structured outputs, or multi-step workflows.

**What is the difference between a chain and an agent?**  
A chain follows a predefined workflow. An agent dynamically decides what action or tool to use next.

**What is a vector database?**  
It stores numerical representations of text called embeddings and retrieves content with similar meaning.

**What are its disadvantages?**  
It can introduce extra abstraction, complexity, debugging difficulty, latency, and dependency changes. It should be used when its orchestration features provide real value.