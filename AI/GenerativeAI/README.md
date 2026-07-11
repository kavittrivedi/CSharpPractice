# Generative AI — Simple Interview Notes

## What is Generative AI?

Generative AI is a type of Artificial Intelligence that can **create new content** such as text, images, audio, video, and computer code.

It learns patterns from a large amount of existing data and uses those patterns to generate a new response.

**Example:** When we ask ChatGPT to write an email, it generates a new email based on the instructions in our prompt.

## AI, Machine Learning, and Generative AI

- **Artificial Intelligence (AI):** The broad idea of making computers perform tasks that normally require human intelligence.
- **Machine Learning (ML):** A part of AI in which computers learn patterns from data instead of being explicitly programmed for every situation.
- **Deep Learning:** A part of ML that uses multi-layer neural networks to learn complex patterns.
- **Generative AI:** A part of AI that uses learned patterns to produce new content.

```text
Artificial Intelligence
└── Machine Learning
    └── Deep Learning
        └── Generative AI models
```

## How does Generative AI work?

In simple terms, it works in three stages:

1. **Training:** The model studies a very large amount of data.
2. **Learning patterns:** It learns relationships between words, images, sounds, or code.
3. **Generation:** When a user provides a prompt, the model predicts and creates a suitable response.

A text model does not think exactly like a human. It generally generates text by repeatedly predicting the most suitable next **token**.

## Important Terms

### Large Language Model (LLM)

An LLM is an AI model trained on a large amount of text. It can understand and generate human-like language.

Examples include GPT, Claude, Gemini, and Llama.

### Token

A token is a small unit of text processed by a language model. It may be a complete word, part of a word, punctuation, or a number.

The model's input and output limits are usually measured in tokens.

### Prompt

A prompt is the instruction or question given to an AI model.

**Simple prompt:**

> Explain cloud computing.

**Better prompt:**

> Explain cloud computing in simple language for a beginner. Give one real-world example and keep the answer under 150 words.

### Prompt Engineering

Prompt engineering means writing clear and structured instructions so that an AI model produces a more useful answer.

A good prompt commonly includes:

- The task
- Relevant context
- The expected format
- Constraints
- An example, when helpful

### Context Window

The context window is the amount of information a model can consider in one request or conversation. It includes the prompt, previous messages, and generated output.

### Hallucination

A hallucination happens when an AI model gives an answer that sounds confident but is incorrect or invented.

Hallucinations can be reduced by using reliable source data, giving clear prompts, using RAG, and validating important answers.

### Temperature

Temperature controls the randomness of a model's response.

- **Low temperature:** More consistent and focused answers
- **High temperature:** More varied and creative answers

### Foundation Model

A foundation model is a large, general-purpose model trained on broad data. It can later be adapted for different tasks such as summarization, question answering, or coding.

## What is a Transformer?

A Transformer is a neural-network architecture used by many modern Generative AI models.

Its important feature is **attention**, which helps the model identify which words or parts of the input are most relevant to one another. This allows it to understand context and process language effectively.

## Embeddings

An embedding converts content such as text into a list of numbers called a **vector**. Content with a similar meaning usually has vectors that are close to each other.

Embeddings are commonly used for:

- Semantic search
- Finding similar documents
- Recommendations
- Retrieval-Augmented Generation

## Retrieval-Augmented Generation (RAG)

RAG improves an AI response by giving the model relevant information from an external knowledge source before it generates the answer.

```text
User question
     ↓
Search relevant company documents
     ↓
Send the question and retrieved content to the LLM
     ↓
Generate a grounded answer
```

**Example:** An employee asks a chatbot about the company's leave policy. The application retrieves the latest HR policy and gives it to the LLM to prepare the answer.

Benefits of RAG include:

- Uses current or private business information
- Reduces hallucinations
- Can provide answers based on source documents
- Usually does not require retraining the model

## Fine-Tuning vs. RAG

| RAG | Fine-tuning |
| --- | --- |
| Provides external information at request time | Further trains a model using selected examples |
| Best for changing facts and private documents | Best for specialized behavior, style, or repeated tasks |
| Documents can be updated easily | Training must be repeated when training needs change |
| Helps the model know relevant facts | Helps the model respond in a desired way |

A simple rule is: use **RAG for knowledge** and consider **fine-tuning for behavior or style**.

## AI Agents

An AI agent is a system that uses an AI model to decide what actions to take to achieve a goal. It may use tools such as web search, databases, APIs, email, or code execution.

For example, a travel agent could search flights, compare options, and prepare an itinerary. The LLM provides reasoning and language ability, while tools allow the system to take useful actions.

## Common Applications

- Chatbots and virtual assistants
- Text summarization and translation
- Email and document generation
- Code generation and explanation
- Image, music, and video generation
- Customer-support assistance
- Document search and question answering
- Personalized learning and recommendations

## Benefits

- Saves time by automating repetitive work
- Improves productivity
- Helps create and summarize content quickly
- Makes information easier to access
- Supports creativity and brainstorming
- Can provide personalized user experiences

## Limitations and Risks

- **Hallucination:** The model may generate incorrect information.
- **Bias:** The response may reflect bias present in training data.
- **Privacy:** Sensitive information must not be sent to an unapproved service.
- **Security:** Prompts and generated code can introduce security risks.
- **Copyright:** Generated content may create ownership or attribution concerns.
- **Lack of explainability:** It can be difficult to explain exactly why a model generated an answer.
- **Cost and latency:** Large models can be expensive or slow to run.

Important AI output should be reviewed by a human, especially for medical, legal, financial, or security-related decisions.

## Responsible AI

Responsible AI means designing and using AI safely, fairly, and transparently.

Important practices include:

- Protecting personal and confidential data
- Testing responses for bias and harmful content
- Validating important facts
- Controlling who can access AI features and data
- Monitoring model quality and cost
- Keeping human approval for high-impact decisions
- Clearly telling users when they are interacting with AI

## Typical Generative AI Application Flow

```text
User Interface
      ↓
Application / API
      ↓
Prompt + optional retrieved data
      ↓
Generative AI model
      ↓
Safety checks and response validation
      ↓
Answer shown to the user
```

## Common Interview Questions

### 1. What is Generative AI?

Generative AI is a type of AI that learns patterns from existing data and creates new content such as text, images, audio, video, or code. ChatGPT generating an email from a prompt is a common example.

### 2. How is Generative AI different from traditional AI?

Traditional AI often classifies information or predicts an outcome, such as detecting spam. Generative AI creates new content, such as writing a reply to an email.

### 3. What is an LLM?

An LLM is a Large Language Model trained on a very large amount of text. It processes prompts and generates language by predicting suitable tokens based on context.

### 4. What is prompt engineering?

Prompt engineering is the practice of giving an AI model clear instructions, context, constraints, and an expected output format to improve its response.

### 5. What is a hallucination?

A hallucination is a fluent but incorrect or invented AI response. We can reduce it by grounding answers in trusted data through RAG, improving prompts, and validating the output.

### 6. What is RAG?

RAG stands for Retrieval-Augmented Generation. It retrieves relevant information from an external source and supplies it to the model so that the model can produce a more accurate and up-to-date answer.

### 7. What is the difference between RAG and fine-tuning?

RAG supplies knowledge to a model at request time, while fine-tuning changes model behavior by training it further with selected examples. RAG is generally suitable for current documents; fine-tuning is useful for consistent style or specialized behavior.

### 8. What are embeddings?

Embeddings are numerical representations of content. They allow an application to compare meaning and retrieve similar text, even when the exact words are different.

### 9. What is temperature?

Temperature controls response randomness. A low value produces more predictable answers, while a high value produces more diverse and creative answers.

### 10. What are the main risks of Generative AI?

The main risks include hallucination, bias, privacy leaks, insecure output, copyright concerns, harmful content, and over-reliance on AI without human review.

### 11. How would you build a chatbot for company documents?

I would use a RAG approach: split the documents into smaller sections, create embeddings, store them in a vector database, retrieve the most relevant sections for each question, and send those sections with the question to an LLM. I would also add access control, citations, monitoring, and output safety checks.

### 12. Can Generative AI replace humans?

Generative AI can automate and assist with many tasks, but it still needs human judgment for accuracy, ethics, creativity, and important decisions. It is usually best treated as a productivity tool rather than a complete replacement for people.

## 30-Second Interview Answer

> Generative AI is a type of artificial intelligence that learns patterns from large amounts of data and creates new content such as text, images, audio, or code. Large Language Models generate text by predicting tokens based on the prompt and its context. Applications can improve accuracy using techniques such as RAG, which provides relevant external information to the model. Generative AI improves productivity, but its output must be checked because it can hallucinate, contain bias, or expose privacy and security risks.

## Quick Revision

- Generative AI **creates new content**.
- An LLM processes and generates human language.
- A prompt is the instruction given to the model.
- Tokens are the small text units processed by an LLM.
- Transformers use attention to understand context.
- Embeddings represent meaning as numerical vectors.
- RAG gives relevant external knowledge to an LLM.
- Fine-tuning adapts model behavior using training examples.
- Temperature controls output randomness.
- AI agents use models and tools to perform multi-step tasks.
- AI responses can hallucinate and should be validated.
