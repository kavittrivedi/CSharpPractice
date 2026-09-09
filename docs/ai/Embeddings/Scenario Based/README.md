
---

## 1. Your search quality is poor after switching embedding models. What would you investigate?

First, I would check whether the problem is caused by the **new embedding model or something else in the RAG pipeline**.

### I would investigate:

**1. Are indexing and querying using the same model?**

```text
Documents → New Model → Vectors
Questions → New Model → Vectors
```

Both must use the same compatible model.

**2. Check vector dimensions**

For example:

```text
Old model → 1536 dimensions
New model → 3072 dimensions
```

The index configuration must match the new model.

**3. Check chunking**

The new model may behave differently with:

* Very large chunks
* Very small chunks
* Poorly separated content

**4. Check similarity scores**

Compare retrieval scores from the old and new models.

**5. Check Top-K**

Maybe the relevant document is being retrieved but ranked lower.

**6. Check metadata filters**

Make sure tenant, department, document type, security filters, etc. haven't accidentally excluded relevant documents.

**7. Check hybrid search/reranking**

If using vector + keyword search, verify that the weighting and reranking configuration still works well.

**8. Compare against a test dataset**

Use known questions and expected documents to objectively compare old vs new model.

### Interview answer

> **After switching models, I would first verify that both indexing and querying use the new model and that the vector dimensions are correct. Then I would compare retrieval scores, Top-K results, chunking, metadata filters, hybrid-search configuration, and reranking. Finally, I would run a representative evaluation dataset to determine whether the quality problem is really caused by the new embedding model.**

---

# 2. Your documents are multilingual. How would you choose an embedding model?

I would choose a model with **strong multilingual support**.

For example, suppose my documents contain:

```text
English
Hindi
French
German
Japanese
```

I would test whether the model can understand semantic similarity **across languages**.

For example:

```text
English:
"How can I apply for leave?"

Hindi:
"मैं छुट्टी के लिए आवेदन कैसे कर सकता हूँ?"
```

A good multilingual embedding model should understand that these questions have essentially the same meaning.

### What I would evaluate:

* Supported languages
* Cross-language semantic search
* Retrieval quality for each important language
* Domain-specific terminology
* Cost
* Latency
* Vector dimensions

I wouldn't select the model just because it claims to support many languages. I would **test it using my actual documents and queries**.

### Interview answer

> **For multilingual documents, I would select a multilingual embedding model and evaluate it using representative queries from each important language. I would specifically test cross-language retrieval, domain terminology, quality, cost, and latency before selecting the model.**

---

# 3. Your documents contain code, tables and natural language. What embedding strategy would you use?

I would **not treat all content the same way**.

Different content types need different handling.

### Natural language

For:

```text
Policies
Articles
Documentation
FAQs
```

I would use normal semantic chunking and embeddings.

### Code

For code, I would preserve:

```text
Class
Method
Namespace
Comments
Dependencies
```

I would avoid blindly splitting code in the middle of an important method or class.

Depending on the use case, I may also use **code-aware embeddings/search**.

### Tables

Tables are more difficult.

I would preserve:

```text
Table name
Column names
Rows
Relationships
```

I wouldn't simply convert a large table into arbitrary 500-token chunks.

For highly structured/analytical data, I may use a **database or structured query tool instead of relying only on embeddings**.

### Architecture

Conceptually:

```text
Documents
     ↓
Classify content
     ↓
 ┌──────────┬──────────┬─────────────┐
 │  Text    │   Code   │   Tables    │
 ↓          ↓          ↓
Text       Code       Structured
chunking   chunking   representation
 ↓          ↓          ↓
Embedding  Embedding  DB/Search
```

### Interview answer

> **I would use a content-aware strategy rather than treating everything as plain text. Natural language can use semantic chunking and embeddings, code should be chunked around classes and methods, and tables should preserve their structure. For highly structured or analytical table queries, I would consider using a database or tool-based approach rather than depending only on vector search.**

---

# 4. Your embedding cost has become significant. How would you optimize it?

I would first identify **where the cost is coming from**.

Then I would optimize the pipeline.

### 1. Don't re-embed unchanged documents

Use:

```text
Content Hash
ETag
Last Modified
Version
```

If the document hasn't changed:

```text
No re-embedding
```

This can save a lot of money.

---

### 2. Remove unnecessary content

Before generating embeddings:

```text
Remove duplicate content
Remove unnecessary whitespace
Remove repeated headers/footers
Remove irrelevant content
```

Less text means fewer tokens.

---

### 3. Optimize chunking

Don't create unnecessarily large chunks or excessive overlapping chunks.

For example:

```text
Bad:
1000 chunks → lots of embedding calls

Better:
Meaningful chunks → fewer useful embeddings
```

But don't reduce chunk size just to save money if retrieval quality becomes worse.

---

### 4. Batch embedding requests

Where the provider supports it, send multiple chunks together instead of making one request per chunk.

```text
Chunk 1 ┐
Chunk 2 ├──→ Embedding API
Chunk 3 │
Chunk 4 ┘
```

---

### 5. Cache embeddings

If exactly the same content appears again:

```text
Content Hash
     ↓
Already embedded?
     ↓
Yes → Reuse vector
```

---

### 6. Use an appropriate model

A smaller/cheaper model may be sufficient if evaluation shows that quality remains acceptable.

### Interview answer

> **I would optimize embedding cost mainly by avoiding unnecessary embedding generation. I would use content hashing or versioning to process only new or changed documents, remove duplicate content, optimize chunking, batch embedding requests, cache reusable embeddings, and evaluate whether a smaller model can provide acceptable retrieval quality.**

---

# 5. How would you identify bad or meaningless embeddings?

This is an interesting question because **a vector itself doesn't tell us whether the original content was meaningful**.

I would check it at multiple levels.

### 1. Check the source content

Examples of bad content:

```text
Empty document
Very short text
Only headers/footers
Corrupted OCR
Repeated garbage text
Random characters
```

These should ideally be filtered **before embedding**.

---

### 2. Check embedding generation

Verify:

* Vector is not null
* Correct dimensionality
* No invalid values such as NaN/Infinity
* API didn't return an error

For example:

```text
Expected: 1536 dimensions

Actual: 1000 dimensions
```

That's a problem.

---

### 3. Check similarity behavior

Take known queries and see whether meaningless documents frequently appear as top results.

For example:

```text
Query:
"How many vacation days do employees get?"

Top result:
"asdf qwer xyz 123..."
```

That's a strong indication that something is wrong with the content/indexing pipeline.

---

### 4. Monitor retrieval metrics

Track:

* Precision
* Recall
* Similarity-score distribution
* Zero-result rate
* User feedback
* Top-K relevance

### Interview answer

> **I would identify bad embeddings by checking both the source content and the generated vectors. I would filter empty or corrupted content before embedding, validate vector dimensions and values, and then test retrieval behavior using known queries. If meaningless documents frequently appear in Top-K results, I would investigate the ingestion, chunking, embedding, and indexing pipeline.**

---

# 6. How would you benchmark two embedding models before selecting one?

This is probably the **most important question** from this group.

I would **not select the model based only on vendor documentation or vector dimensions**.

I would create a representative evaluation dataset.

For example:

```text
Question                         Expected Document
---------------------------------------------------
"How many leave days?"           LeavePolicy.pdf
"How do I reset password?"       ITSecurity.pdf
"How do I claim expenses?"       ExpensePolicy.pdf
```

Then run both models.

```text
                 Model A       Model B
Question 1       Relevant      Relevant
Question 2       Relevant      Wrong
Question 3       Relevant      Relevant
...
```

### Measure:

**1. Retrieval precision**

Are the retrieved documents relevant?

**2. Recall**

Are we finding the documents that should be found?

**3. Top-K accuracy**

Is the correct document appearing in Top-3, Top-5, etc.?

**4. Latency**

How quickly are embeddings generated and searches completed?

**5. Cost**

How much does embedding the entire document corpus cost?

**6. Storage**

How much vector storage does the model require?

**7. Domain/language performance**

Does it work well with our actual content?

---

### Example decision

```text
                    Model A      Model B
Retrieval Quality    88%          94%
Recall               90%          95%
Latency              100ms        130ms
Cost                 $100         $150
```

I wouldn't automatically choose Model A because it's cheaper or Model B because it has higher accuracy.

I would consider the **business requirement and trade-offs**.

### Interview answer

> **I would create a representative evaluation dataset containing real user questions and expected relevant documents. I would run both embedding models against the same dataset and compare Top-K retrieval accuracy, precision, recall, similarity quality, latency, cost, storage, and performance across different content types and languages. I would then select the model based on the overall quality and cost trade-off rather than just vector dimensions or benchmark claims.**

---

# ⭐ Senior-Level Quick Revision

| Scenario                       | What should you say?                                                                                           |
| ------------------------------ | -------------------------------------------------------------------------------------------------------------- |
| **Search quality became poor** | Check model consistency, dimensions, chunking, filters, Top-K, hybrid search, reranking and benchmark results. |
| **Multilingual documents**     | Use multilingual model and test each important language + cross-language retrieval.                            |
| **Code + tables + text**       | Use content-aware processing; don't blindly chunk everything the same way.                                     |
| **Embedding cost is high**     | Incremental processing, hashing, caching, batching, optimized chunking and appropriate model selection.        |
| **Bad embeddings**             | Validate source content, vector dimensions/values and retrieval behavior.                                      |
| **Benchmark two models**       | Same dataset → compare retrieval quality, precision, recall, Top-K, latency, cost and storage.                 |

### ⭐ One strong answer for a senior interview

If the interviewer asks **"How would you evaluate an embedding model for production?"**, you can say:

> **I would evaluate it using our actual domain data rather than relying only on generic benchmarks. I would create a representative question-and-expected-document dataset, compare Top-K retrieval, precision and recall, and also evaluate latency, cost, storage, multilingual support and domain-specific terminology. I would run the new model alongside the existing model, compare the results, and migrate only if it provides a meaningful improvement for our business requirements.**
