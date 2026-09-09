 **Design a research system with**  

| Research Agent                                                |
| ------------------------------------------------------------- |
| Web Search Agent                                              |
| Document/RAG Agent                                            |
| Data Analysis Agent                                           |
| Fact-Checking Agent                                           |
| Report Generation Agent                                       |

## 1. Overall Architecture

```text
                         User
                           |
                           v
                  +------------------+
                  |  Research Agent  |
                  |   (Supervisor)   |
                  +--------+---------+
                           |
          +----------------+----------------+
          |                |                |
          v                v                v
   Web Search Agent   Document/RAG Agent  Data Analysis Agent
          |                |                |
          +----------------+----------------+
                           |
                           v
                  Fact-Checking Agent
                           |
                           v
                Report Generation Agent
                           |
                           v
                    Final Report
```

The **Research Agent is the orchestrator/supervisor**. It understands the user's question, breaks it into tasks, sends tasks to specialized agents, collects their results, asks the Fact-Checking Agent to verify important claims, and finally sends the verified information to the Report Generation Agent.

---

# 2. What is the responsibility of each agent?

| Agent                       | Responsibility                                       |
| --------------------------- | ---------------------------------------------------- |
| **Research Agent**          | Orchestrates the complete research workflow          |
| **Web Search Agent**        | Searches external web sources                        |
| **Document/RAG Agent**      | Searches internal documents/knowledge base           |
| **Data Analysis Agent**     | Analyzes structured data, calculations, trends       |
| **Fact-Checking Agent**     | Verifies important claims and detects contradictions |
| **Report Generation Agent** | Creates the final structured report                  |

The important point is: **each agent should have a limited responsibility and limited tools/permissions.**

---

# 3. Scenario: User asks a research question

Suppose the user asks:

> "Prepare a report on the impact of AI on the software development industry."

### Step 1 — Research Agent

The Research Agent receives the request.

It breaks the problem into:

```text
1. Search latest web information
2. Search internal company documents
3. Analyze available industry data
4. Verify important claims
5. Generate final report
```

It then invokes the appropriate agents.

---

# 4. Web Search Agent

The Web Search Agent searches external sources.

For example:

```text
Research Agent
      |
      v
Web Search Agent
      |
      +--> Search AI software development trends
      +--> Search productivity studies
      +--> Search industry reports
      +--> Search recent statistics
```

It should return **structured results**, not just a large block of text.

For example:

```json
{
  "source": "Industry Report",
  "title": "AI in Software Development",
  "url": "...",
  "claims": [
    "...",
    "..."
  ],
  "publishedDate": "..."
}
```

### Interview answer

> "The Web Search Agent is responsible for collecting information from external sources. I would return structured results containing the source, title, URL, date and relevant claims so that downstream agents can verify and use the information."

---

# 5. Document/RAG Agent

Suppose the company has internal documents:

```text
Azure_AI_Strategy.pdf
Engineering_Guidelines.pdf
AI_Adoption_Report.pdf
Company_Policies.docx
```

The Document/RAG Agent searches the internal knowledge base.

```text
User Question
      |
      v
Document/RAG Agent
      |
      v
Query Embedding
      |
      v
Azure AI Search
      |
      v
Relevant Chunks
      |
      v
RAG Answer
```

For an enterprise system, I would apply **security filters during retrieval**.

For example:

```text
TenantId
Department
DocumentType
UserPermissions
```

### Interview answer

> "The Document/RAG Agent handles internal knowledge. I would use vector or hybrid search with metadata and security filters to retrieve only documents the user is authorized to access."

---

# 6. Data Analysis Agent

This agent handles structured data.

For example, suppose we have:

```text
Year       AI Adoption
2023       25%
2024       40%
2025       58%
2026       72%
```

The Data Analysis Agent can calculate:

* Growth
* Average
* Trends
* Percentages
* Correlations
* Comparisons
* Statistical analysis

Architecture:

```text
Research Agent
      |
      v
Data Analysis Agent
      |
      +--> SQL
      +--> Python/Data tools
      +--> Calculations
      +--> Charts/statistics
      |
      v
Structured Results
```

Important interview point:

**Don't ask an LLM to perform everything itself if a deterministic tool can do it.**

For example:

```text
"Calculate revenue growth"
        |
        v
Data Analysis Agent
        |
        v
SQL/Python/calculation tool
        |
        v
Exact result
```

### Interview answer

> "For numerical or structured analysis, I would use the Data Analysis Agent with deterministic tools such as SQL or Python rather than relying only on the LLM. This improves accuracy."

---

# 7. Fact-Checking Agent

This is a very important component.

Suppose Web Search Agent returns:

> "AI improves developer productivity by 40%."

We should **not immediately put that into the final report**.

The Fact-Checking Agent verifies:

```text
Claim
  |
  v
Find original source
  |
  v
Check source credibility
  |
  v
Compare supporting evidence
  |
  v
Check date/context
  |
  v
Verified / Not Verified / Conflicting
```

It might return:

```json
{
  "claim": "AI improves developer productivity by 40%",
  "status": "Partially verified",
  "confidence": 0.82,
  "source": "...",
  "notes": "Result applies only to the studied development task."
}
```

### Interview answer

> "I would put a Fact-Checking Agent between research and report generation. It validates important claims against reliable sources, identifies contradictions and prevents unsupported information from reaching the final report."

---

# 8. Report Generation Agent

Once the research is completed:

```text
Web Search
     |
Document/RAG
     |
Data Analysis
     |
     v
Fact Checking
     |
     v
Report Generation
```

The Report Generation Agent receives the **verified information**.

It can generate:

```text
Executive Summary
        ↓
Introduction
        ↓
Key Findings
        ↓
Data Analysis
        ↓
Supporting Evidence
        ↓
Risks / Limitations
        ↓
Conclusion
        ↓
References
```

It should also maintain citations/source references.

### Interview answer

> "The Report Generation Agent consumes the verified research results and produces the final structured report. I would ensure that important statements have source references so the report is traceable."

---

# 9. How do the agents communicate?

I would **not rely on free-form conversations between agents**.

Instead, define a structured contract.

For example:

```text
ResearchRequest
----------------
ResearchId
Question
SubTasks
UserContext
RequiredOutput
```

And:

```text
ResearchResult
----------------
ResearchId
AgentName
Findings
Sources
Confidence
Errors
```

So communication becomes:

```text
Research Agent
      |
      | ResearchTask
      v
Web Search Agent
      |
      | ResearchResult
      v
Research Agent
```

This makes the system easier to debug and monitor.

---

# 10. Sequential vs Parallel Execution

This is a good scenario-based interview question.

Some tasks can run **in parallel**:

```text
              Research Agent
              /      |      \
             /       |       \
            v        v        v
         Web       RAG      Data
        Search    Agent    Analysis
```

These don't necessarily depend on each other.

After they finish:

```text
Web Results
RAG Results
Data Results
      |
      v
Fact Checking
      |
      v
Report Generation
```

So the workflow becomes:

```text
                 ┌── Web Search ──┐
                 │                │
Research ────────┼── RAG ─────────┼──> Fact Check
                 │                │
                 └── Data ────────┘
                                      |
                                      v
                              Report Generation
```

This reduces overall latency.

---

# 11. How do you handle agent failure?

Suppose the Web Search Agent fails.

I wouldn't fail the entire research immediately.

I would use:

* Timeout
* Cancellation
* Bounded retry
* Exponential backoff
* Circuit breaker
* Fallback
* Partial-result handling

For example:

```text
Web Search Agent
       |
       X Failure
       |
       v
Retry
       |
       X
       |
       v
Mark Web Search unavailable
       |
       v
Continue with RAG + Data
```

The final report should clearly indicate:

> "Web research was unavailable, therefore external-source validation is limited."

Don't silently hide the failure.

---

# 12. How do you prevent duplicate work?

Imagine two agents both search the same information.

I would maintain workflow state:

```text
TaskId
Agent
Status
AssignedTo
StartedAt
CompletedAt
Result
```

Example:

```text
Task-101
Web Search
Completed
```

Before starting a task, the orchestrator checks the state.

For important operations, I would also use **idempotency keys**.

---

# 13. How do you manage state?

I wouldn't keep the entire research state only inside the LLM conversation.

I would maintain external workflow state.

For example:

```text
              Research Agent
                    |
                    v
             Workflow State
                    |
        +-----------+-----------+
        |           |           |
       Web         RAG        Data
      Result      Result      Result
```

The state could be stored in a database, Redis, or durable workflow state depending on the requirements.

The state might contain:

```text
ResearchId
UserQuery
Tasks
TaskStatus
AgentResults
Sources
FactCheckResults
FinalReportStatus
```

---

# 14. Security

For an enterprise research system, this is very important.

I would apply:

### Authentication

```text
User → API → Identity
```

### Authorization

Agents should only access the tools/data they need.

For example:

```text
Web Search Agent
→ Internet access

RAG Agent
→ Internal document search

Data Agent
→ Approved databases

Report Agent
→ Research results only
```

The Report Generation Agent should **not automatically have direct access to the company's entire database**.

This follows the **least-privilege principle**.

---

# 15. How would you prevent hallucination?

This is probably one of the most important interview questions.

I would use:

```text
Grounded Retrieval
        +
Source Citations
        +
Fact Checking
        +
Structured Outputs
        +
Confidence/Validation
        +
"Insufficient evidence" handling
```

The system should be allowed to say:

> "I don't have enough reliable evidence to answer this."

rather than inventing information.

---

# 16. How would you monitor the system?

I would track:

```text
Research success rate
Agent success/failure rate
Agent latency
End-to-end latency
LLM token usage
LLM cost
Number of agent calls
Search quality
RAG retrieval quality
Fact-check accuracy
Report quality
Retry count
```

For every research request, I would have a correlation ID:

```text
ResearchId = R12345
```

Then all agent calls use the same ID.

This makes troubleshooting much easier.

---

# 17. Strong interview answer to remember

If the interviewer asks:

> **"How would you design a multi-agent research system?"**

You can give this answer:

> **"I would use a supervisor-based multi-agent architecture. The Research Agent would act as the orchestrator and break the user's question into specialized tasks. The Web Search Agent would collect external information, the Document/RAG Agent would search internal knowledge, and the Data Analysis Agent would handle structured data and calculations. These independent tasks could run in parallel to reduce latency.**
>
> **Once the results are collected, I would send important claims to a Fact-Checking Agent to validate sources and identify contradictions. Only verified results would then be passed to the Report Generation Agent to create the final report with citations.**
>
> **I would use structured contracts between agents and maintain workflow state externally. For reliability, I would use timeouts, bounded retries, cancellation, and failure handling. I would also apply authentication, authorization and least-privilege access to each agent's tools and data. Finally, I would monitor agent latency, failures, token usage, cost, retrieval quality and end-to-end research accuracy."**

### Easy way to remember

**Research → Search/RAG/Data → Fact Check → Report**

And architecturally:

**Orchestration → Communication → State → Security → Reliability → Evaluation**.


## Follow up question How would these agents communicate, share state, recover from failure and decide when the workflow is complete? 

This is a very good **senior-level follow-up question**. I would answer it in four parts:

## 1. How would agents communicate?

I would use **structured messages/contracts** between agents rather than allowing agents to freely chat with each other.

For example:

```text
Research Agent
      |
      | ResearchTask
      v
Web Search Agent
      |
      | ResearchResult
      v
Research Agent
```

A task might contain:

```text
ResearchId
TaskId
Question
RequiredOutput
Context
```

And the result might contain:

```text
TaskId
Status
Findings
Sources
Confidence
Error
```

For distributed agents, these messages could be sent through **APIs, queues, or events**.

### Simple interview answer

> "I would use well-defined structured contracts for communication. Depending on the architecture, agents can communicate through API calls, messages, queues or events. This keeps agents loosely coupled and makes the system easier to monitor and debug."

---

# 2. How would they share state?

I would **not keep the complete state inside the agents or LLM conversation**.

I would maintain workflow state in an external store.

For example:

```text
              Workflow State
                    |
        +-----------+-----------+
        |           |           |
       Web         RAG        Data
      Result      Result      Result
```

The state could contain:

```text
ResearchId
UserQuestion
Tasks
TaskStatus
AgentResults
Sources
FactCheckResults
FinalReportStatus
```

For example:

```text
Task-101 → Web Search → Completed
Task-102 → RAG         → Completed
Task-103 → Data        → Running
```

The orchestrator can check this state at any time.

### Important point

Don't share everything with every agent.

For example:

```text
Data Analysis Agent
        ↓
Only receives required data

Not:
Entire conversation + all company documents
```

This improves **security, performance and maintainability**.

### Interview answer

> "I would maintain shared workflow state in an external state store and pass only the required context to each agent. I would use unique ResearchId and TaskId values to track the workflow."

---

# 3. How would the system recover from failure?

Suppose the Web Search Agent fails:

```text
Research Agent
      |
      v
Web Search Agent
      |
      X
   Failure
```

I would use:

### Transient failure

For example:

* Network timeout
* HTTP 503
* Temporary service unavailable

Use:

```text
Retry
  ↓
Exponential Backoff
  ↓
Retry again
```

But retries should be **bounded**.

Don't retry forever.

### Permanent failure

For example:

* Invalid request
* Authorization failure
* Invalid configuration

Don't keep retrying.

Instead:

```text
Mark task as Failed
       ↓
Use fallback / continue
       ↓
Record failure
```

### Example

If Web Search fails but RAG and Data Analysis succeed:

```text
Web Search     ❌
RAG            ✅
Data Analysis  ✅
Fact Check     ⚠️
Report         ✅
```

The system can still generate a report but should clearly mention that external web research was unavailable.

For long-running workflows, I would also persist the workflow state so that the system can **resume from the last successful step instead of starting everything again**.

### Interview answer

> "For failures, I would use timeouts, cancellation and bounded retries with exponential backoff for transient errors. For permanent failures, I would mark the task as failed and use a fallback or continue with partial results where possible. I would persist workflow state so that a long-running workflow can resume from the last successful step."

---

# 4. How does the system know when the workflow is complete?

This is mainly the **Research Agent's responsibility**.

The orchestrator knows the workflow requirements.

For example:

```text
Research Request
       |
       v
Required Tasks
-----------------
✓ Web Search
✓ RAG Search
✓ Data Analysis
✓ Fact Checking
✓ Report Generation
```

The workflow is complete when:

1. Required research tasks are completed.
2. Important claims have been fact-checked.
3. Required data is available.
4. Report generation succeeds.
5. No critical task is still running or failed without an acceptable fallback.

So:

```text
All required tasks completed?
             |
        +----+----+
       Yes        No
        |          |
        v          v
 Generate       Continue/
   Report        Recover
        |
        v
    COMPLETE
```

### Interview answer

> "The orchestrator should have explicit completion criteria. It checks whether all mandatory tasks are completed successfully, required claims are fact-checked and the final report is generated. If a non-critical task fails but a valid fallback exists, the workflow can still complete with a warning. Otherwise, the workflow remains incomplete or is marked failed."

---

# ⭐ Strong interview answer to remember

If the interviewer asks the complete question:

> **"How would these agents communicate, share state, recover from failure and decide when the workflow is complete?"**

Say:

> **"I would use structured contracts for agent communication, using APIs, messages, queues or events depending on whether the agents are local or distributed. I would maintain the workflow state externally using a ResearchId and TaskId, rather than keeping the entire state inside the LLM conversation.**
>
> **For failures, I would use timeouts, cancellation and bounded retries with exponential backoff for transient failures. For permanent failures, I would mark the task as failed and use fallback or partial-result processing where appropriate. Persisting the workflow state also allows us to resume from the last successful step.**
>
> **Finally, the Research Agent acts as the orchestrator and knows the completion criteria. Once all mandatory tasks are completed, important claims are verified, and the final report is successfully generated, it marks the workflow as complete."**

### Easy formula to remember

**Communication → Structured Messages**

**State → External Workflow Store**

**Failure → Timeout + Retry + Fallback + Resume**

**Completion → Explicit Criteria**

That is a strong answer for a **15-year/senior architect interview**.


## Follow up question Why wouldn't you simply implement this as one LLM with six tools?

Yes, **we could implement it as one LLM with six tools**. In fact, for a simple research application, that would probably be my **first choice**.

The reason to introduce multiple agents is **not simply because we have six different tasks**. We use multiple agents when the responsibilities, permissions, context, or workflows are different enough that separating them provides a real benefit.

## 1. One LLM + Six Tools

A simpler architecture would be:

```text
                    User
                      |
                      v
                 One LLM
                      |
       +--------------+--------------+
       |      |       |       |      |
       v      v       v       v      v
     Web     RAG     Data   Fact    Report
    Search          Analysis Check  Generator
```

The LLM decides which tool to call and in what order.

For many applications, **this is perfectly reasonable**.

### Advantages

* Simpler architecture
* Easier to develop
* Easier to debug
* Less agent-to-agent communication
* Lower latency
* Potentially lower cost
* Easier state management

---

# 2. So why use multiple agents?

I would use multiple agents when the tasks have **strongly different responsibilities**.

For example:

```text
Web Search Agent
    → Internet access

RAG Agent
    → Internal company documents

Data Analysis Agent
    → SQL / Python / analytical tools

Fact Checking Agent
    → Independent verification

Report Agent
    → Report formatting
```

Each agent can have its own:

* Tools
* Instructions
* Context
* Permissions
* Security boundaries
* Evaluation criteria

That's difficult to manage cleanly with one huge LLM prompt.

---

# 3. Security is a major reason

Suppose the Data Analysis Agent has access to a sensitive database.

I don't necessarily want the Web Search Agent to have that access.

With separate agents:

```text
Web Agent
   |
   +-- Internet Search

RAG Agent
   |
   +-- Internal Documents

Data Agent
   |
   +-- Database
```

Each agent gets **only the permissions it needs**.

This follows the **least-privilege principle**.

With one LLM having six tools, you have to be much more careful about which tools it can invoke and under what conditions.

---

# 4. Different agents can have different instructions

For example:

### Data Analysis Agent

```text
Never guess numbers.
Use SQL/Python for calculations.
Return exact results.
```

### Fact-Checking Agent

```text
Don't trust the original research blindly.
Look for supporting evidence.
Identify contradictions.
```

### Report Agent

```text
Use only verified information.
Include citations.
Clearly identify limitations.
```

These are very different responsibilities.

Separating them can make the system easier to control.

---

# 5. Independent scaling

Suppose we have:

```text
10,000 Web searches/hour

1,000 RAG searches/hour

100 Data Analysis requests/hour
```

The workloads are different.

With separate agents/services, we can scale them independently.

```text
Web Agent        → 10 instances
RAG Agent        → 5 instances
Data Agent       → 2 instances
```

This becomes more useful in a large enterprise system.

---

# 6. Independent failure handling

Suppose the web-search capability is unavailable.

With separate agents:

```text
Web Search Agent ❌

RAG Agent        ✅
Data Agent       ✅
```

The orchestrator can decide:

> "Web research failed, but I can continue with internal documents and available data."

This gives us better control over partial failures.

---

# 7. Independent evaluation

We can evaluate each agent separately.

For example:

```text
Web Agent
→ Search relevance

RAG Agent
→ Retrieval accuracy

Data Agent
→ Calculation accuracy

Fact Checker
→ Verification accuracy

Report Agent
→ Report quality
```

With one LLM + six tools, evaluation can become more coupled.

---

# 8. But don't over-engineer

This is **very important for an architect interview**.

I would **not automatically choose six agents**.

My decision would be:

```text
Simple problem?
      |
      v
One LLM + tools
```

If the problem has:

```text
Different responsibilities
        +
Different permissions
        +
Different context
        +
Independent scaling
        +
Complex workflows
```

then:

```text
Multiple specialized agents
```

So the answer isn't:

> "Multi-agent is always better."

The better answer is:

> **"Choose the simplest architecture that meets the requirements."**

---

# ⭐ Strong interview answer

If the interviewer asks:

> **"Why wouldn't you simply implement this as one LLM with six tools?"**

I would say:

> **"Actually, I would consider that approach first. If the system is relatively simple, one LLM with six well-defined tools would be easier to build, cheaper and easier to maintain.**
>
> **I would introduce multiple agents only when there is a clear architectural benefit—for example, when different capabilities need different instructions, permissions, context, security boundaries, scaling or failure handling. For example, my Data Analysis Agent may need database access while my Web Search Agent only needs internet search.**
>
> **So I wouldn't use multiple agents just because we have six tasks. I would use them when separation of responsibility provides real value. Otherwise, one LLM with tools is the simpler and better solution."**

### One line to remember

> **"Multi-agent is not automatically better; I use it when separation of responsibility, security, scaling or workflow complexity justifies the additional complexity."**


