Absolutely. These are **very common embedding interview questions**. I’ll explain them in **simple language**, with a small example and an **interview-ready answer** for each.

---

## 1. What is an embedding?

An **embedding is a numerical representation of text** that captures its meaning.

For example:

* `"I love my car"`
* `"I really like my automobile"`

These sentences have different words, but their meaning is similar. Their embeddings will therefore be **close to each other in vector space**.

Example:

```text
"I love my car"
        ↓
Embedding Model
        ↓
[0.21, -0.45, 0.78, 0.12, ...]
```

The vector can have hundreds or thousands of numbers.

### Interview answer

> **An embedding is a vector representation of data, such as text, that captures its semantic meaning. We use embeddings mainly for similarity search, recommendation, clustering, and RAG applications.**

---

# 2. How does text become a vector?

We use an **embedding model**.

The basic flow is:

```text
Text
 ↓
Tokenization
 ↓
Embedding Model
 ↓
Vector
```

For example:

```text
"How can I apply for leave?"
             ↓
      Embedding Model
             ↓
[0.12, -0.34, 0.76, 0.21, ...]
```

The model has been trained to understand relationships between words and sentences.

The important point is:

> We don't manually convert words into numbers. The embedding model does it.

### Interview answer

> **We pass the text to an embedding model, and the model converts the text into a fixed-length numerical vector. The values in that vector represent semantic features learned by the model.**

---

# 3. Why do semantically similar sentences have similar vectors?

Because the embedding model is **trained to represent similar meanings close together** in vector space.

For example:

```text
"How do I apply for leave?"
"What's the process to request vacation?"
```

Although the words are different, the meaning is similar.

So their vectors may look conceptually like:

```text
Sentence A → [0.2, 0.8, 0.4, ...]
Sentence B → [0.21, 0.79, 0.41, ...]
```

Their vectors are close.

But:

```text
"How do I reset my password?"
```

would produce a vector farther away because the meaning is different.

### Important interview point

The model **learns these relationships during training**. We don't define manually that "leave" and "vacation" should be similar.

### Interview answer

> **Embedding models are trained on large amounts of data to capture semantic relationships. Therefore, texts with similar meanings tend to produce vectors that are close together, even when they use different words.**

---

# 4. What determines embedding dimensionality?

**The embedding model determines the dimensionality.**

For example, a model may produce:

```text
Model → 768 dimensions
Model → 1536 dimensions
Model → 3072 dimensions
```

If a model produces a 1536-dimensional embedding, every piece of text produces exactly **1536 numbers**.

You generally **cannot arbitrarily change it** to 1000 dimensions without applying some separate dimensionality-reduction technique, and that changes the representation.

### Why does dimensionality matter?

Higher dimensionality can potentially represent more information, but it also means:

* More storage
* More memory
* More computation
* Potentially higher search cost

### Interview answer

> **Embedding dimensionality is determined by the embedding model. Different models produce different vector sizes, and the vector index must be configured to match that dimension.**

---

# 5. How do you select an embedding model?

For an enterprise RAG system, I would consider:

### 1. Semantic quality

Does it retrieve the correct documents?

### 2. Domain performance

Does it work well with our domain?

For example:

```text
Healthcare
Finance
Legal
Technology
```

### 3. Language support

If documents contain multiple languages, the model should support them well.

### 4. Dimensionality

Higher dimensions generally mean more storage and computation.

### 5. Cost

Consider:

* Embedding generation cost
* Storage
* Search infrastructure

### 6. Latency

How quickly can embeddings be generated?

### 7. Maximum input size

Important when embedding large chunks.

### Interview answer

> **I don't select an embedding model only based on vector dimensions. I evaluate retrieval quality, domain and language support, latency, cost, input limits, and infrastructure compatibility. For production, I validate the model using a representative evaluation dataset and compare retrieval metrics before choosing it.**

---

# 6. What is cosine similarity?

**Cosine similarity measures how similar two vectors are based on the angle between them.**

Conceptually:

```text
Vector A ───────►
                 \
                  \ Vector B
```

If they point in almost the same direction, they are considered similar.

genui{"learning_viz":{"type_id":"VECTOR_DOT_PRODUCT"}}

A common formula is:

**cosine similarity = dot product / (magnitude of A × magnitude of B)**

Typically:

```text
1    → very similar direction
0    → unrelated/orthogonal
-1   → opposite direction
```

The exact useful range can depend on the embedding model and whether vectors are normalized, but the core idea is:

> **Higher cosine similarity generally means greater semantic similarity.**

### Example

```text
Question embedding
        ↓
[ ... ]

Document A embedding
        ↓
[ ... ]

Cosine similarity = 0.91
```

Document A is likely highly relevant.

### Interview answer

> **Cosine similarity measures the similarity between two vectors by comparing their direction rather than just their absolute distance. In RAG, we can use it to find document embeddings that are semantically closest to the user's query embedding.**

---

# 7. What is embedding normalization?

Normalization means converting a vector so that its **magnitude, or length, becomes 1**.

For example:

```text
Original vector
[3, 4]

Magnitude = 5

Normalized vector
[0.6, 0.8]
```

Why do this?

It makes vector comparisons more consistent because we remove the effect of vector magnitude.

This is especially useful when using similarity calculations where vector direction is what matters.

### Interview answer

> **Embedding normalization scales a vector so that its magnitude becomes 1. It can make similarity calculations more consistent, especially when using cosine similarity. Whether normalization should be done manually depends on the embedding model and vector-search system.**

---

# 8. Can you use different embedding models for indexing and querying?

**Generally, no.**

This is a very important RAG interview question.

Suppose during indexing you use:

```text
Model A

Document → Model A → Vector A
```

But during querying you use:

```text
Model B

Question → Model B → Vector B
```

You then compare:

```text
Vector A ↔ Vector B
```

The problem is that the two vectors were generated using **different embedding spaces**.

Even if both vectors have the same number of dimensions, their values don't necessarily have compatible meanings.

### Correct approach

Use the same embedding model:

```text
Document
   ↓
Embedding Model A
   ↓
Vector
   ↓
Index


User Question
   ↓
Embedding Model A
   ↓
Vector
   ↓
Search
```

### Interview answer

> **Normally, indexing and querying should use the same embedding model and compatible configuration. Using different models can make similarity scores unreliable because the vectors belong to different embedding spaces.**

---

# 9. What happens if you change your embedding model?

Changing the embedding model can make your existing vectors **incompatible** with the new query embeddings.

Suppose your existing index was created using:

```text
Old Model
     ↓
Documents → Old vectors
```

Now you change to:

```text
New Model
     ↓
Questions → New vectors
```

You cannot reliably compare:

```text
Old document vectors
        ↕
New query vectors
```

Therefore, changing the embedding model usually means you need to **re-embed your documents**.

### Other things that may change

Changing the model can affect:

* Retrieval quality
* Vector dimensions
* Storage requirements
* Similarity scores
* Search performance
* Cost

### Interview answer

> **If I change the embedding model, I don't simply start generating new query embeddings. I evaluate the new model and normally re-embed the existing documents because the old and new vectors may belong to different vector spaces.**

---

# 10. How do you migrate an existing vector index to a new embedding model?

This is a good **senior-level scenario question**.

I would avoid changing everything at once.

### Step 1 — Evaluate the new model

Take a representative test dataset:

```text
Questions
Expected documents
Expected answers
```

Compare:

```text
Old model retrieval
        VS
New model retrieval
```

---

### Step 2 — Create a new index

Don't immediately destroy the existing index.

For example:

```text
documents-v1
    ↓
Old Embedding Model

documents-v2
    ↓
New Embedding Model
```

---

### Step 3 — Re-embed documents

Process existing documents again:

```text
Documents
   ↓
New Embedding Model
   ↓
New vectors
   ↓
New index
```

For a large enterprise system, I would do this **asynchronously and in batches**.

---

### Step 4 — Validate

Compare:

* Retrieval precision
* Recall
* Relevance
* Latency
* Cost
* Index size

---

### Step 5 — Switch traffic

Once the new index is validated:

```text
Application
     ↓
New Vector Index
```

This can be done using configuration/feature flags or a controlled deployment.

---

### Step 6 — Keep the old index temporarily

Don't delete the old index immediately.

If something goes wrong:

```text
New Index
   ❌
   ↓
Rollback
   ↓
Old Index
```

Once the new system is stable, retire the old index.

### Interview answer

> **For an embedding-model migration, I would create a new version of the vector index, re-embed the existing documents using the new model, validate retrieval quality and performance against the old model, and then switch traffic to the new index. I would keep the old index temporarily so I can quickly roll back if required.**

---

# ⭐ Quick Interview Revision

| Question                                        | Simple answer                                                                   |
| ----------------------------------------------- | ------------------------------------------------------------------------------- |
| **What is embedding?**                          | Numerical representation of data that captures meaning.                         |
| **How text becomes vector?**                    | Embedding model converts text into a fixed-length vector.                       |
| **Why similar sentences have similar vectors?** | Model learns semantic relationships during training.                            |
| **What determines dimensions?**                 | The embedding model.                                                            |
| **How select model?**                           | Quality, domain, language, cost, latency, dimensions and evaluation results.    |
| **What is cosine similarity?**                  | Measures similarity based mainly on vector direction.                           |
| **What is normalization?**                      | Scaling vector magnitude to 1.                                                  |
| **Different model for indexing/querying?**      | Generally no; use the same compatible model.                                    |
| **What if model changes?**                      | Existing vectors may become incompatible; re-embedding is normally required.    |
| **How migrate?**                                | New index → re-embed → validate → switch traffic → keep old index for rollback. |

### One sentence to remember for interviews

> **In a RAG system, I use the same embedding model for both document indexing and query embedding. If I change the model, I normally create a new index, re-embed the documents, evaluate retrieval quality, and switch traffic only after validation.**
