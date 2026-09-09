# 1. The LLM gives incorrect answers even though the prompt is well written. How would you investigate?

I would **not immediately change the prompt**. I would investigate where the problem is coming from.

### Step 1 — Check the input

First check:

* Is the user's question clear?
* Are we sending the correct system/user prompts?
* Is important conversation history missing?

### Step 2 — If using RAG, check retrieval

This is very important.

I would check:

```text
Question
   ↓
Embedding
   ↓
Vector/Search
   ↓
Retrieved chunks
   ↓
LLM
```

Maybe the LLM is correct, but we're giving it the **wrong documents**.

I would check:

* Were the correct documents retrieved?
* Is the similarity score good?
* Are we retrieving enough chunks?
* Are chunks too large/small?
* Are metadata filters working?

### Step 3 — Check model behavior

I would check:

* Temperature
* Model version
* Context window
* Prompt
* Token limits

### Step 4 — Check the output

Compare:

```text
Question
+
Retrieved Context
+
Prompt
↓
Expected Answer
vs
Actual Answer
```

### Step 5 — Create test cases

I would create a fixed evaluation dataset with expected answers and test the model consistently.

**Interview answer:**

> "I would troubleshoot the complete pipeline rather than assuming the prompt is the problem. For a RAG application, I would first verify retrieval quality, then context, prompt, model configuration and finally the generated answer. I would also create an evaluation dataset to reproduce the issue consistently."

---

# 2. Application requires answers strictly from company documents. How would you minimize hallucination?

I would use a **grounded RAG architecture**.

```text
Company Documents
       ↓
Chunking
       ↓
Embeddings
       ↓
Vector/Search Index
       ↓
User Question
       ↓
Retrieve relevant chunks
       ↓
LLM
       ↓
Answer
```

Then I would add strict instructions:

> "Answer only using the provided context. If the answer is not present, say 'I don't know based on the available documents.'"

### Additional controls

**1. Retrieval filtering**

Ensure users only retrieve documents they are authorized to access.

**2. Good chunking**

Don't send entire documents unnecessarily.

**3. Citation/grounding**

Return the source document and relevant section/page where possible.

**4. Confidence/relevance threshold**

If retrieval doesn't find sufficiently relevant information, don't ask the LLM to guess.

```text
Relevant context found?
       ↓
     No → "I don't know"
       ↓
     Yes
       ↓
      LLM
```

**5. Lower temperature**

For factual enterprise Q&A, use a relatively low temperature where supported.

**6. Output validation**

Validate the response before returning it.

**Interview answer:**

> "I would use RAG with strong retrieval, authorization filters and a strict grounded prompt. If relevant information isn't retrieved, the system should refuse to answer rather than allow the model to guess. I would also provide citations and monitor groundedness."

---

# 3. GPT-4-class model costs $100K/month. How would you reduce cost?

I would first understand **where the money is going**.

I would measure:

```text
Requests
Input tokens
Output tokens
Model
Tenant/user
Cost per request
```

Then optimize.

### 1. Use a smaller model where possible

For simple tasks:

```text
Classification → Small model
Summarization → Medium model
Complex reasoning → Large model
```

Don't use the most expensive model for every request.

### 2. Reduce input tokens

Avoid sending:

* Unnecessary chat history
* Entire documents
* Duplicate instructions
* Irrelevant RAG chunks

### 3. Reduce output tokens

Set appropriate output limits.

### 4. Improve RAG

Instead of sending:

```text
50 chunks → LLM
```

send:

```text
5 highly relevant chunks → LLM
```

### 5. Cache responses

If many users ask the same question, reuse the result where appropriate.

### 6. Batch asynchronous workloads

For suitable background workloads, batching can improve efficiency depending on the provider.

### 7. Route requests intelligently

Example:

```text
Simple request
      ↓
Small/cheap model

Complex request
      ↓
Large model
```

**Interview answer:**

> "I would first establish token and cost metrics. Then I would reduce unnecessary input/output tokens, improve RAG retrieval, introduce caching, use smaller models for simpler workloads and reserve the expensive model for tasks that actually require it."

---

# 4. Users complain that responses are too slow. How would you optimize LLM latency?

I would break the latency into different stages.

```text
Total latency
=
API latency
+
Retrieval latency
+
LLM queue time
+
LLM generation time
+
Network latency
```

Then identify which part is slow.

### Possible optimizations

### 1. Reduce prompt size

Smaller context generally means less processing.

### 2. Optimize RAG

Use efficient search/indexing and retrieve only relevant chunks.

### 3. Use a faster/smaller model

If quality is acceptable.

### 4. Stream the response

Instead of:

```text
Wait 10 seconds
↓
Show complete answer
```

stream:

```text
Hello...
Hello, how...
Hello, how can I...
```

The user sees the answer much earlier.

### 5. Parallelize independent operations

If multiple external services are needed:

```csharp
await Task.WhenAll(task1, task2, task3);
```

rather than calling them sequentially.

### 6. Caching

Cache frequently requested information.

### 7. Infrastructure

Check:

* API CPU/memory
* Network latency
* Database latency
* Connection pools
* Queue delays

**Interview answer:**

> "I would first measure where the latency is occurring. Then I would optimize retrieval and prompt size, consider a faster model, parallelize independent operations, use caching and stream the response to improve perceived latency."

---

# 5. Need to support OpenAI, Azure OpenAI, Gemini and Anthropic. How would you architect the LLM integration layer?

I would **not put provider-specific code throughout the application**.

I would create an abstraction.

For example:

```csharp
public interface ILlmClient
{
    Task<LlmResponse> GenerateAsync(
        LlmRequest request,
        CancellationToken cancellationToken);
}
```

Then implementations:

```text
ILlmClient
   │
   ├── OpenAiClient
   ├── AzureOpenAiClient
   ├── GeminiClient
   └── AnthropicClient
```

The application talks only to:

```text
ILlmClient
```

not directly to each provider.

### Add a provider factory/router

```text
Application
     ↓
LLM Service
     ↓
Provider Router
     ↓
┌──────────┬────────────┬─────────┐
OpenAI   Azure OpenAI  Gemini  Anthropic
```

Configuration could determine the provider:

```text
Provider = AzureOpenAI
```

### Common capabilities

The abstraction should handle common concepts such as:

* Generate text
* Streaming
* Token usage
* Tool calling where supported
* Structured output where supported
* Timeout
* Retry
* Logging

But I wouldn't pretend every provider has exactly the same capabilities.

Provider-specific features should remain behind the provider implementation.

**Interview answer:**

> "I would create a provider-agnostic LLM abstraction using interfaces and the Strategy/Factory pattern. Each provider would have its own adapter. The business layer would depend only on the abstraction, while provider-specific authentication, request formats and capabilities stay inside the adapters."

This is a **very strong .NET interview answer** because it demonstrates **SOLID and loose coupling**.

---

# 6. Model sometimes returns invalid JSON despite being instructed to return JSON. How would you solve it?

I would not rely only on the prompt saying:

> "Return valid JSON."

I would use multiple layers of protection.

### 1. Structured output

If the selected model/provider supports **structured outputs or schema-constrained generation**, I would use that.

For example:

```json
{
  "name": "John",
  "age": 30
}
```

with a defined schema.

### 2. Validate the response

In .NET:

```csharp
try
{
    var result = JsonSerializer.Deserialize<Customer>(response);
}
catch (JsonException)
{
    // Handle invalid response
}
```

### 3. Retry/repair

If the response is invalid, we can retry with a controlled correction strategy.

For example:

```text
LLM
 ↓
Invalid JSON
 ↓
Validate
 ↓
Retry / repair
 ↓
Validate again
```

But retries should be **limited**.

### 4. Don't blindly trust the model

Validate:

* JSON syntax
* Required properties
* Data types
* Business rules

For example, JSON may be syntactically valid but still wrong:

```json
{
  "age": -500
}
```

So schema validation and business validation are both useful.

**Interview answer:**

> "I would prefer structured/schema-based output when supported rather than relying only on prompting. I would validate the response against a schema, reject invalid data, and use a limited retry or repair mechanism. I would also apply business validation after JSON deserialization."

---

# 7. How would you evaluate whether a smaller model can replace a larger model?

I would **not decide based only on cost or benchmark scores**.

I would run an evaluation using our **real production-like data**.

### Step 1 — Create evaluation dataset

For example:

```text
Question
Expected Answer
```

Create hundreds or thousands of representative test cases.

### Step 2 — Test both models

```text
                    Large Model
Test Dataset ──────→
                    Small Model
```

Measure:

* Accuracy
* Groundedness
* Hallucination rate
* Instruction following
* Structured-output accuracy
* Tool-calling accuracy

### Step 3 — Measure performance

Compare:

| Metric        | Large Model |     Small Model |
| ------------- | ----------: | --------------: |
| Accuracy      |         95% |             92% |
| Latency       |       5 sec |           2 sec |
| Cost          |        High |             Low |
| Hallucination |         Low | Slightly higher |

### Step 4 — Run real-world testing

Use A/B testing or shadow testing if appropriate.

For example:

```text
90% → Current model
10% → Smaller model
```

Monitor the results.

### Step 5 — Make the decision

If the smaller model provides acceptable quality with significantly lower:

* Cost
* Latency
* Infrastructure requirements

then use it.

**Interview answer:**

> "I would create a representative evaluation dataset and compare both models on accuracy, groundedness, hallucination, latency, token usage and cost. If the smaller model meets our business quality threshold, I would gradually introduce it through controlled testing rather than switching everything immediately."

---

# ⭐ Quick Revision

| Scenario                            | Key answer                                                                          |
| ----------------------------------- | ----------------------------------------------------------------------------------- |
| **LLM gives incorrect answers**     | Check **RAG → retrieval → context → prompt → model → output**                       |
| **Strict company-document answers** | **RAG + grounding + authorization + citations + refusal when no evidence**          |
| **$100K LLM cost**                  | **Smaller models + reduce tokens + RAG optimization + caching + routing**           |
| **LLM is slow**                     | **Measure latency → optimize context/model/retrieval + streaming + parallel calls** |
| **Multiple LLM providers**          | **Interface + Provider adapters + Factory/Strategy + common abstraction**           |
| **Invalid JSON**                    | **Structured output + schema validation + limited retry/repair**                    |
| **Smaller model replacement**       | **Real evaluation dataset + quality + latency + cost + gradual rollout**            |

## ⭐ A strong senior-level pattern

For almost every GenAI production question, think in this order:

**Accuracy → Cost → Latency → Scalability → Reliability → Security → Observability**

And when answering an interview scenario, don't just say **"I would use RAG"** or **"I would use caching."**

Explain:

> **What problem it solves + where you would put it + what happens when it fails.**

That is usually what differentiates a **senior/architect-level GenAI answer** from a basic implementation answer.
