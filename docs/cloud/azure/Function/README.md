## What is default timeout and maximum timeout of Azure Function?

For **Azure Functions**, the timeout depends on the hosting plan:

| Hosting plan                | Default timeout | Maximum timeout |
| --------------------------- | --------------: | --------------: |
| **Consumption**             |       5 minutes |  **10 minutes** |
| **Flex Consumption**        |      30 minutes |  **Unlimited*** |
| **Premium**                 |      30 minutes |  **Unlimited*** |
| **Dedicated (App Service)** |      30 minutes |  **Unlimited*** |

* You can configure an unlimited timeout using `functionTimeout: -1`, but the platform/runtime and trigger-specific limits can still apply.

### `host.json`

You configure it like this:

```json
{
  "version": "2.0",
  "functionTimeout": "00:10:00"
}
```

For example, on **Consumption**, you cannot increase this beyond **10 minutes**.

### Interview answer

> **Azure Functions Consumption plan has a default timeout of 5 minutes and a maximum of 10 minutes. Premium and Dedicated plans default to 30 minutes and can be configured for longer or unlimited execution. For long-running workloads, I would use Durable Functions or an asynchronous queue-based architecture rather than keeping a single function execution running for a long time.**

For your earlier **1-million-file embedding** scenario, I would **not** process all files in one Function execution. I would use **Queue/Service Bus + parallel Function executions + batching + rate limiting**, and potentially Durable Functions for orchestration.

## So we have to do all this " **Queue/Service Bus + parallel Function executions + batching + rate limiting**, and potentially Durable Functions for orchestration." to process 1 million files?

Not necessarily **all of them**. The architecture depends on the workload and how much control you need.

For **1 million files → generate embeddings → store in vector DB**, I would typically use:

```text
                    ┌───────────────┐
1M Files ──────────►│ Blob Storage  │
                    └───────┬───────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ Queue/Service │
                    │     Bus       │
                    └───────┬───────┘
                            │
              ┌─────────────┼─────────────┐
              ▼             ▼             ▼
          Function       Function       Function
          Instance       Instance       Instance
              │             │             │
              └─────────────┼─────────────┘
                            ▼
                     Embedding Model
                            │
                            ▼
                       Vector DB
```

### What is actually needed?

| Component                       | Needed?       | Why                                                           |
| ------------------------------- | ------------- | ------------------------------------------------------------- |
| **Queue / Service Bus**         | ✅ Recommended | Decouple files and allow parallel processing                  |
| **Multiple Function instances** | ✅ Yes         | Process many files concurrently                               |
| **Batching**                    | ✅ Recommended | Reduce API calls and improve throughput                       |
| **Rate limiting**               | ✅ Yes         | Prevent embedding API throttling                              |
| **Durable Functions**           | ⚠️ Optional   | Useful for orchestration/progress tracking, but not mandatory |

### The important point

You **don't want one Function execution to process 1 million files**:

```text
Function
   │
   ├── File 1
   ├── File 2
   ├── File 3
   ...
   └── File 1,000,000   ❌
```

Instead:

```text
1,000,000 files
       ↓
 Queue
       ↓
 ┌─────┬─────┬─────┬─────┐
 F1    F2    F3    F4 ... Fn
 ↓     ↓     ↓     ↓
Batch Batch Batch Batch
       ↓
Embedding API
       ↓
Vector DB
```

The **queue + autoscaling Functions** handle the scale-out. **Rate limiting** protects the embedding model from exceeding its TPM/RPM limits.

### Do you need Durable Functions?

For a simple ingestion pipeline:

> **Blob → Queue → Function → Embedding → Vector DB**

**No, Durable Functions are not mandatory.**

I'd introduce Durable Functions if you need things like:

* Track the overall 1-million-file job
* Know when all files are completed
* Retry failed batches
* Resume from failures
* Maintain job-level status/progress
* Run multiple stages such as:
  `Extract → Chunk → Embed → Index`

So for an interview, I'd answer:

> **For 1 million files, I would use an asynchronous queue-based architecture with Azure Functions scaled horizontally. I'd process files in batches and apply rate limiting based on the embedding model's RPM/TPM limits. Durable Functions are optional and would be useful when I need reliable orchestration and tracking of the overall ingestion job.**
