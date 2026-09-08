# AWS Bedrock and Knowledge Bases - Interview Notes

## 1. One-Line Answer

Amazon Bedrock is a fully managed AWS service for building generative AI applications using foundation models from Amazon and third-party providers without managing ML infrastructure.

Amazon Bedrock Knowledge Bases is the managed RAG feature in Bedrock. It connects your private data to foundation models by ingesting documents, converting them into embeddings, storing them in a vector store, retrieving relevant chunks at query time, and generating grounded answers with citations.

Remember this:

```text
Bedrock = managed foundation models
Knowledge Base = managed RAG over your data
Agent = reasoning + tool use + optional knowledge base
```

## 2. Why AWS Bedrock Exists

Before Bedrock, teams had to choose models, host them, scale GPUs, secure endpoints, fine-tune, monitor, and integrate everything manually.

Bedrock simplifies this by giving:

- Access to many foundation models through one AWS service.
- Serverless model invocation.
- Enterprise security with IAM, VPC options, encryption, logging, and guardrails.
- Features for RAG, agents, evaluation, fine-tuning/customization, and prompt management.
- Integration with the AWS ecosystem.

Interview answer:

> Bedrock lets companies build GenAI applications without owning the model infrastructure. They can choose a model, call it through AWS APIs, secure it with AWS controls, and add enterprise features like RAG, agents, guardrails, and monitoring.

## 3. Foundation Model Basics

A foundation model is a large pre-trained AI model that can perform many tasks, such as:

- Text generation
- Summarization
- Question answering
- Code generation
- Image generation
- Embeddings
- Multimodal understanding

Bedrock provides models from multiple providers, depending on region and availability, such as:

- Amazon Nova
- Amazon Titan
- Anthropic Claude
- Cohere
- Meta Llama
- Mistral AI
- Stability AI

Do not memorize every model name for interview. Memorize the model categories:

```text
Text/chat model       -> generate answers
Embedding model      -> convert text/images to vectors
Image model          -> generate or edit images
Reranker model       -> improve retrieved document ranking
```

## 4. Core Bedrock Features

### Model Invocation

You call a model using Bedrock APIs or SDKs.

Common runtime APIs:

- `InvokeModel` - invoke a foundation model.
- `InvokeModelWithResponseStream` - stream model output.
- `Converse` - unified conversational API for supported models.
- `ConverseStream` - streaming conversation API.

Typical flow:

```text
Application -> Amazon Bedrock Runtime -> Foundation Model -> Response
```

### Model Choice

Different models are good for different use cases.

Examples:

- Fast and cheap chatbot: smaller text model.
- Complex reasoning: stronger large language model.
- Search over documents: embedding model plus text generation model.
- Enterprise RAG: Knowledge Base plus model.
- Image generation: image model.

Interview sentence:

> In Bedrock, model selection is based on latency, cost, reasoning quality, context length, modality, and compliance requirements.

### Guardrails

Guardrails help control model behavior.

They can be used to:

- Block harmful categories.
- Filter sensitive information.
- Restrict topics.
- Apply denied words or phrases.
- Reduce unsafe or off-policy responses.

Interview sentence:

> Guardrails are not a replacement for application security, but they are an important safety layer around model input and output.

### Agents

Agents for Amazon Bedrock allow a model to reason, plan, call tools/APIs, and optionally use a Knowledge Base.

Agent pattern:

```text
User asks a task
Agent decides what is needed
Agent may retrieve knowledge
Agent may call an action group/API
Agent returns final answer
```

Example:

User: "Book a service appointment for my car."

Agent can:

- Ask for missing details.
- Search available slots.
- Call a booking API.
- Confirm the appointment.

### Model Customization

Bedrock supports ways to adapt models, depending on model support:

- Prompt engineering
- RAG with Knowledge Bases
- Fine-tuning
- Continued pre-training
- Model evaluation

Important interview distinction:

```text
Prompt engineering = change instruction
RAG = add external knowledge at runtime
Fine-tuning = change model behavior using training examples
Continued pre-training = adapt model to domain language/data
```

## 5. What Problem Knowledge Bases Solve

Large language models have limitations:

- They may not know your private company data.
- Their training data may be outdated.
- They can hallucinate.
- They may not cite sources.
- Sending the entire document set in every prompt is too expensive and slow.

Knowledge Bases solve this using Retrieval Augmented Generation, or RAG.

RAG means:

```text
Retrieve relevant data first.
Augment the prompt with that data.
Generate an answer grounded in that data.
```

Easy memory:

```text
RAG = Search first, answer second.
```

## 6. Knowledge Base Architecture

High-level architecture:

```text
Data Source
   |
   v
Ingestion Job
   |
   v
Chunking
   |
   v
Embedding Model
   |
   v
Vector Store
   |
   v
Retrieve / RetrieveAndGenerate
   |
   v
Foundation Model Answer + Citations
```

### Main Components

#### 1. Data Source

Where your knowledge comes from.

Examples:

- Amazon S3 documents
- Web pages
- Confluence
- Salesforce
- SharePoint
- Custom data source
- Structured data sources, depending on supported configuration

Common files from S3 can include formats such as PDF, text, HTML, Markdown, Word, CSV, and other supported document types.

#### 2. Ingestion

Ingestion means Bedrock reads your data source and prepares it for retrieval.

During ingestion:

1. Documents are scanned.
2. Text is extracted.
3. Content is split into chunks.
4. Chunks are converted to embeddings.
5. Embeddings and metadata are stored in a vector store.

#### 3. Chunking

Chunking breaks large documents into smaller pieces.

Why chunking matters:

- Too large: retrieval becomes noisy.
- Too small: chunk may lose context.
- Good chunking improves answer quality.

Interview answer:

> Chunking is important because the model does not retrieve whole documents in most RAG systems. It retrieves relevant pieces. The chunk size and overlap affect recall, precision, and answer quality.

#### 4. Embeddings

An embedding is a numeric vector representation of text, image, or multimodal content.

Meaning:

```text
"refund policy" -> [0.12, -0.54, 0.91, ...]
```

Similar meanings produce nearby vectors.

Example:

- "How can I return my product?"
- "What is the refund process?"

These may have similar embeddings even if the words are different.

#### 5. Vector Store

A vector store stores embeddings and supports similarity search.

Bedrock Knowledge Bases can work with AWS and third-party vector stores, depending on region and configuration, such as:

- Amazon OpenSearch Serverless
- Amazon OpenSearch Service managed clusters
- Amazon Aurora PostgreSQL-Compatible Edition / pgvector
- Amazon Neptune Analytics
- Amazon S3 Vectors
- Pinecone
- Redis Enterprise Cloud
- MongoDB Atlas

Interview sentence:

> The vector store is where the embedded chunks live. At query time, Bedrock searches this store to find the chunks most semantically similar to the user question.

#### 6. Retrieval

When a user asks a question:

1. The question is converted into an embedding.
2. The vector store finds similar chunks.
3. Optional filters or reranking can improve relevance.
4. The retrieved chunks are sent as context to the model.

#### 7. Generation

The foundation model uses the retrieved chunks to generate an answer.

The generated answer can include citations to source documents.

## 7. Query APIs: Retrieve vs RetrieveAndGenerate

### Retrieve

`Retrieve` only searches the Knowledge Base and returns relevant chunks.

Use it when:

- You want to build your own prompt.
- You want custom ranking or filtering logic.
- You want full control over generation.

Flow:

```text
Question -> Retrieve -> Relevant chunks -> Your app handles generation
```

### RetrieveAndGenerate

`RetrieveAndGenerate` retrieves chunks and asks a model to generate the answer.

Use it when:

- You want AWS to manage the RAG flow.
- You want quick implementation.
- You want answer generation with citations.

Flow:

```text
Question -> RetrieveAndGenerate -> Answer + citations
```

Interview answer:

> Retrieve gives me raw relevant results. RetrieveAndGenerate gives me the complete RAG answer by retrieving context and invoking a model.

## 8. Knowledge Base Creation Flow

Steps:

1. Choose or create a Knowledge Base.
2. Select data source.
3. Choose parsing/chunking strategy.
4. Choose embedding model.
5. Choose vector store.
6. Configure IAM role and permissions.
7. Sync or ingest data.
8. Test retrieval and generated answers.
9. Connect to application or Bedrock Agent.

Memory shortcut:

```text
Source -> Chunk -> Embed -> Store -> Retrieve -> Generate
```

## 9. Example Use Case

Suppose a company has HR policy PDFs in S3.

Goal:

> Build a chatbot that answers employee questions using only approved HR documents.

Architecture:

```text
HR PDFs in S3
   |
Bedrock Knowledge Base sync
   |
Titan/Cohere embedding model
   |
OpenSearch Serverless vector index
   |
Application calls RetrieveAndGenerate
   |
Claude/Nova model generates answer
   |
Employee receives answer with citations
```

Question:

> "How many paid leaves can I carry forward?"

RAG behavior:

1. Search HR policy chunks.
2. Retrieve leave-policy section.
3. Send relevant text to model.
4. Generate answer.
5. Cite the HR policy document.

## 10. Why Not Just Fine-Tune?

This is a common interview question.

Use RAG when:

- Knowledge changes often.
- You need citations.
- You need private documents.
- You want easier updates.
- You want to reduce hallucination.

Use fine-tuning when:

- You want the model to follow a style or format.
- You have repeated task examples.
- You want behavior adaptation, not just knowledge injection.

Best answer:

> For company documents, I usually prefer RAG over fine-tuning because documents change frequently and RAG can provide citations. Fine-tuning is better when I need to teach the model a behavior, tone, or task pattern.

## 11. Security and Governance

Important security points:

- Access is controlled through IAM.
- Bedrock uses service roles to access data sources and vector stores.
- Data can be encrypted with AWS KMS.
- Network access can be restricted using VPC/private connectivity where supported.
- Logging and monitoring can be integrated with CloudWatch and CloudTrail.
- Guardrails can filter unsafe or sensitive inputs/outputs.
- Data permissions must be designed carefully so users only retrieve documents they are allowed to see.

Interview sentence:

> In production, the most important RAG security concern is authorization. The system must not retrieve chunks from documents the user is not allowed to access.

## 12. RAG Quality Tuning

If answers are poor, check:

- Data quality: Are documents clean and up to date?
- Chunking: Are chunks too large or too small?
- Metadata: Can we filter by department, product, region, date, or access level?
- Embedding model: Is it suitable for language/domain/multimodal data?
- Retrieval count: Are we retrieving enough chunks?
- Reranking: Are best chunks moved to the top?
- Prompt: Is the model instructed to answer only from context?
- Generation model: Is it strong enough for reasoning?
- Evaluation: Are we testing with real user questions?

Debug flow:

```text
Bad answer?
   |
   v
Did retrieval find the right chunks?
   |
   +-- No -> fix data, chunking, embeddings, filters, reranking
   |
   +-- Yes -> fix prompt, model, guardrails, generation settings
```

Very useful interview line:

> In RAG, first debug retrieval, then debug generation. If the right context is not retrieved, even the best model will answer poorly.

## 13. Hallucination Handling

Bedrock Knowledge Bases reduce hallucination by grounding answers in retrieved documents, but they do not magically remove all hallucination.

Good practices:

- Ask the model to answer only from provided context.
- Return citations.
- Say "I don't know" when context is insufficient.
- Use guardrails.
- Evaluate answers against test questions.
- Monitor user feedback.

Interview answer:

> RAG reduces hallucination because the model receives relevant source context at runtime, but I still need prompt constraints, citations, retrieval tuning, and evaluation.

## 14. Metadata Filtering

Metadata helps narrow retrieval.

Example metadata:

```json
{
  "department": "HR",
  "region": "India",
  "documentType": "policy",
  "year": "2026"
}
```

Question:

> "What is the maternity leave policy in India?"

Useful filter:

```text
department = HR
region = India
```

Why it matters:

- Improves relevance.
- Reduces wrong-context retrieval.
- Supports access control patterns.
- Reduces token usage.

## 15. Reranking

Initial vector search may return many related chunks, but not always the best ones.

Reranking means a model or ranking algorithm reorders retrieved chunks by relevance before generation.

Interview sentence:

> Reranking improves answer quality by making sure the most relevant retrieved chunks are placed at the top of the context sent to the model.

## 16. Agents + Knowledge Bases

A Knowledge Base answers from data.

An Agent can:

- Use a Knowledge Base for information.
- Call APIs through action groups.
- Ask follow-up questions.
- Complete multi-step tasks.

Example:

User:

> "Can I upgrade my insurance plan, and if yes, submit the request."

Agent flow:

1. Query Knowledge Base for upgrade policy.
2. Ask user for missing details.
3. Call backend API to submit upgrade request.
4. Return confirmation.

Memory:

```text
Knowledge Base = knows
Agent = knows + acts
```

## 17. Common Architecture Patterns

### Simple Chatbot

```text
Frontend -> API Gateway/Lambda -> Bedrock model -> Response
```

### RAG Chatbot

```text
Frontend -> Backend -> Bedrock Knowledge Base -> Bedrock model -> Cited answer
```

### Agentic Workflow

```text
Frontend -> Bedrock Agent -> Knowledge Base + Action Groups -> Final response
```

### Enterprise RAG

```text
Documents -> S3
Ingestion -> Bedrock Knowledge Base
Vectors -> OpenSearch/Aurora/S3 Vectors/etc.
App -> RetrieveAndGenerate
Security -> IAM + KMS + filters + Guardrails
Monitoring -> CloudWatch/CloudTrail
```

## 18. Important Interview Comparisons

### Bedrock vs SageMaker

Bedrock:

- Use managed foundation models.
- Faster GenAI application development.
- Less infrastructure management.
- Best for model consumption, RAG, agents, guardrails.

SageMaker:

- Build, train, tune, deploy ML models with more control.
- Best for custom ML lifecycle and advanced model training/deployment.

Answer:

> Bedrock is usually chosen when I want managed access to foundation models and GenAI features quickly. SageMaker is chosen when I need full ML training, hosting, and lifecycle control.

### RAG vs Prompt Engineering

Prompt engineering:

- Improves instructions.
- Does not add new private knowledge unless included in the prompt.

RAG:

- Retrieves external knowledge dynamically.
- Better for large or changing document sets.

### RAG vs Fine-Tuning

RAG:

- Adds knowledge at runtime.
- Easier to update.
- Supports citations.

Fine-tuning:

- Changes model behavior.
- Requires training data.
- Does not automatically keep knowledge current.

### Vector Search vs Keyword Search

Keyword search:

- Matches exact words.
- Good for IDs, names, exact phrases.

Vector search:

- Matches semantic meaning.
- Good for natural language questions.

Best systems can combine both.

## 19. Common Interview Questions and Answers

### Q1. What is Amazon Bedrock?

Amazon Bedrock is a fully managed AWS service that provides access to foundation models through APIs. It helps build generative AI applications with features like model invocation, Knowledge Bases for RAG, Agents, Guardrails, model evaluation, and model customization.

### Q2. What is a Knowledge Base in Bedrock?

A Knowledge Base is a managed RAG capability. It connects data sources to a vector store, uses embedding models to index content, retrieves relevant chunks for a user query, and can generate grounded responses using a foundation model.

### Q3. What is RAG?

RAG stands for Retrieval Augmented Generation. It retrieves relevant information from external data sources and provides that context to a language model so the model can generate a more accurate, grounded answer.

### Q4. Why do we need embeddings?

Embeddings convert text or other content into numerical vectors so semantic similarity search becomes possible. This lets the system find relevant content even when the user uses different words than the original document.

### Q5. What happens during Knowledge Base ingestion?

Bedrock reads the data source, extracts content, splits it into chunks, converts chunks into embeddings using an embedding model, and stores the embeddings with metadata in a vector store.

### Q6. What is the difference between Retrieve and RetrieveAndGenerate?

`Retrieve` returns relevant chunks from the Knowledge Base. `RetrieveAndGenerate` retrieves relevant chunks and then uses a foundation model to generate an answer with source citations.

### Q7. How do you reduce hallucination in Bedrock?

Use RAG with Knowledge Bases, instruct the model to answer only from retrieved context, return citations, use guardrails, tune retrieval quality, and evaluate responses against known questions.

### Q8. How do you secure a Knowledge Base?

Use IAM roles and least privilege, encrypt data with KMS, secure data sources and vector stores, use CloudTrail/CloudWatch for audit and monitoring, apply guardrails, and enforce document-level authorization through metadata or application logic.

### Q9. What is a vector store?

A vector store is a database optimized for storing embeddings and performing similarity search. It returns chunks that are semantically close to the user query.

### Q10. When would you use an Agent instead of only a Knowledge Base?

Use a Knowledge Base when the app only needs question answering from documents. Use an Agent when the app needs multi-step reasoning, follow-up questions, API calls, or actions in external systems.

### Q11. Why might a Knowledge Base return a wrong answer?

Possible causes:

- Wrong or outdated source data.
- Bad chunking.
- Poor metadata filtering.
- Embedding model mismatch.
- Not enough chunks retrieved.
- Missing reranking.
- Weak generation prompt.
- User does not have clear query intent.

### Q12. How do you improve Knowledge Base performance?

Tune chunking, use metadata filters, choose a good embedding model, retrieve an appropriate number of chunks, use reranking, clean the source documents, and evaluate with real queries.

### Q13. Does Bedrock train the model on my private data?

For typical Bedrock inference and Knowledge Base usage, your data is used to process your request and retrieve/generate responses. You should still explain data handling based on the organization's AWS configuration and current AWS service terms. In an interview, say that Bedrock is designed for enterprise-controlled data use and security, and you would verify the exact compliance and data-retention requirements from AWS documentation and company policy.

### Q14. What is the role of metadata in RAG?

Metadata allows filtering and better retrieval. For example, filtering documents by country, department, product, document type, date, or access level improves relevance and security.

### Q15. What is chunk overlap?

Chunk overlap means repeating some text between adjacent chunks so important context is not lost at chunk boundaries.

Example:

```text
Chunk 1: lines 1-100
Chunk 2: lines 80-180
```

This helps when an answer depends on content near the boundary.

### Q16. What is reranking?

Reranking reorders initially retrieved chunks using a stronger relevance model or ranking logic so the best context is sent to the generator.

### Q17. What are Guardrails in Bedrock?

Guardrails are safety and policy controls that can filter or block harmful, sensitive, or off-policy content in model interactions.

### Q18. What is the difference between semantic search and keyword search?

Semantic search uses embeddings to match meaning. Keyword search matches exact words. Semantic search is better for natural language questions, while keyword search is better for exact identifiers.

### Q19. How would you design a production Bedrock RAG application?

I would store documents in S3 or a supported source, create a Bedrock Knowledge Base, choose an embedding model, store vectors in a supported vector store, run ingestion jobs, expose an application backend that calls `RetrieveAndGenerate` or `Retrieve`, add IAM/KMS/security controls, use metadata filters for authorization, apply guardrails, log requests, and evaluate answer quality continuously.

### Q20. What should you monitor?

Monitor:

- Latency
- Cost
- Token usage
- Retrieval quality
- Answer quality
- Failed ingestion jobs
- Data freshness
- Guardrail interventions
- User feedback
- Access/audit logs

## 20. Two-Minute Interview Explanation

Use this when the interviewer says: "Explain AWS Bedrock and Knowledge Bases."

> Amazon Bedrock is AWS's fully managed service for building generative AI applications using foundation models from Amazon and other providers. Instead of hosting models ourselves, we call Bedrock APIs for chat, text generation, embeddings, image generation, and other AI tasks. Bedrock also provides enterprise features like Guardrails, Agents, Knowledge Bases, model evaluation, and model customization.
>
> Knowledge Bases for Amazon Bedrock is the managed RAG feature. RAG means Retrieval Augmented Generation. The idea is that the model should first retrieve relevant company data, then generate an answer using that context. During ingestion, Bedrock reads documents from a data source like S3, chunks them, converts chunks into embeddings using an embedding model, and stores those embeddings in a vector store like OpenSearch Serverless, Aurora pgvector, S3 Vectors, Pinecone, Redis, or MongoDB Atlas. At query time, Bedrock converts the user question into an embedding, retrieves similar chunks, optionally filters or reranks them, and then uses a foundation model to generate an answer with citations.
>
> I would use Knowledge Bases when I need a chatbot or application to answer from private or frequently changing documents. Compared with fine-tuning, RAG is easier to update and can provide citations. For production, I would focus on data quality, chunking, metadata filters, security, guardrails, monitoring, and evaluation.

## 21. Memory Map

```text
BEDROCK
B = Base/foundation models
E = Enterprise security
D = Different model providers
R = RAG with Knowledge Bases
O = Orchestration with Agents
C = Customization and evaluation
K = Keeps infrastructure managed
```

```text
KNOWLEDGE BASE
K = Keep company data connected
B = Break documents into chunks
E = Embed chunks
D = Database/vector store
R = Retrieve relevant chunks
O = Output grounded answer
C = Citations
K = Keep data synced
```

## 22. Quick Revision Sheet

Memorize these lines:

- Bedrock is managed access to foundation models.
- Knowledge Bases are managed RAG.
- RAG means search first, answer second.
- Embeddings convert meaning into vectors.
- Vector stores find semantically similar chunks.
- Ingestion means read, chunk, embed, and store.
- Retrieve returns chunks.
- RetrieveAndGenerate returns final answer with citations.
- Agents can use Knowledge Bases and call tools/APIs.
- Guardrails add safety controls.
- RAG is better than fine-tuning for changing private knowledge.
- Fine-tuning is better for behavior, format, and style.
- Debug RAG by checking retrieval before generation.
- Production RAG needs IAM, KMS, authorization, monitoring, and evaluation.

## 23. Official References

- [Amazon Bedrock overview](https://docs.aws.amazon.com/bedrock/latest/userguide/what-is-bedrock.html)
- [Amazon Bedrock Knowledge Bases](https://docs.aws.amazon.com/bedrock/latest/userguide/knowledge-base.html)
- [How Knowledge Bases work](https://docs.aws.amazon.com/bedrock/latest/userguide/kb-how-it-works.html)
- [Turning data into a Knowledge Base](https://docs.aws.amazon.com/bedrock/latest/userguide/kb-how-data.html)
- [RetrieveAndGenerate API](https://docs.aws.amazon.com/bedrock/latest/APIReference/API_agent-runtime_RetrieveAndGenerate.html)
- [Retrieve API](https://docs.aws.amazon.com/bedrock/latest/APIReference/API_agent-runtime_Retrieve.html)
- [Knowledge Bases with Agents](https://docs.aws.amazon.com/bedrock/latest/userguide/agents-kb-add.html)
