Below are **simple, interview-ready answers** for RAG fundamentals. I’ll use one common example throughout:

> **Example:** An employee asks: *“How many leaves can I carry forward?”*

---

# RAG Fundamentals

## 1. What is RAG?

**RAG stands for Retrieval-Augmented Generation.**

It combines:

* **Retrieval** → Find relevant information from documents.
* **Augmented** → Add that information to the LLM prompt.
* **Generation** → LLM generates the answer.

```text
User Question
     ↓
Retrieve relevant documents
     ↓
Add relevant content to prompt
     ↓
LLM
     ↓
Answer
```

**Interview answer:**

> "RAG is a technique where we retrieve relevant information from external documents and provide it to the LLM as context before generating the answer. This helps the LLM answer using current and private data."

---

## 2. Explain the complete RAG pipeline

A typical RAG pipeline has two major parts.

### A. Indexing / Ingestion

```text
Documents
   ↓
Extract Text
   ↓
Chunk Documents
   ↓
Generate Embeddings
   ↓
Store in Vector Database/Search Index
```

### B. Query / Retrieval

```text
User Question
   ↓
Generate Query Embedding
   ↓
Search Vector Database
   ↓
Retrieve Relevant Chunks
   ↓
Add Chunks to LLM Prompt
   ↓
LLM Generates Answer
```

**Interview answer:**

> "The RAG pipeline first ingests documents by extracting text, chunking it, generating embeddings and storing them in a search or vector index. At query time, we retrieve relevant chunks and send them with the user's question to the LLM."

---

## 3. What is document ingestion?

Document ingestion means **taking source documents and preparing them for retrieval**.

For example:

```text
PDF
DOCX
TXT
HTML
     ↓
Extract content
     ↓
Clean/Normalize
     ↓
Chunk
     ↓
Create embeddings
     ↓
Store
```

During ingestion, we may also extract metadata:

```text
DocumentName
PageNumber
Department
CreatedDate
TenantId
```

**Interview answer:**

> "Document ingestion is the process of taking source documents, extracting and preparing their content, creating chunks and embeddings, and storing them with metadata so they can later be retrieved."

---

## 4. What is chunking?

A document can be too large to send directly to an LLM.

So we divide it into smaller pieces called **chunks**.

Example:

```text
Employee Handbook
       ↓
Chunk 1
Chunk 2
Chunk 3
Chunk 4
```

Instead of retrieving the complete handbook, RAG retrieves only relevant chunks.

**Interview answer:**

> "Chunking is dividing a large document into smaller meaningful pieces so that we can search and retrieve only the information relevant to the user's question."

---

## 5. Why is chunk size important?

Chunk size affects retrieval quality.

### Very large chunks

```text
Large chunk
```

Problems:

* Contains too much unrelated information.
* More tokens are sent to the LLM.
* Higher cost.
* Important information may be diluted.

### Very small chunks

```text
Very small chunk
```

Problems:

* Context may be incomplete.
* A sentence may lose its surrounding meaning.

The goal is to find a good balance.

**Interview answer:**

> "Chunk size is important because very large chunks may contain irrelevant information and increase cost, while very small chunks may lose context. The best size depends on the document type and use case."

---

## 6. Fixed-size vs semantic chunking?

### Fixed-size chunking

Split after a fixed number of:

* Characters
* Words
* Tokens

Example:

```text
Every 500 tokens = one chunk
```

**Advantages:** Simple and fast.

**Disadvantage:** It may split content in the middle of a logical section.

---

### Semantic chunking

Split based on meaning or document structure.

For example:

```text
Leave Policy
   ↓
Annual Leave section

Expense Policy
   ↓
Travel Expense section
```

**Advantages:** Better logical context.

**Disadvantages:** More complex and can cost more to process.

**Interview answer:**

> "Fixed-size chunking splits content based on a fixed length, while semantic chunking tries to split based on meaning or logical boundaries. Semantic chunking can improve context quality but is more complex."

---

## 7. What is chunk overlap?

Chunk overlap means repeating some content between consecutive chunks.

Example:

```text
Chunk 1:
Sentence 1 → Sentence 10

Chunk 2:
Sentence 8 → Sentence 18
```

Here, Sentences 8–10 overlap.

This helps preserve context when an important idea crosses a chunk boundary.

**Interview answer:**

> "Chunk overlap repeats some content between neighboring chunks so that context is not lost when related information spans the boundary between two chunks."

---

## 8. What are embeddings?

Embeddings are **numerical representations of text that capture semantic meaning**.

For example:

```text
"What is the leave policy?"
        ↓
Embedding Model
        ↓
[0.12, -0.45, 0.89, ...]
```

Similar meanings usually produce vectors that are close together in embedding space.

Example:

```text
"How many leaves do I have?"
"What is my annual leave allowance?"
```

These have similar meanings, so their embeddings should be relatively similar.

**Interview answer:**

> "Embeddings are numerical vector representations of text that capture semantic meaning. They allow us to search for content based on meaning rather than only exact keyword matches."

---

## 9. What is vector similarity?

Vector similarity measures **how similar two embeddings are**.

Example:

```text
User Question Embedding
        ↓
Compare
        ↓
Document Chunk Embeddings
        ↓
Most Similar Chunks
```

The chunk with the highest similarity is considered more relevant.

**Interview answer:**

> "Vector similarity measures how close two embedding vectors are. In RAG, we compare the user's question embedding with document chunk embeddings to find semantically relevant information."

---

## 10. Cosine similarity vs Euclidean distance?

Both compare vectors, but differently.

### Cosine similarity

Measures the **angle or direction** between vectors.

It focuses more on how similarly the vectors point.

```text
Similar direction
→ Higher similarity
```

### Euclidean distance

Measures the **straight-line distance** between two vectors.

```text
Closer vectors
→ Smaller distance
```

Simple analogy:

```text
Cosine     = Are they pointing in a similar direction?
Euclidean  = How physically close are they?
```

**Interview answer:**

> "Cosine similarity compares the direction of vectors, while Euclidean distance measures the straight-line distance between them. The appropriate choice depends on how the embedding model and vector database are designed."

---

## 11. What is metadata filtering?

Metadata filtering means applying conditions while retrieving documents.

Suppose documents contain:

```text
Document
Department
Year
Country
TenantId
```

User belongs to:

```text
Department = HR
TenantId = ABC
```

We can search using:

```text
Semantic Search
+
TenantId = ABC
+
Department = HR
```

This improves:

* Relevance
* Security
* Multi-tenant data isolation

**Interview answer:**

> "Metadata filtering combines search with conditions such as department, date, document type or tenant. It improves relevance and is also important for security and tenant isolation."

---

## 12. What is hybrid search?

Hybrid search combines:

1. **Keyword search**
2. **Vector/semantic search**

Example:

```text
User Query
    ↓
 ┌───────────────┐
 │ Keyword Search│
 └───────────────┘
         +
 ┌───────────────┐
 │ Vector Search │
 └───────────────┘
         ↓
Combined Results
```

This is useful because some queries need exact keyword matching, while others need semantic understanding.

**Interview answer:**

> "Hybrid search combines traditional keyword search with vector search. This gives us both exact keyword matching and semantic matching, which can improve retrieval quality."

---

## 13. Dense retrieval vs sparse retrieval?

### Dense retrieval

Uses embeddings.

```text
Question
   ↓
Embedding
   ↓
Vector similarity search
```

It understands semantic meaning.

Example:

```text
"annual leave"
≈
"vacation entitlement"
```

---

### Sparse retrieval

Usually represents documents using keyword-based approaches such as **BM25**.

It focuses on important matching terms.

```text
Question
   ↓
Keyword matching
   ↓
Relevant documents
```

### Simple difference

```text
Dense Retrieval  = Meaning
Sparse Retrieval = Keywords
```

Hybrid search combines both.

---

## 14. What is reranking?

Initial search may retrieve 20 results.

But they are not necessarily ordered perfectly.

Reranking means using another model or scoring process to reorder them.

```text
Search
  ↓
Top 20 Results
  ↓
Reranker
  ↓
Best Top 5 Results
  ↓
LLM
```

**Interview answer:**

> "Reranking is a second step after retrieval where we rescore the retrieved results and place the most relevant chunks at the top before sending them to the LLM."

---

## 15. What is query expansion?

Query expansion means adding related words or concepts to improve retrieval.

Example:

```text
Original:
"leave carry forward"
```

Expanded:

```text
"leave carry forward OR annual leave rollover OR unused vacation days"
```

This can help find documents that use different terminology.

**Interview answer:**

> "Query expansion improves retrieval by adding related terms, synonyms or alternative expressions to the original user query."

---

## 16. What is multi-query retrieval?

Instead of searching using only one version of the question, we generate multiple versions.

Example:

```text
Original:
"How many leaves can I carry forward?"
```

Generate:

```text
1. What is the leave carry-forward policy?
2. How many unused annual leave days can be rolled over?
3. What is the maximum vacation rollover?
```

Then search using all of them.

```text
Multiple Queries
      ↓
Retrieve Results
      ↓
Combine Results
      ↓
Select Best Context
```

**Interview answer:**

> "Multi-query retrieval generates multiple versions of the user's question and retrieves results for each one. This can improve recall when documents use different terminology."

---

## 17. What is parent-child retrieval?

A document is stored at two levels.

```text
Parent Chunk
Large section
     ↓
Child Chunks
Small searchable pieces
```

### Example

```text
Parent:
Leave Policy - Full Section

Children:
Carry Forward
Eligibility
Annual Leave
Exceptions
```

We search using small child chunks because they are more precise.

After finding a relevant child, we can retrieve the larger parent context.

```text
Search Child
    ↓
Relevant Child Found
    ↓
Return Parent Context
```

**Interview answer:**

> "Parent-child retrieval uses small chunks for precise searching and larger parent sections for providing complete context to the LLM."

---

## 18. What is contextual retrieval?

Contextual retrieval means adding useful context to chunks before or during indexing so they can be understood even when retrieved independently.

For example, this chunk alone:

```text
"Employees can carry forward up to 10 days."
```

doesn't tell us:

**10 days of what? Which policy?**

We can add context:

```text
Document: Employee Leave Policy
Section: Annual Leave Carry Forward

Employees can carry forward up to 10 days.
```

Now the chunk is easier to retrieve and understand.

**Interview answer:**

> "Contextual retrieval improves retrieval by adding document or section-level context to individual chunks, making each chunk more meaningful when it is searched independently."

---

## 19. What is RAG fusion?

RAG fusion generally means **combining results from multiple retrieval approaches or multiple query variations** to produce better final context.

For example:

```text
Original Query ───→ Results A
Expanded Query ───→ Results B
Multi-Query  ─────→ Results C
                       ↓
                   Fusion
                       ↓
                 Best Results
```

The results can be combined and then ranked.

**Interview answer:**

> "RAG fusion combines results from multiple searches or retrieval strategies so that we get a stronger set of relevant documents instead of relying on only one query or retrieval method."

---

## 20. What is Reciprocal Rank Fusion (RRF)?

RRF is a technique used to combine multiple ranked result lists.

Suppose:

```text
Search 1:
A → Rank 1
B → Rank 2

Search 2:
B → Rank 1
A → Rank 2
```

RRF combines the rankings.

A document that appears high in multiple result lists gets a better combined score.

Simple formula:

```text
Score(document) =
Σ 1 / (k + rank)
```

You don't normally need to calculate this manually in an interview.

**Interview answer:**

> "Reciprocal Rank Fusion is a method for combining multiple ranked search result lists. Documents that consistently rank highly across different searches receive a stronger final ranking."

---

## 21. How do you evaluate RAG?

I would evaluate **both retrieval and final answer quality**.

### Retrieval evaluation

Ask:

> Did we retrieve the correct information?

Metrics can include:

* Precision
* Recall
* Relevance

### Generation evaluation

Ask:

> Did the final answer correctly use the retrieved context?

Metrics can include:

* Correctness
* Groundedness
* Relevance
* Completeness
* Hallucination rate

I would create a test dataset:

```text
Question
Expected Source Documents
Expected Answer
```

Then test changes consistently.

**Interview answer:**

> "I evaluate RAG in two layers: retrieval quality and answer quality. I use representative questions with expected documents and answers, then measure metrics such as precision, recall, correctness, relevance and groundedness."

---

## 22. What are retrieval precision and recall?

### Precision

Of the results we retrieved, how many were actually relevant?

Example:

```text
Retrieved = 10 chunks
Relevant = 8 chunks
```

Precision:

```text
8 / 10 = 80%
```

### Recall

Of all relevant chunks that existed, how many did we successfully retrieve?

Example:

```text
Total relevant chunks available = 10
Retrieved relevant chunks = 8
```

Recall:

```text
8 / 10 = 80%
```

Simple difference:

```text
Precision = Are retrieved results good?

Recall = Did we find all important results?
```

---

## 23. What is grounding?

Grounding means making the LLM generate its answer based on **trusted provided information**.

For RAG:

```text
Company Documents
       ↓
Relevant Chunks
       ↓
LLM Context
       ↓
Grounded Answer
```

Instead of allowing the model to rely only on its learned knowledge, we provide evidence.

Example instruction:

> "Answer only using the provided context. If the answer is not available, say that you don't have enough information."

**Interview answer:**

> "Grounding means connecting the LLM response to trusted evidence or context. In RAG, we ground the answer using retrieved document chunks to reduce unsupported answers."

---

## 24. How do you detect hallucination in RAG?

I would check whether the answer is **supported by the retrieved context**.

### Example

Retrieved document says:

```text
Employees can carry forward up to 10 days.
```

LLM says:

```text
Employees can carry forward 15 days.
```

This is clearly unsupported.

### Approaches

#### 1. Source citations

Return:

```text
Answer
+
Source Document
+
Page/Section
```

#### 2. Groundedness evaluation

Check:

> Can every important claim in the answer be supported by retrieved context?

#### 3. LLM-as-a-judge

Another model can evaluate whether the answer is supported by the provided context.

#### 4. Automated evaluation

Create test questions with known answers and expected sources.

#### 5. Refuse when evidence is missing

If retrieval confidence is too low:

```text
No good context
      ↓
"I don't know based on the available documents."
```

**Interview answer:**

> "In RAG, I detect hallucination by checking whether the generated claims are supported by the retrieved context. I use citations, groundedness evaluation and automated test datasets. If sufficient evidence is not retrieved, the application should avoid answering rather than allow the model to guess."

---

# ⭐ Quick Revision Table

| Topic                       | Simple Answer                                                  |
| --------------------------- | -------------------------------------------------------------- |
| **RAG**                     | Retrieve relevant data and give it to the LLM                  |
| **Ingestion**               | Prepare documents for search                                   |
| **Chunking**                | Split documents into smaller pieces                            |
| **Chunk overlap**           | Repeat some text between chunks to preserve context            |
| **Embeddings**              | Numerical representation of meaning                            |
| **Vector similarity**       | Measures how semantically similar vectors are                  |
| **Cosine similarity**       | Compares vector direction                                      |
| **Euclidean distance**      | Measures vector distance                                       |
| **Metadata filtering**      | Filter by properties such as TenantId or Department            |
| **Hybrid search**           | Keyword search + vector search                                 |
| **Dense retrieval**         | Semantic/vector-based retrieval                                |
| **Sparse retrieval**        | Keyword-based retrieval                                        |
| **Reranking**               | Reorder retrieved results using a better relevance score       |
| **Query expansion**         | Add related words/phrases to improve search                    |
| **Multi-query retrieval**   | Search using multiple versions of the same question            |
| **Parent-child retrieval**  | Search small chunks, return larger context                     |
| **Contextual retrieval**    | Add document/section context to chunks                         |
| **RAG fusion**              | Combine multiple retrieval result sets                         |
| **RRF**                     | Combine rankings from multiple searches                        |
| **Precision**               | How many retrieved results are relevant                        |
| **Recall**                  | How many relevant results we successfully found                |
| **Grounding**               | Base answers on trusted evidence                               |
| **Hallucination detection** | Check whether answer claims are supported by retrieved context |

# ⭐ Complete RAG Flow to Remember

```text
                 INGESTION

Documents
   ↓
Extract Text
   ↓
Clean Text
   ↓
Chunking + Overlap
   ↓
Add Metadata
   ↓
Create Embeddings
   ↓
Vector/Search Index


                 QUERY TIME

User Question
   ↓
Query Expansion / Multi-Query (Optional)
   ↓
Create Query Embedding
   ↓
Vector Search / Hybrid Search
   ↓
Metadata Filtering
   ↓
Reranking / Fusion
   ↓
Best Relevant Chunks
   ↓
LLM Prompt
   ↓
Grounded Answer + Citations
```

### Best one-line definition to remember:

> **"RAG is not about training the LLM with company documents. It retrieves relevant information at runtime and provides it as context to the LLM."**

This is one of the most important points to explain correctly in a **RAG/GenAI interview**.
