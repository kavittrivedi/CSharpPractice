# Microservices Interview Questions & Answers

## 1. Microservices Fundamentals

### 1. What is Microservices Architecture?

**Answer:**
Microservices is an architectural style where an application is divided into **small, independently deployable services**, with each service responsible for a specific business capability.

Each service typically has:

* Its own business logic
* Its own data
* Its own API
* Independent deployment
* Independent scaling

---

### 2. What is the difference between Monolithic and Microservices architecture?

| Monolith                   | Microservices                   |
| -------------------------- | ------------------------------- |
| Single application         | Multiple independent services   |
| Usually one deployment     | Independent deployments         |
| Often shared database      | Usually database per service    |
| Scaling entire application | Scale individual services       |
| Tight coupling can develop | Services aim for loose coupling |
| Simpler initially          | More operational complexity     |

---

### 3. What are the advantages of Microservices?

Main advantages:

* Independent deployment
* Independent scaling
* Fault isolation
* Technology flexibility
* Smaller codebases
* Faster development by multiple teams
* Better alignment with business capabilities

---

### 4. What are the disadvantages?

* Distributed-system complexity
* Network failures
* Data consistency problems
* Distributed transactions
* More monitoring required
* Deployment complexity
* Service-to-service security
* Debugging becomes harder
* Infrastructure cost can increase

**Good interview point:**

> Microservices don't automatically make an application better. They trade application complexity for distributed-system and operational complexity.

---

### 5. When should you use Microservices?

Use microservices when:

* The system is large and complex
* Different components need independent scaling
* Multiple teams work independently
* Independent deployment is important
* Different business capabilities have clear boundaries

---

### 6. When should you NOT use Microservices?

For a small application or small team where:

* Business boundaries aren't clear
* Independent deployment isn't required
* Operational infrastructure isn't available
* The additional distributed-system complexity isn't justified

A **modular monolith** can be a better starting point.

---

### 7. What is a Microservice?

A microservice is an independently deployable service that owns a **specific business capability** and communicates with other services through well-defined interfaces.

---

### 8. How small should a microservice be?

There is no fixed number of lines of code or classes.

The service should be **as small as necessary but as large as required to own a meaningful business capability**.

A good rule:

> Design around business capabilities, not technical layers.

---

# 2. Service Boundaries

### 9. How do you identify Microservice boundaries?

I identify boundaries using:

* Business capabilities
* Domain-Driven Design
* Bounded contexts
* Data ownership
* Team ownership
* Change frequency
* Scalability requirements

For example:

```text
E-Commerce

Customer Service
Order Service
Product Service
Payment Service
Inventory Service
Shipping Service
```

---

### 10. What is Bounded Context?

A bounded context is a **logical boundary within which a particular domain model and terminology have a specific meaning**.

For example:

```text
Order Context
    Order
    Customer
    PaymentStatus

Shipping Context
    Shipment
    Address
    DeliveryStatus
```

The same concept may have different meanings in different contexts.

---

### 11. What is Domain-Driven Design?

DDD is an approach to designing software around the **business domain**.

Important concepts include:

* Entities
* Value Objects
* Aggregates
* Aggregate Roots
* Domain Events
* Repositories
* Bounded Contexts

---

### 12. Should every database table become a microservice?

**No.**

A microservice should represent a **business capability**, not a database table.

Bad:

```text
CustomerTableService
AddressTableService
OrderTableService
```

Better:

```text
Customer Service
Order Service
Payment Service
```

---

# 3. Database Questions

### 13. What is Database-per-Service?

Each microservice owns its own database/data store.

```text
Order Service → Order DB

Payment Service → Payment DB

Inventory Service → Inventory DB
```

Other services should not directly access its database.

---

### 14. Why should each microservice have its own database?

Because it provides:

* Data ownership
* Loose coupling
* Independent schema changes
* Independent scaling
* Better service autonomy

---

### 15. Can two microservices share a database?

Technically yes, but it creates **tight coupling**.

If services directly share tables, one service can break another by changing the schema.

Prefer:

> Database-per-service.

---

### 16. How do microservices share data?

Usually through:

* APIs
* Events
* Messages

For example:

```text
Order Service
     ↓ Event
OrderCreated
     ↓
Inventory Service
```

---

# 4. Communication

### 17. How do Microservices communicate?

Two major approaches:

**Synchronous**

```text
Service A → REST → Service B
```

**Asynchronous**

```text
Service A → Message Broker → Service B
```

---

### 18. REST vs gRPC?

**REST**

* HTTP-based
* Easy to consume
* Good for public APIs
* JSON commonly used

**gRPC**

* HTTP/2
* Protocol Buffers
* High performance
* Strong contracts
* Good for internal service-to-service communication

---

### 19. When would you use synchronous communication?

Use it when the caller immediately needs a response.

Example:

```text
Order → Payment
```

The order process may need an immediate payment authorization result.

---

### 20. When would you use asynchronous communication?

Use it when:

* Immediate response isn't required
* You want loose coupling
* Processing can happen later
* You need better resilience

Example:

```text
OrderCreated
      ↓
Message Broker
      ↓
Inventory
      ↓
Email
      ↓
Analytics
```

---

### 21. What is Event-Driven Architecture?

Services communicate by publishing and consuming events.

Example:

```text
Order Service
      ↓
OrderCreated Event
      ↓
Message Broker
   ↙       ↓       ↘
Inventory Payment Notification
```

The publisher doesn't need to know all consumers.

---

### 22. Event vs Command?

**Command:**

> Do something.

Example:

```text
CreateOrder
ProcessPayment
CancelOrder
```

**Event:**

> Something already happened.

Example:

```text
OrderCreated
PaymentProcessed
OrderCancelled
```

---

### 23. What is Pub/Sub?

Publish/Subscribe allows one publisher to publish a message/event while multiple subscribers consume it.

```text
Publisher
   ↓
OrderCreated
   ↓
 ┌──────┬──────────┬─────────┐
 ↓      ↓          ↓
Email Inventory Analytics
```

---

# 5. API Gateway

### 24. What is an API Gateway?

An API Gateway is a single entry point between clients and backend services.

```text
Angular
   ↓
API Gateway
   ↓
 ┌──────┬───────┬────────┐
Order  Payment Inventory
```

---

### 25. Why use API Gateway?

It can handle:

* Routing
* Authentication
* Authorization
* Rate limiting
* Logging
* SSL termination
* Request aggregation
* Load balancing

---

### 26. What is the difference between API Gateway and Load Balancer?

**Load Balancer:** primarily distributes traffic across instances.

**API Gateway:** provides API-level capabilities such as routing, authentication, transformation, rate limiting and aggregation.

They can coexist.

---

### 27. What is BFF?

BFF means **Backend for Frontend**.

Instead of one generic API for all clients:

```text
Mobile → Mobile BFF
Web → Web BFF
```

Each BFF is optimized for its client.

---

# 6. Distributed Transactions

### 28. What is the biggest database challenge in Microservices?

Maintaining **data consistency across multiple independent databases**.

---

### 29. Why is a distributed transaction difficult?

Suppose:

```text
Order DB
Payment DB
Inventory DB
```

A single transaction cannot easily span all three databases without introducing significant coupling and distributed transaction complexity.

---

### 30. What is the Saga Pattern?

Saga manages a business transaction as a sequence of **local transactions**.

Example:

```text
Create Order
   ↓
Reserve Inventory
   ↓
Process Payment
   ↓
Confirm Order
```

If payment fails:

```text
Release Inventory
Cancel Order
```

---

### 31. Choreography vs Orchestration?

**Choreography:**

Services react to events.

```text
OrderCreated
    ↓
Inventory
    ↓
InventoryReserved
    ↓
Payment
```

No central coordinator.

**Orchestration:**

A central orchestrator controls the workflow.

```text
Saga Orchestrator
   ↓
Order
   ↓
Inventory
   ↓
Payment
```

---

### 32. What is Eventual Consistency?

It means data across services may temporarily be inconsistent but will become consistent after events/messages are processed.

Example:

```text
Order = Confirmed

Inventory = still processing

Payment = processing
```

After processing:

```text
Order = Confirmed
Inventory = Reserved
Payment = Successful
```

---

# 7. CQRS

### 33. What is CQRS?

CQRS means:

**Command Query Responsibility Segregation**

It separates:

```text
Commands → Change data

Queries → Read data
```

---

### 34. Why use CQRS?

It can provide:

* Independent scaling
* Optimized read/write models
* Better separation of responsibilities
* Complex domain modeling support

But CQRS shouldn't be used everywhere.

---

### 35. CQRS vs CRUD?

CRUD uses the same model for:

```text
Create
Read
Update
Delete
```

CQRS separates the read and write responsibilities.

---

# 8. Resilience

### 36. What happens if one microservice goes down?

The system should ideally continue operating where possible.

This requires:

* Timeouts
* Retries
* Circuit breakers
* Fallbacks
* Queues
* Health checks

---

### 37. What is Circuit Breaker?

Circuit breaker prevents repeated calls to a failing service.

```text
Normal
  ↓
Failure
  ↓
Open Circuit
  ↓
Stop calling service
  ↓
Wait
  ↓
Half Open
  ↓
Test
  ↓
Closed
```

---

### 38. What is Retry?

Retry attempts a failed operation again.

Example:

```text
Call Service
   ↓
Failure
   ↓
Wait
   ↓
Retry
```

Use **exponential backoff** rather than immediately retrying continuously.

---

### 39. Why can retries be dangerous?

Suppose payment succeeds but the response is lost.

The client retries:

```text
Payment → SUCCESS

Response lost

Retry Payment
```

You could charge the customer twice.

Therefore, important operations need **idempotency**.

---

### 40. What is Idempotency?

An operation is idempotent when performing it multiple times produces the same final result as performing it once.

Example:

```text
PaymentRequestId = ABC123
```

If the same request arrives again, the service recognizes it and doesn't charge again.

---

### 41. What is Timeout?

A timeout prevents a service from waiting indefinitely for another service.

Example:

```text
Service A
   ↓
Service B
   ↓
Timeout after 3 seconds
```

---

# 9. Messaging

### 42. What is a Message Broker?

A message broker manages communication between producers and consumers.

Examples:

* Azure Service Bus
* RabbitMQ
* Apache Kafka

---

### 43. Azure Service Bus vs Kafka?

**Azure Service Bus**

* Enterprise messaging
* Queues/topics
* Message-oriented workloads
* Dead-letter queues
* Strong Azure integration

**Kafka**

* Distributed event streaming
* Very high throughput
* Event replay
* Long-lived event streams
* Analytics/event-streaming scenarios

---

### 44. What is a Dead Letter Queue?

Messages that cannot be successfully processed after configured attempts can be moved to a **dead-letter queue** for investigation or later processing.

---

### 45. What is Message Ordering?

It means consumers process messages in a defined sequence.

Ordering is not automatically guaranteed across distributed systems; it usually requires partitioning/session or broker-specific mechanisms.

---

### 46. At-most-once vs At-least-once delivery?

**At-most-once:**

```text
0 or 1 delivery
```

Message may be lost.

**At-least-once:**

```text
1 or more deliveries
```

Duplicates are possible.

Therefore consumers should often be **idempotent**.

---

# 10. Outbox Pattern

### 47. What is the Outbox Pattern?

It solves the problem of updating a database and publishing an event reliably.

Instead of:

```text
Update DB
Publish Event
```

Use:

```text
Database Transaction
 ├── Update Business Data
 └── Insert Event into Outbox

Outbox Processor
      ↓
Message Broker
```

Both DB changes happen in the same local transaction.

---

### 48. What problem does Outbox solve?

It prevents situations such as:

```text
DB update = SUCCESS
Event publish = FAILURE
```

Without the event, other services may never know that the business operation happened.

---

# 11. .NET Microservices

### 49. How do you build Microservices using ASP.NET Core?

Typically:

```text
ASP.NET Core Web API
       ↓
Business/Application Layer
       ↓
Domain
       ↓
Repository/Data Access
       ↓
Database
```

Each service is independently deployable.

---

### 50. How do you configure Microservices in .NET?

Use:

* appsettings.json
* Environment variables
* Azure App Configuration
* Key Vault
* Options pattern

Secrets should not be hardcoded.

---

### 51. How do you implement health checks in ASP.NET Core?

Use ASP.NET Core Health Checks.

Typical endpoints:

```text
/health
/ready
```

You can check:

* Database
* External services
* Message broker
* Dependencies

---

### 52. How do you handle service-to-service authentication in .NET?

Common approaches:

* OAuth 2.0
* JWT
* Microsoft Entra ID
* Managed identities in Azure

For Azure workloads, **Managed Identity** is often preferable to storing credentials.

---

# 12. Docker

### 53. Why is Docker important for Microservices?

Docker packages the service and its dependencies into a consistent container.

```text
Microservice
    +
Runtime
    +
Dependencies
    ↓
Docker Image
    ↓
Container
```

---

### 54. What is a Docker Image?

A read-only package containing:

* Application
* Runtime/dependencies
* Configuration needed to run the application

A container is a running instance of an image.

---

### 55. Why use Docker Compose?

Docker Compose helps run multiple containers locally.

Example:

```text
Order API
Payment API
Inventory API
SQL Server
RabbitMQ
```

---

# 13. Kubernetes

### 56. Why is Kubernetes used with Microservices?

Kubernetes provides:

* Container orchestration
* Scaling
* Service discovery
* Load balancing
* Self-healing
* Rolling deployments
* Configuration management

---

### 57. What is a Pod?

A Pod is the smallest deployable unit in Kubernetes.

Usually one application container runs inside a Pod.

---

### 58. What is a Kubernetes Deployment?

A Deployment manages replicas of Pods and supports:

* Scaling
* Rolling updates
* Rollbacks
* Desired-state management

---

### 59. What is a Kubernetes Service?

A Kubernetes Service provides a stable network endpoint for accessing Pods.

```text
Client
  ↓
Kubernetes Service
  ↓
Pod
Pod
Pod
```

---

### 60. What is Ingress?

Ingress manages external HTTP/HTTPS access to services inside a Kubernetes cluster.

```text
Internet
   ↓
Ingress
   ↓
Order Service
Payment Service
```

---

### 61. What is AKS?

AKS is **Azure Kubernetes Service**, Microsoft's managed Kubernetes service.

Azure manages much of the Kubernetes control-plane infrastructure while you manage workloads and cluster configuration according to the chosen architecture.

---

# 14. Service Discovery

### 62. What is Service Discovery?

Service discovery allows one service to find another service without hardcoding its IP address.

Example:

```text
Order Service
     ↓
payment-service
     ↓
Payment Pods
```

Kubernetes Services provide service discovery inside a cluster.

---

### 63. Client-side vs Server-side Service Discovery?

**Client-side:**

Client determines which service instance to call.

**Server-side:**

Client calls a load balancer/service registry, which routes to an instance.

---

# 15. Observability

### 64. How do you monitor Microservices?

Three important pillars:

```text
Logs
Metrics
Traces
```

Tools can include:

* OpenTelemetry
* Azure Monitor
* Application Insights
* Prometheus
* Grafana

---

### 65. What is Distributed Tracing?

It tracks a request across multiple services.

```text
Angular
 ↓
API Gateway
 ↓
Order Service
 ↓
Payment Service
 ↓
Inventory Service
```

You can trace the complete request using a correlation/trace ID.

---

### 66. What is Correlation ID?

A unique identifier used to associate logs across multiple services for the same request/workflow.

Example:

```text
CorrelationId = 12345

Order Service → 12345
Payment Service → 12345
Inventory Service → 12345
```

---

# 16. Security

### 67. How do you secure Microservices?

Use:

* HTTPS
* OAuth 2.0
* OpenID Connect
* JWT
* API Gateway
* Service-to-service authentication
* Managed Identity
* Key Vault
* Network policies/firewalls

---

### 68. Where should authentication happen?

Usually authentication can be handled at the **API Gateway/edge**, but individual services should still enforce authorization and validate trusted identity information according to the security architecture.

Don't blindly trust internal traffic.

---

### 69. How do you store secrets?

Never hardcode them.

Use:

```text
Azure Key Vault
Environment/secret management
Managed Identity
Kubernetes Secrets
```

For Azure, prefer Managed Identity where possible.

---

# 17. Deployment

### 70. What is Independent Deployment?

Each microservice can be deployed without deploying the entire application.

```text
Order Service → Deploy v2

Payment Service → remains v1
Inventory → remains v3
```

---

### 71. What is Blue-Green Deployment?

Two environments:

```text
Blue → Current
Green → New
```

Deploy the new version to Green, validate it, then switch traffic.

---

### 72. What is Canary Deployment?

Release the new version to a small percentage of users first.

```text
95% → V1
5%  → V2
```

If everything is good:

```text
50% → V2
100% → V2
```

---

### 73. What is Rolling Deployment?

Gradually replace old instances with new instances.

```text
V1 V1 V1 V1

V2 V1 V1 V1

V2 V2 V1 V1

V2 V2 V2 V2
```

---

# 18. Versioning

### 74. Why is API versioning important in Microservices?

Because different consumers may not upgrade simultaneously.

Example:

```text
/api/v1/orders
/api/v2/orders
```

It provides backward compatibility during migration.

---

### 75. How do you handle breaking changes?

Options:

* API versioning
* Backward-compatible contracts
* Consumer-driven contract testing
* Gradual migration
* Deprecation period

---

# 19. Testing

### 76. How do you test Microservices?

Different levels:

```text
Unit Tests
Integration Tests
Contract Tests
Component Tests
End-to-End Tests
```

Don't rely only on E2E testing.

---

### 77. What is Contract Testing?

It verifies that the contract between a consumer and provider remains compatible.

Example:

```text
Order Service → Payment Service
```

Payment Service changes its API.

Contract testing can detect whether Order Service will break.

---

# 20. Architecture Scenarios

### 78. Design an E-Commerce Microservices Architecture.

A good answer:

```text
             Angular
                ↓
          API Gateway
                ↓
 ┌────────┬────────┬─────────┬─────────┐
 ↓        ↓        ↓         ↓
Product  Order   Payment  Customer
 Service Service  Service  Service
            ↓
       Message Broker
        ↓    ↓    ↓
   Inventory Email Analytics
```

Each service owns its data.

Use asynchronous events where immediate responses aren't required.

---

### 79. How would you design an Order Processing system?

```text
Create Order
     ↓
Order Service
     ↓
OrderCreated
     ↓
Inventory
     ↓
InventoryReserved
     ↓
Payment
     ↓
PaymentSuccessful
     ↓
OrderConfirmed
```

Use Saga for workflow coordination and an Outbox Pattern for reliable event publishing.

---

### 80. Payment succeeded but Order Service didn't receive the response. What would you do?

This is a classic interview question.

I would use:

* Idempotency
* Transaction/reference IDs
* Asynchronous events
* Retry
* Outbox
* Reconciliation process

The payment operation must not be executed twice.

---

### 81. Payment Service is down. Should Order creation fail?

**It depends on the business requirement.**

If payment is mandatory:

```text
Order → Pending Payment
```

and process asynchronously.

Or reject the order if immediate authorization is required.

The important point is to define the business consistency requirement rather than blindly failing the entire system.

---

### 82. One service is very slow. What will you do?

I would investigate:

1. Metrics
2. Logs
3. Distributed traces
4. Database performance
5. External dependencies
6. CPU/memory
7. Network latency

Then apply appropriate:

* Timeout
* Retry
* Circuit breaker
* Caching
* Scaling
* Async processing

---

### 83. One Microservice receives 10x traffic. What do you do?

Scale that service independently.

```text
Before:

Order Service
   2 replicas

After:

Order Service
   10 replicas
```

Kubernetes HPA can automate this based on metrics.

---

# 21. Advanced Architecture Questions

### 84. What is Strangler Fig Pattern?

It is a migration strategy where you gradually replace parts of a monolith with new services.

```text
Old Monolith
     ↓
New Order Service
     ↓
New Payment Service
     ↓
Remaining Monolith
```

Eventually the monolith can be removed.

---

### 85. What is Sidecar Pattern?

A supporting component runs alongside the main application container and provides cross-cutting functionality.

Examples:

* Logging
* Proxy
* Telemetry
* Security

---

### 86. What is Service Mesh?

A service mesh manages service-to-service communication.

It can provide:

* Traffic management
* mTLS
* Observability
* Retries
* Routing
* Security policies

Examples include Istio and Linkerd.

---

### 87. What is Event Sourcing?

Instead of storing only the current state, the system stores the sequence of events that produced that state.

```text
OrderCreated
PaymentReceived
ItemShipped
OrderDelivered
```

Current state can be reconstructed from events.

---

### 88. CQRS vs Event Sourcing?

They are different concepts.

**CQRS:**

Separates reads and writes.

**Event Sourcing:**

Stores state changes as events.

They can be used together, but neither requires the other.

---

# 22. Very Important Architect-Level Questions

### 89. How would you decide between Microservices and Modular Monolith?

I would consider:

* Team size
* Domain complexity
* Deployment requirements
* Scaling requirements
* Business boundaries
* Operational maturity
* Infrastructure cost

If boundaries are unclear, I would usually start with a **modular monolith** and extract services when there is a strong reason.

---

### 90. What is the biggest mistake when implementing Microservices?

Creating **distributed monoliths**.

For example:

```text
Service A
   ↓
Service B
   ↓
Service C
   ↓
Service D
```

Every request requires all services to be available.

Although technically separate services exist, the system is still tightly coupled.

---

### 91. What is a Distributed Monolith?

A system where applications are deployed separately but remain heavily dependent on each other.

Characteristics:

* Excessive synchronous calls
* Shared database
* Coordinated deployments
* Tight coupling
* One service failure affects everything

---

### 92. How do you prevent a Distributed Monolith?

Use:

* Clear service boundaries
* Database ownership
* Asynchronous communication where appropriate
* Domain events
* Independent deployments
* Loose coupling
* Contract testing

---

### 93. How do you handle backward compatibility?

Use:

* Versioned APIs
* Backward-compatible contracts
* Additive changes
* Consumer-driven contract tests
* Deprecation strategy

---

### 94. How do you handle database schema changes?

Use an **expand-and-contract** approach.

Example:

**Step 1**

Add new column.

**Step 2**

Deploy application supporting both old/new schema.

**Step 3**

Migrate data.

**Step 4**

Start using new column.

**Step 5**

Remove old column later.

This avoids breaking running service versions.

---

# 23. Scenario Questions You Should Definitely Prepare

These are especially important for **Architect/Senior Developer interviews**:

### 95. How would you migrate a monolith to Microservices?

### 96. How would you design an Order Management system?

### 97. How would you design a Payment system?

### 98. How would you handle distributed transactions?

### 99. How would you handle duplicate messages?

### 100. How would you handle message loss?

### 101. How would you handle service failure?

### 102. How would you handle cascading failures?

### 103. How would you implement retry safely?

### 104. How would you implement authentication between services?

### 105. How would you secure an API Gateway?

### 106. How would you monitor 50+ microservices?

### 107. How would you trace a request across multiple services?

### 108. How would you handle database consistency?

### 109. How would you scale one service independently?

### 110. How would you deploy microservices without downtime?

### 111. How would you roll back a failed deployment?

### 112. How would you handle a breaking API change?

### 113. How would you design microservices on Azure?

### 114. Why would you choose AKS over Azure Container Apps?

### 115. Why would you choose Azure Service Bus over Kafka?

---

# 24. Azure Microservices Architecture

For an Azure interview, be prepared to explain something like:

```text
                    Client
                      │
                      ▼
              Azure API Management
                      │
          ┌───────────┼───────────┐
          ▼           ▼           ▼
      Order API   Payment API  Product API
          │           │           │
          ▼           ▼           ▼
      Order DB    Payment DB   Product DB
          │
          ▼
    Azure Service Bus
       │       │
       ▼       ▼
 Inventory   Notification
   Service      Service
       │
       ▼
   Inventory DB
```

Supporting services:

```text
Azure Container Registry
          ↓
         AKS
          ↓
Azure Monitor / App Insights
          ↓
      OpenTelemetry

Azure Key Vault
      ↓
Secrets / Certificates

Microsoft Entra ID
      ↓
Authentication / Authorization
```

---

# 25. The 15 Questions I Would Prioritize for Your Interview

Given your **.NET + Azure + AKS + Service Bus** background, make sure you can answer these extremely well:

1. **What is Microservices architecture?**
2. **Microservices vs Monolith?**
3. **How do you identify service boundaries?**
4. **What is Database-per-Service?**
5. **How do Microservices communicate?**
6. **REST vs messaging?**
7. **What is Event-Driven Architecture?**
8. **What is Saga Pattern?**
9. **Choreography vs Orchestration?**
10. **What is Outbox Pattern?**
11. **How do you handle distributed transactions?**
12. **How do you handle failures/retries/idempotency?**
13. **How do you deploy Microservices using Docker + AKS?**
14. **How do you monitor and trace Microservices?**
15. **Design an E-Commerce/Order Management Microservices architecture.**

### One important interview strategy

Don't just memorize definitions. For **Senior/Architect interviews**, interviewers will usually move from:

> **"What is Saga?"**

to:

> **"Suppose Payment succeeds but Inventory fails. What happens?"**

and then:

> **"How would you implement that in .NET/Azure?"**

and finally:

> **"How do you make the operation idempotent if the message is delivered twice?"**

So your preparation should be at **three levels**:

**Level 1 — Definition**
What is the concept?

**Level 2 — Design**
When and why would you use it?

**Level 3 — Implementation**
How would you implement it using **ASP.NET Core + Azure Service Bus + Docker + AKS + SQL Server**?

That third level is what will distinguish an experienced Microservices candidate from someone who has only studied the terminology.
