
---

# 1. How do you integrate an LLM into a Python backend?

The basic flow is:

```text
User
  ↓
Python API (FastAPI / Flask)
  ↓
LLM Service
  ↓
LLM Response
  ↓
Python API
  ↓
User
```

For example:

```text
POST /chat
{
   "message": "Explain RAG"
}
```

The Python backend:

1. Validates the request.
2. Builds the prompt/messages.
3. Calls the LLM provider.
4. Handles timeout/errors/retries.
5. Returns the response.

For production, I would add:

* Authentication/authorization
* Input validation
* Retry and timeout
* Rate-limit handling
* Logging/monitoring
* Token/cost tracking
* Sensitive-data protection

### Interview answer

> **"I would integrate the LLM through the provider's SDK or API from a Python service such as FastAPI. I would keep the LLM call behind a service abstraction and add timeout, retry, rate-limit, security, logging and token-cost tracking around it."**

---

# 2. What is an LLM Gateway?

An **LLM Gateway is a common layer between your applications and one or more LLM providers.**

```text
                 Applications
                      |
                      v
                 LLM Gateway
                /     |      \
               /      |       \
              v       v        v
           OpenAI  Azure      Other
                    OpenAI    Models
```

Instead of every application directly calling an LLM provider:

```text
App 1 → OpenAI
App 2 → Azure OpenAI
App 3 → Other Provider
```

we have:

```text
App 1 ─┐
App 2 ─┼→ LLM Gateway → LLM Providers
App 3 ─┘
```

The gateway can provide:

* Authentication
* Rate limiting
* Model routing
* Retries
* Logging
* Cost tracking
* Guardrails
* Provider fallback

### Interview answer

> **"An LLM Gateway is a centralized layer between applications and LLM providers. It can standardize authentication, routing, rate limiting, retries, monitoring, cost tracking and provider fallback."**

---

# 3. Why would you build an abstraction over the LLM provider?

I don't want my business code tightly coupled to one provider.

Bad:

```text
OrderService
    ↓
OpenAI SDK
```

Better:

```text
OrderService
    ↓
ILLMService
    ↓
AzureOpenAI / OpenAI / Other Provider
```

For example:

```text
ILLMService
    |
    +-- AzureOpenAIService
    +-- OpenAIService
    +-- BedrockService
```

Now changing providers doesn't require rewriting the entire application.

It also makes:

* Testing easier
* Mocking easier
* Fallback easier
* Model switching easier
* Provider migration easier

### Interview answer

> **"I would put an abstraction around the LLM provider so that business logic doesn't depend directly on a specific SDK or provider. This makes model switching, testing, fallback and future provider migration easier."**

---

# 4. How do you implement retries?

Not every error should be retried.

For **transient errors**, such as:

```text
429 Too Many Requests
500
502
503
Network timeout
```

I would use:

```text
Request
  ↓
Failure
  ↓
Wait
  ↓
Retry
  ↓
Wait longer
  ↓
Retry
```

Use **exponential backoff**.

For example:

```text
Retry 1 → 1 second
Retry 2 → 2 seconds
Retry 3 → 4 seconds
```

Usually add some **jitter** so many clients don't retry at exactly the same time.

Don't retry indefinitely.

And don't blindly retry permanent errors such as invalid authentication or invalid request parameters.

### Interview answer

> **"I use bounded retries with exponential backoff and jitter for transient failures such as rate limits, temporary server errors and network failures. I don't retry permanent errors indefinitely."**

---

# 5. How do you handle rate limits?

LLM providers commonly limit:

```text
Requests per minute
Tokens per minute
Concurrent requests
```

If I receive a rate-limit response such as `429`, I would:

```text
429
 ↓
Check retry information
 ↓
Wait/backoff
 ↓
Retry
```

I would also control traffic on my side using:

* Rate limiting
* Concurrency limits
* Request queues
* Backpressure
* Batching where appropriate
* Model selection

For example:

```text
1000 requests
      ↓
Queue
      ↓
Controlled concurrency
      ↓
LLM
```

### Interview answer

> **"I handle rate limits using client-side throttling, concurrency limits and queues. For 429 responses, I use the provider's retry information where available, together with bounded exponential backoff. For high-volume workloads, I would also consider asynchronous processing."**

---

# 6. How do you implement timeout handling?

I would always put a timeout around LLM calls.

Example:

```text
Request
  ↓
LLM call
  ↓
Timeout = 30 seconds
```

If the LLM doesn't respond within the limit:

```text
Timeout
   ↓
Cancel request
   ↓
Retry/fallback if appropriate
   ↓
Return controlled response
```

In Python, this can be implemented using the HTTP client's timeout/cancellation mechanisms.

The important point is:

> **Never allow an LLM call to hang indefinitely.**

### Interview answer

> **"I configure explicit timeouts for LLM calls. If the timeout occurs, I cancel the operation and either retry if the error is transient, use a fallback model, or return a controlled error response."**

---

# 7. How do you stream responses?

Instead of waiting for the complete answer:

```text
Request
  ↓
Wait 10 seconds
  ↓
Complete response
```

we stream tokens/chunks:

```text
Request
  ↓
Token 1
Token 2
Token 3
Token 4
...
```

Architecture:

```text
Browser
   ↑
   | Streaming
   |
Python API
   ↑
   |
LLM Provider
```

Common approaches include:

* Server-Sent Events (SSE)
* WebSockets
* HTTP streaming

For a chatbot, **SSE is often a simple choice for one-way server-to-client token streaming**.

### Interview answer

> **"For chat applications, I can stream the LLM response instead of waiting for the complete response. The backend receives chunks from the provider and forwards them to the client using a streaming mechanism such as SSE. This improves perceived response time."**

---

# 8. How do you track token usage?

The provider usually gives usage information such as:

```text
Input tokens
Output tokens
Total tokens
```

I would capture this for every request.

Example:

```text
Request ID: R123
Model: Model-A
Input: 1200 tokens
Output: 500 tokens
Total: 1700 tokens
```

Then aggregate by:

```text
Application
User/tenant
Model
Day/month
Feature
```

This helps with cost management.

### Interview answer

> **"I capture input and output token usage from the provider response and associate it with a correlation ID, application, model and tenant where appropriate. This allows me to monitor consumption and calculate cost."**

---

# 9. How do you calculate LLM cost?

Usually:

```text
Cost =
(Input Tokens × Input Price)
+
(Output Tokens × Output Price)
```

For example, suppose hypothetically:

```text
Input = 1,000 tokens
Input price = $1 per million tokens

Output = 500 tokens
Output price = $2 per million tokens
```

Then:

```text
Input cost  = 1000 / 1,000,000 × $1
Output cost = 500 / 1,000,000 × $2
```

Total:

```text
$0.001 + $0.001 = $0.002
```

**Actual pricing depends on the provider and model**, so I would keep pricing configuration outside the application code and update it when provider pricing changes.

### Interview answer

> **"I calculate cost from input and output token usage multiplied by the current model-specific token prices. I keep pricing configuration separate from business logic because pricing can change between models and providers."**

---

# 10. How do you implement fallback models?

Suppose the primary model is unavailable:

```text
Primary Model
     |
     X
  Failure
     |
     v
Fallback Model
```

Example:

```text
Primary:
High-quality expensive model

Fallback:
Smaller cheaper model
```

But I wouldn't fallback for every error.

For example:

```text
Service unavailable → fallback
Timeout → potentially fallback
Rate limit → potentially fallback

Invalid request → don't blindly fallback
Authorization error → don't blindly fallback
```

I would also make sure the fallback model is capable of the required task.

### Interview answer

> **"I would configure a primary and fallback model. For transient provider failures, timeouts or capacity issues, the gateway can route the request to the fallback model. I would only do this when the fallback model meets the quality and security requirements of the use case."**

---

# 11. How do you manage API keys?

**Don't hard-code API keys.**

Bad:

```python
api_key = "sk-xxxxxxxx"
```

Better:

```text
Application
    ↓
Secret Store
    ↓
API Key
```

For Azure environments, I would typically prefer:

```text
Managed Identity
        ↓
Azure Key Vault / Entra-based authentication
```

where the service supports it.

For local development:

```text
Environment variable
or
Developer secret store
```

I would also:

* Rotate secrets
* Restrict permissions
* Never commit keys to Git
* Never put keys in frontend code
* Never log keys

### Interview answer

> **"I would never hard-code API keys or expose them to the frontend. I would use a managed secret store or managed identity where supported, apply least privilege, rotate credentials and ensure secrets never appear in logs or source control."**

---

# 12. How do you prevent sensitive data from being sent to an LLM?

This is a very important enterprise question.

I would put a **data-protection layer before the LLM**.

```text
User Request
     ↓
Sensitive Data Detection
     ↓
Redaction / Masking
     ↓
LLM
```

For example:

```text
Before:
"Customer John, SSN 123-45-6789..."

After:
"Customer [REDACTED], SSN [REDACTED]..."
```

I would also:

* Minimize the data sent
* Apply data classification
* Mask/redact PII
* Enforce tenant isolation
* Control which tools can access sensitive data
* Use approved models/providers
* Configure enterprise data/privacy controls appropriately
* Audit access

### Interview answer

> **"I would apply data minimization and a privacy layer before sending information to the LLM. I would detect and redact sensitive information such as PII where it isn't required, enforce tenant and authorization boundaries, and only send the minimum data necessary for the task."**

---

# 13. How do you log LLM requests without exposing PII?

I would **not log the complete prompt and response by default**.

Instead:

```text
CorrelationId
User/Tenant ID
Model
Timestamp
Token usage
Latency
Status
Cost
```

For example:

```text
RequestId: R123
Model: Model-A
InputTokens: 1500
OutputTokens: 500
Latency: 2.4 sec
Status: Success
```

If I need prompts/responses for debugging, I would consider:

* Redaction
* Masking
* Sampling
* Restricted access
* Short retention
* Encryption

### Interview answer

> **"I would avoid logging raw prompts and responses by default because they may contain PII or confidential information. I would log metadata such as correlation ID, model, token usage, latency and status. If content logging is required for debugging, I would redact sensitive data, restrict access and apply retention policies."**

---

# 14. How do you implement prompt versioning?

I would treat prompts like **code**.

Don't keep important production prompts only inside application code or edit them manually without tracking.

For example:

```text
CustomerSupportPrompt
    |
    +-- V1
    +-- V2
    +-- V3
```

Store them in:

* Git
* Prompt management system
* Configuration repository

Each prompt should have:

```text
Prompt ID
Version
Model
Parameters
Created date
Owner
```

Example:

```text
SupportPrompt
Version: 3
Model: Model-A
```

Then I can reproduce which prompt generated a particular response.

### Interview answer

> **"I version prompts like application code. Each prompt has an identifier and version, and changes go through review and testing. I record the prompt version with the model and request telemetry so production behavior is reproducible."**

---

# 15. How do you perform model evaluation before changing models?

This is one of the **most important architect-level questions**.

Suppose:

```text
Current Model → Model A

New Model → Model B
```

I would **not simply replace Model A with Model B in production**.

First create a representative evaluation dataset.

For example:

```text
Question                  Expected Result
------------------------------------------------
Q1                        Answer A
Q2                        Answer B
Q3                        Document X
Q4                        Calculation Y
```

Run both models:

```text
              Model A     Model B
Q1              90%         93%
Q2              92%         91%
Q3              88%         94%
```

Evaluate:

### Quality

* Correctness
* Relevance
* Grounding
* Hallucination
* Instruction following

### Performance

* Latency
* Throughput
* Token usage

### Cost

```text
Cost per request
Cost per successful task
```

### Safety

* PII handling
* Prompt injection resistance
* Policy compliance

Then use:

```text
Offline Evaluation
       ↓
Small Pilot
       ↓
Canary / A-B testing
       ↓
Production rollout
```

If Model B performs worse, don't migrate.

### Interview answer

> **"Before changing models, I would create a representative evaluation dataset based on real production use cases. I would run the existing and new models against the same dataset and compare quality, hallucination, grounding, latency, token usage, cost and safety. If the new model passes the acceptance criteria, I would first deploy it to a small percentage of traffic before doing a full rollout."**

---

# ⭐ Quick Revision Table

| Question              | Simple answer                                         |
| --------------------- | ----------------------------------------------------- |
| **LLM integration**   | Backend → LLM API/SDK → response                      |
| **LLM Gateway**       | Central layer between apps and LLM providers          |
| **Abstraction**       | Avoid provider lock-in; easier testing/fallback       |
| **Retries**           | Bounded retry + exponential backoff + jitter          |
| **Rate limits**       | Throttling + concurrency limits + queues + backoff    |
| **Timeout**           | Set explicit timeout; cancel/retry/fallback           |
| **Streaming**         | Send response chunks as they arrive                   |
| **Token tracking**    | Capture input/output tokens                           |
| **Cost**              | Tokens × model-specific prices                        |
| **Fallback**          | Primary model → alternate model for suitable failures |
| **API keys**          | Secret store/managed identity; never hard-code        |
| **Sensitive data**    | Minimize + detect + redact + authorize                |
| **PII logging**       | Log metadata; redact content if needed                |
| **Prompt versioning** | Prompt ID + version + Git/review                      |
| **Model evaluation**  | Benchmark old vs new → pilot → canary → rollout       |

---

# ⭐ Strong Architect-Level Answer

If the interviewer asks:

> **"How would you design a production-ready LLM integration?"**

You can answer:

> **"I would put an LLM abstraction or gateway between my application and the model provider. This gives me centralized control over authentication, model routing, retries, rate limiting, timeout handling, fallback and monitoring.**
>
> **I would track token usage, latency and cost for every request using correlation IDs. I would never hard-code API keys and would use managed identity or a secure secret store where appropriate. Before sending data to the model, I would apply data minimization and sensitive-data protection to avoid exposing unnecessary PII or confidential information.**
>
> **I would also version prompts and evaluate model changes against a representative test dataset before production deployment. I would compare correctness, grounding, hallucination, latency, cost and safety, then use a pilot or canary rollout rather than immediately replacing the production model."**

### Easy formula to remember

**LLM Integration =**

**Gateway → Security → Reliability → Token/Cost → Privacy → Versioning → Evaluation**

That structure will work for most **senior-level LLM integration scenario questions**.
