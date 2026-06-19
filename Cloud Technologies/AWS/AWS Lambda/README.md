AWS Lambda is a **serverless compute service**. In simple words: you upload a small piece of code, tell AWS when to run it, and AWS runs it for you without you creating or managing servers.

Think of Lambda like this:

> “When this event happens, run this code.”

Example:  
When a user uploads an image to S3, run code to resize that image.

**What Lambda Does**
Lambda runs your code only when needed. You do not manage EC2 servers, operating systems, scaling, patching, or availability. AWS handles that.

You are charged mainly for:
- Number of requests
- Time your code runs
- Memory/CPU configuration

Official AWS pricing says Lambda charges are based on requests and execution duration, rounded to the nearest millisecond: https://aws.amazon.com/lambda/pricing/

**Key Terms**
`Function`: The code you write and deploy to Lambda.

`Handler`: The entry point method that Lambda calls when the function runs.

`Event`: Input data that triggers the Lambda. For example, an S3 upload event, API request, SQS message, or scheduled event.

`Runtime`: The language environment, such as Node.js, Python, Java, .NET, Go, or custom runtime.

`Trigger`: The AWS service or event source that starts the Lambda.

`Execution role`: IAM role that gives Lambda permission to access AWS services.

`Timeout`: Maximum time a function can run. Standard Lambda functions can run up to 15 minutes.

`Memory`: Configurable memory. More memory also gives more CPU power.

`/tmp`: Temporary storage available during execution. AWS docs mention `/tmp` storage can be between 512 MB and 10,240 MB.

**How Lambda Works**
1. An event happens.
2. AWS Lambda receives the event.
3. Lambda creates or reuses an execution environment.
4. Lambda runs your handler function.
5. Function returns a result or fails.
6. Logs and metrics go to CloudWatch.

Example flow:

```text
API Gateway -> Lambda -> DynamoDB -> Response to user
```

**Common Triggers**
Lambda can be triggered by many AWS services:

- API Gateway: Build REST or HTTP APIs
- S3: Run code when a file is uploaded
- DynamoDB Streams: React to database changes
- SQS: Process queue messages
- SNS: Process notifications
- EventBridge / CloudWatch Events: Scheduled jobs
- Kinesis: Process streaming data
- Cognito: Authentication workflows
- ALB: HTTP request handling
- Step Functions: Workflow steps

**Cold Start vs Warm Start**
A **cold start** happens when Lambda has to create a new execution environment before running your code. This adds extra latency.

A **warm start** happens when Lambda reuses an existing environment, so it starts faster.

AWS docs describe cold start as the time spent downloading code, setting up the environment, starting the runtime, and running initialization code before the handler runs: https://docs.aws.amazon.com/lambda/latest/dg/lambda-runtime-environment.html

Ways to reduce cold starts:
- Keep package size small
- Initialize heavy resources outside the handler only when useful
- Avoid unnecessary dependencies
- Use Provisioned Concurrency for predictable low latency
- Choose suitable runtime and memory

**Basic Lambda Example**
C# style concept:

```csharp
public string FunctionHandler(string input)
{
    return input.ToUpper();
}
```

Node.js example:

```javascript
exports.handler = async (event) => {
  return {
    statusCode: 200,
    body: "Hello from Lambda"
  };
};
```

Python example:

```python
def lambda_handler(event, context):
    return {
        "statusCode": 200,
        "body": "Hello from Lambda"
    }
```

**Lambda Context Object**
Lambda gives your function a `context` object. It contains runtime information such as:
- Function name
- Remaining execution time
- Request ID
- Memory limit
- Log group/log stream

**Synchronous vs Asynchronous Invocation**
`Synchronous`: Caller waits for the response.

Example:
```text
API Gateway -> Lambda -> Response
```

`Asynchronous`: Caller sends event and does not wait.

Example:
```text
S3 upload -> Lambda runs in background
```

`Poll-based`: Lambda reads from a queue or stream.

Example:
```text
SQS -> Lambda polls messages -> Processes them
```

**Concurrency**
Concurrency means how many Lambda executions run at the same time.

If 100 requests arrive at once, Lambda may run 100 separate executions in parallel.

Important types:
- `Account concurrency`: Total concurrent executions allowed in account/region
- `Reserved concurrency`: Limit or guarantee concurrency for one function
- `Provisioned concurrency`: Pre-warmed environments to reduce cold starts

**Error Handling**
Error behavior depends on invocation type.

For synchronous calls:
- Lambda returns the error directly to caller.

For asynchronous calls:
- Lambda can retry automatically.
- Failed events can go to a dead-letter queue or failure destination.

For SQS:
- Failed messages return to the queue.
- After repeated failures, they can move to a DLQ.

**Monitoring**
Lambda integrates with CloudWatch.

You can monitor:
- Invocation count
- Errors
- Duration
- Throttles
- Concurrent executions
- Logs
- Traces using AWS X-Ray

**Security**
Lambda uses IAM.

Important security ideas:
- Give the Lambda execution role only the permissions it needs.
- Store secrets in AWS Secrets Manager or Parameter Store.
- Do not hardcode credentials.
- Use VPC only when Lambda must access private resources.
- Validate all inputs.

**Advantages**
- No server management
- Automatic scaling
- Pay only when code runs
- Easy integration with AWS services
- Good for event-driven systems
- Fast to build APIs, background jobs, and automation

**Disadvantages / Limitations**
- Maximum execution time for standard functions is 15 minutes
- Cold starts can affect latency
- Not ideal for long-running processes
- Debugging distributed serverless systems can be harder
- Vendor lock-in with AWS integrations
- Large dependencies can slow startup

**When To Use Lambda**
Use Lambda for:
- APIs
- Image/file processing
- Scheduled jobs
- Queue processing
- Data transformation
- Notifications
- Automation scripts
- Serverless backends
- Event-driven workflows

Avoid Lambda for:
- Long-running jobs over 15 minutes
- Apps needing constant server memory
- Very low-latency workloads where cold starts are unacceptable unless using Provisioned Concurrency
- Heavy stateful applications

**Best Practices**
- Keep functions small and focused
- Use environment variables for configuration
- Reuse database connections across warm invocations
- Keep deployment package small
- Use IAM least privilege
- Add proper logging
- Handle retries safely
- Make functions idempotent
- Use DLQ or destinations for failures
- Set timeout carefully
- Monitor errors and throttles
- Avoid storing permanent data in `/tmp`

**Important Interview Questions**
1. What is AWS Lambda?  
AWS Lambda is a serverless compute service that runs code in response to events without managing servers.

2. What is serverless?  
Serverless means you do not manage servers directly. The cloud provider handles infrastructure, scaling, and availability.

3. What is a Lambda trigger?  
A trigger is an event source that invokes the Lambda function, such as S3, API Gateway, SQS, or EventBridge.

4. What is cold start?  
Cold start is the extra startup time when Lambda creates a new execution environment.

5. How do you reduce cold starts?  
Use smaller packages, optimize initialization, increase memory if needed, and use Provisioned Concurrency.

6. What is Lambda timeout?  
The maximum time a Lambda function is allowed to run. Standard Lambda functions support up to 15 minutes.

7. How does Lambda scale?  
Lambda automatically creates more execution environments to handle more concurrent events.

8. How does Lambda access other AWS services?  
Using an IAM execution role attached to the Lambda function.

9. Where are Lambda logs stored?  
In Amazon CloudWatch Logs.

10. Difference between Lambda and EC2?  
EC2 gives you virtual servers to manage. Lambda runs code without server management and charges only when code runs.

**One-Line Summary**
AWS Lambda is AWS’s event-driven, serverless way to run code automatically without managing servers.