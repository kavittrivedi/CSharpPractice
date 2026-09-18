## If I am designing the infrastructure of  RAG Based AI application, what question should I ask?

If you are designing the **infrastructure architecture for a RAG-based AI application**, don't start by asking only *“Which Azure services should I use?”*.

Start with questions around **requirements → data → RAG pipeline → AI model → security → scalability → reliability → cost → operations**.

For an interview or architecture discussion, I would use this checklist.

### 1. Business & functional requirements

* What problem is the RAG application solving?
* Who are the users?
* What types of questions will users ask?
* Is this internal, customer-facing, or public?
* What response time is expected?
* Do responses need citations/references?
* Is the application read-only, or can the AI perform actions?

### 2. Data / document questions

* Where are the source documents stored?

  * Blob Storage?
  * SharePoint?
  * Database?
  * File system?
  * APIs?
* What document formats do we support — PDF, Word, Excel, HTML, etc.?
* How frequently does the data change?
* How do we detect new/updated/deleted documents?
* Do we need OCR for scanned documents?
* Are documents tenant/user/department specific?
* What is the total document size and expected growth?

### 3. Document ingestion pipeline

Ask:

* How will documents enter the RAG system?
* Is ingestion synchronous or asynchronous?
* Do we need a queue/event-driven architecture?
* How will we chunk documents?
* What chunk size and overlap should we use?
* How will embeddings be generated?
* Where will embeddings and metadata be stored?
* How do we handle ingestion failures?
* Do we need retry and dead-letter handling?
* How do we re-index documents?

For Azure, this could lead to something like:

**Blob Storage → Event Grid/Queue → Azure Function → Document Processing → Embedding Model → Azure AI Search**

### 4. Retrieval questions

This is one of the most important areas.

* Are we using **vector search**, keyword search, or hybrid search?
* How many documents/chunks are expected?
* What metadata should be indexed?
* Do we need metadata filtering?
* Do we need semantic ranking?
* How many chunks should be retrieved?
* Do we need reranking?
* How do we prevent irrelevant chunks from reaching the LLM?
* How do we handle “no relevant information found”?

For example:

**User Query → Embedding → Azure AI Search → Hybrid/Vector Search → Top K chunks → Reranking → LLM**

### 5. LLM questions

* Which LLM do we use?
* Azure OpenAI or another provider?
* Which model is appropriate?
* What is the context-window requirement?
* What are the token limits?
* What is the expected request volume?
* What happens if the model API is unavailable?
* Do we need model fallback?
* Do we need streaming responses?
* Do we need different models for different tasks?

Also ask:

**What should happen when the retrieved documents don't contain the answer?**

The application should generally avoid allowing the LLM to simply invent an answer.

### 6. Security questions

This should be discussed early, not at the end.

* Who can access the application?
* How is authentication handled?
* Entra ID?
* How is authorization handled?
* Can User A access documents belonging to User B?
* Do we need document-level security?
* How are secrets stored?
* Are API keys required?
* Can we use Managed Identity?
* Is PII/sensitive information present?
* Is customer data allowed to be sent to the LLM?
* What data must be masked?

For Azure:

**Entra ID + Managed Identity + Key Vault + RBAC**

would be areas to investigate.

### 7. API / application architecture

Ask:

* Do we need a .NET API?
* What responsibilities belong to the API?
* Should retrieval and LLM orchestration be separate services?
* Do we need background workers?
* Do we need an API gateway?
* Do we need rate limiting?
* How will we handle concurrent users?
* Do we need streaming from API → Angular/UI?

A typical architecture could be:

```text
Angular
   |
   v
API Management
   |
   v
.NET Core API
   |
   +----> Azure AI Search
   |
   +----> Azure OpenAI
   |
   +----> Blob Storage
```

### 8. Scalability questions

Ask:

* How many concurrent users?
* How many requests per second?
* What is the expected document growth?
* What is the expected embedding volume?
* Can ingestion scale independently from querying?
* Can the API scale horizontally?
* What happens during traffic spikes?
* Are Azure OpenAI/Search quotas sufficient?

This helps determine whether you need:

**App Service / Container Apps / AKS / Functions / Service Bus / autoscaling**, etc.

### 9. Reliability & failure handling

Think through:

> **“What happens if each component fails?”**

For example:

* What if Azure OpenAI is unavailable?
* What if Azure AI Search is unavailable?
* What if document processing fails?
* What if embedding generation fails?
* What if a message is processed twice?
* What if the same document is uploaded twice?
* What if indexing partially succeeds?
* Do we need retry?
* Do we need dead-letter queues?
* Do we need idempotency?
* Do we need disaster recovery?

### 10. Observability

Ask:

* What do we need to log?
* How do we trace one user's question through the entire RAG pipeline?
* How do we measure retrieval quality?
* How do we measure LLM response quality?
* How do we monitor token consumption?
* How do we monitor latency?
* How do we detect hallucinations/incorrect answers?
* Do we need Application Insights / Azure Monitor?
* Do we need distributed tracing?

A useful correlation flow is:

```text
User Request
    ↓
Correlation ID
    ↓
.NET API
    ↓
AI Search
    ↓
Azure OpenAI
    ↓
Response
```

### 11. Cost questions

This is frequently asked in architecture interviews.

Ask:

* What is the expected number of users?
* How many queries per day?
* How many tokens per query?
* How many documents?
* How frequently are documents re-indexed?
* How much Azure AI Search capacity is required?
* What is the storage requirement?
* Can we reduce token usage?
* Can we cache frequently asked questions?
* Can we use a smaller model for simpler tasks?

### 12. Deployment / DevOps

Ask:

* How many environments?

  * Dev
  * Test
  * UAT
  * Production
* How will infrastructure be provisioned?
* Terraform or Bicep?
* How will application deployment happen?
* How will database/search index changes be deployed?
* How are secrets/configurations managed?
* Do we need blue-green or canary deployment?
* How do we roll back?

### 13. RAG quality / evaluation

This is an important architectural area that is sometimes missed.

Ask:

* How do we know whether retrieval is good?
* How do we measure relevance?
* How do we evaluate groundedness?
* How do we detect hallucinations?
* What is our test question dataset?
* How do we compare different chunking strategies?
* How do we compare embedding models?
* How do we evaluate Top-K?
* How do we monitor quality after deployment?

---

## A good interview sequence

If an interviewer asks:

> **“Design the infrastructure for a RAG-based AI application.”**

You can drive the discussion in this order:

```text
1. Business Requirements
        ↓
2. Users & Traffic
        ↓
3. Data Sources
        ↓
4. Document Ingestion
        ↓
5. Chunking & Embeddings
        ↓
6. Vector / Hybrid Search
        ↓
7. LLM
        ↓
8. API & UI
        ↓
9. Security
        ↓
10. Scalability
        ↓
11. Reliability
        ↓
12. Monitoring
        ↓
13. Cost
        ↓
14. CI/CD & Infrastructure as Code
```

### For your Azure RAG architecture

Given your **.NET + Angular + Azure OpenAI + Azure AI Search + Blob Storage + Azure Functions** architecture, I would particularly focus on these **15 architect-level questions**:

1. **Where does the source data come from?**
2. **How does a document enter the ingestion pipeline?**
3. **How do we process and chunk the document?**
4. **How are embeddings generated?**
5. **Where are vectors and metadata stored?**
6. **Are we using vector or hybrid search?**
7. **How do we enforce document-level authorization?**
8. **How does the API communicate with Azure OpenAI and AI Search?**
9. **How do we authenticate Azure services — API keys or Managed Identity?**
10. **How do we handle OpenAI/Search throttling and transient failures?**
11. **How do we scale ingestion independently from query traffic?**
12. **How do we monitor RAG latency, token usage and failures?**
13. **How do we evaluate retrieval quality and hallucination?**
14. **How do we protect sensitive/PII data?**
15. **What is our DR, backup and recovery strategy?**

If you can answer these 15 questions clearly, you are moving from **“I built a RAG application”** toward **“I can architect a production RAG platform.”**
