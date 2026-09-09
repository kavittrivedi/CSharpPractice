

> **The application should not know which LLM provider it is calling. The AI Gateway should handle that. ("Your production application currently uses Azure OpenAI. The business wants to add Gemini and Anthropic without changing application code. Design the architecture.")**

## 1. Proposed architecture

```text
                  Application
                       |
                       v
                  AI Gateway
                       |
              +--------+--------+
              |        |        |
              v        v        v
          Azure      Gemini   Anthropic
          Adapter    Adapter   Adapter
              |        |        |
              v        v        v
        Azure OpenAI  Gemini   Claude
```

The application calls a **common interface** exposed by the AI Gateway.

For example:

```text
POST /ai/chat
```

The application doesn't care whether the request goes to Azure OpenAI, Gemini or Anthropic.

---

# 2. Why do we need Provider Adapters?

Each provider has different:

* SDKs
* APIs
* Authentication
* Model names
* Request/response formats
* Token/usage information
* Error formats

So I would hide those differences behind adapters.

```text
IAIProvider
    |
    +-- AzureOpenAIAdapter
    +-- GeminiAdapter
    +-- AnthropicAdapter
```

For example:

```text
IAIProvider.GenerateResponse(request)
```

The application always calls the same interface.

The adapter converts the common request into the provider-specific format.

```text
Application
     |
Common AI Request
     |
AI Gateway
     |
Azure Adapter
     |
Azure OpenAI specific request
```

---

# 3. How does routing work?

The gateway decides which provider/model should handle the request.

For example:

```text
Request
   |
   v
AI Gateway
   |
   +-- Simple task → Gemini
   |
   +-- Enterprise task → Azure OpenAI
   |
   +-- Long-context task → Anthropic
```

Routing could be based on:

* Use case
* Model capability
* Cost
* Latency
* Region
* Availability
* Data/privacy requirements

For example, configuration could say:

```text
CustomerSupport → Azure OpenAI
Summarization   → Anthropic
General Chat    → Gemini
```

The important point is that **this decision stays outside the application code**.

---

# 4. What happens if Azure OpenAI fails?

The gateway can implement fallback.

```text
Application
     |
AI Gateway
     |
Azure OpenAI ❌
     |
     v
Gemini ✅
     |
Response
```

But I wouldn't blindly fallback for every error.

For example:

```text
Timeout             → Maybe fallback
503 Service Error   → Fallback
Rate limit          → Retry/fallback

Invalid request     → Don't blindly fallback
Authentication      → Don't blindly fallback
```

Also, the fallback model must be suitable for the task.

---

# 5. Retry handling

The gateway should centrally handle retries.

```text
Request
   |
Azure OpenAI
   |
  503
   |
Retry + exponential backoff
   |
  503
   |
Fallback provider
```

This prevents every application from implementing its own retry logic.

---

# 6. Monitoring

The gateway becomes the ideal place to collect common telemetry.

For every request:

```text
CorrelationId
Provider
Model
Latency
Input tokens
Output tokens
Total tokens
Success/Failure
Retry count
Fallback used
Cost
```

For example:

```text
Request: R12345
Provider: Azure OpenAI
Model: GPT-X
Latency: 1.8 sec
Input Tokens: 1200
Output Tokens: 400
Status: Success
Cost: $X
```

Now I can compare providers.

```text
Provider       Success    P95 Latency    Cost
------------------------------------------------
Azure OpenAI     99%        1.5 sec       $$
Gemini           98%        1.2 sec       $
Anthropic        99%        2.0 sec       $$$
```

---

# 7. Policy enforcement

The gateway should also enforce enterprise policies.

For example:

```text
Application
     |
     v
AI Gateway
     |
     +--> Authentication
     +--> Authorization
     +--> PII/Data protection
     +--> Model policy
     +--> Rate limiting
     +--> Content safety
     |
     v
Provider
```

For example, the company might say:

> "Confidential customer data can only be sent to approved enterprise models."

The gateway can enforce that rule.

---

# 8. Token and cost tracking

Different providers have different pricing models.

The gateway can normalize usage into a common format:

```text
AIUsage
----------------
Provider
Model
InputTokens
OutputTokens
TotalTokens
Cost
```

Then the business can track:

```text
Cost per application
Cost per user/tenant
Cost per model
Cost per provider
Cost per feature
```

---

# 9. Configuration instead of code changes

Suppose today:

```text
CustomerSupport → Azure OpenAI
```

Tomorrow the business wants:

```text
CustomerSupport → Anthropic
```

I should ideally change configuration:

```text
CustomerSupport:
    Provider: Anthropic
    Model: ...
```

rather than modify:

```text
CustomerSupportService.cs
```

and redeploy the application.

This is one of the biggest benefits of the abstraction.

---

# 10. What about provider-specific features?

This is an important architect-level consideration.

A common abstraction should cover **common capabilities**:

```text
Chat
Streaming
Embeddings (if supported)
Token usage
```

But providers may have unique features.

For example:

```text
Provider-specific feature
          |
          v
Optional capability interface
```

I wouldn't try to force every provider into an abstraction that hides important differences.

Otherwise, the abstraction becomes too complicated.

---

# ⭐ Strong interview answer

If the interviewer asks:

> **"Your application uses Azure OpenAI, but the business wants Gemini and Anthropic without changing application code. How would you design it?"**

Say:

> **"I would introduce an AI Gateway between the application and the LLM providers. The application would communicate with the gateway through a provider-neutral interface, so the application doesn't know whether the request is going to Azure OpenAI, Gemini or Anthropic.**
>
> **Inside the gateway, I would have provider adapters such as AzureOpenAIAdapter, GeminiAdapter and AnthropicAdapter. Each adapter would translate the common request into the provider-specific API format.**
>
> **The gateway would also handle provider selection and routing based on configuration, model capability, cost, latency or data policies. I would implement bounded retries with exponential backoff and fallback to another suitable provider for transient failures.**
>
> **I would centralize authentication, authorization, data-protection policies, rate limiting, logging, monitoring, token usage and cost tracking in the gateway. I would use correlation IDs so that every request can be traced across the gateway and provider.**
>
> **This way, adding or changing a provider mostly becomes a gateway configuration or adapter change rather than an application-code change."**

---

## ⭐ Architecture to remember

```text
                 Application
                      |
                      v
                AI Gateway
                      |
        +-------------+-------------+
        |             |             |
        v             v             v
   Azure Adapter  Gemini Adapter  Anthropic Adapter
        |             |             |
        v             v             v
   Azure OpenAI     Gemini       Anthropic
```

### Remember these 7 keywords:

**Abstraction → Adapter → Routing → Retry → Fallback → Monitoring → Policy**

And the most important sentence:

> **"The application talks to a provider-neutral AI interface; the gateway hides provider-specific differences."**
