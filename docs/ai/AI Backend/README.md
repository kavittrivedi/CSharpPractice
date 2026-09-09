
---

## 1. LLM API takes 10–30 seconds. Thousands of users invoke it simultaneously. How would you design the backend?

I would **not keep everything synchronous** if the operation can take 10–30 seconds.

My design would be:

**Frontend → API → Queue → Background Worker → LLM → Database/Cache → Frontend**

* API should quickly validate the request and create a `requestId`.
* Put the request into **Azure Service Bus** or another queue.
* Background workers process LLM requests asynchronously.
* Limit the number of concurrent LLM calls so we don't overload the provider.
* Store the status: `Pending → Processing → Completed/Failed`.
* Frontend can either poll using `requestId` or use **SignalR/WebSocket** for updates.
* Use **Redis caching** where the same questions/results can be reused.
* Scale the workers horizontally based on queue length.

**Interview line:**

> "For thousands of concurrent users, I would decouple the API from the long-running LLM processing using a queue and scalable background workers. I would also control concurrency, use caching, and monitor queue depth and LLM latency."

---

# 2. An API occasionally becomes unresponsive under heavy load. How would you troubleshoot it?

I would troubleshoot it systematically rather than immediately increasing server size.

### Step 1 — Check monitoring

I would check:

* CPU
* Memory
* Thread pool
* Request count
* Response time
* Error rate
* Database performance
* External API latency

### Step 2 — Check application logs

Using tools such as **Application Insights**, I would look for:

* Slow requests
* Exceptions
* Timeouts
* Dependency failures
* Database queries taking too long

### Step 3 — Check database

Look for:

* Slow queries
* Missing indexes
* Connection pool exhaustion
* Blocking/deadlocks

### Step 4 — Check code

Common problems could be:

* Blocking calls like `.Result` or `.Wait()`
* Synchronous I/O
* Memory leaks
* Excessive database calls
* Unbounded parallel processing
* Long-running operations inside API requests

### Step 5 — Load test

I would reproduce the issue using a load-testing tool and identify the bottleneck.

**Interview line:**

> "I would first identify whether the bottleneck is CPU, memory, thread pool, database, or an external dependency. I would use Application Insights and distributed tracing to identify the slow component before deciding on scaling or code changes."

---

# 3. You need to process 1 million documents asynchronously. How would you design?

I would use a **queue-based architecture**.

```text
Blob Storage
     ↓
Queue / Service Bus
     ↓
Worker Functions
     ↓
Extract Text
     ↓
Chunk
     ↓
Generate Embeddings
     ↓
AI Search / Vector DB
```

For example, in Azure:

* Store documents in **Azure Blob Storage**.
* Put document-processing messages into **Azure Service Bus**.
* Use **Azure Functions** or containerized workers.
* Process documents in batches.
* Scale workers horizontally.
* Use retry policies for temporary failures.
* Use a **Dead Letter Queue** for messages that repeatedly fail.
* Make processing **idempotent**, so the same document isn't incorrectly processed twice.
* Track processing status.

For 1 million documents, I would also control:

* Maximum concurrent workers
* LLM/embedding API rate limits
* Database/vector-store throughput
* Queue size

**Interview line:**

> "I would never process 1 million documents directly inside an API request. I would use Blob Storage plus a durable queue and scalable workers, with retries, dead-letter handling, idempotency and controlled concurrency."

---

# 4. API needs to call three external AI services and combine their responses. How would you handle timeouts, retries and failures?

Suppose:

```text
        ┌── AI Service 1
API ────┼── AI Service 2
        └── AI Service 3
              ↓
        Combine Results
```

I would call them **in parallel**, rather than one after another.

In .NET I can use `Task.WhenAll()`.

But I would give each external service:

### Timeout

For example:

```text
AI Service 1 → 10 sec timeout
AI Service 2 → 10 sec timeout
AI Service 3 → 10 sec timeout
```

### Retry

Retry only **transient failures**, such as:

* 429
* 502
* 503
* 504

I would use **exponential backoff**.

Example:

```text
1st retry → 1 second
2nd retry → 2 seconds
3rd retry → 4 seconds
```

### Failure handling

If one AI service fails, I would decide whether:

* The entire request should fail, or
* We can continue with the other two services.

That depends on whether that service is mandatory.

I would also use a **circuit breaker** so that a continuously failing service doesn't receive thousands of requests.

**Interview line:**

> "I would call the three services concurrently, apply per-service timeout and retry policies, use exponential backoff for transient errors, and use a circuit breaker. I would also define whether partial results are acceptable."

---

# 5. LLM provider intermittently returns 429 errors. How would you handle this?

`429` normally means **Too Many Requests / Rate Limit**.

I would handle it using:

### 1. Exponential backoff

Instead of immediately retrying:

```text
Request
 ↓
429
 ↓
Wait 1 sec
 ↓
Retry
 ↓
429
 ↓
Wait 2 sec
 ↓
Retry
```

### 2. Respect Retry-After

If the provider sends a `Retry-After` value, I would respect it.

### 3. Limit concurrency

Don't allow unlimited requests to the LLM provider.

For example:

```text
1000 incoming requests
        ↓
Queue
        ↓
Only 50 LLM calls at a time
```

### 4. Queue requests

For long-running workloads, queue the requests rather than directly calling the provider.

### 5. Circuit breaker

If the provider is consistently failing, temporarily stop sending requests.

### 6. Fallback

If business requirements allow it, use another model/provider.

**Interview line:**

> "For 429 errors, I would combine rate limiting, bounded concurrency, exponential backoff, Retry-After support, and circuit breaker. For asynchronous workloads I would also use a queue to smooth traffic."

---

# 6. How would you implement streaming LLM responses to a frontend?

Instead of waiting for the entire LLM response:

```text
LLM
"Hello"
"Hello, how"
"Hello, how can"
"Hello, how can I help?"
```

we send tokens/chunks to the frontend as they arrive.

Architecture:

```text
Angular
   ↑
Streaming HTTP / SSE
   ↑
.NET API
   ↑
LLM Streaming API
```

The backend calls the LLM using its **streaming API** and forwards each chunk to the frontend.

For a simple HTTP streaming scenario, **Server-Sent Events (SSE)** is a good option.

The frontend receives:

```text
chunk 1 → Hello
chunk 2 → how
chunk 3 → can
chunk 4 → I help?
```

and displays them progressively.

For bidirectional communication, **WebSockets/SignalR** can be considered.

**Interview line:**

> "I would use the LLM provider's streaming API and stream the chunks through the .NET backend to Angular using SSE. If I need bidirectional real-time communication, I would consider SignalR/WebSockets."

---

# 7. How would you prevent a single long-running AI request from consuming all backend resources?

This is mainly about **resource protection**.

I would implement:

### 1. Timeout

Every request gets a maximum execution time.

Example:

```text
Maximum request duration = 60 seconds
```

### 2. CancellationToken

If the client disconnects or timeout occurs, cancel the downstream operation.

```csharp
await aiService.GenerateAsync(request, cancellationToken);
```

### 3. Concurrency limit

Don't allow unlimited AI requests.

For example:

```text
Maximum 50 concurrent AI calls
```

### 4. Queue

Move long-running operations to background workers.

### 5. Rate limiting

Limit requests per user/tenant.

### 6. Circuit breaker

Protect the application from unhealthy downstream services.

### 7. Resource quotas

For example:

```text
Tenant A → 100 requests/minute
Tenant B → 200 requests/minute
```

**Interview line:**

> "I would protect the backend using timeout, cancellation tokens, bounded concurrency, rate limiting and queues. The key is to ensure that one expensive AI request cannot consume all available threads, connections or downstream capacity."

---

# 8. How would you design a multi-tenant GenAI backend?

Suppose we have:

```text
Tenant A
Tenant B
Tenant C
```

using the same GenAI platform.

I would make the **tenant ID part of the entire request context**.

Architecture:

```text
Angular
   ↓
API Gateway
   ↓
.NET API
   ↓
Tenant Context
   ↓
AI / RAG Services
   ↓
Tenant-specific Data
```

### 1. Tenant identification

Get tenant information from the authenticated user's token/claims.

For example:

```text
User → TenantId = ABC
```

### 2. Data isolation

This is very important for GenAI.

Tenant A must **never retrieve Tenant B's documents**.

For vector search, I would store:

```text
Document
TenantId
Embedding
Metadata
```

and filter searches by:

```text
TenantId = currentTenant
```

### 3. Authentication & authorization

Use Microsoft Entra ID or another identity provider.

Then apply:

```text
User → Tenant → Role → Permissions
```

### 4. Tenant-specific configuration

Different tenants might have:

* Different models
* Different token limits
* Different prompts
* Different RAG indexes
* Different quotas

### 5. Rate limiting

For example:

```text
Tenant A → 100 requests/min
Tenant B → 500 requests/min
```

### 6. Cost tracking

Track:

```text
Tenant
 ↓
Requests
 ↓
Tokens
 ↓
Model
 ↓
Cost
```

This is especially important in GenAI systems.

### 7. Security

I would ensure:

* Tenant-aware authorization
* Data isolation
* Encryption
* Secret management using Key Vault
* Audit logging
* No cross-tenant retrieval

**Interview line:**

> "For a multi-tenant GenAI system, the most important concern is tenant isolation. I would propagate TenantId through the request, enforce tenant-level authorization and apply TenantId filtering at the data and vector-search layers. I would also implement tenant-specific rate limits, quotas, configuration and cost tracking."

---

# ⭐ Quick Interview Revision

You can remember these scenarios using this table:

| Scenario                       | Main things to mention                                                 |
| ------------------------------ | ---------------------------------------------------------------------- |
| Thousands of slow LLM requests | **Queue + Workers + Scaling + Concurrency limit**                      |
| API unresponsive               | **Monitoring + Logs + DB + CPU/Memory + Thread pool**                  |
| 1M documents                   | **Blob + Queue + Workers + Retry + DLQ + Idempotency**                 |
| 3 AI services                  | **Parallel calls + Timeout + Retry + Circuit Breaker**                 |
| LLM 429                        | **Rate limit + Retry-After + Backoff + Concurrency control**           |
| LLM streaming                  | **Streaming API + SSE / SignalR**                                      |
| Long-running request           | **Timeout + CancellationToken + Rate limit + Queue**                   |
| Multi-tenant GenAI             | **Tenant isolation + Authorization + Vector filtering + Quota + Cost** |

### One very important principle for all 8 questions

For a senior/architect-level answer, try to consistently mention these **8 building blocks** where applicable:

**Timeout → Retry → Circuit Breaker → Rate Limit → Queue → Concurrency Control → Cancellation → Monitoring**

That makes your answer sound much more like a **production architecture** rather than just a coding solution.
