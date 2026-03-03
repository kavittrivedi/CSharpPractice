# CSharpPractice

## What is CTE, Explain why and when to use CTE in simple language

### What is a CTE?

CTE (Common Table Expression) is a temporary result set in SQL that you can refer to within the same query. Think of it as a way to simplify complex queries by breaking them into smaller, readable parts. You define a CTE at the beginning of your query and use it as if it were a table.

### How to Write a CTE?

Here's the syntax:

```sql
WITH CTE_Name AS (
    SELECT column1, column2
    FROM TableName
    WHERE some_condition
)
SELECT * 
FROM CTE_Name;
```

### Why Use CTE?

- **Improves Readability:** It makes your SQL query easier to read and understand, especially when dealing with complex logic.
- **Reusable Result Set:** You can refer to the CTE multiple times in your query, avoiding repetitive code.
- **Divide-and-Conquer:** Break down large queries into smaller, more manageable parts.
- **Recursion Support:** CTEs allow recursive queries, which are useful for hierarchical data like organizational charts or family trees.

### When to Use CTE?

- **Simplifying Nested Queries:** Use a CTE to replace subqueries, making the query more organized.

  **Example:**  
  Instead of:
  ```sql
  SELECT column1
  FROM (SELECT column1, column2 FROM TableName WHERE some_condition) AS TempTable
  WHERE column2 = some_value;
  ```
  Use:
  ```sql
  WITH CTE_Name AS (
      SELECT column1, column2
      FROM TableName
      WHERE some_condition
  )
  SELECT column1
  FROM CTE_Name
  WHERE column2 = some_value;
  ```

- **Working with Hierarchical Data:** For example, finding all employees reporting to a manager recursively.
- **Improving Query Readability:** When you have multiple intermediate steps to calculate results.
- **Avoiding Temporary Tables:** CTEs provide a clean alternative to using temporary tables for intermediate calculations.

By using CTEs effectively, you can make your SQL queries more efficient and easier to maintain.

## Where CTE's temporary result set resides?

A CTE's temporary result set resides in memory and is not physically stored in the database. It exists only for the duration of the query in which it is used.

### Key Points:

- **Scope:** The CTE result set is available only to the query that defines it. Once the query finishes executing, the CTE result set is gone.
- **Execution Location:** The database engine processes the CTE result set in memory during query execution. It does not create a separate physical table or write data to disk.
- **Optimization:** The query optimizer determines how to handle the CTE internally. In many cases, the CTE is "inlined" into the main query during execution, meaning it's treated as part of the larger query, not as an independent result set.
- **Performance:** Since the CTE resides in memory, its performance is tied to the complexity of the query and the available system resources. For very large datasets, consider whether alternative approaches (like indexed temporary tables) might be more efficient.

In summary, a CTE is a logical construct that resides in memory and is optimized by the query engine as part of the overall query execution plan.

## When we should not use CTE?

While CTEs are powerful and useful in many scenarios, there are situations where their use might not be the best choice. Here are some cases when you should avoid or reconsider using CTEs:

1. **Performance-Intensive Scenarios**

   **Issue:** CTEs do not inherently optimize performance. For large datasets or queries that require significant computation, CTEs might lead to inefficiencies because they are not materialized (i.e., stored temporarily).

   **Alternative:** Use temporary tables or indexed tables if the result set needs to be reused multiple times or if query performance is critical.

2. **When Multiple Reuses Are Required**

   **Issue:** CTEs are recalculated every time they are referenced in the query. If the same CTE is used multiple times in a query, the database engine processes it repeatedly, potentially slowing down the query.

   **Alternative:** Use a temporary table to store the intermediate result set for reuse.

3. **Large Result Sets**

   **Issue:** For queries that generate a large intermediate result set, CTEs can be resource-intensive because the result resides in memory and is recalculated as needed.

   **Alternative:** Use a physical table or a temporary table to store the data and improve performance.

4. **When Query Optimization Is Complex**

   **Issue:** CTEs are sometimes inlined into the main query by the optimizer, which can result in inefficient execution plans, especially for complex queries with multiple joins or aggregations.

   **Alternative:** Use views or restructure the query for better performance.

5. **Recursive Queries on Large Hierarchies**

   **Issue:** Recursive CTEs can be inefficient for large hierarchies or deeply nested data, leading to high CPU and memory usage.

   **Alternative:** For very large hierarchies, consider iterative solutions or database-specific features like hierarchical queries (e.g., CONNECT BY in Oracle or nested sets model).

6. **For Persistent Data**

   **Issue:** Since CTEs are temporary and exist only for the duration of the query, they are not suitable for storing or persisting intermediate results for later use.

   **Alternative:** Use a temporary table or a physical table for data that needs to persist across multiple queries.

7. **Complex Queries with Many CTEs**

   **Issue:** Excessive nesting of CTEs can make the query harder to debug, read, and maintain.

   **Alternative:** Break the query into smaller, manageable parts using temporary tables, views, or even multiple queries.

### Summary of When Not to Use CTE:

- Queries with large datasets or complex execution plans.
- Scenarios requiring repeated usage of the same intermediate result set.
- Tasks involving persistent or reusable data.
- Operations that demand high performance for large hierarchies or recursive structures.

By carefully evaluating the query's complexity and performance requirements, you can determine whether a CTE is the right choice or if alternative approaches like temporary tables or views are better.

## What is a View?

A View in SQL is like a virtual table. It doesn't store data itself but provides a way to see data from one or more tables. Think of it as a saved SQL query that you can use like a table.

For example:  
If you often write a query to find active customers, you can create a view called ActiveCustomers instead of writing the query repeatedly.

```sql
CREATE VIEW ActiveCustomers AS
SELECT CustomerID, CustomerName, IsActive
FROM Customers
WHERE IsActive = 1;
```

Now you can use the view like this:

```sql
SELECT * FROM ActiveCustomers;
```

### Where is a View Stored in the Database?

A view's definition (SQL query) is stored in the database's system catalog (also called the data dictionary), which holds metadata about the database objects.

A view does not store actual data. Instead, when you query a view, the database dynamically executes the query stored in the view definition to fetch the latest data from the underlying tables.

### Key Points:

- **Virtual Table:** A view doesn't hold its own data; it pulls data from the original tables at the time of query execution.
- **Dynamic Data:** Since views are based on underlying tables, any changes in those tables (like adding or updating rows) are automatically reflected in the view.
- **Stored Definition:** Only the query definition of the view is stored in the database, not the result set.
- **Materialized Views (Special Case):** In some databases, you can create a materialized view. This is different because it stores the query results physically in the database, making data retrieval faster but requiring periodic refreshes to stay updated.

### Why Use Views?

- **Simplify Queries:** Instead of writing complex queries repeatedly, create a view to encapsulate the logic.
- **Security:** Restrict access to specific columns or rows by exposing only the data you want through the view.
- **Consistency:** Provide a consistent way to access data, even if the underlying table structure changes.
- **Reusability:** Use the same view in multiple queries or applications.

### Example:

**Without a View:**
```sql
SELECT CustomerID, CustomerName
FROM Customers
WHERE IsActive = 1 AND Country = 'USA';
```

**With a View:**
```sql
CREATE VIEW ActiveUSCustomers AS
SELECT CustomerID, CustomerName
FROM Customers
WHERE IsActive = 1 AND Country = 'USA';
```

Now you can simply do:
```sql
SELECT * FROM ActiveUSCustomers;
```

### Summary:

A view is a virtual table that shows data from one or more tables based on a saved SQL query.

The view definition (not the data) is stored in the database system catalog.

Use views for simplifying queries, improving security, ensuring consistency, and reusing complex logic.

## SQL Table Vs SQL Views difference? explain in short and simple language

Here’s a simple comparison of SQL Tables vs SQL Views:

| **Aspect**       | **SQL Table**                                    | **SQL View**                                                                                                        |
| ---------------- | ------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------- |
| **Definition**   | A table stores actual data in rows and columns.  | A view is a virtual table based on a SQL query; it does not store data.                                             |
| **Storage**      | Data is physically stored in the database.       | Only the query definition is stored; no data is stored.                                                             |
| **Purpose**      | Used to store and manage data permanently.       | Used to simplify queries and provide a custom way to view data.                                                     |
| **Data Updates** | You can directly insert, update, or delete data. | Data in the view reflects changes in the underlying tables. Updates are limited and depend on the view's structure. |
| **Performance**  | Accessing data is usually faster as it’s stored. | Query execution can be slower since the view pulls data dynamically.                                                |
| **Security**     | Permissions control access to the entire table.  | Can restrict access by exposing only specific columns or rows.                                                      |
| **Dependency**   | Independent; tables exist on their own.          | Depends on one or more tables for data.                                                                             |
| **Example Use**  | Storing employee details permanently.            | Showing only active employees without writing a complex query every time.                                           |

### Key Takeaway:

Use tables to store data.

Use views to simplify complex queries, ensure data consistency, or improve security by restricting access.

## What is deadlock? how it occurs? Apart from sql deadlock what are the other places it can occur in application. Explain in short and simple language

### What is a Deadlock?

A deadlock occurs when two or more processes are stuck waiting for each other to release resources, and none can proceed. It's like two people blocking a narrow doorway, each waiting for the other to move first, causing a standoff.

### How Does a Deadlock Occur in SQL?

Deadlocks happen in SQL when:

- Two or more transactions are trying to access resources (e.g., tables or rows) but in a conflicting order.
- Each transaction holds a lock on a resource and waits for another resource that is locked by the other transaction.

**Example:**

- Transaction A locks Table 1 and wants to update Table 2.
- Transaction B locks Table 2 and wants to update Table 1.
- Both are waiting for each other, causing a deadlock.

### Other Places Where Deadlocks Can Occur:

- **Multithreading in Applications:**

  **Scenario:** Two threads each hold a lock on an object and try to acquire a lock on another object held by the other thread.

  **Example:**

  - Thread 1 locks Resource A and waits for Resource B.
  - Thread 2 locks Resource B and waits for Resource A.

- **Operating Systems:**

  Deadlocks can occur when multiple processes compete for limited resources like CPU, memory, or I/O devices.

- **File Systems:**

  Deadlocks happen when processes try to access files locked by others in a conflicting order.

- **Distributed Systems:**

  Deadlocks can occur across servers when processes wait for responses from each other in a circular dependency.

- **Networking:**

  Deadlocks can happen when two systems wait for each other's acknowledgment in communication protocols.

### Key Takeaways:

- Deadlocks occur when processes or threads are stuck waiting for resources in a circular dependency.
- Prevention involves carefully ordering resource acquisition and avoiding circular waits.
- Apart from SQL, deadlocks can occur in multithreading, OS resource management, file systems, distributed systems, and networking.

## How to resolve deadlock in SQL?

To resolve deadlocks in SQL, you need to identify the causes, avoid circular dependencies, and implement best practices to prevent or handle them. Here's how you can manage and resolve deadlocks:

### 1. Identify Deadlock Causes

- **Analyze Deadlock Graph:** SQL Server and other databases often provide tools like SQL Server Profiler or Extended Events to capture deadlock graphs. These graphs show which queries and resources are involved in the deadlock, helping you understand what went wrong.
- **Deadlock Trace:** SQL Server can be configured to generate deadlock trace files (.xdl) which provide detailed information about the deadlock occurrence.

### 2. Prevent Deadlocks

- **Consistent Locking Order:** Always access resources (tables, rows, etc.) in a consistent order across all transactions. This prevents circular wait conditions. For example, always lock Table 1 before Table 2, not the other way around.

  **Example:**

  - Transaction A locks Table 1 → locks Table 2.
  - Transaction B locks Table 1 → locks Table 2 (same order).

- **Short Transactions:** Keep transactions as short as possible. The longer a transaction holds a lock, the higher the chance of a deadlock.
- **Reduce Locking Granularity:** Avoid unnecessary locking of entire tables or large ranges of data. Instead, lock only the rows you need (e.g., use WITH (ROWLOCK) instead of WITH (TABLOCK)).
- **Use Row-Level Locking:** If you can, use row-level locks (ROWLOCK) to prevent locking entire tables, which reduces contention.
- **Optimize Indexes:** Well-indexed tables are less likely to result in long-running transactions, reducing the chance of deadlocks.

### 3. Handle Deadlocks Dynamically

- **Use SET DEADLOCK_PRIORITY:** In SQL Server, you can specify which transaction should be rolled back in case of a deadlock by setting a deadlock priority. Transactions with lower priority will be automatically rolled back to break the deadlock.

  **Example:** `SET DEADLOCK_PRIORITY LOW;`

- **Retry Logic:** Implement automatic retry logic in your application for deadlock errors. If a transaction is rolled back due to a deadlock, retry the operation after a short delay. This can be done using a try-catch block or equivalent error-handling mechanism.

  **Example in T-SQL:**
  ```sql
  BEGIN TRY
      -- Your transaction code here
  END TRY
  BEGIN CATCH
      IF ERROR_NUMBER() = 1205 -- Deadlock error
      BEGIN
          -- Retry logic, e.g., wait and retry
      END
  END CATCH
  ```

### 4. Optimize Transaction Design

- **Batch Updates:** If you're performing large data manipulations (e.g., updating many rows), break them into smaller batches. This reduces lock contention.
- **Lock Timeout:** Use lock timeouts to prevent waiting indefinitely for a lock. In SQL Server, you can set LOCK_TIMEOUT to avoid waiting for a lock forever. `SET LOCK_TIMEOUT 1000; -- Time in milliseconds`

### 5. Use Snapshot Isolation (Optional)

- **Snapshot Isolation:** This isolation level allows transactions to access the database as it was at the start of the transaction, without acquiring locks on the data. This can reduce deadlock situations, especially in read-heavy systems. `ALTER DATABASE MyDB SET ALLOW_SNAPSHOT_ISOLATION ON;`

### 6. Database-Specific Tools

- **SQL Server:** SQL Server has a deadlock graph and trace events to capture detailed information about deadlocks.
- **Other Databases:** Different databases have their own tools for detecting and handling deadlocks. For instance, MySQL provides the SHOW ENGINE INNODB STATUS command to view deadlock information.

### Summary of Strategies:

- Use consistent locking order.
- Keep transactions short and lock only necessary resources.
- Optimize indexing to reduce the time spent waiting for locks.
- Use SET DEADLOCK_PRIORITY to control which transaction gets rolled back.
- Implement retry logic in your application for deadlock errors.
- Consider using snapshot isolation for reducing lock contention.

By following these strategies, you can either avoid deadlocks altogether or minimize their impact on your SQL transactions.

## Explain the meaning of query execution plan. In short and simple language

A Query Execution Plan is a roadmap created by the database engine that shows how a query will be executed. It explains the steps the database will take to retrieve or modify data.

### Key Points:

- **Purpose:** It helps optimize queries by showing how resources like tables, indexes, and joins are used.
- **Contains:** Details like the order of operations, which indexes are used, and how data is filtered or sorted.
- **View Execution Plan:** You can see it in most databases using tools like:
  - **SQL Server:** Use EXPLAIN or "Display Estimated Execution Plan."
  - **MySQL:** Use EXPLAIN.

### Example:

If you run a query: `SELECT * FROM Employees WHERE Department = 'IT';`

The execution plan will show:

- Whether it scans the whole table or uses an index.
- The sequence of steps it takes to find the data.

### Why It’s Important:

It helps:

- Identify bottlenecks in queries.
- Improve performance by suggesting optimizations (e.g., adding indexes).

In short, it’s like a "GPS map" for your query, showing how the database gets your results.

## How to use Query Execution Plan? explain in short and simple language

Using a Query Execution Plan helps you understand and optimize how a query runs. Here’s how to use it, step by step:

### 1. Generate the Execution Plan

**SQL Server:**

- Use the "Estimated Execution Plan" option in tools like SQL Server Management Studio (SSMS).
- Or run: `SET SHOWPLAN_TEXT ON; -- Text-based plan`
- `SET SHOWPLAN_XML ON; -- XML-based plan`

### 2. Read the Plan

The execution plan shows:

- **Steps:** What operations (e.g., table scans, joins) are performed.
- **Order:** The sequence of these steps.
- **Indexes:** Whether indexes are used or not.
- **Cost:** An estimate of how much time or resources each step takes.

### 3. Look for Optimization Opportunities

- **Full Table Scan:** If the query scans the whole table, consider adding an index to improve performance.
- **Missing Indexes:** Check if the execution plan suggests missing indexes.
- **Costly Joins:** Use better indexing or rewrite the query to optimize joins.
- **Sorting or Filtering:** Ensure unnecessary sorting or filtering is avoided by writing efficient WHERE and ORDER BY clauses.

### 4. Implement Optimizations

Based on what you learn from the plan:

- Add or update indexes.
- Rewrite queries to be more efficient.
- Avoid unnecessary columns or operations.

### 5. Test Again

After making changes, generate the execution plan again to see if the query is now faster and uses fewer resources.

### Why Use It?

It identifies bottlenecks and shows where time and resources are being spent.

Helps make queries faster and more efficient.

In short, the execution plan is a guide to fine-tuning your database queries for better performance.

## Difference between clustered and non-clustered indexes? Which one is faster? Advantages and disadvantages of non-clustered indexes. Explain in short and simple language

### Difference Between Clustered and Non-Clustered Indexes

| **Aspect**        | **Clustered Index**                                                                         | **Non-Clustered Index**                                                     |
| ----------------- | ------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------- |
| **Data Storage**  | Physically sorts and stores rows in the table based on the indexed column(s).               | Stores pointers to the actual data rows without rearranging them.           |
| **One per Table** | Only one clustered index allowed per table (because it defines the table's physical order). | Multiple non-clustered indexes can exist on a table.                        |
| **Size**          | Generally smaller, as it’s part of the main table.                                          | Requires additional storage for pointers and data structure.                |
| **Performance**   | Faster for retrieving sorted and range-based queries.                                       | Slightly slower for lookups, as it needs to follow pointers to access data. |

### Which One is Faster?

- **Clustered Index** is faster for retrieving data directly because the data is stored in the same order as the index.
- **Non-Clustered Index** is slower in comparison, especially for large datasets, as it requires an extra step to fetch the actual data (lookup).

### Advantages of Non-Clustered Indexes

- **Supports Multiple Indexes:** You can create several non-clustered indexes on a table, each tailored for different queries.
- **Improves Read Performance:** Speeds up SELECT queries for specific columns without affecting the physical order of data.
- **Useful for Covering Indexes:** Can include non-key columns, making the index self-sufficient for some queries.

### Disadvantages of Non-Clustered Indexes

- **Slower than Clustered Index:** Requires an extra step (lookup) to find the actual data.
- **Takes More Space:** Needs additional storage for pointers and the index structure.
- **Performance Hit on INSERT/UPDATE/DELETE:** Maintaining non-clustered indexes adds overhead to these operations.

### When to Use Non-Clustered Indexes

- When you need to optimize queries for columns that are not part of the primary key.
- For queries involving multiple WHERE conditions or frequent searches on non-primary key columns.

## What is tempdb? Explain in short and simple language

TempDB is a special system database in SQL Server used to store temporary data. It is automatically created when the SQL Server starts and is re-created every time the server restarts.

### Uses of TempDB

- **Temporary Tables:** Stores data for temporary tables (#TempTable or ##GlobalTempTable).
- **Work Tables:** Holds intermediate results created by SQL Server during query execution (e.g., sorting or joining).
- **Version Store:** Stores row versions for features like snapshot isolation or online index rebuilding.
- **Table Variables:** Sometimes uses TempDB for table variable storage.
- **Sorting/Hashing:** Used for operations requiring temporary sorting or hashing (e.g., ORDER BY, GROUP BY).

### Characteristics

- **Shared Across All Users:** TempDB is used by all users and sessions in SQL Server.
- **Non-Persistent:** Data in TempDB is not saved permanently. It is wiped clean when the SQL Server restarts.
- **Performance-Optimized:** Designed for high-speed temporary data storage.

### Why It’s Important

Without TempDB, SQL Server cannot perform many critical operations, making it essential for smooth database functioning.

In short, TempDB is like a scratchpad for SQL Server, helping it handle temporary and intermediate data efficiently.

## What optimization techniques are there in SQL? in explain with short and simple language

### SQL Optimization Techniques

Here are some common techniques to improve SQL query performance, explained simply:

1. **Use Indexes**

   What it does: Speeds up data retrieval by creating a "shortcut" to rows.

   Example: Add an index to frequently queried columns in WHERE, JOIN, or ORDER BY.

   Tip: Use clustered indexes for primary keys and non-clustered indexes for other important columns.

2. **Avoid SELECT ***

   Why: Fetching all columns wastes resources and slows performance.

   What to do: Fetch only the required columns.

   Example: Replace: `SELECT * FROM Employees;` with: `SELECT Name, Department FROM Employees;`

3. **Use Joins Instead of Subqueries**

   Why: Joins are generally faster than subqueries because they optimize execution plans better.

   Example: Replace:
   ```sql
   SELECT Name FROM Employees WHERE DepartmentID IN (SELECT ID FROM Departments WHERE Name = 'IT');
   ```
   with:
   ```sql
   SELECT e.Name 
   FROM Employees e 
   JOIN Departments d ON e.DepartmentID = d.ID 
   WHERE d.Name = 'IT';
   ```

4. **Add WHERE Clauses**

   Why: Limits the rows processed, reducing workload.

   Example: Instead of querying the entire table: `SELECT * FROM Employees;` Add conditions: `SELECT * FROM Employees WHERE Department = 'IT';`

5. **Use Proper Data Types**

   Why: Avoids extra storage and speeds up operations.

   Example: Use INT for IDs instead of VARCHAR.

6. **Optimize Index Usage**

   Why: Index scans are faster than table scans.

   How: Ensure the query uses the correct index. Check execution plans to verify.

7. **Avoid Functions on Columns in WHERE**

   Why: Functions prevent the database from using indexes.

   Example: Replace: `WHERE YEAR(CreatedDate) = 2023;` with: `WHERE CreatedDate BETWEEN '2023-01-01' AND '2023-12-31';`

8. **Use LIMIT/OFFSET or TOP**

   Why: Fetching fewer rows improves performance.

   Example: `SELECT * FROM Employees LIMIT 10;`

9. **Use UNION ALL Instead of UNION**

   Why: UNION ALL skips the duplicate-checking step of UNION, making it faster.

   Example: Replace:
   ```sql
   SELECT Name FROM Employees 
   UNION 
   SELECT Name FROM Contractors;
   ```
   with:
   ```sql
   SELECT Name FROM Employees 
   UNION ALL 
   SELECT Name FROM Contractors;
   ```

10. **Use Temporary Tables or Common Table Expressions (CTEs)**

    Why: Helps break down complex queries into manageable and reusable steps.

11. **Analyze Execution Plans**

    Why: Identify slow parts of your query (e.g., table scans, missing indexes).

12. **Avoid Overusing Cursors**

    Why: Cursors are slow for large datasets. Replace them with set-based operations like UPDATE or SELECT.

13. **Optimize Joins**

    Why: Proper indexing and reducing unnecessary columns in joins can significantly improve speed.

14. **Partition Large Tables**

    Why: Dividing data into smaller partitions improves query performance.

15. **Update Statistics**

    Why: Keeps the query optimizer informed about data distribution.

By applying these techniques, you can improve query speed, reduce resource usage, and make your SQL applications more efficient.

## How will you increase performance for your query operations? explain with short and simple language

To increase query performance, you can follow these techniques:

1. **Use Indexes**

   Add indexes to frequently searched or joined columns to speed up lookups.

   Use clustered indexes for primary keys and non-clustered indexes for other key columns.

2. **Fetch Only Needed Data**

   Avoid SELECT *—retrieve only the required columns.

   Example: Use `SELECT Name, Salary FROM Employees` instead of `SELECT *`.

3. **Filter Rows Early**

   Use WHERE clauses to filter data as early as possible.

   Example: `SELECT * FROM Orders WHERE OrderDate > '2023-01-01'`.

4. **Use Joins Wisely**

   Use INNER JOIN for relevant data and avoid unnecessary joins.

   Ensure columns used in joins are indexed.

5. **Optimize WHERE Conditions**

   Avoid functions in WHERE clauses; they prevent index usage.

   Replace: `WHERE YEAR(Date) = 2023`
   With: `WHERE Date BETWEEN '2023-01-01' AND '2023-12-31'`.

6. **Limit Rows**

   Use LIMIT (or TOP in SQL Server) to fetch only what you need.

   Example: `SELECT TOP 10 * FROM Employees`.

7. **Avoid Duplicate Work**

   Use CTEs or temporary tables to store intermediate results for reuse.

8. **Use UNION ALL**

   If duplicate records are not an issue, use UNION ALL instead of UNION to skip the extra step of removing duplicates.

9. **Keep Queries Simple**

   Avoid overly complex queries. Break them into smaller, manageable steps.

10. **Analyze Execution Plan**

    Check the execution plan to identify slow operations like table scans or missing indexes.

11. **Update Statistics**

    Ensure database statistics are up-to-date to help the query optimizer.

12. **Partition Large Tables**

    Divide large tables into smaller chunks (partitions) for faster query access.

13. **Use Proper Data Types**

    Use efficient data types (e.g., INT instead of VARCHAR for IDs).

14. **Optimize Sorting**

    Use indexed columns in ORDER BY clauses to speed up sorting.

15. **Reduce Lock Contention**

    Use transactions carefully and keep them short to avoid blocking other queries.

By applying these techniques, you can make your query operations faster and more efficient!

## SP are pre-compiled means what? explain with short and simple language

When we say Stored Procedures (SPs) are pre-compiled, it means:

### Once Compiled, Reused:

The first time a stored procedure is executed, SQL Server compiles it into an execution plan, which is a step-by-step guide for running the procedure.

This compiled plan is stored in memory and reused for subsequent calls to the stored procedure, saving the time and effort of compiling it again.

### Improves Performance:

Instead of compiling the SQL query each time it runs (like ad-hoc queries), the stored procedure's pre-compiled execution plan speeds up execution.

### Example

If you frequently run this query: `SELECT * FROM Orders WHERE OrderDate > '2023-01-01';`

Instead of sending this query each time, wrap it in a stored procedure:

```sql
CREATE PROCEDURE GetRecentOrders
AS
BEGIN
    SELECT * FROM Orders WHERE OrderDate > '2023-01-01';
END;
```

The first run compiles it, and later calls reuse the compiled plan, reducing overhead.

### Key Benefit

**Consistency and Speed:** Pre-compilation reduces query parsing and compilation time, making stored procedures faster and more efficient than executing raw SQL each time.

## How to design multi-tenant DB? Explain with short and simple language

Designing a multi-tenant database means creating a system where multiple tenants (customers or users) share the same database while keeping their data isolated. Here's how to do it:

### Approaches for Multi-Tenant DB Design

#### Shared Database with Shared Schema

**Description:** All tenants use the same database and the same tables. Data is separated using a column like TenantID.

**Example:** Employees Table:
| TenantID | EmployeeID | Name   | Department |
|----------|------------|--------+------------|
| 1        | 101        | Alice  | HR         |
| 2        | 201        | Bob    | IT         |

**Advantages:**

- Simple to set up and maintain.
- Cost-efficient.

**Disadvantages:**

- Complex queries for tenant-specific data.
- Risk of data leakage if TenantID is not handled correctly.

#### Shared Database with Separate Schemas

**Description:** One database, but each tenant has their own schema (a group of tables).

**Example:** Tenant1.Employees
Tenant2.Employees

**Advantages:**

- Better data isolation.
- Easier customization for each tenant.

**Disadvantages:**

- Slightly higher maintenance.
- May not scale well with many tenants.

#### Separate Database per Tenant

**Description:** Each tenant has their own database.

**Example:** Tenant1_DB
Tenant2_DB

**Advantages:**

- Complete data isolation.
- Best for security and customization.

**Disadvantages:**

- Expensive to maintain with many tenants.
- Harder to manage at scale.

### Best Practices for Multi-Tenant DB

- **Use Tenant Identifiers:** Always include a unique TenantID to identify data belonging to a tenant.
- **Plan for Scalability:** Choose the right design (shared schema, separate schema, or separate DB) based on expected tenant growth.
- **Secure Data:** Ensure strong isolation using proper indexing, access control, and encryption.
- **Centralized Metadata:** Use a metadata table to map tenants to their schemas or databases.

  **Example:** TenantMetadata Table:
  | TenantID | Schema/DatabaseName |
  |----------|---------------------|
  | 1        | Tenant1_DB          |
  | 2        | Tenant2_DB          |

- **Optimize Performance:** Index TenantID in shared schema designs. Use partitioning for large datasets.

### When to Use Which Approach

- **Shared Schema:** Small apps with low data sensitivity and many tenants.
- **Separate Schema:** Medium-sized apps with some need for customization.
- **Separate Database:** High-security apps or large tenants needing complete isolation.

In short, choose the design based on your app’s size, security needs, and scalability goals.

## How to improve the performance of DB. Explain with short and simple language

Improving the performance of a database involves optimizing how data is stored, retrieved, and processed. Here are key techniques in simple terms:

1. **Use Indexes**

   Create indexes on columns used frequently in WHERE, JOIN, and ORDER BY clauses.

   Avoid too many indexes; they slow down INSERT/UPDATE operations.

2. **Optimize Queries**

   Fetch only the needed columns (SELECT specific_column instead of SELECT *).

   Write efficient WHERE conditions (e.g., avoid functions on columns like YEAR(Date)).

   Use LIMIT or TOP to retrieve a small set of rows when needed.

3. **Normalize and Denormalize**

   Normalize: Organize data into smaller related tables to reduce redundancy.

   Denormalize: Combine tables for frequently accessed data to reduce joins, if necessary.

4. **Partition Large Tables**

   Divide large tables into smaller, manageable partitions based on criteria (e.g., date ranges).

   Makes querying faster by scanning only relevant partitions.

5. **Use Proper Data Types**

   Choose efficient data types (e.g., INT for IDs instead of VARCHAR).

   Avoid unnecessarily large data types like TEXT when not needed.

6. **Archive Old Data**

   Move rarely accessed historical data to an archive table or database.

   Reduces the size of active tables and speeds up queries.

7. **Avoid Over-Locking**

   Use transactions carefully and keep them short.

   Reduce locking by using NOLOCK (if safe) for read-heavy operations.

8. **Maintain Updated Statistics**

   Ensure database statistics are up-to-date so the query optimizer makes better decisions.

9. **Use Query Execution Plan**

   Analyze execution plans to identify and fix slow operations like table scans or missing indexes.

10. **Optimize Database Configuration**

    Tune memory allocation, disk I/O, and connection settings based on workload.

11. **Caching**

    Use caching for frequently accessed data to reduce repeated database queries.

    Tools: Redis, Memcached, or in-memory caching within the application.

12. **Avoid Overly Complex Joins**

    Simplify queries with too many joins. Break them into smaller steps or use temporary tables.

13. **Use Stored Procedures**

    Replace repetitive ad-hoc queries with stored procedures to reduce compilation overhead.

14. **Backup and Clean Up**

    Regularly remove unused indexes, shrink log files, and clean up unused tables.

By applying these strategies, you can ensure your database performs efficiently and scales effectively as your application grows.

## What is the better to use EF or SQL procedure? Explain with short and simple language

Choosing between Entity Framework (EF) and SQL Stored Procedures depends on your project needs. Here’s a simple comparison to help you decide:

### Entity Framework (EF)

**Advantages:**

- **Ease of Use:** EF allows you to write queries in C# (LINQ) instead of SQL. Reduces the need to know complex SQL syntax.
- **Maintainability:** Changes in the database are reflected automatically (with migrations). Easier for developers to manage and understand code.
- **Flexibility:** Works well when you need to interact with multiple tables or complex data structures dynamically.
- **Cross-Platform:** EF Core works on .NET Core, making it suitable for modern cross-platform applications.

**Disadvantages:**

- **Performance:** Slower for complex queries compared to stored procedures due to overhead.
- **Control:** Less control over how queries are executed compared to raw SQL.

### SQL Stored Procedures

**Advantages:**

- **Performance:** Pre-compiled and optimized, leading to faster execution for complex operations. Can handle bulk operations efficiently.
- **Security:** Provides better control over access (only execute permissions needed). Prevents SQL injection when properly used.
- **Complex Logic:** Great for heavy data processing or reusable business logic directly in the database.

**Disadvantages:**

- **Tight Coupling:** Logic is tied to the database, making it harder to migrate or change databases.
- **Developer Dependency:** Requires knowledge of SQL, which may not be ideal for all developers.

### When to Use EF

- Projects with frequent schema changes.
- Applications requiring dynamic, complex queries in C#.
- When ease of development and maintainability is a priority.

### When to Use SQL Stored Procedures

- Performance-critical operations.
- Applications with heavy database processing.
- Need for secure, pre-defined business logic at the database level.

### Best Practice

**Combine both:** Use EF for simpler queries and CRUD operations. Use stored procedures for complex, performance-critical operations.

This approach gives you the best of both worlds!

## How we can prevent SQL injection in stored procedure?

To prevent SQL injection in stored procedures, you need to ensure that user input is handled safely and securely. Here are the key practices explained simply:

1. **Use Parameterized Queries**

   Always use parameters to pass values into the stored procedure instead of concatenating user input into SQL strings.

   **Example:**
   ```sql
   CREATE PROCEDURE GetEmployeeById
       @EmployeeID INT
   AS
   BEGIN
       SELECT * FROM Employees WHERE EmployeeID = @EmployeeID;
   END;
   ```
   This approach ensures that input is treated as data, not executable SQL.

2. **Avoid Dynamic SQL**

   Do not build SQL queries dynamically by concatenating strings with user input.

   **Bad Example (Vulnerable to SQL Injection):**
   ```sql
   CREATE PROCEDURE GetEmployees
       @Department NVARCHAR(50)
   AS
   BEGIN
       EXEC('SELECT * FROM Employees WHERE Department = ''' + @Department + '''');
   END;
   ```
   **Instead, use parameterized execution:**
   ```sql
   CREATE PROCEDURE GetEmployees
       @Department NVARCHAR(50)
   AS
   BEGIN
       SELECT * FROM Employees WHERE Department = @Department;
   END;
   ```

3. **Validate Input**

   Validate user input at the application level and, if needed, in the stored procedure itself.

   Ensure that the input matches expected formats (e.g., numbers, email addresses, etc.).

4. **Use Proper Permissions**

   Limit database permissions for the application user:

   - Read-only for SELECT operations.
   - Only necessary permissions for executing specific stored procedures.
   - Avoid granting direct access to underlying tables.

5. **Avoid Executing Untrusted Code**

   Do not use user input to determine table or column names in SQL queries.

6. **Use QUOTENAME for Dynamic SQL**

   If dynamic SQL is unavoidable, use the QUOTENAME function to safely handle identifiers (e.g., table or column names).

   **Example:**
   ```sql
   CREATE PROCEDURE GetTableData
       @TableName NVARCHAR(50)
   AS
   BEGIN
       DECLARE @SQL NVARCHAR(MAX);
       SET @SQL = 'SELECT * FROM ' + QUOTENAME(@TableName);
       EXEC sp_executesql @SQL;
   END;
   ```

7. **Use sp_executesql for Dynamic SQL**

   If dynamic SQL is required, use sp_executesql with parameters.

   **Example:**
   ```sql
   CREATE PROCEDURE GetEmployees
       @Department NVARCHAR(50)
   AS
   BEGIN
       DECLARE @SQL NVARCHAR(MAX);
       SET @SQL = 'SELECT * FROM Employees WHERE Department = @Dept';
       EXEC sp_executesql @SQL, N'@Dept NVARCHAR(50)', @Dept = @Department;
   END;
   ```

8. **Regularly Monitor and Test**

   Conduct regular security tests to ensure your stored procedures are not vulnerable.

   Use tools to simulate and identify injection attempts.

By following these practices, you can safeguard your stored procedures from SQL injection attacks effectively.

## What is seek index and Scan Index? Explain in short and simple language

### Seek Index vs. Scan Index

When a database executes a query, it uses indexes to locate rows in a table. The way it accesses the data can either be an Index Seek or an Index Scan. Here’s a simple explanation:

1. **Index Seek**

   **Definition:** The database directly goes to the specific location of the required data using the index, like looking up a word in a dictionary.

   **How it Works:** Uses the index to narrow down the search to just the rows that match the condition (efficient).

   **When it Happens:**

   - When a query uses a filter (WHERE clause) that matches the indexed column.
   - Example: Searching for EmployeeID = 123 in an indexed column.

   **Performance:** Faster and more efficient, as it only accesses relevant rows.

2. **Index Scan**

   **Definition:** The database reads the entire index (or table) row by row, like scanning every page in a book to find a word.

   **How it Works:** Goes through all the rows in the index, even if only a few match the condition (less efficient).

   **When it Happens:**

   - No suitable index exists.
   - The query needs most of the rows (e.g., no filtering or searching by a non-indexed column).

   **Performance:** Slower and less efficient, especially for large tables.

### Key Difference

| **Feature**       | **Index Seek**                 | **Index Scan**                      |
| ----------------- | ------------------------------ | ----------------------------------- |
| **Search Method** | Directly locates matching rows | Reads all rows sequentially         |
| **Efficiency**    | Highly efficient               | Less efficient for large datasets   |
| **When Used**     | When a good index exists       | When no suitable index is available |

### Which is Better?

- **Index Seek** is better for performance because it retrieves only the required data.
- **Index Scan** is okay when most of the table’s data is needed (e.g., full table report).

### Optimization Tip

To avoid unnecessary scans, create appropriate indexes on frequently queried columns.

## Explain query of each seek index and scan index please

### Examples of Query for Index Seek and Index Scan

Here are examples to illustrate how queries result in Index Seek and Index Scan operations:

1. **Query Example for Index Seek**

   **Scenario:** You have an index on the EmployeeID column.

   **Query:**
   ```sql
   SELECT * 
   FROM Employees 
   WHERE EmployeeID = 123;
   ```

   **Explanation:**

   - The database uses the index to directly locate the row where EmployeeID = 123.
   - Only the relevant rows are fetched.

   **Result:** Index Seek because it’s a targeted search using the indexed column.

2. **Query Example for Index Scan**

   **Scenario:** You query on a non-indexed column (e.g., LastName) or use a query without a specific filter.

   **Query:**
   ```sql
   SELECT * 
   FROM Employees 
   WHERE LastName = 'Smith';
   ```

   **Explanation:**

   - If LastName is not indexed, the database has no choice but to scan the entire table or index to find rows where LastName = 'Smith'.
   - Every row is checked, even if only a few match.

   **Result:** Index Scan because the database reads the whole table or index.

### How to Check Whether the Query Uses Seek or Scan

**View Execution Plan:**

- In SQL Server, before executing the query, click "Include Actual Execution Plan".
- Run the query and check the execution plan.
- Look for Index Seek or Index Scan in the plan diagram.

**Interpreting the Plan:**

- **Index Seek:** Indicates a direct, efficient search.
- **Index Scan:** Indicates a full scan of rows or index.

### Optimization

To improve query performance and avoid scans:

- Create indexes on columns used in filters (WHERE clauses) or joins.
- Use SELECT specific columns instead of SELECT *.
- Avoid functions or expressions on indexed columns in queries (e.g., WHERE UPPER(Name) = 'JOHN' prevents index usage).
