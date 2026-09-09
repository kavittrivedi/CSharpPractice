Absolutely. Since MCP is a current standard and has evolved, I checked the latest official specification. The current **2026-07-28 MCP specification** defines MCP as an open protocol for connecting AI applications to tools, resources, and prompts; it also has current authorization/security mechanisms. ([Model Context Protocol Blog][1])

Below are **simple, interview-ready answers**.

---

# 1. What is MCP?

**MCP = Model Context Protocol.**

It is an **open standard/protocol that allows an AI application to connect to external data and capabilities in a standardized way**.

Instead of building a custom integration for every AI application, we can expose capabilities through an MCP server.

```text
                AI Application
                      |
                  MCP Client
                      |
                MCP Protocol
                      |
                MCP Server
                 /    |    \
                /     |     \
             DB     API    Files
```

For example, an MCP server could expose:

```text
searchCustomer()
getOrder()
createTicket()
searchDocuments()
```

### Interview answer

> **"MCP is an open protocol that standardizes how AI applications connect to external tools, data and capabilities. It provides a common way for an AI client to discover and use tools, resources and prompts exposed by an MCP server."**

([MCP TypeScript SDK][2])

---

# 2. Why was MCP introduced?

Before MCP, every AI application could implement integrations differently.

For example:

```text
AI App → Custom DB integration
AI App → Custom GitHub integration
AI App → Custom Jira integration
AI App → Custom File integration
```

This creates a lot of duplicated integration code.

MCP provides a **common protocol**:

```text
                 MCP
                  |
       +----------+----------+
       |          |          |
      DB         Jira      GitHub
```

An MCP-compatible client can connect to MCP servers using the same protocol.

### Interview answer

> **"MCP was introduced to standardize the connection between AI applications and external tools and data. It reduces the need to build a separate custom integration for every AI application and every system."**

---

# 3. What problem does MCP solve?

The main problem is:

> **How can AI applications consistently discover and use external capabilities?**

Without MCP:

```text
AI App
   |
Custom integration
   |
Database/API/File system
```

With MCP:

```text
AI App
   |
MCP Client
   |
MCP Server
   |
Tool / Resource
```

It gives us a standard interface for things such as:

* Tool discovery
* Tool invocation
* Resource access
* Prompt templates
* Protocol communication
* Authorization mechanisms

### Simple example

Suppose you build a customer-support AI.

Without MCP, you might create custom code for:

```text
CRM
Ticketing System
Order System
Knowledge Base
```

With MCP, these capabilities can be exposed through MCP servers.

---

# 4. What are MCP Clients and Servers?

This is very important.

### MCP Client

The **client is the component inside/alongside the AI application that connects to an MCP server**.

```text
AI Application
      |
  MCP Client
      |
      v
MCP Server
```

Examples of hosts/applications can connect to MCP servers through MCP client implementations. Official SDKs provide client and server APIs. ([MCP TypeScript SDK][3])

### MCP Server

The MCP server exposes capabilities to the client.

For example:

```text
Customer MCP Server

Tools:
  getCustomer
  createTicket

Resources:
  customer://123
  policy://refund

Prompts:
  customer-support-response
```

### Interview answer

> **"The MCP client is the component that connects an AI application to MCP servers. The MCP server exposes capabilities such as tools, resources and prompts that the client can discover and use."**

---

# 5. What are MCP Tools?

**Tools are executable actions.**

For example:

```text
searchOrders()
getCustomer()
createTicket()
sendEmail()
calculateTax()
```

Think:

> **Tool = Do something**

Example:

```text
Tool:
getOrderStatus

Input:
OrderId = 12345

Output:
Shipped
```

The model can decide that it needs the tool, and the MCP client invokes it through the MCP protocol.

### Important

Tools can potentially perform **side effects**.

For example:

```text
createOrder()
deleteCustomer()
sendEmail()
```

So they need strong authorization and validation.

### Interview answer

> **"MCP tools represent executable operations. For example, an MCP server could expose a getOrderStatus or createTicket tool. Because tools can perform actions or side effects, they need proper authorization and validation."**

---

# 6. What are MCP Resources?

Resources represent **data/context that can be read or accessed**.

Think:

> **Resource = Give me information**

Examples:

```text
customer://123
file://company/policy.pdf
database://schema
documentation://api
```

Tools:

```text
getCustomer()
createTicket()
```

Resources:

```text
customer://123
policy://refund
```

### Easy difference

| MCP Concept | Think                             |
| ----------- | --------------------------------- |
| Tool        | **Do something**                  |
| Resource    | **Read/access something**         |
| Prompt      | **Reusable instruction/template** |

MCP servers can expose resources and clients can read them; the current SDK documentation lists resources as a server capability. ([MCP Go SDK][4])

---

# 7. What are MCP Prompts?

Prompts are **reusable prompt templates/instructions exposed by an MCP server**.

For example:

```text
Prompt:
"Analyze this customer's support history and
prepare a response."
```

It can have parameters:

```text
customerId
issueType
language
```

The client can retrieve the prompt and provide the required arguments.

Think:

> **Prompt = Reusable instruction/template**

### Interview answer

> **"MCP prompts are reusable prompt templates exposed by an MCP server. They help standardize how a particular task should be presented to the model."**

---

# 8. How does an MCP client discover tools?

The client connects to the MCP server and can request the server's available capabilities, including its tool catalog.

Conceptually:

```text
Client
   |
   | "What tools do you provide?"
   |
   v
MCP Server
   |
   | tools/list
   v
[
  searchCustomer,
  getOrder,
  createTicket
]
```

The server provides information such as:

```text
Tool name
Description
Input schema
Output information
```

The client can then make those tools available to the model.

The current MCP specification also supports cacheable list results for `tools/list`, `prompts/list`, and `resources/list`. ([Model Context Protocol Blog][1])

### Interview answer

> **"The MCP client discovers the server's capabilities through the MCP protocol. It can list the available tools and their schemas, and then expose those tools to the model so the model knows what operations are available."**

---

# 9. How does an LLM invoke an MCP tool?

The LLM normally **doesn't directly connect to the MCP server**.

The flow is:

```text
User
  |
  v
LLM
  |
  | "I need order status"
  v
MCP Client
  |
  | tools/call
  v
MCP Server
  |
  v
Order API / Database
  |
  v
Result
  |
  v
MCP Client
  |
  v
LLM
  |
  v
User
```

For example:

```text
LLM decides:

Tool = getOrderStatus
Arguments:
{
    "orderId": "12345"
}
```

The MCP client sends the tool call to the MCP server.

### Important interview point

**The LLM decides what tool is needed; the MCP client handles the protocol communication.**

---

# 10. MCP vs REST API

This is a common interview question.

| REST API                               | MCP                                                  |
| -------------------------------------- | ---------------------------------------------------- |
| General application-to-application API | Designed for AI application ↔ capability integration |
| Developer defines endpoints            | Server exposes tools/resources/prompts               |
| Client usually knows API contract      | MCP supports capability discovery                    |
| Example: `GET /orders/123`             | Example: `getOrderStatus` tool                       |
| Primarily application API              | AI-oriented integration protocol                     |

### Simple example

REST:

```text
GET /api/orders/123
```

MCP:

```text
getOrderStatus
{
   "orderId": "123"
}
```

### Important

MCP **doesn't replace REST**.

An MCP server can actually call REST APIs internally.

```text
LLM
 ↓
MCP Client
 ↓
MCP Server
 ↓
REST API
 ↓
Database
```

### Interview answer

> **"REST is a general-purpose API style, while MCP is a protocol designed to standardize how AI applications discover and interact with tools and context. An MCP server can internally call REST APIs."**

---

# 11. MCP vs Function Calling

These are related but **not the same thing**.

### Function calling

Function calling allows an LLM to say:

```text
I want to call:

getWeather({
   "city": "Ahmedabad"
})
```

The application then executes that function.

```text
LLM
 |
 | Function call
 v
Application
 |
 v
Function
```

### MCP

MCP standardizes how tools are **exposed, discovered and invoked across AI applications and servers**.

```text
LLM
 |
MCP Client
 |
MCP Server
 |
Tool
```

### Easy difference

> **Function calling = model mechanism for requesting a function.**

> **MCP = standardized protocol for connecting AI applications to external capabilities.**

You can use function/tool calling as part of an MCP-based architecture.

---

# 12. MCP vs Plugins

Conceptually:

**Plugins** are usually tied to a particular platform/ecosystem and its plugin mechanism.

MCP is intended as an **open, standardized protocol** that different compatible AI hosts/clients can use.

For example:

```text
Plugin approach

AI Platform
   |
   +-- Plugin A
   +-- Plugin B
```

MCP:

```text
MCP-compatible host
       |
    MCP Client
       |
  +----+----+
  |         |
MCP       MCP
Server    Server
```

### Interview answer

> **"A plugin mechanism is usually specific to a particular AI platform or ecosystem. MCP is an open protocol intended to standardize how AI applications connect to external tools and resources across compatible clients and servers."**

---

# 13. Can an MCP server expose database operations?

**Yes.**

For example:

```text
Database MCP Server

Tools:
  getCustomer()
  searchOrders()
  getSalesSummary()

Resources:
  database://schema
```

Architecture:

```text
LLM
 |
MCP Client
 |
MCP Server
 |
Database
```

But I would **not expose arbitrary SQL execution** to the model.

Bad:

```text
executeAnySql(sql)
```

Better:

```text
getCustomer(customerId)
getOrders(customerId)
getSalesSummary(fromDate, toDate)
```

Why?

Because it gives us:

* Better security
* Better validation
* Better authorization
* Less risk of destructive operations
* More predictable behavior

### Strong interview point

> **"Yes, but I would expose controlled business operations rather than giving the LLM unrestricted database or SQL access."**

---

# 14. How would you secure an MCP server?

I would use multiple layers:

```text
Authentication
      ↓
Authorization
      ↓
Input Validation
      ↓
Tool-level permissions
      ↓
Rate Limiting
      ↓
Audit Logging
      ↓
Monitoring
```

### Authentication

Verify:

> Who is calling the MCP server?

Depending on the deployment, this can use OAuth/OIDC-based mechanisms or other appropriate authentication.

The current MCP authorization specification has been hardened around OAuth/OIDC practices. ([Model Context Protocol Blog][1])

### Authorization

Authentication answers:

> **Who are you?**

Authorization answers:

> **What are you allowed to do?**

For example:

```text
User A
   ↓
Can read orders

User B
   ↓
Can read + create refunds
```

### Input validation

Never trust model-generated arguments.

```text
LLM
 ↓
orderId = "123"
 ↓
Validate
 ↓
Authorize
 ↓
Execute
```

### Least privilege

Give each MCP server/tool only the permissions it needs.

---

# 15. How do you handle authentication and authorization?

I would separate them.

### Authentication

```text
Client
  |
  | Access Token
  v
MCP Server
  |
  v
Validate token
```

Check:

* Token validity
* Issuer
* Audience
* Expiration
* Required claims/scopes

### Authorization

After authentication:

```text
User
 ↓
Role / Scope
 ↓
Tool permission
 ↓
Resource permission
 ↓
Execute
```

For example:

```text
Scope:
orders.read
```

allows:

```text
getOrder()
```

but not:

```text
cancelOrder()
```

The current MCP specification includes authorization mechanisms and recent hardening such as issuer validation and credential binding. ([Model Context Protocol Blog][1])

### Interview answer

> **"I would authenticate the MCP client using an appropriate OAuth/OIDC-based mechanism, validate the token and then perform authorization at the server and tool level using roles, scopes and resource permissions. Authentication tells me who the caller is; authorization determines what that caller can do."**

---

# 16. How would you audit MCP tool calls?

This is especially important because an AI can invoke tools automatically.

For every tool call, I would record:

```text
Timestamp
CorrelationId
UserId / ClientId
MCP Server
Tool Name
Input/Arguments
Authorization Result
Execution Result
Duration
Error
```

Example:

```text
CorrelationId: R12345
User: user123
Tool: createRefund
OrderId: 456
Authorization: Allowed
Result: Success
Duration: 250ms
```

For sensitive systems, I would be careful about logging secrets or sensitive data.

So I might log:

```text
OrderId = 456
```

but **not**:

```text
CreditCardNumber = 411111...
AccessToken = ...
Password = ...
```

### Architecture

```text
MCP Tool Call
      |
      v
Authorization
      |
      v
Tool Execution
      |
      +---------> Audit Log
      |
      v
Result
```

Use a correlation ID so the entire request can be traced.

### Interview answer

> **"I would audit every important MCP tool invocation with a correlation ID, caller identity, server, tool name, timestamp, authorization result, execution status, duration and error information. I would avoid logging secrets or unnecessary sensitive data. These logs would feed centralized monitoring and audit storage."**

---

# ⭐ Quick revision table

| Question                     | Simple answer                                                                                 |
| ---------------------------- | --------------------------------------------------------------------------------------------- |
| **What is MCP?**             | Open protocol connecting AI apps to tools/data/context                                        |
| **Why introduced?**          | Standardize AI-to-system integrations                                                         |
| **What problem?**            | Avoid custom integration for every AI app/system                                              |
| **Client?**                  | Connects AI application to MCP servers                                                        |
| **Server?**                  | Exposes tools/resources/prompts                                                               |
| **Tool?**                    | Performs an action                                                                            |
| **Resource?**                | Provides readable data/context                                                                |
| **Prompt?**                  | Reusable instruction/template                                                                 |
| **Tool discovery?**          | Client discovers server capabilities/tool catalog                                             |
| **Tool invocation?**         | LLM decides → client sends MCP tool call → server executes                                    |
| **MCP vs REST?**             | MCP is AI-oriented protocol; REST is general API style                                        |
| **MCP vs Function Calling?** | Function calling = model requests a function; MCP = standardized external capability protocol |
| **MCP vs Plugins?**          | MCP is open/standardized; plugins are often platform-specific                                 |
| **Database operations?**     | Yes, but expose controlled business operations                                                |
| **Security?**                | Authentication + authorization + validation + least privilege                                 |
| **Authentication?**          | Verify caller identity/token                                                                  |
| **Authorization?**           | Verify what caller/tool can access                                                            |
| **Auditing?**                | Log caller + tool + correlation ID + result + duration                                        |

## ⭐ One strong answer to remember

> **"I think of MCP as a standard communication layer between an AI application and external capabilities. The MCP client connects the AI application to MCP servers, and the servers expose tools, resources and prompts. Tools perform actions, resources provide data, and prompts provide reusable instructions. The client can discover the available capabilities and the model can request a tool through the client.**
>
> **From an architecture perspective, I would secure MCP servers using authentication, authorization, input validation and least privilege. For sensitive operations, I would expose controlled business tools rather than unrestricted database access. Finally, I would audit tool calls using correlation IDs, caller identity, tool name, authorization result, execution status and latency, while avoiding sensitive information in logs."**

**Key words:** **Client → Server → Tools → Resources → Prompts → Security → Audit**.

[1]: https://blog.modelcontextprotocol.io/posts/2026-07-28/?utm_source=chatgpt.com "The 2026-07-28 Specification | Model Context Protocol Blog"
[2]: https://ts.sdk.modelcontextprotocol.io/v2/?utm_source=chatgpt.com "MCP TypeScript SDK"
[3]: https://ts.sdk.modelcontextprotocol.io/v2/api/%40modelcontextprotocol/client/?utm_source=chatgpt.com "@modelcontextprotocol/client | MCP TypeScript SDK"
[4]: https://go.sdk.modelcontextprotocol.io/?utm_source=chatgpt.com "Overview - MCP Go SDK"
