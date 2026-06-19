# AWS AppSync Interview Notes

## 1. One-Line Answer

**AWS AppSync** is a fully managed AWS service for building secure, scalable **GraphQL APIs**, **real-time subscriptions**, and **Pub/Sub event APIs** without managing API servers.

Simple interview answer:

> AWS AppSync is a serverless GraphQL service from AWS. It lets clients query, mutate, and subscribe to data through a single API endpoint. AppSync connects to data sources like DynamoDB, Lambda, HTTP APIs, OpenSearch, RDS through Lambda, and other AWS services using resolvers. It also provides built-in authentication, authorization, caching, real-time subscriptions, monitoring, and scaling.

Memory hook:

```text
AppSync = GraphQL + Resolvers + Data Sources + Real-time + Security
```

If you remember only one line:

```text
Client asks GraphQL -> AppSync resolver runs -> data source responds -> client gets exactly requested data.
```

---

## 2. Why AWS AppSync Exists

Without AppSync, teams often build and operate their own GraphQL server:

- Host the server.
- Scale it.
- Manage WebSocket connections for subscriptions.
- Write authentication and authorization logic.
- Connect to many databases and APIs.
- Handle caching, logging, throttling, and monitoring.

AppSync removes much of that operational work.

Interview answer:

> AppSync is useful when we want a managed GraphQL API with built-in integrations to AWS data sources, real-time updates, security, and serverless scaling. Instead of running our own GraphQL server, we define a schema, connect fields to resolvers, and AppSync handles request execution and infrastructure.

---

## 3. GraphQL Basics

GraphQL is an API query language and runtime.

Main idea:

> The client asks for exactly the fields it needs, and the server returns only those fields.

Example query:

```graphql
query GetUser {
  getUser(id: "101") {
    id
    name
    email
  }
}
```

Response:

```json
{
  "data": {
    "getUser": {
      "id": "101",
      "name": "Kavita",
      "email": "kavita@example.com"
    }
  }
}
```

Why GraphQL is useful:

- Avoids over-fetching: client does not receive unnecessary fields.
- Avoids under-fetching: client can get related data in one request.
- Strongly typed schema.
- Good for mobile and frontend apps.
- One endpoint can hide multiple backend data sources.

Memory:

```text
REST = many endpoints
GraphQL = one endpoint, client selects fields
```

---

## 4. Core AppSync Components

AppSync has five core pieces:

```text
Schema -> Operations -> Resolvers -> Data Sources -> Response
```

### 1. Schema

The schema defines the API contract.

It contains:

- Types
- Queries
- Mutations
- Subscriptions
- Inputs
- Enums
- Authorization directives

Example:

```graphql
type Todo {
  id: ID!
  title: String!
  completed: Boolean!
}

type Query {
  getTodo(id: ID!): Todo
  listTodos: [Todo]
}

type Mutation {
  createTodo(title: String!): Todo
}

type Subscription {
  onCreateTodo: Todo
    @aws_subscribe(mutations: ["createTodo"])
}
```

Interview line:

> In AppSync, the schema is the contract between frontend and backend. It defines what operations clients can call and what data shape they receive.

### 2. Queries

Queries read data.

Examples:

```graphql
getUser(id: ID!): User
listOrders(userId: ID!): [Order]
```

Memory:

```text
Query = read
```

### 3. Mutations

Mutations create, update, or delete data.

Examples:

```graphql
createOrder(input: CreateOrderInput!): Order
updateProfile(input: UpdateProfileInput!): User
deleteTodo(id: ID!): Todo
```

Memory:

```text
Mutation = write/change
```

### 4. Subscriptions

Subscriptions provide real-time updates over WebSocket.

Example:

```graphql
subscription OnCreateOrder {
  onCreateOrder {
    id
    status
    total
  }
}
```

When a matching mutation happens, subscribed clients receive an update.

Memory:

```text
Subscription = real-time notification
```

### 5. Resolvers

A resolver is the logic that connects a GraphQL field to a data source.

Example:

```text
Query.getTodo -> resolver -> DynamoDB GetItem
```

Interview answer:

> A resolver tells AppSync how to fetch or write data for a specific schema field. It maps the GraphQL request to a backend data source request and maps the backend response back to GraphQL.

---

## 5. High-Level Architecture

Typical AppSync architecture:

```text
Web/Mobile Client
      |
      v
AWS AppSync GraphQL API
      |
      v
Resolver
      |
      +--> DynamoDB
      +--> Lambda
      +--> HTTP endpoint
      +--> OpenSearch
      +--> EventBridge / AWS services
      +--> None data source
```

Important point:

> AppSync can combine multiple backend systems behind one GraphQL API.

Example:

```text
User profile     -> DynamoDB
Payment details  -> Lambda
Search products  -> OpenSearch
Shipping status  -> HTTP API
```

The frontend still calls one GraphQL endpoint.

---

## 6. Data Sources

AppSync supports several data source types.

Common data sources:

| Data Source | Use Case |
|---|---|
| DynamoDB | Serverless NoSQL application data |
| Lambda | Custom business logic or unsupported integrations |
| HTTP endpoint | Call REST APIs or external services |
| OpenSearch | Search and analytics queries |
| EventBridge | Send events to event bus |
| Relational DB | Usually accessed through Lambda or RDS Data API patterns |
| None data source | Local resolver logic, mock data, or publishing events |

Interview sentence:

> AppSync resolvers can connect GraphQL fields to AWS services and external APIs, so the client does not need to know where the data actually lives.

---

## 7. Resolver Types

There are two main resolver styles:

```text
Unit resolver     = one resolver talks to one data source
Pipeline resolver = multiple functions run in sequence
```

### Unit Resolver

A unit resolver performs one operation against one data source.

Example:

```text
Query.getUser -> DynamoDB GetItem
```

Use when:

- The field maps directly to one backend call.
- Logic is simple.
- You want low latency and less complexity.

### Pipeline Resolver

A pipeline resolver runs multiple functions in order.

Example:

```text
Mutation.createOrder
   |
   v
Function 1: validate input
Function 2: check inventory
Function 3: create order in DynamoDB
Function 4: publish event
```

Use when:

- You need multi-step logic.
- You need to call multiple data sources.
- You want reusable resolver functions.
- You want authorization, validation, enrichment, and writes in sequence.

Interview answer:

> Unit resolvers are simple one-step resolvers. Pipeline resolvers are used when an operation needs multiple steps or multiple data sources. Pipeline functions run in sequence and can pass context to each other.

---

## 8. Resolver Runtime: VTL and JavaScript

AppSync resolvers can be written using:

- **VTL**, Velocity Template Language
- **AppSync JavaScript runtime**

### VTL

VTL is the older mapping-template style.

It has:

- Request mapping template
- Response mapping template

Flow:

```text
GraphQL request
   |
Request mapping template
   |
Data source
   |
Response mapping template
   |
GraphQL response
```

### JavaScript Resolvers

JavaScript resolvers let you write resolver logic in JavaScript.

They are often easier to read and maintain than VTL.

Interview answer:

> AppSync resolvers transform GraphQL requests into data source operations. Historically this was done with VTL mapping templates, and AppSync also supports JavaScript resolvers, which can make resolver logic easier to maintain.

---

## 9. Resolver Context Object

Resolvers receive a context object.

Important things inside context:

| Context Field | Meaning |
|---|---|
| `arguments` | GraphQL arguments passed by client |
| `identity` | Authenticated user information |
| `source` | Parent object in nested resolver |
| `result` | Result from previous step or data source |
| `stash` | Temporary storage across pipeline functions |
| `request` | Request metadata |
| `info` | GraphQL field and selection info |

Memory:

```text
args = what client sent
identity = who client is
source = parent object
stash = pass data between pipeline steps
```

---

## 10. AppSync Request Flow

For a query:

```text
1. Client sends GraphQL query.
2. AppSync authenticates the request.
3. AppSync validates the query against the schema.
4. AppSync finds the resolver for the requested field.
5. Resolver creates a data source request.
6. Data source returns result.
7. Resolver maps result to GraphQL response.
8. AppSync returns only the requested fields.
```

For a mutation with subscription:

```text
1. Client A subscribes to onCreateMessage.
2. Client B calls createMessage mutation.
3. Mutation resolver writes message.
4. AppSync publishes result to matching subscribers.
5. Client A receives real-time update.
```

---

## 11. Authentication and Authorization

AppSync supports multiple authorization modes.

Common modes:

| Auth Mode | Best For |
|---|---|
| API Key | Development, public read-only APIs, simple temporary access |
| IAM | AWS service-to-service access, backend access, signed requests |
| Amazon Cognito User Pools | User login for web/mobile apps |
| OIDC | External identity providers |
| Lambda authorizer | Custom authorization logic |

### API Key

Simple but limited.

Use for:

- Development
- Public read-only APIs
- Temporary access

Do not use for:

- Sensitive user data
- Fine-grained production authorization

### IAM

Uses AWS Signature Version 4.

Use for:

- Backend services
- Lambda calling AppSync
- Cross-service AWS access
- Admin operations

### Cognito User Pools

Common for user-facing apps.

Use for:

- User login
- JWT-based authentication
- Group-based authorization

### OIDC

Use when authentication comes from external identity providers.

Examples:

- Auth0
- Okta
- Azure AD

### Lambda Authorizer

Use when authorization rules are custom.

Examples:

- Validate custom token.
- Call internal auth service.
- Apply tenant-specific rules.

Interview answer:

> AppSync supports API Key, IAM, Cognito User Pools, OIDC, and Lambda authorizers. In production, Cognito or OIDC is common for user apps, IAM is common for backend services, and Lambda authorizer is used for custom authorization logic.

---

## 12. Field-Level Authorization

AppSync can apply authorization rules at type or field level using schema directives.

Example idea:

```graphql
type Query {
  publicProducts: [Product] @aws_api_key
  myOrders: [Order] @aws_cognito_user_pools
  adminReports: [Report] @aws_iam
}
```

Why it matters:

- Different clients can access different operations.
- Sensitive fields can be protected.
- Public and private data can exist in the same API.

Interview sentence:

> AppSync authorization is not only API-level. We can also restrict specific types and fields using authorization directives and resolver logic.

---

## 13. Subscriptions and Real-Time

AppSync GraphQL subscriptions provide real-time updates using WebSockets.

Common use cases:

- Chat apps
- Live order status
- Notifications
- Collaborative apps
- Dashboards
- IoT status updates

Typical flow:

```text
Client subscribes
   |
Mutation happens
   |
AppSync pushes update to subscribed clients
```

Example:

```graphql
type Mutation {
  updateOrderStatus(id: ID!, status: String!): Order
}

type Subscription {
  onUpdateOrderStatus(id: ID!): Order
    @aws_subscribe(mutations: ["updateOrderStatus"])
}
```

Interview answer:

> AppSync subscriptions are used for real-time updates. A client opens a WebSocket subscription, and when a related mutation succeeds, AppSync pushes the mutation result to subscribed clients.

Important trap:

> AppSync GraphQL subscriptions are usually tied to mutations. If you need general Pub/Sub event broadcasting independent of GraphQL mutations, AWS AppSync Events can be a better fit.

---

## 14. AWS AppSync Events

AWS AppSync Events is used for serverless WebSocket Pub/Sub event APIs.

Use it for:

- Live chat
- Notifications
- Live comments
- Score updates
- Activity feeds
- Broadcast events to many subscribers

Difference from GraphQL subscriptions:

| GraphQL Subscriptions | AppSync Events |
|---|---|
| Part of GraphQL API | Event API for Pub/Sub |
| Often triggered by mutations | Publish/subscribe event channels |
| Good when real-time data is linked to GraphQL schema | Good for general event broadcasting |

Interview sentence:

> AppSync GraphQL subscriptions are schema-driven real-time updates, while AppSync Events is more like managed WebSocket Pub/Sub for event broadcasting.

---

## 15. Caching

AppSync supports caching to improve latency and reduce calls to data sources.

Caching can help when:

- Same query is called repeatedly.
- Data does not change every second.
- Backend data source is expensive or slow.
- Read traffic is high.

Cache can be configured with TTL.

Interview answer:

> AppSync caching stores resolver or request results for a configured time, reducing latency and load on backend data sources. It is best for read-heavy data that can tolerate short-lived staleness.

Caching caution:

```text
Do not cache user-specific sensitive data unless cache keys include identity/user context correctly.
```

---

## 16. Error Handling

GraphQL responses can contain both `data` and `errors`.

Example:

```json
{
  "data": {
    "getOrder": null
  },
  "errors": [
    {
      "message": "Order not found"
    }
  ]
}
```

Important:

- GraphQL can return partial data.
- Resolver errors should be meaningful but not leak sensitive details.
- Use validation before writes.
- Handle conditional check failures from DynamoDB carefully.

Interview sentence:

> In GraphQL, errors can be returned alongside partial data, so clients must check both the data and errors fields.

---

## 17. AppSync with DynamoDB

AppSync and DynamoDB are a common serverless pair.

Architecture:

```text
Frontend -> AppSync -> Resolver -> DynamoDB
```

Good for:

- Mobile apps
- Todo apps
- Chat metadata
- User profiles
- Orders
- Serverless CRUD APIs

Example resolver mapping:

```text
Query.getTodo(id)       -> DynamoDB GetItem
Query.listTodos         -> DynamoDB Query or Scan
Mutation.createTodo     -> DynamoDB PutItem
Mutation.updateTodo     -> DynamoDB UpdateItem
Mutation.deleteTodo     -> DynamoDB DeleteItem
```

Important DynamoDB interview points:

- Prefer `Query` over `Scan`.
- Design partition keys based on access patterns.
- Use conditional writes to prevent accidental overwrite.
- Use pagination for list operations.
- Handle hot partitions.

Interview sentence:

> With DynamoDB, AppSync can create a fully serverless API. The most important design work is modeling DynamoDB keys around GraphQL access patterns.

---

## 18. AppSync with Lambda

Use Lambda as a data source when:

- Business logic is complex.
- You need to call multiple services.
- You need to access RDS or third-party APIs.
- Resolver logic is too large for direct resolvers.
- You need custom validation or orchestration.

Architecture:

```text
Client -> AppSync -> Lambda resolver -> database/API/service
```

Tradeoff:

| Direct Resolver | Lambda Resolver |
|---|---|
| Lower latency | More flexible |
| Less code | More business logic |
| Good for simple DynamoDB operations | Good for complex workflows |
| Fewer moving parts | Extra Lambda cost/cold starts |

Interview answer:

> I use direct resolvers for simple operations like DynamoDB reads and writes. I use Lambda resolvers when the operation needs custom business logic, multiple service calls, or integrations that are not easily handled directly in AppSync.

---

## 19. AppSync with HTTP APIs

AppSync can call HTTP endpoints.

Use cases:

- Existing REST API
- Microservice endpoint
- Third-party service
- API Gateway endpoint

Architecture:

```text
GraphQL field -> HTTP resolver -> REST endpoint
```

Interview sentence:

> AppSync can wrap existing REST services with a GraphQL API, allowing clients to use GraphQL while backend services remain unchanged.

---

## 20. Merged APIs

Merged APIs allow multiple AppSync source APIs to be combined into one AppSync endpoint.

Use when:

- Multiple teams own different domains.
- Each team wants to deploy independently.
- Frontend wants one unified GraphQL API.
- Organization wants a shared graph.

Example:

```text
User API      \
Order API      -> Merged AppSync API -> Frontend
Inventory API /
```

Interview answer:

> AppSync Merged APIs help with team ownership and schema composition. Different teams can maintain separate source APIs, and AppSync can merge them into one GraphQL endpoint for clients.

---

## 21. Offline and Mobile Support

AppSync is often used with AWS Amplify for frontend and mobile development.

Amplify/AppSync can support:

- GraphQL client generation
- Authentication integration
- Real-time subscriptions
- Optimistic UI
- Offline-first patterns, depending on client setup

Interview sentence:

> AppSync is popular for mobile apps because GraphQL reduces network round trips, subscriptions provide real-time updates, and Amplify can simplify client integration.

---

## 22. Security Best Practices

Security checklist:

1. Choose the right auth mode.
2. Avoid API keys for sensitive production data.
3. Use least privilege IAM roles for data sources.
4. Apply field-level authorization where needed.
5. Validate user identity in resolvers.
6. Protect tenant-specific data using identity and metadata checks.
7. Use AWS WAF for public APIs if needed.
8. Use CloudWatch logs carefully and avoid logging secrets.
9. Use CloudTrail for audit visibility.
10. Encrypt backend data sources such as DynamoDB, OpenSearch, and S3.

Important interview line:

> Authentication proves who the user is. Authorization decides what that user can access. In AppSync, both schema directives and resolver logic can enforce authorization.

Multi-tenant warning:

```text
Never trust tenantId only because the client sent it.
Compare tenantId with ctx.identity claims or server-side authorization data.
```

---

## 23. Monitoring and Observability

AppSync can integrate with:

- Amazon CloudWatch metrics
- CloudWatch logs
- AWS X-Ray tracing
- AWS CloudTrail

Monitor:

- Request count
- Latency
- Error rate
- Resolver errors
- 4xx and 5xx responses
- Cache hit ratio
- Data source latency
- Subscription connection behavior
- Throttling

Interview answer:

> For production AppSync APIs, I monitor latency, resolver errors, data source errors, request volume, throttling, cache behavior, and logs/traces using CloudWatch, X-Ray, and CloudTrail.

---

## 24. Performance Best Practices

Performance checklist:

- Use direct DynamoDB resolvers for simple CRUD.
- Avoid unnecessary Lambda calls.
- Use pagination for list queries.
- Avoid DynamoDB scans for large tables.
- Design schema around client access patterns.
- Use caching for read-heavy stable data.
- Use batching where possible.
- Avoid deeply nested expensive queries.
- Limit query depth/complexity using app-level controls and schema design.
- Keep resolver logic small and focused.

Interview sentence:

> AppSync scales automatically, but performance still depends on resolver design, data source design, caching, pagination, and avoiding expensive nested queries.

---

## 25. Cost Factors

AppSync cost can depend on:

- Number of GraphQL operations.
- Real-time subscription messages and connection minutes.
- AppSync Events usage.
- Caching instance usage.
- Data source costs, such as DynamoDB, Lambda, OpenSearch, or HTTP backend.
- Logging and monitoring volume.

Interview answer:

> AppSync itself is serverless, but cost depends on API request volume, real-time usage, caching, and the backend services called by resolvers.

---

## 26. AppSync vs API Gateway

| AppSync | API Gateway |
|---|---|
| Managed GraphQL and Pub/Sub APIs | Managed REST, HTTP, and WebSocket APIs |
| Schema-driven | Route/resource-driven |
| Client asks for exact fields | Endpoint returns predefined response |
| Built-in GraphQL subscriptions | WebSocket APIs available separately |
| Good for frontend data aggregation | Good for REST/HTTP microservices |
| Resolvers connect fields to data sources | Routes connect methods to integrations |

Interview answer:

> I choose AppSync when the client benefits from GraphQL, data aggregation, typed schema, and real-time subscriptions. I choose API Gateway when I need REST/HTTP APIs, simple service endpoints, or non-GraphQL routing.

---

## 27. AppSync vs Running Your Own GraphQL Server

| AppSync | Self-Hosted GraphQL |
|---|---|
| Managed/serverless | You manage servers or containers |
| Built-in AWS integrations | Full custom control |
| Built-in auth modes | You implement auth integration |
| Built-in subscriptions | You manage WebSocket scaling |
| Less ops work | More flexibility |
| AWS-specific patterns | Portable across clouds if designed that way |

Interview answer:

> AppSync is better when we want managed GraphQL with AWS integrations and less operational burden. A self-hosted GraphQL server is better when we need full runtime control, custom plugins, or portability outside AWS.

---

## 28. AppSync vs REST

| GraphQL/AppSync | REST |
|---|---|
| One endpoint | Multiple endpoints |
| Client selects fields | Server defines response shape |
| Strong schema | Often OpenAPI/schema optional |
| Good for complex frontend data needs | Simple and widely understood |
| Can reduce round trips | Easy caching with HTTP semantics |
| Query complexity must be controlled | Endpoint behavior is more fixed |

Interview answer:

> GraphQL is useful when clients need flexible data shapes or data from multiple sources. REST is simpler for resource-based APIs with predictable responses. AppSync gives managed GraphQL on AWS.

---

## 29. Common Architecture Patterns

### Pattern 1: Serverless CRUD App

```text
React/Angular/Mobile -> AppSync -> DynamoDB
Auth -> Cognito
Monitoring -> CloudWatch
```

Use for:

- Todo app
- User profiles
- Orders
- Simple dashboards

### Pattern 2: GraphQL API with Business Logic

```text
Frontend -> AppSync -> Lambda -> RDS / third-party API / internal services
```

Use for:

- Complex validations
- Payment workflows
- RDS access
- Multi-service orchestration

### Pattern 3: Real-Time Chat

```text
Client sends message mutation
   |
AppSync writes message
   |
AppSync subscription pushes message to room members
```

Use:

- Cognito for users
- DynamoDB for messages
- Subscriptions for real-time delivery

### Pattern 4: Existing REST APIs Wrapped by GraphQL

```text
Frontend -> AppSync -> HTTP resolvers -> existing REST services
```

Use when:

- You want GraphQL without rewriting all backends.
- You need one frontend API over many microservices.

### Pattern 5: Multi-Team GraphQL

```text
Team A Source API
Team B Source API
Team C Source API
       |
       v
Merged AppSync API
       |
       v
Frontend
```

Use when:

- Domains are owned by separate teams.
- Frontend wants one endpoint.

---

## 30. Common Interview Questions and Answers

### Q1. What is AWS AppSync?

AWS AppSync is a fully managed service for building GraphQL APIs and real-time Pub/Sub APIs. It connects clients to data sources such as DynamoDB, Lambda, HTTP APIs, and OpenSearch using resolvers, while handling scaling, authentication, authorization, caching, subscriptions, and monitoring.

### Q2. What problem does AppSync solve?

It removes the need to run and scale your own GraphQL server. It also simplifies connecting frontend apps to multiple backend data sources through one secure, typed API.

### Q3. What is a resolver in AppSync?

A resolver is the function or mapping logic that connects a GraphQL field to a data source. It converts the GraphQL request into a backend request and converts the backend response into a GraphQL response.

### Q4. What is the difference between a unit resolver and a pipeline resolver?

A unit resolver performs one operation against one data source. A pipeline resolver runs multiple functions in sequence and is used for multi-step workflows or multiple data sources.

### Q5. What are AppSync data sources?

Data sources are backends that AppSync resolvers call, such as DynamoDB, Lambda, HTTP endpoints, OpenSearch, EventBridge, or a None data source.

### Q6. What are the main GraphQL operation types?

Query reads data, Mutation writes or changes data, and Subscription receives real-time updates.

### Q7. How do AppSync subscriptions work?

Clients open a WebSocket subscription. When a related mutation succeeds, AppSync pushes the mutation result to subscribed clients.

### Q8. What authorization modes does AppSync support?

AppSync supports API Key, IAM, Amazon Cognito User Pools, OIDC, and Lambda authorizers.

### Q9. When would you use Cognito with AppSync?

Use Cognito when building a web or mobile app where users need to sign in and access data based on user identity or groups.

### Q10. When would you use IAM authorization?

Use IAM when AWS services, backend systems, or trusted roles need to call the AppSync API using signed AWS requests.

### Q11. When would you use Lambda authorizer?

Use Lambda authorizer when authorization logic is custom, such as validating a custom token, calling an internal identity system, or applying tenant-specific rules.

### Q12. When should you use Lambda as a resolver?

Use Lambda when the operation needs complex business logic, multiple service calls, RDS access, third-party APIs, or logic that is difficult to express in direct resolvers.

### Q13. What is the benefit of direct DynamoDB resolvers?

They avoid Lambda overhead, reduce latency, reduce cost, and keep the architecture fully serverless for simple CRUD operations.

### Q14. How do you secure an AppSync API?

Use proper auth mode, least privilege IAM roles, field-level authorization, resolver-level identity checks, WAF if needed, CloudWatch/CloudTrail monitoring, and careful logging.

### Q15. How is AppSync different from API Gateway?

AppSync is mainly for GraphQL and real-time data APIs. API Gateway is for REST, HTTP, and WebSocket APIs. AppSync is schema and resolver based; API Gateway is route and integration based.

### Q16. How is AppSync different from REST?

REST exposes multiple endpoints with fixed response shapes. GraphQL through AppSync exposes one endpoint where clients request exactly the fields they need.

### Q17. What is over-fetching and under-fetching?

Over-fetching means the API returns more data than needed. Under-fetching means the client must call multiple APIs to get all required data. GraphQL helps reduce both.

### Q18. What is AppSync caching?

AppSync caching stores API or resolver responses for a configured TTL to reduce latency and backend load. It is useful for read-heavy data that can tolerate temporary staleness.

### Q19. What are Merged APIs?

Merged APIs combine multiple AppSync source APIs into one AppSync endpoint. They are useful when multiple teams own different parts of a larger GraphQL API.

### Q20. What should you monitor in AppSync?

Monitor request count, latency, resolver errors, data source errors, cache hit ratio, throttling, subscription metrics, CloudWatch logs, X-Ray traces, and CloudTrail audit events.

### Q21. What is a None data source?

A None data source lets a resolver run without calling an external backend. It is useful for local logic, mock responses, or some real-time publishing patterns.

### Q22. Can AppSync connect to RDS?

Yes, commonly through Lambda or supported data API patterns. Lambda is often used when complex SQL, transactions, or custom RDS access is required.

### Q23. What is the biggest risk in GraphQL APIs?

Clients can request deeply nested or expensive queries if the schema is not designed carefully. Production systems should control query complexity, authorization, pagination, and resolver cost.

### Q24. How do you handle multi-tenant authorization?

Use authenticated identity claims and server-side checks. Do not trust tenant IDs from client input alone. Resolver logic should ensure users can access only their tenant's data.

### Q25. When should you not use AppSync?

Avoid AppSync if the API is a very simple REST API, if the team does not need GraphQL, if strict portability outside AWS is required, or if you need full control over a custom GraphQL runtime.

---

## 31. Scenario-Based Answers

### Scenario 1: Build a todo app

Recommended architecture:

```text
Frontend -> AppSync -> DynamoDB
Auth -> Cognito
Real-time -> Subscriptions
```

Answer:

> I would define Todo types, queries, mutations, and subscriptions in the GraphQL schema. AppSync resolvers would map create, update, delete, get, and list operations to DynamoDB. Cognito would authenticate users, and subscriptions would push real-time updates when todos change.

### Scenario 2: Build a chat app

Architecture:

```text
Client -> AppSync mutation -> DynamoDB
Client <- AppSync subscription <- new message
```

Mention:

- Cognito for users.
- DynamoDB for messages.
- Subscriptions for real-time delivery.
- Authorization to ensure only room members receive messages.

### Scenario 3: Build order dashboard with live status

Architecture:

```text
Frontend -> AppSync -> DynamoDB / Lambda
Order status changes -> Mutation/Event -> Subscription -> Dashboard
```

Mention:

- Query for initial order list.
- Subscription for live changes.
- Cache stable reference data.
- Use identity/group checks for access.

### Scenario 4: Wrap existing microservices

Architecture:

```text
Frontend -> AppSync -> HTTP resolvers / Lambda -> REST microservices
```

Answer:

> AppSync can provide one GraphQL API over multiple existing REST services. This lets the frontend fetch exactly what it needs without calling each microservice separately.

### Scenario 5: Multi-team enterprise GraphQL

Architecture:

```text
Customer API + Order API + Inventory API -> Merged AppSync API
```

Answer:

> I would let each team own its source AppSync API and combine them using a Merged API. This gives frontend teams a single endpoint while backend teams deploy independently.

---

## 32. Common Traps Interviewers Ask

| Trap Question | Best Answer |
|---|---|
| Is AppSync only for DynamoDB? | No, it supports multiple data sources like Lambda, HTTP, OpenSearch, EventBridge, and more. |
| Is GraphQL always better than REST? | No, GraphQL is better for flexible client-driven data; REST can be simpler for fixed resources. |
| Are subscriptions the same as polling? | No, subscriptions push updates over WebSocket. |
| Should API keys be used for secure production user data? | Usually no. Use Cognito, OIDC, IAM, or Lambda authorizer. |
| Does AppSync remove the need for backend design? | No, schema, resolver, data model, and auth design are still critical. |
| Can a GraphQL response include partial data and errors? | Yes. Clients should check both `data` and `errors`. |
| Is Lambda always required? | No. Direct resolvers can call data sources like DynamoDB without Lambda. |
| Does AppSync automatically solve multi-tenant security? | No. You must enforce tenant authorization in schema and resolver logic. |
| Are GraphQL subscriptions always the best for all events? | Not always. AppSync Events may be better for general Pub/Sub event APIs. |

---

## 33. Two-Minute Interview Explanation

Use this when the interviewer says: "Explain AWS AppSync."

> AWS AppSync is a fully managed AWS service for building GraphQL APIs and real-time APIs. In AppSync, we define a GraphQL schema with queries, mutations, and subscriptions. Queries are for reading data, mutations are for changing data, and subscriptions are for real-time updates. Each schema field can be connected to a backend using a resolver. A resolver maps the GraphQL request to a data source like DynamoDB, Lambda, HTTP APIs, OpenSearch, EventBridge, or a None data source.
>
> The main benefit is that clients get one typed API endpoint and can request exactly the fields they need, while AppSync handles scaling, authentication, authorization, caching, logging, monitoring, and WebSocket subscriptions. For simple serverless apps, AppSync with DynamoDB is common because direct resolvers can read and write data without Lambda. For complex business logic, we can use Lambda resolvers or pipeline resolvers.
>
> For security, AppSync supports API Key, IAM, Cognito User Pools, OIDC, and Lambda authorizers. We can also do field-level authorization and resolver-level identity checks. In production, I would focus on least privilege data source roles, correct tenant authorization, pagination, caching where appropriate, monitoring with CloudWatch and X-Ray, and avoiding expensive nested GraphQL queries.

---

## 34. Memory Map

Remember AppSync using:

```text
S R D A C M
```

| Letter | Meaning |
|---|---|
| S | Schema |
| R | Resolvers |
| D | Data sources |
| A | Authorization |
| C | Caching / CloudWatch |
| M | Mutations, subscriptions, merged APIs |

Another memory hook:

```text
QMS
Query = Read
Mutation = Write
Subscription = Real-time
```

Resolver memory:

```text
Resolver = Translator
GraphQL language <-> Backend language
```

Data source memory:

```text
D L H O N
DynamoDB, Lambda, HTTP, OpenSearch, None
```

---

## 35. Quick Revision Sheet

Memorize these lines:

- AppSync is managed GraphQL and real-time API service.
- GraphQL uses one endpoint and lets clients request exact fields.
- Schema defines the contract.
- Query reads data.
- Mutation changes data.
- Subscription sends real-time updates.
- Resolver connects a GraphQL field to a data source.
- Unit resolver is one step.
- Pipeline resolver is multiple steps.
- Data sources include DynamoDB, Lambda, HTTP, OpenSearch, EventBridge, and None.
- Direct DynamoDB resolvers are fast and serverless.
- Lambda resolvers are best for complex logic.
- Auth modes are API Key, IAM, Cognito, OIDC, and Lambda authorizer.
- Use Cognito/OIDC for user apps.
- Use IAM for AWS service access.
- Use Lambda authorizer for custom auth.
- AppSync caching improves read latency but must be used carefully with user-specific data.
- Subscriptions use WebSockets for real-time updates.
- AppSync Events is for managed Pub/Sub event APIs.
- Merged APIs combine multiple AppSync APIs into one endpoint.
- Monitor latency, errors, resolver behavior, data source issues, and throttling.
- Production AppSync requires strong schema design, auth, pagination, and resolver optimization.

---

## 36. Best Final Answer Template

For any AppSync interview question, answer in this format:

```text
1. Define the concept.
2. Explain where it fits in AppSync.
3. Give one practical use case.
4. Mention security, performance, or tradeoff.
```

Example:

> A resolver is the AppSync component that connects a GraphQL field to a backend data source. For example, `Query.getOrder` can use a resolver to call DynamoDB and fetch an order by ID. Resolvers can be unit resolvers for simple calls or pipeline resolvers for multi-step logic. In production, I would also include authorization checks and avoid expensive backend operations.

---

## 37. Official References

- AWS AppSync overview: https://docs.aws.amazon.com/appsync/latest/devguide/what-is-appsync.html
- AWS AppSync GraphQL developer guide: https://docs.aws.amazon.com/appsync/latest/devguide/designing-a-graphql-api.html
- AppSync data sources: https://docs.aws.amazon.com/appsync/latest/devguide/data-source-components.html
- AppSync resolvers: https://docs.aws.amazon.com/appsync/latest/devguide/resolver-components.html
- AppSync authorization: https://docs.aws.amazon.com/appsync/latest/devguide/security-authz.html
- AppSync subscriptions: https://docs.aws.amazon.com/appsync/latest/devguide/aws-appsync-real-time-data.html
- AppSync caching: https://docs.aws.amazon.com/appsync/latest/devguide/enabling-caching.html
- AppSync Merged APIs: https://docs.aws.amazon.com/appsync/latest/devguide/merged-api.html
- AWS AppSync Events: https://docs.aws.amazon.com/appsync/latest/eventapi/event-api-welcome.html
