
---

# Scenario

> **You have 50 million vectors and need <500 ms retrieval latency with metadata filtering. Design the vector-search architecture.**

I would design it like this:

```text
                    User
                      ↓
                .NET API
                      ↓
             Authentication
                      ↓
          Build Search Query
                      ↓
       ┌──────────────┴──────────────┐
       │                             │
 Vector Query                  Metadata Filters
       │                             │
       └──────────────┬──────────────┘
                      ↓
             Vector Search Engine
                      ↓
              ANN Index / HNSW
                      ↓
             Top 50 Candidates
                      ↓
                 Reranker
                      ↓
               Top 5-10 Results
                      ↓
                 .NET API
                      ↓
                    User
```

## 1. Use an ANN-based vector index

With **50 million vectors**, I would not perform a brute-force search against every vector.

I would use an **ANN index**, such as HNSW if supported by the chosen platform.

```text
50M vectors
     ↓
   HNSW
     ↓
Small candidate set
     ↓
Top results
```

This significantly reduces the amount of data that needs to be searched.

---

## 2. Apply metadata filtering

Suppose every vector contains:

```text
DocumentId
TenantId
Department
DocumentType
Date
SecurityLevel
```

A request might be:

```text
Query:
"How many vacation days?"

Filter:
TenantId = T100
Department = HR
DocumentType = Policy
```

The search should use the metadata filter together with vector search.

```text
Vector similarity
       +
Metadata filter
       ↓
Relevant candidates
```

This is important not only for performance but also for **security**.

> I would apply security-related filters on the backend, not trust filters sent by the Angular frontend.

---

## 3. Reduce the candidate set

I wouldn't retrieve hundreds or thousands of results unnecessarily.

For example:

```text
50M vectors
    ↓
Metadata filtering
    ↓
ANN search
    ↓
Top 50 candidates
    ↓
Reranking
    ↓
Top 5-10
```

The exact numbers would be determined through benchmarking.

---

## 4. Use reranking carefully

Reranking improves relevance but adds latency.

Since the requirement is **<500 ms**, I would rerank only a small candidate set.

For example:

```text
ANN Search
   ↓
50 candidates
   ↓
Reranker
   ↓
Top 5
```

I wouldn't send thousands of candidates to the reranker.

---

# 5. Partition or shard the data

50 million vectors is large enough that I would evaluate **partitioning/sharding**.

For example, if this is a multi-tenant application:

```text
                50M vectors
                     ↓
          ┌──────────┼──────────┐
          ↓          ↓          ↓
       Shard 1    Shard 2    Shard 3
       Tenant A   Tenant B   Tenant C
```

But I wouldn't automatically partition by tenant.

I would choose the partitioning strategy based on:

* Data distribution
* Query patterns
* Tenant sizes
* Filtering requirements
* Infrastructure capabilities

---

# 6. Keep the search path fast

For a **<500 ms** requirement, I would avoid unnecessary synchronous operations.

The query path should be:

```text
Request
  ↓
Authentication
  ↓
Query embedding
  ↓
Vector + metadata search
  ↓
Optional reranking
  ↓
Response
```

I would **not** call unrelated services synchronously in this path.

---

# 7. Cache where appropriate

Some queries may repeat frequently.

For example:

```text
Same query + same filters
              ↓
            Cache
              ↓
          Cached results
```

I could use Redis or another caching layer for frequently repeated searches.

But I wouldn't depend on caching to meet the 500 ms requirement. The vector-search system itself should meet the latency target.

---

# 8. Monitor P95/P99, not just average latency

This is an important senior-level point.

I would monitor:

```text
Embedding latency
Vector search latency
Metadata-filter latency
Reranking latency
End-to-end latency
```

Especially:

```text
P50
P95
P99
```

If the requirement is **<500 ms**, I would clarify whether that means:

> **P95 < 500 ms**

or

> **every request <500 ms**

because those are very different requirements.

---

# 9. Benchmark with real data

Before finalizing the architecture, I would load the actual **50 million vectors** and test realistic queries.

I would measure:

| Metric             |                  Target |
| ------------------ | ----------------------: |
| P50 latency        |                     Low |
| P95 latency        |                 <500 ms |
| P99 latency        |       As defined by SLA |
| Recall@K           |              Acceptable |
| Throughput         | Based on expected users |
| Filter performance |              Acceptable |
| CPU/Memory         |                 Healthy |

I would tune ANN parameters, Top-K, filtering, shard count, and reranking based on the results.

---

# ⭐ Interview-ready answer

You can give this answer in an interview:

> **For 50 million vectors with a sub-500 ms retrieval requirement, I would use a vector-search engine with an ANN index such as HNSW. I would store important metadata like TenantId, Department and DocumentType with each vector and apply those filters during server-side retrieval. I would first use ANN search to get a small candidate set, for example 50 results, and optionally rerank those candidates to get the final 5-10 results.**
>
> **For 50 million vectors, I would also evaluate partitioning or sharding based on the query and tenant distribution. I would keep the query path lightweight, use caching for frequently repeated searches where useful, and avoid unnecessary synchronous dependencies. Finally, I would benchmark the system with realistic data and traffic and monitor P50, P95 and P99 latency, recall, throughput, CPU, memory and filter performance.**

### ⭐ Remember this architecture

```text
50M Vectors
     ↓
Partition / Shard
     ↓
ANN Index (HNSW)
     ↓
Metadata Filter
     ↓
Small Candidate Set
     ↓
Reranking
     ↓
Top 5-10
     ↓
< 500 ms
```

**The three keywords to remember for this question are:**

> **ANN + Metadata Filtering + Small Candidate Set**

These three directly address the **50M scale + filtering + latency** requirements.
