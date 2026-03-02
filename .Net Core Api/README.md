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



