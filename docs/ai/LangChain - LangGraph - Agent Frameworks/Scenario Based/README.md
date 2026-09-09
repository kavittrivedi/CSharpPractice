## Scenario

> A LangGraph workflow has 8 nodes. Occasionally, Node 6 receives incorrect state. How would you debug it?

### Simple approach

I would debug it **from Node 6 backward**, because the problem may not actually be in Node 6. One of the earlier nodes may have changed or overwritten the state incorrectly.

```text
Node 1
  ↓
Node 2
  ↓
Node 3
  ↓
Node 4
  ↓
Node 5
  ↓
Node 6  ← Incorrect state
  ↓
Node 7
  ↓
Node 8
```

### Step 1 — Add a correlation/workflow ID

Every workflow execution should have a unique ID.

```text
WorkflowId = WF-12345
```

This allows me to follow one execution across all 8 nodes.

---

### Step 2 — Log state at every node

I would capture the important state **before and after each node**.

```text
Node 4
Before: CustomerId=100, Status=Pending
After:  CustomerId=100, Status=Approved

Node 5
Before: CustomerId=100, Status=Approved
After:  CustomerId=100, Status=Approved

Node 6
Received: CustomerId=100, Status=Pending   ← Problem
```

Now I know that the state became incorrect somewhere between Node 5 and Node 6.

**Don't necessarily log sensitive data.** Log only the fields needed for debugging.

---

### Step 3 — Check state mutation

I would check whether any node is accidentally:

* Overwriting a field
* Removing a field
* Returning an incomplete state
* Using stale data
* Modifying shared/mutable objects
* Merging state incorrectly

For example:

```text
Node 5:
Status = Approved

Node 6:
Status = Pending
```

I would investigate how `Status` changed.

---

### Step 4 — Check conditional routing

Because the issue happens **occasionally**, I would check whether some executions take a different path.

For example:

```text
Node 4
   ↓
Condition
  / \
 A   B
 |   |
 ↓   ↓
Node 5 / Node X
       ↓
      Node 6
```

Maybe Node 6 receives different state depending on which path was taken.

So I would compare:

```text
Successful execution
vs
Failed execution
```

---

### Step 5 — Check parallel execution

If nodes can execute in parallel, I would investigate race conditions or incorrect state merging.

For example:

```text
        Node 4
       /      \
   Node 5    Node 5B
       \      /
        Merge
          ↓
        Node 6
```

One branch may overwrite data produced by another branch.

---

### Step 6 — Check persistence/recovery

If the workflow is persisted, I would verify whether Node 6 is loading:

* The latest state
* The correct workflow instance
* The correct checkpoint/version

An old checkpoint could result in stale state.

---

### Step 7 — Reproduce and compare

I would take one failed `WorkflowId` and compare its execution with a successful execution.

```text
Failed Workflow
Node 1 → State A
Node 2 → State B
Node 3 → State C
...
Node 6 → WRONG

Successful Workflow
Node 1 → State A
Node 2 → State B
Node 3 → State C
...
Node 6 → CORRECT
```

The first point where the states differ is usually the best place to investigate.

---

## ⭐ Interview answer

> **"I would first assign a unique correlation or workflow ID to every execution and trace that execution through all 8 nodes. I would capture the relevant state before and after each node, without logging sensitive information unnecessarily.**
>
> **Since Node 6 receives incorrect state, I would work backward and find the first node where the state becomes incorrect. I would also check conditional routing, state merging, parallel execution and any state mutation or overwriting.**
>
> **Because the problem occurs occasionally, I would compare a failed execution with a successful execution and check whether they followed different paths or used different checkpoints. If the workflow is persisted, I would also verify that Node 6 is reading the latest checkpoint and the correct workflow instance.**
>
> **Once I identify the first incorrect state transition, I would fix that node or state-management logic and add a regression test for that scenario."**

### Easy formula to remember

**Trace → Log → Compare → Find first wrong state → Check routing/parallelism → Fix → Regression test**
