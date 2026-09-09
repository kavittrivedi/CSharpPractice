
---

# 1. Why use LangChain instead of calling an LLM API directly?

You can absolutely call an LLM API directly.

```text
Application
    ↓
LLM API
    ↓
Response
```

For a simple chatbot, this may be enough.

But when the application becomes more complex, LangChain provides reusable components for:

* Prompt management
* LLM integration
* Tools
* Retrieval/RAG
* Chains
* Agents
* Memory/state
* Structured output
* Workflow integration

Without a framework, you may end up building much of this yourself.

```text
Direct API:

Application
   ↓
Custom prompt code
Custom tool code
Custom retry
Custom RAG
Custom state
Custom workflow
   ↓
LLM
```

With LangChain:

```text
Application
   ↓
LangChain
   ↓
LLM + Tools + RAG + Workflow
```

### Interview answer

> **"For a simple LLM call, I would use the provider API directly. I would use LangChain when the application needs reusable abstractions for prompts, tools, retrieval, agents and structured workflows. The framework reduces custom integration code, but I wouldn't introduce it if the application doesn't need those capabilities."**

---

# 2. What are Chains?

A **chain is a predefined sequence of operations**.

For example:

```text
User Question
      ↓
Prompt
      ↓
LLM
      ↓
Parse Result
      ↓
Final Answer
```

Another example:

```text
Question
   ↓
Retrieve Documents
   ↓
Create Prompt
   ↓
LLM
   ↓
Answer
```

The important characteristic is:

> **The workflow is mostly predefined.**

### Example

```text
Input
 ↓
Step A
 ↓
Step B
 ↓
Step C
```

The application knows the sequence.

### Interview answer

> **"A chain is a predefined sequence of operations where the output of one step becomes the input to the next step. I would use a chain when the workflow is predictable and doesn't require the model to dynamically decide what to do next."**

---

# 3. What are Tools?

A **tool is an external capability that an LLM or agent can invoke**.

Examples:

```text
getCustomer()
searchOrders()
searchDocuments()
calculateTax()
sendEmail()
```

Think:

> **Tool = something the AI can use to perform an operation.**

Example:

```text
User:
"What is the status of order 123?"

LLM
 ↓
getOrderStatus(123)
 ↓
Order API
 ↓
"Shipped"
```

### Important

Tools should have:

* Clear name
* Clear description
* Defined input schema
* Defined output
* Appropriate authorization

### Interview answer

> **"A tool is an external function or capability that an LLM or agent can invoke. Examples are database queries, APIs, search systems or business operations."**

---

# 4. What are Agents?

An **agent is an AI-driven component that can decide what action to take next based on the task and available tools.**

A chain says:

```text
A → B → C
```

An agent can decide:

```text
Question
   ↓
Agent
   ↓
Should I search?
   ↓
Yes
   ↓
Search Tool
   ↓
Agent
   ↓
Do I need another tool?
   ↓
Yes
   ↓
Database Tool
```

So:

> **Chain = predefined workflow**

> **Agent = dynamic decision-making**

### Example

User:

> "Find my order and tell me whether I'm eligible for a refund."

The agent might decide:

```text
1. Get order
2. Check order status
3. Check refund policy
4. Determine eligibility
```

### Interview answer

> **"An agent uses an LLM to decide which tools or actions to take and in what order. I use agents when the workflow is dynamic and the next step depends on the current result."**

---

# 5. What is LangGraph?

**LangGraph is a framework for building stateful, multi-step agent workflows as a graph.**

Think of it like:

```text
                 Start
                   |
                   v
                Search
                   |
             Is result good?
              /          \
            Yes           No
             |             |
             v             v
          Generate       Search Again
             |             |
             +------<------+
                   |
                   v
                  End
```

Each box is a **node**.

The connections are **edges**.

The workflow has **state**.

So:

```text
LangGraph
   |
   +-- Nodes
   +-- Edges
   +-- State
   +-- Conditions
   +-- Loops
   +-- Persistence
```

It is particularly useful when agent workflows are not just simple linear sequences.

### Interview answer

> **"LangGraph is used to build stateful agent workflows as graphs. It allows me to define nodes, transitions, conditional routing, loops and state, which is useful for complex and long-running agent workflows."**

---

# 6. LangChain vs LangGraph?

This is a very common question.

| LangChain                                 | LangGraph                              |
| ----------------------------------------- | -------------------------------------- |
| Components/framework for LLM applications | Framework for stateful agent workflows |
| Prompts                                   | Graph nodes                            |
| Models                                    | Edges/transitions                      |
| Tools                                     | State                                  |
| Retrievers                                | Conditional routing                    |
| Chains                                    | Loops                                  |
| Agents                                    | Persistence/workflow control           |

A simple way to remember:

> **LangChain = building blocks**

> **LangGraph = workflow orchestration**

They can also be used together.

```text
LangGraph
    |
    +-- LangChain model
    +-- LangChain tools
    +-- LangChain retriever
```

### Interview answer

> **"I see LangChain as a collection of abstractions and components for building LLM applications, while LangGraph is focused on orchestrating stateful, multi-step and potentially cyclic agent workflows. They can be used together."**

---

# 7. Why is state important in agent workflows?

Suppose an agent performs:

```text
Step 1 → Search
Step 2 → Analyze
Step 3 → Verify
Step 4 → Generate Report
```

The agent needs to remember:

```text
What was searched?
What were the results?
What has completed?
What failed?
What should happen next?
```

That information is the **state**.

Example:

```text
ResearchState
-------------------
Question
SearchResults
Documents
AnalysisResults
FactCheckResults
CurrentStep
Errors
FinalReport
```

State becomes especially important for:

* Multi-step workflows
* Loops
* Long-running workflows
* Failure recovery
* Human approval
* Resuming execution

### Interview answer

> **"State allows an agent workflow to remember what has already happened, what results were produced, what step is currently running and what should happen next. It is especially important for long-running workflows, retries and failure recovery."**

---

# 8. How do you implement conditional routing?

Conditional routing means:

> **Based on the result, decide which step runs next.**

Example:

```text
             Search
                |
                v
         Is result relevant?
           /          \
         Yes           No
          |             |
          v             v
       Generate      Search Again
```

In code/framework terms, you define a condition that examines the current state.

For example:

```text
if confidence > 0.8
    → Generate Answer
else
    → Search Again
```

Another example:

```text
if request == "customer"
    → CRM Agent

if request == "invoice"
    → Finance Agent
```

### Interview answer

> **"I implement conditional routing by evaluating the current workflow state and selecting the next node based on a condition. For example, if retrieval confidence is high, I generate the answer; otherwise, I perform another search or use a fallback."**

---

# 9. How do you implement retries?

I would not simply retry everything.

For transient failures:

```text
Tool call
   ↓
Failure
   ↓
Retry
   ↓
Exponential Backoff
   ↓
Retry
```

Examples:

* Timeout
* Temporary network failure
* 429
* 503

For permanent errors:

```text
Invalid request
Authorization failure
Invalid parameters
```

I would normally **not keep retrying**.

Also set a maximum:

```text
max_retries = 3
```

For agent workflows, I would also make important operations **idempotent** so retrying doesn't create duplicate side effects.

### Interview answer

> **"I use bounded retries with exponential backoff for transient failures and don't blindly retry permanent errors. For important operations, I also use idempotency so that a retry doesn't accidentally perform the same business action twice."**

---

# 10. How do you persist agent state?

For a short request:

```text
Memory
```

may be enough.

For long-running or production workflows, I would persist state externally.

For example:

```text
Agent
  ↓
Workflow State
  ↓
Database / Redis / Durable Store
```

State might contain:

```text
WorkflowId
CurrentNode
CompletedTasks
Results
Errors
UserApprovalStatus
Timestamp
```

Then:

```text
Workflow
   ↓
Failure
   ↓
Save State
   ↓
Restart
   ↓
Load State
   ↓
Continue
```

### Interview answer

> **"For production workflows, I would persist important state outside the process so that it survives restarts and failures. The persisted state would contain the workflow ID, current step, completed tasks, results and any pending approvals."**

---

# 11. How do you implement human approval?

This is important for **high-risk operations**.

Example:

```text
Agent
  ↓
Generate Refund Request
  ↓
Approval Required
  ↓
WAIT
  ↓
Human approves
  ↓
Execute Refund
```

The important point is:

> **The workflow should pause rather than keep running.**

Example:

```text
State:
WAITING_FOR_APPROVAL
```

When the human approves:

```text
WAITING_FOR_APPROVAL
        ↓
APPROVED
        ↓
Continue workflow
```

For sensitive operations such as:

```text
Delete customer
Issue large refund
Deploy production code
Send legal communication
```

human approval may be appropriate.

### Interview answer

> **"I would model human approval as an explicit workflow state. When the agent reaches a sensitive operation, the workflow pauses in a waiting-for-approval state. After the authorized person approves or rejects it, the workflow resumes or terminates."**

---

# 12. How do you debug an agent workflow?

Agent workflows can be difficult to debug because the LLM may make different decisions.

I would use:

### 1. Structured logging

```text
WorkflowId
Node
Tool
Input
Output
Duration
Error
```

### 2. Distributed tracing

```text
Request
 ↓
Agent
 ↓
Tool
 ↓
API
 ↓
Database
```

### 3. Save intermediate state

```text
Search result
Tool result
Agent decision
Final answer
```

### 4. Capture model information

```text
Model
Prompt version
Token usage
Temperature/configuration
```

### 5. Replay/test

Use the same input and state to reproduce failures where possible.

### Interview answer

> **"I would debug agent workflows using structured logs, correlation IDs, distributed tracing and persisted intermediate state. I would capture which node ran, which tool was called, the result, latency, model and prompt version. This allows me to identify where the workflow went wrong."**

---

# 13. What is LangSmith used for?

**LangSmith is used for observability, tracing, evaluation and debugging of LLM/agent applications.**

Conceptually:

```text
Agent
  |
  +-- LLM call
  +-- Tool call
  +-- Retriever
  +-- Another LLM call
  |
  v
LangSmith
```

It can help you understand:

```text
Which model was called?
Which tool was called?
What was the latency?
What input/output occurred?
Where did the workflow fail?
```

It is also useful for evaluating and testing LLM applications.

### Interview answer

> **"LangSmith provides observability and evaluation capabilities for LLM and agent applications. I can use it to trace agent execution, inspect LLM and tool calls, debug failures and evaluate application quality."**

---

# 14. When would you avoid an agent framework?

This is an **excellent architect question**.

I would avoid an agent framework when the problem is simple and deterministic.

For example:

```text
API
 ↓
Prompt
 ↓
LLM
 ↓
Response
```

There is no need for LangGraph or a complex agent framework here.

Another example:

```text
Request
 ↓
Validate
 ↓
SQL
 ↓
Response
```

A normal application workflow is better.

I would also avoid it when:

* The workflow is completely deterministic.
* We only need one or two LLM calls.
* Framework overhead isn't justified.
* Performance/latency is extremely sensitive.
* The team doesn't need the framework's capabilities.

### Strong architectural principle

> **Don't use an agent framework just because the application uses an LLM.**

### Interview answer

> **"I would avoid an agent framework when the workflow is simple, deterministic and can be implemented cleanly with normal application code and direct LLM APIs. Adding an agent framework introduces abstraction and operational complexity, so I would use it only when its capabilities provide real value."**

---

# ⭐ Quick Revision Table

| Question                    | Simple answer                                  |
| --------------------------- | ---------------------------------------------- |
| **Why LangChain?**          | Reusable LLM, tool, RAG and agent abstractions |
| **Chains?**                 | Predefined sequence of steps                   |
| **Tools?**                  | External capabilities the AI can invoke        |
| **Agents?**                 | AI-driven dynamic decision-making              |
| **LangGraph?**              | Stateful graph-based agent workflows           |
| **LangChain vs LangGraph?** | Building blocks vs workflow orchestration      |
| **Why state?**              | Remember progress/results and recover          |
| **Conditional routing?**    | Choose next step based on state/result         |
| **Retries?**                | Bounded retry + backoff for transient errors   |
| **Persist state?**          | External durable store                         |
| **Human approval?**         | Pause workflow → human decision → resume/stop  |
| **Debug workflow?**         | Logs + tracing + intermediate state            |
| **LangSmith?**              | Tracing, debugging and evaluation              |
| **Avoid framework?**        | Simple/deterministic workflows                 |

---

# ⭐ One strong answer to remember

If the interviewer asks:

> **"How would you choose between direct LLM APIs, LangChain, LangGraph or an agent framework?"**

Say:

> **"I would start with the simplest solution. If I only need a few direct LLM calls, I would use the provider SDK directly. If I need reusable components such as prompts, tools, retrieval and chains, I would consider LangChain. If the workflow becomes stateful, has conditional routing, loops, retries, human approval or long-running execution, I would consider LangGraph or another workflow-oriented agent framework.**
>
> **I would not introduce an agent framework just because I'm using an LLM. The decision should be based on workflow complexity and the capabilities the framework provides."**

### Easy formula

**Direct API → Simple LLM call**

**LangChain → LLM building blocks**

**Agent → Dynamic decision-making**

**LangGraph → Stateful workflow**

**LangSmith → Observe + Debug + Evaluate**