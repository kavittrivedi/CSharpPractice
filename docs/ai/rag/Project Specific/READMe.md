## In my Project I have implemented Hybrid RAG (Vector + Semantic Search)

Yes. Now that I can see the code, I can say this confidently:

### Your implementation is **Vector RAG**, not Hybrid RAG.

The key part is:

```csharp
var vectorQuery = new VectorizedQuery(embedding)
{
    KNearestNeighborsCount = _options.TopK
};

vectorQuery.Fields.Add(VectorFieldName);
```

And then:

```csharp
searchOptions.VectorSearch.Queries.Add(vectorQuery);
```

You are sending **only a vector query** to Azure AI Search.

Also:

```csharp
_client.SearchAsync<SearchDocument>(null, searchOptions, cancellationToken)
```

The first parameter is `null`, meaning you are **not providing a keyword/full-text search query**.

### Your actual flow

```text
User Question
      ↓
Generate Embedding
      ↓
VectorizedQuery
      ↓
Azure AI Search
      ↓
K-nearest vector chunks
      ↓
Similarity score filtering
      ↓
Azure OpenAI
      ↓
Answer + Citations
```

So the correct description for your project is:

> **Vector-based RAG using Azure AI Search and embeddings.**

It would become **Hybrid RAG** if you combined **keyword/full-text search + vector search**, for example:

```text
                 User Question
                      ↓
            ┌─────────┴─────────┐
            ↓                   ↓
     Keyword Search       Vector Search
            ↓                   ↓
            └─────────┬─────────┘
                      ↓
               Ranked Results
                      ↓
                 Azure OpenAI
```

 **Your code clearly shows Vector RAG.**

 ## If I want to implement Hybrid RAG, what I have to do?

 Yes. Based on your current code, you already have the **vector-search part**. To make it **Hybrid RAG**, you need to add **keyword/full-text search** and combine it with vector search in the same Azure AI Search request.

### 1. Your current code

You currently have:

```csharp
var searchOptions = new SearchOptions
{
    Size = _options.TopK,
    VectorSearch = new VectorSearchOptions()
};

searchOptions.VectorSearch.Queries.Add(vectorQuery);

var response = await _client.SearchAsync<SearchDocument>(
    null,
    searchOptions,
    cancellationToken);
```

The first parameter is `null`, so there is no text/keyword query.

---

### 2. Change it to Hybrid Search

Pass the user's original question to `SearchAsync()` in addition to the embedding:

```csharp
public async Task<SearchResult> SearchAsync(
    string query,
    float[] embedding,
    CancellationToken cancellationToken)
{
    var vectorQuery = new VectorizedQuery(embedding)
    {
        KNearestNeighborsCount = _options.TopK
    };

    vectorQuery.Fields.Add(VectorFieldName);

    var searchOptions = new SearchOptions
    {
        Size = _options.TopK,
        VectorSearch = new VectorSearchOptions()
    };

    // Keyword / full-text search
    searchOptions.SearchFields.Add("content");

    // Vector search
    searchOptions.VectorSearch.Queries.Add(vectorQuery);

    var response = await _client.SearchAsync<SearchDocument>(
        query,
        searchOptions,
        cancellationToken);

    // Process results...
}
```

The important difference is:

```csharp
SearchAsync<SearchDocument>(
    query,              // ← keyword/full-text search
    searchOptions,      // ← vector search also configured
    cancellationToken);
```

Instead of:

```csharp
SearchAsync<SearchDocument>(
    null,
    searchOptions,
    cancellationToken);
```

### 3. What happens now?

Suppose the user asks:

> "How do I reset my MFA password?"

Azure AI Search can use:

**Keyword search**

```text
MFA
password
reset
```

and **vector search**

```text
"How can I regain access when my authentication credentials need to be changed?"
```

The vector search understands semantic similarity, while keyword search can match important exact terms.

Conceptually:

```text
                  User Question
                        │
              ┌─────────┴─────────┐
              │                   │
              ▼                   ▼
       Keyword Search       Vector Search
       (BM25/text)          (embeddings)
              │                   │
              └─────────┬─────────┘
                        ▼
                Azure AI Search
                Hybrid Ranking
                        │
                        ▼
                 Relevant Chunks
                        │
                        ▼
                   Azure OpenAI
                        │
                        ▼
                  Final Answer
```

### 4. One important point

Don't manually retrieve keyword results and vector results separately and simply concatenate them.

Azure AI Search supports **hybrid search**, where the text and vector queries are sent together and Azure AI Search combines/ranks the results. It can also use **semantic ranking** on top of the hybrid results.

So there are three related concepts:

| Approach          | Your query                       |
| ----------------- | -------------------------------- |
| Vector RAG        | Vector query only                |
| **Hybrid RAG**    | **Text + vector query**          |
| Hybrid + Semantic | Text + vector + semantic ranking |

For your existing project, the natural next step is:

**Current:** Vector RAG → **Next:** Hybrid RAG → optionally **Hybrid + Semantic Ranking**.

## My RAG based chatbot will be used by 20000+ user. How should I design my architecture?

For **20,000+ users**, I would not design your RAG chatbot as a single .NET API + single Azure AI Search + single Azure OpenAI deployment. The key is to make the **application/orchestration layer stateless and horizontally scalable**, while deliberately planning capacity for the AI model and search tiers. Microsoft’s current Azure architecture guidance follows this same separation of orchestration, knowledge, inference, and conversation layers. ([Microsoft Learn][1])

### Recommended architecture

```text
                         ┌──────────────────────┐
                         │   20,000+ Users      │
                         │ Angular / Web / App   │
                         └──────────┬───────────┘
                                    │
                                    ▼
                    ┌──────────────────────────────┐
                    │ Azure Front Door + WAF       │
                    │ Global routing / protection  │
                    └──────────────┬───────────────┘
                                   │
                                   ▼
                 ┌──────────────────────────────────┐
                 │      .NET 10 RAG API              │
                 │      Stateless + Auto Scaling     │
                 │                                  │
                 │  ┌────────────┐ ┌─────────────┐ │
                 │  │Orchestrator│ │Auth/Guardrail│ │
                 │  └────────────┘ └─────────────┘ │
                 └───────┬───────────┬─────────────┘
                         │           │
             ┌───────────┘           └──────────────┐
             ▼                                      ▼
   ┌──────────────────────┐              ┌─────────────────────┐
   │ Azure AI Search      │              │ Azure OpenAI        │
   │                      │              │                     │
   │ Hybrid Search        │              │ Chat/Reasoning      │
   │ Vector + Keyword     │              │ Embeddings          │
   │ Semantic Ranking     │              │ Multiple deployments│
   └──────────┬───────────┘              └─────────────────────┘
              │
              ▼
      ┌─────────────────┐
      │ Blob Storage    │
      │ Source Documents│
      └─────────────────┘

          Conversation / Cache
                  │
        ┌─────────┴──────────┐
        ▼                    ▼
 Azure Managed Redis    Cosmos DB
 Cache                  Chat history
                        User/session data


       Asynchronous ingestion
                  │
                  ▼
       ┌─────────────────────┐
       │ Service Bus / Queue │
       └──────────┬──────────┘
                  ▼
       ┌─────────────────────┐
       │ Azure Functions     │
       │ Document Processing │
       └──────────┬──────────┘
                  ▼
       Chunk → Embed → Index
```

Microsoft's RAG guidance describes essentially the same logical flow: application → orchestrator → Azure AI Search → prompt/context → language model. ([Microsoft Learn][2])

## 1. Make your .NET API stateless

This is probably the **most important architectural decision**.

Don't store conversation state like this:

```text
API Server 1
   └── User A conversation

API Server 2
   └── User B conversation
```

Instead:

```text
                 Load Balancer
                      │
       ┌──────────────┼──────────────┐
       ▼              ▼              ▼
   API Instance 1  API Instance 2  API Instance 3
       │              │              │
       └──────────────┼──────────────┘
                      │
               External state
             Redis / Cosmos DB
```

Then you can scale:

```text
5 instances
   ↓
10 instances
   ↓
30 instances
```

without changing the application design.

Azure's architecture guidance specifically identifies API/orchestration layers as stateless layers that can scale horizontally. ([Microsoft Learn][1])

---

# 2. Don't size infrastructure based only on "20,000 users"

This is a very important interview point.

**20,000 registered users ≠ 20,000 simultaneous requests.**

You need:

```text
Total users
      ↓
Concurrent users
      ↓
Requests per second
      ↓
Tokens per request
      ↓
Tokens per minute
      ↓
Azure OpenAI capacity
```

For example, suppose:

```text
20,000 registered users

5% simultaneously active
       ↓
1,000 active users

20% of them send a request
       ↓
200 concurrent RAG requests
```

Your actual capacity requirement is therefore driven by **concurrency, RPS, prompt tokens, output tokens and latency**, not simply the 20,000 number.

---

# 3. Azure OpenAI is likely to become your major bottleneck

Your flow is:

```text
User
 ↓
.NET API
 ↓
Azure AI Search
 ↓
Azure OpenAI
 ↓
Response
```

Even if your .NET API can handle thousands of requests, the model deployment has its own throughput limits.

So don't design:

```text
20,000 users
      ↓
Single Azure OpenAI deployment
```

Instead, plan model capacity according to measured TPM/RPM requirements.

For high predictable workloads, evaluate **Provisioned Throughput Units (PTUs)**. PTU capacity is region-specific and the throughput delivered per PTU depends on the model. ([Microsoft Learn][3])

Conceptually:

```text
                  Azure OpenAI
                       │
             ┌─────────┼─────────┐
             ▼         ▼         ▼
        Deployment A Deployment B Deployment C
          Region 1      Region 1     Region 2
```

Your application can use appropriate deployments/regions based on capacity, availability and latency requirements.

---

# 4. Azure AI Search must also scale

For your current architecture, I would keep:

```text
Azure AI Search
      │
      ├── Keyword search
      ├── Vector search
      ├── Semantic ranking
      └── Metadata filtering
```

You already use **Hybrid Search**, which is a good fit for enterprise RAG.

For 20K+ users, don't think:

> "I need one AI Search service."

Think:

```text
AI Search Service
      │
      ├── Partitions → storage/index capacity
      │
      └── Replicas → query concurrency
```

The exact number should come from load testing against your actual index size, query complexity and target latency.

Microsoft's RAG guidance explicitly includes vector, full-text, hybrid and multiple-search strategies as architectural choices that should be evaluated rather than assumed. ([Microsoft Learn][2])

---

# 5. Add Redis for frequently repeated questions

This can significantly reduce unnecessary calls to Search + LLM.

For example:

```text
User:
"What is the maximum hotel reimbursement?"
             │
             ▼
        .NET API
             │
             ▼
      Redis Cache?
        /        \
      HIT        MISS
       │           │
       ▼           ▼
    Return     AI Search
    answer          │
                    ▼
               Azure OpenAI
                    │
                    ▼
                  Redis
                    │
                    ▼
                 Response
```

Azure's current scalable AI-agent architecture also includes Azure Managed Redis as a supporting component. ([Microsoft Learn][4])

But **don't blindly cache every response**.

Cache things such as:

* repeated identical questions
* frequently requested knowledge
* embeddings/query results where appropriate
* session-related short-lived state

Be careful with personalized answers and security-sensitive data.

---

# 6. Conversation history should not live in memory

For example:

```text
User:
"Tell me about leave policy."

Then:

"What about carry forward?"
```

The second question needs conversation context.

Don't keep that context only inside your .NET process:

```csharp
Dictionary<string, ChatHistory>
```

because request #2 could hit another API instance.

Instead:

```text
.NET API
   │
   ├── Redis → short-lived conversation/session state
   │
   └── Cosmos DB → persistent conversation history
```

This allows any API instance to handle any user.

---

# 7. Separate online RAG from document ingestion

Don't process documents synchronously through your chatbot API.

Use:

```text
                    Document Upload
                          │
                          ▼
                    Blob Storage
                          │
                          ▼
                    Service Bus
                          │
                          ▼
                 Azure Function
                          │
            ┌─────────────┼──────────────┐
            ▼             ▼              ▼
         Chunking      Embedding      Metadata
            │             │              │
            └─────────────┼──────────────┘
                          ▼
                   Azure AI Search
```

Therefore:

```text
User chat traffic
       ≠
Document processing traffic
```

This is very important for reliability.

A large document ingestion job shouldn't consume resources needed for your 20K users.

---

# 8. Put authentication and authorization before RAG

For enterprise users:

```text
Angular
   ↓
Microsoft Entra ID
   ↓
.NET API
   ↓
Authorization
   ↓
RAG
```

And importantly, apply **security trimming at retrieval time**.

For example:

```text
Document A
Department = HR

Document B
Department = Finance

Document C
Department = IT
```

If user belongs to HR:

```text
Azure AI Search query

filter:
Department eq 'HR'
```

You don't want:

```text
Search everything
      ↓
LLM decides what user is allowed to see
```

Authorization should happen in your application/search layer, not be delegated to the LLM.

Microsoft's current multitenant RAG guidance emphasizes application-controlled retrieval when you need control over tenant routing and authorization enforcement. ([Microsoft Learn][5])

---

# 9. Use asynchronous processing wherever possible

For example, don't make this:

```text
POST /upload
     ↓
Extract PDF
     ↓
Chunk
     ↓
Generate 5,000 embeddings
     ↓
Index
     ↓
Return HTTP response
```

Instead:

```text
POST /upload
     ↓
Blob Storage
     ↓
Queue
     ↓
202 Accepted
```

Then:

```text
Queue
  ↓
Function
  ↓
Process document
  ↓
Update Search
```

The user gets:

```text
Document uploaded successfully.
Processing started.
```

This is much more scalable.

---

# 10. Add rate limiting and backpressure

With 20K+ users, you need protection from traffic spikes.

For example:

```text
                 API
                  │
             Rate Limiter
                  │
       ┌──────────┼──────────┐
       ▼          ▼          ▼
      User A     User B     User C
```

You can enforce limits such as:

```text
Per user:
30 requests/minute

Per tenant:
500 requests/minute

Global:
X requests/second
```

The exact values should come from your workload testing and model/search quotas.

Also implement:

* timeout
* retry with exponential backoff
* circuit breaker
* cancellation
* request queueing where appropriate
* graceful degradation

---

# 11. Streaming is important for user experience

Instead of:

```text
User
  │
  │────────────── 8 seconds ──────────────│
  │                                       │
  │                                  Complete response
```

use:

```text
User
  │
  │── "According..." ──►
  │── "to the policy..." ►
  │── "the limit is..." ►
```

Your architecture can be:

```text
Angular
   │
   │ SSE / streaming
   ▼
.NET API
   │
   ▼
Azure OpenAI
   │
   └──── streaming tokens ────►
```

This doesn't necessarily reduce model computation, but it greatly improves perceived latency.

---

# 12. Add observability from day one

For 20K+ users, you need to answer:

> Why was this request slow?

Your telemetry should track:

```text
CorrelationId
UserId
Request duration

Search duration
Search result count

LLM duration
Input tokens
Output tokens

Model deployment

Cache HIT/MISS

Error
Retry count

Time to first token
Total response time
```

Use:

```text
Application Insights
        +
Azure Monitor
        +
OpenTelemetry
```

For example:

```text
Request
  │
  ├── API = 100 ms
  ├── Redis = 5 ms
  ├── AI Search = 180 ms
  ├── Azure OpenAI = 3.2 sec
  └── Total = 3.5 sec
```

Now you know where the bottleneck is.

---

# 13. Production architecture I would recommend for your project

Based on the RAG architecture you've been building, I would evolve it to:

```text
                         USERS
                           │
                           ▼
                 ┌──────────────────┐
                 │ Azure Front Door │
                 │       + WAF      │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │ Angular Frontend │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │ Entra ID / Auth  │
                 └────────┬─────────┘
                          │
                          ▼
              ┌─────────────────────────┐
              │ .NET RAG API            │
              │ Stateless                │
              │ Auto-scaled               │
              └───────────┬─────────────┘
                          │
             ┌────────────┼─────────────┐
             │            │             │
             ▼            ▼             ▼
          Redis       AI Search      Cosmos DB
          Cache       Hybrid Search  Chat History
             │            │
             │            │
             │            ▼
             │      Azure OpenAI
             │      ┌─────────────┐
             │      │ Chat Model  │
             │      │ Embeddings  │
             │      └─────────────┘
             │
             └───────────────────────┐
                                     │
                              Application Insights
                                + Azure Monitor


              DOCUMENT INGESTION
                       │
                       ▼
                 Blob Storage
                       │
                       ▼
                  Service Bus
                       │
                       ▼
                Azure Functions
                       │
              ┌────────┼────────┐
              ▼        ▼        ▼
           Extract   Chunk    Embed
                       │
                       ▼
                 Azure AI Search
```

This is very close to the production architecture I'd discuss in an interview.

---

## One important distinction: 20K users vs 20K concurrent users

If the interviewer says:

> **"We have 20,000 users. How would you design the RAG architecture?"**

I would first say:

> **"I would not size the architecture based only on 20,000 users. I would first determine peak concurrent users, requests per second, average input/output tokens, target latency, document/index size and availability requirements. Then I would horizontally scale the stateless API and provision Azure AI Search and Azure OpenAI based on measured throughput."**

That is a much stronger architecture answer than simply saying "use AKS and multiple servers."

---

## And for your specific RAG project

Your current architecture:

```text
Angular
   ↓
Rag.Api
   ↓
Azure AI Search
   ↓
Azure OpenAI
```

can evolve naturally into:

```text
                  ┌─────────────┐
                  │   Angular   │
                  └──────┬──────┘
                         │
                         ▼
                Front Door + WAF
                         │
                         ▼
              ┌────────────────────┐
              │ Multiple Rag.Api   │
              │ Stateless instances│
              └─────────┬──────────┘
                        │
            ┌───────────┼────────────┐
            ▼           ▼            ▼
          Redis      AI Search    Cosmos DB
            │           │
            │           ▼
            │      Azure OpenAI
            │
            ▼
       Chat/session
         state
```

And your existing:

```text
Blob
 ↓
Queue
 ↓
Rag.Functions
 ↓
Embedding
 ↓
AI Search
```

can remain as the **separate asynchronous ingestion pipeline**.

Microsoft's current Azure guidance also recommends evaluating standard RAG versus agentic RAG based on whether the workload needs fixed retrieval or dynamic multi-step/tool-based reasoning. ([Microsoft Learn][2])

**For your 20K-user scenario, I would keep the core chatbot as standard RAG initially**—Hybrid Search + reranking + Azure OpenAI—and introduce agentic behavior only where the business scenario actually requires multi-step retrieval or actions. That keeps latency, cost, and operational complexity more predictable.

[1]: https://learn.microsoft.com/en-us/azure/well-architected/ai/architecture-pattern?utm_source=chatgpt.com "Architecture pattern for AI workloads on Azure - Microsoft Azure Well-Architected Framework | Microsoft Learn"
[2]: https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/rag/rag-solution-design-and-evaluation-guide?utm_source=chatgpt.com "Design and Develop a RAG Solution on Azure - Azure Architecture Center | Microsoft Learn"
[3]: https://learn.microsoft.com/en-us/azure/foundry/openai/concepts/provisioned-throughput?utm_source=chatgpt.com "Provisioned throughput for Foundry Models - Microsoft Foundry | Microsoft Learn"
[4]: https://learn.microsoft.com/en-us/azure/architecture/solution-ideas/articles/ai-agents-at-scale?utm_source=chatgpt.com "Dynamic AI Agents at Scale Pattern - Azure Architecture Center | Microsoft Learn"
[5]: https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/secure-multitenant-rag?utm_source=chatgpt.com "Design a Secure Multitenant RAG Inferencing Solution - Azure Architecture Center | Microsoft Learn"
