### What is Idempotency? How we have handle in .net core api?

**Idempotency** means:

> **If the same API request is sent multiple times, the result should be the same as if it was sent only once.**

It is especially important for **POST operations** such as payments, orders, or creating resources, where a client might retry because of a network timeout.

### Simple example

Suppose we have:

```http
POST /api/orders
Idempotency-Key: 12345
```

Request:

```json
{
  "productId": 10,
  "quantity": 2
}
```

The client sends it once, but doesn't receive the response because of a network problem. It retries with the **same Idempotency-Key**:

```http
POST /api/orders
Idempotency-Key: 12345
```

Without idempotency:

```text
Request 1 → Order #1001 created
Request 2 → Order #1002 created  ❌
```

With idempotency:

```text
Request 1 → Order #1001 created
Request 2 → Existing result for #1001 returned ✅
```

So we don't create the order twice.

---

## How do we handle Idempotency in .NET Core API?

ASP.NET Core does **not automatically make POST APIs idempotent**. We normally implement an idempotency mechanism.

A common approach is:

```text
Client
   ↓
Idempotency-Key
   ↓
.NET Core API
   ↓
Check Idempotency Store
   ↓
 ┌───────────────┐
 │ Key exists?   │
 └───────┬───────┘
      Yes│   │No
         ↓   ↓
 Return    Process request
 existing      ↓
 response   Save result
```

### 1. Client sends an Idempotency-Key

For example:

```http
POST /api/orders
Idempotency-Key: 8f7c-1234-abc
```

The key should uniquely identify **one intended operation**.

---

### 2. API checks a persistent store

You can store the key and result in a database.

For example:

```text
IdempotencyRecords

IdempotencyKey | Status      | Response
------------------------------------------------
8f7c-1234-abc  | Completed   | { OrderId: 1001 }
```

The important point is that this should generally be a **shared/persistent store**, not just an in-memory dictionary, especially when the API has multiple instances.

---

### 3. If the key already exists

Return the previously stored result instead of executing the operation again.

```csharp
var existing = await repository.GetAsync(idempotencyKey);

if (existing != null)
{
    return existing.Response;
}
```

---

### 4. If the key doesn't exist

Process the request:

```csharp
var order = await orderService.CreateOrderAsync(request);
```

Then store the result:

```csharp
await repository.SaveAsync(
    idempotencyKey,
    order.Id,
    order);
```

---

## Important interview point: Race condition

There is a subtle problem with simply doing:

```text
Check key
   ↓
If not found
   ↓
Create order
   ↓
Save key
```

Two identical requests can arrive **at exactly the same time**:

```text
Request A → Check → Key doesn't exist
Request B → Check → Key doesn't exist

Request A → Create Order ❌
Request B → Create Order ❌
```

Therefore, a production implementation needs **atomicity/concurrency control**.

For example, put a **unique constraint** on `IdempotencyKey`:

```sql
CREATE UNIQUE INDEX UX_Idempotency_Key
ON IdempotencyRecords(IdempotencyKey);
```

And combine this with appropriate transaction/concurrency handling.

---

# What about GET, PUT and DELETE?

This is an important distinction.

### GET

GET is normally idempotent:

```http
GET /api/orders/100
```

Calling it multiple times doesn't normally change the resource.

### PUT

PUT is designed to be idempotent.

```http
PUT /api/users/10
```

```json
{
    "name": "John"
}
```

Calling the same PUT repeatedly should leave the resource in the same state.

### DELETE

DELETE is generally idempotent from the resource-state perspective.

```http
DELETE /api/users/10
```

After the first successful deletion, subsequent calls should not recreate or otherwise change the resource.

### POST

POST is **not inherently idempotent**.

```http
POST /api/orders
```

Repeated requests can create multiple orders.

That's where an **Idempotency-Key** is particularly useful.

---

# Interview answer

If the interviewer asks **"What is idempotency and how do you implement it in .NET Core?"**, I would answer:

> **"Idempotency means that executing the same request multiple times produces the same intended result as executing it once. It is particularly important for POST APIs such as payment or order creation, where a client may retry a request because of a network timeout.**
>
> **In a .NET Core API, I can implement it using an Idempotency-Key supplied by the client. I store that key along with the request status and response in a persistent shared store such as SQL Server or Redis. When a request arrives, I check whether the key has already been processed. If it has, I return the previously stored result instead of executing the business operation again. If it hasn't, I process the request and store the result.**
>
> **I also need to handle concurrent requests, typically using a unique constraint or atomic operation on the idempotency key, so that two simultaneous requests cannot both execute the operation."**

### One sentence to remember

**Idempotency = "Retry the same request safely without performing the business operation twice."**
