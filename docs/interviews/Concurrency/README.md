A **concurrency issue** occurs when **multiple operations access or modify the same shared resource at the same time**, and the final result depends on the timing/order in which those operations execute.

### Simple example

Suppose a bank account has **₹10,000**.

Two requests execute at almost the same time:

* Request A wants to withdraw ₹7,000
* Request B wants to withdraw ₹5,000

Both requests read the balance as **₹10,000** before either update is committed.

```text
Request A → Read balance = ₹10,000
Request B → Read balance = ₹10,000

Request A → Update balance = ₹3,000
Request B → Update balance = ₹5,000
```

The database might finally show **₹5,000**, even though ₹12,000 was withdrawn.

That's a **concurrency problem**.

### In simple words

> **Concurrency issue = multiple users/requests working with the same data simultaneously, causing incorrect or inconsistent results.**

### Common examples in .NET/API applications

1. **Two users update the same record simultaneously**
2. **Two requests try to purchase the last available product**
3. **Two processes update the same inventory quantity**
4. **Multiple requests modify a shared in-memory object**
5. **Two transactions read and update the same database row**

### How do we handle it?

Common approaches include:

* **Optimistic concurrency** — e.g., `RowVersion`/timestamp in SQL Server and EF Core
* **Pessimistic concurrency** — locking database rows/resources
* **Transactions**
* **Atomic database operations**
* **Synchronization mechanisms** such as `lock` for appropriate in-memory scenarios

For an **ASP.NET Core + EF Core interview**, the most important concept to know is **optimistic concurrency using a `RowVersion` column**.

This is a very common **EF Core interview topic**. Let's understand it with a simple real-world example.

## 1. What problem does `RowVersion` solve?

Imagine we have an `Employee` record:

| Id | Name |  Salary |
| -- | ---- | ------: |
| 1  | John | ₹50,000 |

Two users open the same employee record.

```text
User A → reads Salary = ₹50,000
User B → reads Salary = ₹50,000
```

Now:

```text
User A → changes Salary to ₹60,000
User B → changes Salary to ₹70,000
```

If we don't have concurrency protection:

```text
User A → UPDATE Employee SET Salary = 60000 WHERE Id = 1
User B → UPDATE Employee SET Salary = 70000 WHERE Id = 1
```

User B's update can overwrite User A's update.

This is called a **lost update**.

---

# 2. What is optimistic concurrency?

The idea is:

> **"I assume conflicts are rare, so I won't lock the record while someone is reading it. But when I update it, I'll check whether somebody changed it since I read it."**

That's why it is called **optimistic** concurrency.

EF Core can do this using a **concurrency token**.

In SQL Server, a common way is a `rowversion` column.

---

# 3. What is `RowVersion`?

Suppose our table looks like this:

```text
Employee
--------------------------------
Id
Name
Salary
RowVersion
```

`RowVersion` is a SQL Server-generated binary value that changes whenever the row is modified.

For example:

```text
Id    Name    Salary    RowVersion
1     John    50000     0x00000000000007D1
```

User A and User B both read the employee.

Both receive the same `RowVersion`:

```text
User A → Salary = 50000
          RowVersion = X

User B → Salary = 50000
          RowVersion = X
```

---

# 4. User A updates the record

User A changes salary:

```text
50000 → 60000
```

EF Core generates an update conceptually similar to:

```sql
UPDATE Employees
SET Salary = 60000
WHERE Id = 1
  AND RowVersion = X;
```

The update succeeds.

SQL Server automatically changes the `RowVersion`:

```text
RowVersion = Y
```

Now:

```text
Id    Name    Salary    RowVersion
1     John    60000     Y
```

---

# 5. User B tries to update

User B still has the old value:

```text
Salary = 50000
RowVersion = X
```

User B changes salary to:

```text
70000
```

EF Core conceptually sends:

```sql
UPDATE Employees
SET Salary = 70000
WHERE Id = 1
  AND RowVersion = X;
```

But the database currently has:

```text
RowVersion = Y
```

not `X`.

Therefore:

```text
Rows affected = 0
```

EF Core detects that the concurrency check failed and throws:

```text
DbUpdateConcurrencyException
```

So User B's update **does not silently overwrite User A's change**.

---

# 6. How do we implement it in EF Core?

### Entity

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Salary { get; set; }

    public byte[] RowVersion { get; set; }
}
```

Configure it as a concurrency token:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>()
        .Property(e => e.RowVersion)
        .IsRowVersion();
}
```

`IsRowVersion()` tells EF Core that this property is a SQL Server rowversion and should be used for concurrency checking.

---

# 7. What happens internally?

This is the important interview point.

When EF Core loads:

```text
Employee Id = 1
Salary = 50000
RowVersion = X
```

EF Core keeps track of the original `RowVersion`.

When you call:

```csharp
context.SaveChanges();
```

EF Core uses the original concurrency value in the `WHERE` clause.

Conceptually:

```sql
UPDATE Employees
SET Salary = @NewSalary
WHERE Id = @Id
AND RowVersion = @OriginalRowVersion;
```

If another user has already modified the row, the `RowVersion` has changed.

Therefore:

```text
0 rows updated
        ↓
EF Core detects concurrency conflict
        ↓
DbUpdateConcurrencyException
```

---

# 8. How do we handle the exception?

Typically:

```csharp
try
{
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    // Handle concurrency conflict
}
```

You might tell the user:

> "This employee record was modified by another user. Please reload the latest data and try again."

Or you can implement a **retry/merge strategy**, depending on the business requirement.

---

# 9. Why is it called "optimistic"?

Because we're **not locking the record when users read it**.

We allow:

```text
User A → Read
User B → Read
User A → Update
User B → Update
```

But we detect the conflict when User B tries to save.

Compare that with pessimistic concurrency:

```text
User A → Locks record
User B → Must wait
User A → Updates
User A → Releases lock
User B → Can continue
```

So:

| Optimistic                                     | Pessimistic                             |
| ---------------------------------------------- | --------------------------------------- |
| Doesn't normally lock during read              | Uses locking                            |
| Assumes conflicts are uncommon                 | Assumes conflicts may happen            |
| Detects conflict during update                 | Prevents/controls concurrent access     |
| `RowVersion` is common with EF Core/SQL Server | Database locking/transaction mechanisms |

---

## Interview answer

If the interviewer asks:

**"How do you handle concurrency in EF Core?"**

A good answer is:

> **"I can use optimistic concurrency in EF Core with a SQL Server `rowversion` column. EF Core includes the original rowversion value in the update condition. If another user has modified the record, the rowversion changes, the update affects zero rows, and EF Core throws `DbUpdateConcurrencyException`. I can then handle the conflict by reloading the latest data, informing the user, or applying an appropriate retry or merge strategy."**

### One important distinction

Don't say **"`RowVersion` prevents two users from accessing the record simultaneously."**

That's not what it does.

It primarily **detects that the row changed between read and update**, allowing the application to handle the conflict rather than silently losing an update.

But if 2 user tries to update as same time For eg. 01:00:00:000, then?

Yes — this is the important part. **Even if both users click Update at exactly `01:00:00:000`, `RowVersion` still works.**

The key point is: **`RowVersion` does not depend on the clock/time.** SQL Server generates the rowversion value and the database determines which update reaches the row first.

### Example

Initial database:

```text
EmployeeId   Salary    RowVersion
1            50,000    X
```

Both users read the record:

```text
User A → Salary = 50,000, RowVersion = X
User B → Salary = 50,000, RowVersion = X
```

Now suppose both send the update at exactly:

```text
01:00:00:000
```

Let's say **User A's UPDATE reaches SQL Server first**.

Conceptually:

```sql
UPDATE Employee
SET Salary = 60,000
WHERE Id = 1
AND RowVersion = X;
```

✅ It succeeds.

SQL Server changes the rowversion:

```text
Salary = 60,000
RowVersion = Y
```

Then User B's request is processed:

```sql
UPDATE Employee
SET Salary = 70,000
WHERE Id = 1
AND RowVersion = X;
```

But the database now has:

```text
RowVersion = Y
```

not `X`.

Therefore:

```text
0 rows affected
       ↓
EF Core detects concurrency conflict
       ↓
DbUpdateConcurrencyException
```

### What if User B reaches first?

Then the opposite happens:

```text
User B → succeeds → RowVersion changes X → Y
User A → fails because it still has X
```

So **one succeeds and the other gets a concurrency exception**.

### Very important interview point

Don't think of it as:

> "User A updated at 01:00:00:000 and User B also updated at 01:00:00:000, so how does EF know?"

EF doesn't compare timestamps like `01:00:00:000`.

It compares the **original rowversion value**.

```text
                    Database
                       │
                       ▼
             RowVersion = X
                       │
          ┌────────────┴────────────┐
          │                         │
       User A                    User B
       has X                     has X
          │                         │
          └────── Both UPDATE ──────┘
                       │
                First one wins
                       │
                X → Y
                       │
              Second one checks X
                       │
                 X ≠ Y
                       │
          DbUpdateConcurrencyException
```

**So even at exactly the same time, the database serializes the actual row updates enough that one update is applied first; the second update's concurrency check fails.**

That's the main reason `RowVersion` is useful for handling **lost-update concurrency** in EF Core.

Explain **Pessimistic concurrency** — locking database rows/resources  

Sure. Think of **pessimistic concurrency** as:

> **"I don't want another transaction to modify this data while I am working with it, so I'll lock it."**

This is different from optimistic concurrency with `RowVersion`.

---

## 1. Simple example

Suppose we have:

```text
Product
----------------
Id       = 1
Name     = Laptop
Stock    = 1
```

Two users try to buy the **last laptop** at almost the same time.

### Without locking

Both requests could do:

```text
User A → Read Stock = 1
User B → Read Stock = 1

User A → Buy laptop
User B → Buy laptop
```

Now you potentially have a problem: **both users believed the stock was available.**

---

# 2. With pessimistic concurrency

User A starts a transaction and locks the product row.

```text
User A
   ↓
BEGIN TRANSACTION
   ↓
Lock Product row
   ↓
Read Stock = 1
   ↓
Update Stock = 0
   ↓
COMMIT
   ↓
Release lock
```

While User A has the lock:

```text
User B
   ↓
tries to access the same locked row
   ↓
WAIT
```

After User A commits:

```text
User A → releases lock
                 ↓
User B → gets access
                 ↓
         reads Stock = 0
                 ↓
         Cannot purchase
```

So User B doesn't work with stale stock.

---

# 3. How does SQL Server do this?

SQL Server supports locking through transactions and different lock modes/isolation levels.

For example, conceptually:

```sql
BEGIN TRANSACTION;

SELECT *
FROM Products WITH (UPDLOCK)
WHERE Id = 1;

-- Check stock
-- Update stock

UPDATE Products
SET Stock = Stock - 1
WHERE Id = 1;

COMMIT;
```

`UPDLOCK` tells SQL Server to take an **update lock** when reading the row, which helps prevent another transaction from simultaneously taking an incompatible lock and modifying the same row before the first transaction finishes.

The exact locking behavior also depends on the transaction's isolation level and database configuration.

---

# 4. Pessimistic vs Optimistic

This is the easiest way to remember it:

| Optimistic                    | Pessimistic                        |
| ----------------------------- | ---------------------------------- |
| Don't lock while reading      | Lock the resource                  |
| Assume conflicts are uncommon | Assume conflicts may happen        |
| Detect conflict later         | Prevent/control conflicting access |
| `RowVersion` commonly used    | Database locks/transactions        |
| Conflict → exception          | Other transaction may wait/block   |

### Optimistic

```text
User A → Read X
User B → Read X

User A → Update → Success
User B → Update → Conflict ❌
```

### Pessimistic

```text
User A → Lock X
User B → Wait ⏳

User A → Update → Commit
User A → Unlock

User B → Continue
```

---

# 5. Important: Lock ≠ always "block everything"

This is a common interview misconception.

When we say **pessimistic concurrency**, we don't mean:

> "Nobody can access the entire database."

The lock applies to particular database resources, such as:

* Row
* Key
* Page
* Table
* etc.

And whether another operation waits, is allowed, or is incompatible depends on the **type of lock and transaction isolation level**.

---

# 6. What happens if User A never commits?

This is one disadvantage of pessimistic concurrency.

Suppose:

```text
User A
  ↓
BEGIN TRANSACTION
  ↓
Locks row
  ↓
Something goes wrong
  ↓
Transaction remains open
```

User B may remain blocked waiting for the lock.

If transactions hold locks for too long, you can get:

* Blocking
* Poor performance
* Timeouts
* Potential deadlocks

Therefore, transactions using locks should generally be kept **short**.

---

# 7. EF Core perspective

This is where interviewers sometimes try to confuse candidates.

EF Core doesn't have a simple equivalent of:

```csharp
[RowVersion]
```

for pessimistic concurrency.

Instead, pessimistic locking is generally achieved through **database transaction/locking mechanisms**, often using SQL/database-specific features.

For SQL Server, you may use a transaction and SQL locking hints such as:

```sql
UPDLOCK
```

or other appropriate locking/isolation mechanisms depending on the scenario.

---

## Interview answer

If the interviewer asks:

**"What is pessimistic concurrency?"**

You can say:

> **"Pessimistic concurrency assumes that concurrent conflicts are likely, so we use database locks to control access to shared data. For example, when processing the last available product, I can start a transaction and acquire an appropriate lock on the product row while checking and updating its stock. Another transaction attempting to modify the same resource may have to wait until the first transaction commits or rolls back. The advantage is that conflicting updates can be controlled before they happen, but excessive or long-running locks can cause blocking, timeouts, and deadlocks."**

### One-line difference to remember

> **Optimistic concurrency: "Update it, but check whether someone changed it."**
> **Pessimistic concurrency: "Lock it so someone else can't change it while I'm working on it."**

---

 **Transactions** are another important way to handle concurrency, but there is an important distinction:

> A **transaction is not itself a concurrency-control mechanism**.
> A transaction gives you **atomicity and consistency**, and depending on its **isolation level and locking behavior**, it can also control concurrency.

Let's understand it with an example.

## 1. What is a transaction?

Suppose we transfer ₹1,000 from Account A to Account B.

There are two operations:

```text
Account A → -₹1,000
Account B → +₹1,000
```

These two operations should be treated as **one unit of work**.

Either:

```text
A → -₹1,000
B → +₹1,000
```

both succeed,

**OR**

```text
A → -₹1,000
B → +₹1,000
```

both are rolled back.

We don't want this:

```text
Account A → -₹1,000 ✅
Account B → FAILED ❌
```

because ₹1,000 has effectively disappeared.

---

# 2. Transaction gives us Atomicity

A transaction provides the **all-or-nothing** behavior.

For example:

```sql
BEGIN TRANSACTION;

UPDATE Accounts
SET Balance = Balance - 1000
WHERE Id = 1;

UPDATE Accounts
SET Balance = Balance + 1000
WHERE Id = 2;

COMMIT;
```

If something goes wrong:

```sql
ROLLBACK;
```

So:

```text
BEGIN
   ↓
Operation 1
   ↓
Operation 2
   ↓
COMMIT
```

If Operation 2 fails:

```text
BEGIN
   ↓
Operation 1
   ↓
Operation 2 ❌
   ↓
ROLLBACK
```

The first operation is undone.

---

# 3. How does this relate to concurrency?

This is where it becomes interesting.

Suppose two users try to purchase the last product.

```text
Stock = 1
```

Both requests need to:

1. Check stock
2. Decrease stock
3. Create order

These operations should be handled carefully.

A transaction can group these operations:

```text
BEGIN TRANSACTION

    Check stock
        ↓
    Decrease stock
        ↓
    Create order

COMMIT
```

If something fails:

```text
ROLLBACK
```

But **just putting these operations inside a transaction does not automatically mean that another transaction cannot read/update the same row**.

The actual concurrency behavior depends on the **isolation level and locking**.

That's an important interview point.

---

# 4. Transaction + Isolation Level

SQL Server provides isolation levels such as:

```text
READ UNCOMMITTED
READ COMMITTED
REPEATABLE READ
SNAPSHOT
SERIALIZABLE
```

They determine how one transaction interacts with changes made by other transactions.

For example, **SERIALIZABLE** provides the strongest traditional isolation and can prevent certain concurrent operations by using locks/range protection, but it can also increase blocking and reduce concurrency.

So you can think of it as:

```text
Transaction
    +
Isolation Level
    ↓
Defines how concurrent transactions interact
```

---

# 5. Example with two users

Suppose:

```text
ProductId = 1
Stock = 1
```

User A starts a transaction:

```text
BEGIN TRANSACTION

Read Product
Stock = 1

Update Stock = 0

COMMIT
```

At the same time, User B starts another transaction.

Depending on the **isolation level and SQL statements used**, User B may:

* see the old/current data,
* wait for User A,
* or encounter a concurrency-related failure.

So the transaction itself doesn't answer the question:

> "What should another transaction be allowed to see or modify?"

The **isolation level/locking strategy** answers that.

---

# 6. Transaction in EF Core

In EF Core, a simple transaction can look like:

```csharp
await using var transaction =
    await context.Database.BeginTransactionAsync();

try
{
    // Operation 1
    accountA.Balance -= 1000;

    // Operation 2
    accountB.Balance += 1000;

    await context.SaveChangesAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

The important flow is:

```text
BeginTransaction
       ↓
Perform operations
       ↓
SaveChanges
       ↓
Commit
```

If an exception occurs:

```text
Exception
    ↓
Rollback
```

---

# 7. `SaveChanges()` vs Transaction

This is a **very common interview question**.

EF Core's `SaveChanges()` already uses a transaction for the database operations it sends as part of that `SaveChanges` call, when the database provider supports transactions.

So you don't necessarily need to manually create a transaction for every `SaveChanges()`.

For example:

```csharp
context.Accounts.Update(accountA);
context.Accounts.Update(accountB);

await context.SaveChangesAsync();
```

The changes sent by that `SaveChangesAsync()` are normally committed atomically.

But suppose you need multiple `SaveChanges()` calls or need several database operations to belong to **one explicit unit of work**:

```text
SaveChanges()
       ↓
Other database operation
       ↓
SaveChanges()
       ↓
Commit
```

Then an explicit transaction can be appropriate.

---

# 8. Very important distinction

Don't say this in an interview:

> ❌ "Transactions solve concurrency."

A better answer is:

> **"Transactions provide atomicity and consistency for a group of database operations. They also participate in concurrency control through their isolation level and the locks or row-versioning mechanisms used by the database."**

That's much more accurate.

---

## Transaction vs Optimistic vs Pessimistic

| Concept                     | Main purpose                                 |
| --------------------------- | -------------------------------------------- |
| **Transaction**             | Group operations into an atomic unit         |
| **Optimistic concurrency**  | Detect conflicting changes                   |
| **Pessimistic concurrency** | Use locks to control concurrent access       |
| **Atomic operation**        | Make an individual operation indivisible     |
| **`lock` in C#**            | Synchronize access to in-memory shared state |

### Easy way to remember

**Transaction:**

> "These operations must succeed or fail together."

**Optimistic concurrency:**

> "I'll let you work, but I'll check whether someone changed the data before you save."

**Pessimistic concurrency:**

> "I'll lock the data while I'm working so conflicting work has to wait."

---

**Atomic database operations** are a very useful way to avoid certain concurrency problems.

The key idea is:

> **Instead of doing "read → calculate → write" in separate steps, let the database perform the change as one indivisible operation.**

---

## 1. The problem with Read → Modify → Write

Suppose we have:

```text
Product
----------------
Id     Stock
1      10
```

Two users want to buy one item.

A naïve implementation might be:

```csharp
var product = await context.Products
    .FirstAsync(x => x.Id == 1);

product.Stock--;

await context.SaveChangesAsync();
```

It looks fine, but there is a concurrency window:

```text
User A → Read Stock = 10
User B → Read Stock = 10

User A → Stock = 9
User B → Stock = 9
```

The expected result after two purchases is:

```text
Stock = 8
```

But both users read `10`, so you can end up with:

```text
Stock = 9
```

One update effectively overwrites the other.

---

# 2. Atomic database operation

Instead, ask the database to perform the calculation directly:

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = 1;
```

There is no separate:

```text
READ
 ↓
Calculate
 ↓
WRITE
```

The database performs the update as **one statement**.

If two requests execute it:

```text
User A → UPDATE Stock = Stock - 1
User B → UPDATE Stock = Stock - 1
```

The database processes the operations safely according to its concurrency/locking rules.

Starting with:

```text
Stock = 10
```

After both operations:

```text
Stock = 8
```

This is the important advantage.

---

# 3. Even better: check the condition atomically

Suppose the stock is:

```text
Stock = 1
```

We don't want stock to become `-1`.

We can do:

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = 1
  AND Stock > 0;
```

Now the **check and update happen together**.

Conceptually:

```text
IF Stock > 0
    Stock = Stock - 1
```

but the check and modification are performed by the database as one SQL operation rather than as separate application-side operations.

Then we check the number of affected rows:

```text
1 row affected → Purchase can continue
0 rows affected → No stock available
```

This is a very powerful pattern.

---

# 4. Example in EF Core

With modern EF Core, you can perform a direct database update:

```csharp
var rowsAffected = await context.Products
    .Where(p => p.Id == productId && p.Stock > 0)
    .ExecuteUpdateAsync(setters =>
        setters.SetProperty(
            p => p.Stock,
            p => p.Stock - 1));
```

Then:

```csharp
if (rowsAffected == 0)
{
    // Product doesn't exist or stock is already 0
}
else
{
    // Stock successfully reserved/decreased
}
```

The important thing is that you **don't first load the product into memory and then modify it**.

EF Core translates the operation into a database-side `UPDATE`.

---

# 5. Another common example: Money

Suppose:

```text
Account Balance = ₹10,000
```

You want to withdraw ₹2,000.

Instead of:

```text
Read balance
      ↓
Check balance >= 2000
      ↓
Balance = balance - 2000
      ↓
Save
```

you can perform the operation atomically:

```sql
UPDATE Accounts
SET Balance = Balance - 2000
WHERE Id = 1
  AND Balance >= 2000;
```

Then:

```text
Rows affected = 1
    ↓
Withdrawal successful
```

or:

```text
Rows affected = 0
    ↓
Insufficient balance / account not found
```

Again, the **condition and modification are part of the same database operation**.

---

# 6. Why is this useful for concurrency?

Consider two users trying to buy the last item.

Initial:

```text
Stock = 1
```

Both execute:

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = 1
AND Stock > 0;
```

The database coordinates the concurrent updates.

One operation succeeds:

```text
Stock: 1 → 0
Rows affected = 1
```

The other operation then evaluates the condition against the current state:

```text
Stock > 0
```

which is false.

Therefore:

```text
Rows affected = 0
```

So you don't have to rely on:

```text
Application reads Stock = 1
```

and then separately decide what to do.

---

# 7. Atomic operation vs Transaction

This distinction is important for interviews.

### Atomic database operation

Usually refers to **one database statement/action that should happen as one indivisible operation**.

Example:

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = 1 AND Stock > 0;
```

### Transaction

Groups **multiple database operations** into one unit of work.

For example:

```text
BEGIN TRANSACTION

1. Decrease product stock
2. Create order
3. Create payment record

COMMIT
```

If step 3 fails:

```text
ROLLBACK
```

So:

> **Atomic operation = make one database operation safe as a unit.**

> **Transaction = make multiple database operations succeed/fail together.**

---

# 8. Atomic operation vs Optimistic concurrency

These are also different.

### Optimistic concurrency

Uses something like `RowVersion`:

```text
Read row
   ↓
Modify in application
   ↓
UPDATE ... WHERE RowVersion = original value
   ↓
Conflict? → DbUpdateConcurrencyException
```

It **detects that someone changed the row**.

### Atomic operation

Instead of reading and then modifying:

```text
Read → Modify → Write
```

you directly tell the database:

```text
UPDATE Stock = Stock - 1
WHERE Stock > 0
```

It **avoids the read-modify-write race for that particular operation**.

---

## Interview answer

If the interviewer asks:

**"What do you mean by atomic database operations?"**

You can answer:

> **"An atomic database operation means performing a state change as a single database operation rather than doing a separate read, calculation, and write in application code. For example, instead of reading product stock and then decrementing it, I can execute `UPDATE Products SET Stock = Stock - 1 WHERE Id = @id AND Stock > 0` and check the affected rows. This is useful in concurrent scenarios because the condition and update are handled together by the database, reducing read-modify-write race conditions."**

### One line to remember

**Don't do:**

```text
Read → Calculate → Write
```

**Prefer when appropriate:**

```text
Database → Atomic UPDATE
```

And remember: **atomicity of a statement doesn't mean that every multi-step business process is automatically concurrency-safe**. For multiple related operations, you may still need a **transaction**, appropriate **isolation**, and/or **optimistic concurrency**.

---

This one is specifically about **concurrency inside your application's memory**, not directly about database rows.

## 1. What is a synchronization mechanism?

Suppose multiple threads are executing code at the same time and they access the **same in-memory object**.

If they modify that shared object simultaneously, you can get incorrect results.

A synchronization mechanism controls **who can access the shared resource at a given time**.

In C#, one common mechanism is:

```csharp
lock
```

---

# 2. Simple example

Suppose we have:

```csharp
int counter = 0;
```

And 100 threads execute:

```csharp
counter++;
```

It looks like one operation, but internally it is roughly:

```text
Read counter
     ↓
Add 1
     ↓
Write counter
```

Two threads can interfere:

```text
Thread A → Read 0
Thread B → Read 0

Thread A → Write 1
Thread B → Write 1
```

Expected:

```text
2
```

Actual:

```text
1
```

This is a **race condition**.

---

# 3. Using `lock`

We can protect the critical section:

```csharp
private readonly object _lock = new();

private int _counter = 0;

public void Increment()
{
    lock (_lock)
    {
        _counter++;
    }
}
```

Now only **one thread at a time** can execute the code inside:

```csharp
lock (_lock)
{
    _counter++;
}
```

Conceptually:

```text
Thread A → gets lock
             ↓
          counter++
             ↓
        releases lock

Thread B → gets lock
             ↓
          counter++
             ↓
        releases lock
```

So the increment isn't allowed to overlap with another increment using the same lock.

---

# 4. What exactly does `lock` do?

When you write:

```csharp
lock (_lock)
{
    // critical section
}
```

you're saying:

> **"Only one thread that uses this same lock object can enter this critical section at a time."**

If another thread tries to enter while the lock is held:

```text
Thread A
   ↓
LOCK
   ↓
Critical Section
   ↓
UNLOCK

Thread B
   ↓
WAIT ⏳
   ↓
LOCK
   ↓
Critical Section
```

---

# 5. Important: `lock` works only within the process

This is **very important for an ASP.NET Core interview**.

Suppose your application has:

```text
                 Load Balancer
                 /           \
                /             \
        API Server 1       API Server 2
             │                  │
             │                  │
          _counter            _counter
```

If you use:

```csharp
lock (_lock)
{
    _counter++;
}
```

on Server 1, it **does not lock the `_counter` on Server 2**.

Why?

Because each server/process has its **own memory** and its **own lock object**.

So:

> **C# `lock` is suitable for synchronizing shared in-memory state within the same process.**

It is **not a distributed locking mechanism**.

---

# 6. Why is this important in ASP.NET Core?

Consider this:

```csharp
public class CounterService
{
    private int _counter;

    public void Increment()
    {
        _counter++;
    }
}
```

If multiple requests execute `Increment()` concurrently and `_counter` is shared by the service instance, you can have a race condition.

You could use:

```csharp
private readonly object _lock = new();

public void Increment()
{
    lock (_lock)
    {
        _counter++;
    }
}
```

But you need to consider the **service lifetime**.

For example, if the service is registered as:

```csharp
services.AddSingleton<CounterService>();
```

there is one instance in the application process, so the lock can protect that shared state within that process.

If it is:

```csharp
services.AddTransient<CounterService>();
```

you don't necessarily have one shared `_counter`/lock across requests, so the synchronization may not solve the intended problem.

---

# 7. `lock` vs database concurrency

This is where your previous topics connect.

### `lock`

Protects:

```text
Application memory
```

Example:

```csharp
lock (_lock)
{
    sharedObject.Value++;
}
```

### `RowVersion`

Protects against conflicting:

```text
Database updates
```

Example:

```text
User A → UPDATE
User B → UPDATE
         ↓
   RowVersion detects conflict
```

### Database locking

Protects database resources:

```text
Transaction A → locks row
Transaction B → waits
```

### Atomic database operation

Lets the database perform an operation such as:

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = @id AND Stock > 0;
```

---

# 8. Don't use `lock` for database concurrency

This is a common mistake.

Imagine:

```csharp
lock (_lock)
{
    product.Stock--;
    context.SaveChanges();
}
```

This **doesn't provide proper distributed database concurrency control**.

If you have:

```text
API Server 1                 API Server 2
     │                            │
   lock A                       lock B
     │                            │
     └────── Database ────────────┘
```

Server 1 and Server 2 have different lock objects.

Both can enter their respective `lock` blocks.

For database concurrency, use appropriate **database transactions, isolation levels, atomic operations, optimistic concurrency, or database locking**, depending on the requirement.

---

# 9. Other synchronization mechanisms in C#

`lock` isn't the only option.

Depending on the problem, .NET provides mechanisms such as:

* `lock`
* `Monitor`
* `SemaphoreSlim`
* `Mutex`
* `Interlocked`
* concurrent collections such as `ConcurrentDictionary`

For example, if you simply need an atomic increment:

```csharp
Interlocked.Increment(ref _counter);
```

can be more appropriate than using a `lock`.

---

# 10. Interview answer

If the interviewer asks:

**"How do you handle concurrency in an in-memory scenario?"**

You can answer:

> **"For shared in-memory state accessed by multiple threads within the same process, I can use synchronization mechanisms such as `lock`. The `lock` ensures that only one thread at a time executes the critical section protected by the same lock object, preventing race conditions. However, `lock` is process-local, so it doesn't solve concurrency between multiple application instances. For distributed or database-level concurrency, I would use appropriate database or distributed synchronization mechanisms instead."**

### The easiest way to remember all five

```text
Optimistic concurrency
        ↓
"Detect if someone changed it."

Pessimistic concurrency
        ↓
"Lock it while I'm working."

Transaction
        ↓
"These operations succeed/fail together."

Atomic operation
        ↓
"Do this database change as one operation."

lock
        ↓
"Protect this shared memory from multiple threads."
```

That distinction is **very useful in a 10+ years .NET interview**, because an interviewer may give you a scenario and expect you to choose the appropriate mechanism rather than simply saying "use a lock."
