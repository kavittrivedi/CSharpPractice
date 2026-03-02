# CSharpPractice

## Explain Different Types of Authentication for APIs

Explain Different types Authentication for APIs. Authentication for APIs is the process of verifying the identity of the user or system making a request to your API. It ensures that only authorized users or applications can access your API resources.

Here are the common ways to authenticate APIs:

- **API Key**: The client sends a unique key with each request. The server checks if the key is valid before allowing access. It's simple but less secure than other methods.
- **Basic Authentication**: The client sends a username and password in the request header. The server checks if the credentials are valid. This is also simple but not very secure unless used over HTTPS.
- **Bearer Token (JWT)**: The client sends a token (like a JSON Web Token - JWT) in the request header. The server verifies the token to ensure the request is from an authenticated user. This is one of the most secure and common methods used in modern web APIs.
- **OAuth**: This is a more advanced and secure method, often used when your API needs to authenticate users from other services (like Google or Facebook) without sharing their password. OAuth issues access tokens after a user grants permission.

### Example of Bearer Token Authentication in an API

The client sends a request with a token in the Authorization header like this:

```
Authorization: Bearer <your-token-here>
```

The server checks if the token is valid and grants access accordingly.

Authentication ensures that only users with valid credentials can interact with your API and access sensitive data.

## Explain Structure of JWT Token

Explain structure of JWT token. JWT (JSON Web Token) is a compact, URL-safe token used for authentication. It consists of three parts, separated by dots (.):  

### Header
This contains information about how the token is signed (usually with a secret or a key). It typically looks like this:  
```json
{
  "alg": "HS256", 
  "typ": "JWT"
}
```  
- **alg**: The algorithm used to sign the token (e.g., HS256 means HMAC with SHA-256).  
- **typ**: The type of token (usually JWT).

### Payload
This is the main data of the token. It contains claims, which are statements about an entity (usually the user) and additional metadata. Some common claims are:  
- **sub** (subject): The user or entity the token is about.  
- **iat** (issued at): When the token was created.  
- **exp** (expiration): When the token expires.  

Example of a payload:  
```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "iat": 1516239022
}
```

### Signature
This part is used to verify the token's authenticity. It is created by taking the encoded header and payload, and signing them with a secret key or a private key.  

The formula for creating the signature is:  
```
HMACSHA256(
  base64UrlEncode(header) + "." + base64UrlEncode(payload),
  secretKey
)
```  
This ensures that the token has not been tampered with.

### Example JWT
A JWT might look like this:  
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```  
- The first part is the header (base64 encoded).  
- The second part is the payload (base64 encoded).  
- The third part is the signature (base64 encoded).  

JWTs are commonly used for securely transmitting information between a client and server.

## Who Verifies the Signature of a JWT Token?

Who verifies the signature of a JWT token? The server verifies the signature of a JWT token.

Here’s how it works:

- When a client sends a JWT token to the server (usually in the request header), the server needs to ensure that the token is valid and hasn't been tampered with.
- The server takes the header and payload from the token and re-generates the signature using the secret key (or public key if using asymmetric signing) that was used to sign the token.
- The server compares the re-generated signature with the signature part of the token sent by the client.
- If the signatures match, it means the token is valid and hasn’t been altered.
- If they don't match, the token is considered invalid or tampered with, and the server rejects the request.

So, the server is responsible for checking if the JWT's signature is correct, ensuring that the token is authentic and trustworthy.

Explain difference between GET and POST GET vs POST:

GET:

Purpose: Used to retrieve data from a server.

Data Transmission: The data (parameters) is sent in the URL (query string). For example: /api/users?id=1.

Idempotent: GET requests should not change any data on the server.

Caching: GET requests can be cached by browsers and servers.

Use Case: When you need to fetch data (like viewing a webpage or getting information about a user).

POST:

Purpose: Used to send data to the server to create or update resources.

Data Transmission: The data is sent in the body of the request (not visible in the URL).

Non-Idempotent: POST requests can change the state on the server (e.g., creating a new user or submitting a form).

Caching: POST requests are typically not cached.

Use Case: When you need to send data to be processed (like submitting a form or creating a new record).

HTTP Status Codes:

GET:

Typically uses 200 OK if the request is successful and data is returned.

If the resource is not found, it may return 404 Not Found.

POST:

Typically uses 201 Created if a new resource is successfully created.

It can also return 200 OK if the action is successful but doesn't create new data.

If there is an issue with the request, it might return 400 Bad Request.

In summary:

GET is for fetching data, typically returns 200 OK.

POST is for sending data to be processed, often returns 201 Created or 200 OK.

How to persist state/how to sync two web API communications?  To persist state or sync communications between two web APIs, you need to store and manage the data (state) in a way that both APIs can access and update it when needed.

Here are common approaches to achieve this:

1. Using a Database:

Persisting State: Store the necessary information in a database (like SQL, NoSQL, etc.) that both APIs can access.

Syncing: Each API reads from and writes to the same database. Whenever one API updates the data, the other API can read the latest state.

Example: API 1 adds or updates a user in the database, and API 2 fetches the user data from the same database to perform further actions.

2. Using Shared Caching:

Persisting State: Use an in-memory cache (like Redis or Memcached) to temporarily store state that needs to be accessed by both APIs.

Syncing: Both APIs can read from and write to the cache. This allows faster access to frequently used data.

Example: API 1 writes data (e.g., session info) to the cache, and API 2 fetches that data from the cache to continue the session or process.

3. Using Message Queues:

Syncing: Use a message queue (like RabbitMQ, Kafka, or Azure Service Bus) to send messages between APIs. When one API finishes a task, it sends a message (event) to the queue, which the other API listens to and reacts to.

Example: API 1 processes an order and sends a message to a queue. API 2 listens to the queue and processes the payment once it gets the order info.

4. Using Tokens or JWT:

Persisting State: Use tokens (like JWT) to carry information about the current session or user between the two APIs.

Syncing: When API 1 sends a request to API 2, it includes the token. API 2 verifies the token and uses the information in it to continue processing.

Example: API 1 sends a user’s authentication token, and API 2 uses that token to authorize the user and process their request.

5. Using Webhooks:

Syncing: A webhook is a way one API can notify another API about events. When something happens in API 1, it sends an HTTP request to API 2 (the webhook) to notify it.

Example: API 1 processes a new order and sends a webhook to API 2 to start shipping the product.

In Short:

To persist state, store data in a shared location like a database or cache.

To sync communications, you can use mechanisms like shared databases, message queues, tokens, or webhooks to make sure both APIs stay in sync and can access the latest state.



