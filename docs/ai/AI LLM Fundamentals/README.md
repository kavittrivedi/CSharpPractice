Sure. For an interview, I would keep these answers **simple but technically correct**, especially for a senior .NET/Cloud/GenAI role.

# Generative AI / LLM Fundamentals — Interview Answers

### 1. What is an LLM?

**LLM stands for Large Language Model.**

It is an AI model trained on a very large amount of text to understand and generate human-like language.

Examples include GPT, Claude, Gemini, Llama, etc.

At a high level:

```text
User Question
      ↓
LLM
      ↓
Generated Response
```

An LLM predicts what tokens should come next based on the context it has received.

**Interview answer:**

> "An LLM is a large AI model trained on massive amounts of text. It learns patterns in language and can generate text, answer questions, summarize content, write code and perform many other language-related tasks."

---

# 2. How does tokenization work?

LLMs don't directly process complete words like humans do.

They convert text into **tokens**.

For example:

```text
"Hello, how are you?"
        ↓
["Hello", ",", " how", " are", " you", "?"]
```

A token can be:

* A complete word
* Part of a word
* A punctuation mark
* Sometimes a space combined with a word

The exact tokenization depends on the model.

Then the model converts tokens into numbers called **token IDs** and processes those numbers.

```text
Text
 ↓
Tokens
 ↓
Token IDs
 ↓
LLM
 ↓
Output tokens
 ↓
Text
```

**Interview answer:**

> "Tokenization converts text into smaller units called tokens. The LLM processes these tokens rather than raw text."

---

# 3. Difference between tokens, context window and parameters?

These three are often confused.

### Tokens

Tokens are the **units of text** processed by the model.

```text
"Hello world"
↓
Tokens
```

### Context window

The context window is the **maximum amount of input and output information the model can handle in one request**.

For example, if a model supports a 128K-token context window, the conversation, prompt, retrieved documents, etc. must fit within that limit along with the generated output.

### Parameters

Parameters are the **internal learned values of the model**.

Think of them as the model's learned knowledge/pattern representation.

Simple analogy:

```text
Tokens      = Words/pieces being processed
Context     = Working space available to the model
Parameters  = What the model learned during training
```

---

# 4. Explain temperature, top-p and max tokens.

These control how the model generates its response.

### Temperature

Controls **randomness**.

```text
Low temperature
→ More predictable
→ More consistent

High temperature
→ More creative
→ More random
```

For example:

**Temperature = 0.1**

Good for:

* SQL generation
* Structured responses
* Classification

**Temperature = 0.8**

Good for:

* Creative writing
* Brainstorming

---

### Top-p

Top-p controls how many likely candidate tokens are considered based on cumulative probability.

For example:

```text
Top-p = 0.9
```

The model considers the smallest group of likely tokens whose combined probability reaches approximately 90%.

You generally don't need to aggressively tune both temperature and top-p together.

---

### Max tokens

Controls the **maximum number of tokens the model can generate for the response**.

For example:

```text
max output tokens = 500
```

means the generated response cannot exceed that output limit.

**Interview answer:**

> "Temperature controls randomness, top-p controls the probability range of candidate tokens, and max tokens limits the generated output length."

---

# 5. What causes hallucination?

A hallucination happens when the LLM generates information that **sounds correct but is actually incorrect or unsupported**.

Common reasons:

* The model doesn't have the required information.
* The prompt is ambiguous.
* The model is asked about information outside its knowledge.
* The model predicts a plausible answer instead of saying "I don't know."
* Poor or missing grounding data.
* Retrieved documents are incorrect or irrelevant.

Example:

```text
User: What is our company's leave policy?

LLM: Employees receive 35 days of annual leave.
```

If the company document actually says 25 days, that's a hallucination.

### How can we reduce it?

Use:

* RAG
* Good prompts
* Grounding data
* Structured outputs
* Validation
* Tool/function calling
* Lower temperature where appropriate
* Explicit instructions to say "I don't know" when evidence isn't available

**Interview answer:**

> "Hallucination occurs when the model generates plausible but unsupported information. In enterprise applications, I reduce it using RAG, grounding, good prompts, validation and controlled tool calling."

---

# 6. What is prompt engineering?

Prompt engineering means **designing the instructions given to an LLM to get a better and more reliable response**.

For example, instead of:

```text
Summarize this document.
```

we can say:

```text
Summarize the document in 5 bullet points.
Do not add information that isn't present in the document.
Mention important dates and amounts.
```

Good prompt engineering can specify:

* Role
* Task
* Context
* Constraints
* Output format
* Examples

**Interview answer:**

> "Prompt engineering is the process of designing effective instructions and context for an LLM so that we get more accurate, consistent and useful responses."

---

# 7. System prompt vs user prompt vs assistant prompt?

Think of a conversation:

```text
System
   ↓
User
   ↓
Assistant
   ↓
User
   ↓
Assistant
```

### System prompt

Defines the model's **overall behavior and rules**.

Example:

```text
You are a customer support assistant.
Only answer using the provided company documents.
```

### User prompt

The actual request from the user.

```text
How many vacation days do I get?
```

### Assistant message

The model's previous response or generated response.

```text
You receive 20 vacation days.
```

**Simple analogy:**

```text
System    = Rules
User      = Question
Assistant = Answer
```

---

# 8. Zero-shot vs few-shot prompting?

### Zero-shot

You give the model the task **without examples**.

```text
Classify this review as Positive or Negative:

"The product is excellent."
```

No example is provided.

### Few-shot

You provide a few examples first.

```text
"Excellent product" → Positive
"Very bad service" → Negative

"Good experience" → ?
```

The model uses the examples to understand the expected pattern.

**Interview answer:**

> "Zero-shot means asking the model to perform a task without examples. Few-shot means providing a few examples so the model understands the expected behavior or output."

---

# 9. What is structured output?

Normally an LLM may return free-form text:

```text
The customer is John and his age is 35.
```

With structured output, we ask it to return a predefined structure such as JSON:

```json
{
  "name": "John",
  "age": 35
}
```

This is very useful for applications because our backend can deserialize and validate the response.

For example:

```csharp
CustomerResponse response =
    JsonSerializer.Deserialize<CustomerResponse>(json);
```

**Interview answer:**

> "Structured output forces the model response into a predefined format such as JSON or a schema. It makes the response easier and safer for applications to consume."

---

# 10. Function calling/tool calling vs normal text generation?

### Normal text generation

We ask the model a question and it generates text.

```text
User
 ↓
LLM
 ↓
Text
```

### Tool/function calling

The model can decide that it needs an **external function or tool**.

Example:

```text
User:
What's the status of order 123?

LLM
 ↓
Calls getOrderStatus(123)
 ↓
Backend/API
 ↓
Order Status
 ↓
LLM
 ↓
Final answer
```

The LLM itself isn't necessarily executing the business operation. It typically produces a structured tool-call request, and **our application executes the tool**.

Examples of tools:

* Database lookup
* Weather API
* Search
* Order API
* Calculator
* Internal enterprise API

**Interview answer:**

> "Normal generation produces text directly. Tool calling allows the model to request an external function or API when it needs real-time data or needs to perform an action."

---

# 11. What is an LLM context window?

The context window is the **maximum amount of information the model can consider in a single interaction**, measured in tokens.

It can include:

```text
System instructions
+
User question
+
Conversation history
+
Retrieved RAG documents
+
Tool results
+
Output
```

For example:

```text
User Question
     +
Chat History
     +
RAG Context
     +
Instructions
     +
Output
     ↓
Context Window
```

The exact limit depends on the model.

---

# 12. What happens when the prompt exceeds the context window?

The request can fail because the total token count exceeds the model's supported context limit.

In an application, I would avoid blindly sending everything.

I could:

* Remove unnecessary conversation history.
* Summarize old conversations.
* Reduce retrieved documents.
* Chunk documents.
* Retrieve only the most relevant chunks.
* Reduce output limits.
* Use a model with a larger context window.

For RAG, this is particularly important.

Instead of:

```text
10,000 document chunks
       ↓
LLM
```

we should do:

```text
Question
   ↓
Vector/Search
   ↓
Top relevant chunks
   ↓
LLM
```

**Interview answer:**

> "If the prompt exceeds the context window, the request may be rejected or cannot be processed as intended. I would reduce unnecessary context, summarize history, retrieve fewer relevant chunks, or use a model with a larger context window."

---

# 13. What is model fine-tuning?

Fine-tuning means taking an existing pretrained model and **training it further on a specific dataset** to improve its behavior for a particular task.

For example:

```text
Base Model
    ↓
Company-specific training examples
    ↓
Fine-tuned Model
```

It can be useful for:

* Consistent style
* Classification
* Specific task behavior
* Domain-specific response patterns

But fine-tuning is **not normally the best way to provide frequently changing company facts**.

For example:

```text
Company leave policy changes every year
```

RAG is usually more appropriate.

---

# 14. Fine-tuning vs RAG — when would you use each?

This is a **very important interview question**.

| RAG                                 | Fine-tuning                       |
| ----------------------------------- | --------------------------------- |
| Provides external knowledge         | Changes model behavior            |
| Good for frequently changing data   | Good for consistent task behavior |
| Doesn't retrain the model           | Requires training                 |
| Easier to update documents          | Dataset needs to be maintained    |
| Good for enterprise knowledge bases | Good for specialized behavior     |

### Example

Company policies:

```text
Employee Handbook
Leave Policy
Security Policy
```

I would use **RAG**.

Why?

Because when the policy changes, I can update the document/index instead of retraining the model.

### Fine-tuning example

Suppose I want the model to consistently classify customer complaints into:

```text
Billing
Technical
Account
Security
```

with a particular output style.

Fine-tuning could be considered if prompting alone isn't sufficient.

**Best interview answer:**

> "I use RAG when the problem is about providing the model with current or private knowledge. I consider fine-tuning when the problem is about changing or specializing the model's behavior. They can also be used together."

---

# 15. What is LoRA/PEFT?

### PEFT

**Parameter-Efficient Fine-Tuning**.

Instead of modifying all the parameters of a large model, we train only a **small portion of additional parameters**.

### LoRA

**Low-Rank Adaptation** is one popular PEFT technique.

Instead of changing the entire model:

```text
Large Model
 ↓
Train everything ❌
```

LoRA effectively adds small trainable components:

```text
Large Model
     +
Small LoRA adapters
     ↓
Specialized behavior
```

Benefits:

* Less memory
* Less compute
* Faster/cheaper fine-tuning
* Easier to maintain multiple adaptations

**Interview answer:**

> "LoRA is a parameter-efficient fine-tuning technique. Instead of updating the entire large model, we train a much smaller set of adapter parameters, reducing compute and memory requirements."

---

# 16. What is model quantization?

Quantization means representing model parameters using **lower precision numbers**.

For example:

```text
FP32
 ↓
FP16
 ↓
INT8
 ↓
INT4
```

Lower precision can reduce:

* Model size
* Memory usage
* Hardware requirements
* Inference cost

The trade-off is that excessive quantization can reduce model quality.

**Simple analogy:**

A high-resolution image requires more storage. A compressed image uses less storage but may lose some quality.

Similarly:

```text
Higher precision
→ More memory
→ Potentially better numerical accuracy

Lower precision
→ Less memory
→ Faster/cheaper inference
→ Potential quality trade-off
```

---

# 17. How would you select an LLM for an enterprise application?

I wouldn't select a model only based on benchmark scores.

I would consider:

### 1. Use case

What do we need?

* Chatbot
* RAG
* Coding
* Summarization
* Classification
* Agent

### 2. Accuracy

Test it against **our actual business data**.

### 3. Context window

Does it support the amount of context we need?

### 4. Latency

How quickly does it respond?

### 5. Cost

Look at:

```text
Input tokens
+
Output tokens
+
Number of requests
```

### 6. Security & privacy

Important enterprise questions:

* How is data handled?
* Is customer data used for training?
* What compliance certifications are available?
* Where is data processed?

### 7. Reliability

Check:

* Availability
* Rate limits
* SLA
* Regional availability

### 8. Integration

How easily does it integrate with our Azure/.NET architecture?

### 9. Support

Consider vendor support, lifecycle and ecosystem.

**Interview answer:**

> "I would evaluate the model based on accuracy, latency, cost, context size, security, compliance, reliability, rate limits and integration requirements. Most importantly, I would test candidate models against our actual enterprise use cases rather than relying only on public benchmarks."

---

# 18. Open-source LLM vs commercial API?

### Commercial API

Examples:

* Hosted models from major cloud/AI providers.

Advantages:

* Easy to integrate
* No infrastructure management
* Usually strong models
* Provider handles scaling
* Faster to get into production

Disadvantages:

* API cost
* Vendor dependency
* Rate limits
* Less infrastructure-level control

### Open-source LLM

Examples include models from the Llama/Qwen/Mistral ecosystem.

Advantages:

* More control
* Can self-host
* Can customize/fine-tune
* Potentially useful for specific privacy/deployment requirements

Disadvantages:

* We have to manage infrastructure
* GPU cost
* Scaling
* Monitoring
* Security
* Model upgrades
* Operational complexity

### Enterprise decision

I would choose based on requirements.

```text
Need quick implementation?
        ↓
Commercial API may be better

Need maximum infrastructure/control?
        ↓
Open-source/self-hosted may be better
```

**Interview answer:**

> "Commercial APIs are usually easier to adopt and operate because the provider manages the infrastructure and scaling. Open-source models provide more control and customization but introduce infrastructure and operational responsibilities. For an enterprise, I would make the decision based on security, compliance, cost, performance, customization and operational requirements."

---

# ⭐ Quick Revision for Interview

| Question               | One-line answer                                                               |
| ---------------------- | ----------------------------------------------------------------------------- |
| **LLM**                | AI model trained on large amounts of data to understand and generate language |
| **Tokenization**       | Converts text into tokens that the model can process                          |
| **Tokens**             | Units of text                                                                 |
| **Context window**     | Maximum context the model can process in one interaction                      |
| **Parameters**         | Learned internal values of the model                                          |
| **Temperature**        | Controls randomness                                                           |
| **Top-p**              | Controls the probability range of candidate tokens                            |
| **Max tokens**         | Limits generated output                                                       |
| **Hallucination**      | Model generates unsupported/incorrect information                             |
| **Prompt engineering** | Designing effective instructions for the model                                |
| **System prompt**      | Defines model behavior/rules                                                  |
| **User prompt**        | User's request                                                                |
| **Assistant**          | Model's response/message                                                      |
| **Zero-shot**          | No examples                                                                   |
| **Few-shot**           | Give examples                                                                 |
| **Structured output**  | Output follows a defined schema/format                                        |
| **Tool calling**       | Model requests an external function/API                                       |
| **Fine-tuning**        | Further trains model for specialized behavior                                 |
| **RAG**                | Retrieves external knowledge and gives it to the model                        |
| **LoRA**               | Efficient fine-tuning using small adapter parameters                          |
| **PEFT**               | Fine-tuning only a small part of model parameters                             |
| **Quantization**       | Uses lower-precision numbers to reduce model size/cost                        |
| **LLM selection**      | Accuracy + cost + latency + security + context + reliability                  |
| **Commercial API**     | Easier operations, less control                                               |
| **Open-source**        | More control, more operational responsibility                                 |

### ⭐ The most important distinction to remember

For interviews, remember this:

> **RAG changes what the model knows for a particular request. Fine-tuning changes how the model behaves.**

And:

> **Tool calling lets the model interact with the outside world; RAG primarily provides relevant information to the model.**

These two distinctions are asked very frequently in **senior GenAI interviews**.
