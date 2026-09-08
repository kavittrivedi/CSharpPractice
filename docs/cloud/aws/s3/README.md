# AWS S3 Interview Notes

## One-Line Answer

**Amazon S3 (Simple Storage Service)** is AWS's object storage service used to store and retrieve any amount of data from anywhere. It is highly durable, scalable, secure, and commonly used for files, images, videos, backups, logs, data lakes, static websites, and application storage.

Memory hook:

> **S3 = Simple Storage Service = object storage for unlimited files.**

If interviewer asks "What is S3?", answer like this:

> Amazon S3 is a managed object storage service from AWS. We store data as objects inside buckets. Each object has data, metadata, and a key. S3 is highly scalable, designed for 11 nines durability, supports security through IAM and bucket policies, and provides features like versioning, lifecycle rules, replication, encryption, event notifications, and different storage classes for cost optimization.

---

## Core Concepts

### Bucket

A **bucket** is a container for objects.

Think of it like a top-level folder, but technically it is not a normal file-system folder.

Important points:

- Bucket names must be globally unique.
- A bucket is created in a specific AWS Region.
- You apply permissions, versioning, encryption, lifecycle rules, and replication at bucket level.
- Buckets can contain unlimited objects.

Example:

```text
Bucket: my-company-documents
Object: invoices/2026/january.pdf
```

### Object

An **object** is the actual file stored in S3.

Each object contains:

- **Key**: object name/path, like `images/profile.png`
- **Value/Data**: actual content
- **Metadata**: content type, size, custom metadata, etc.
- **Version ID**: if versioning is enabled
- **Access control information**

Memory hook:

> **Bucket stores objects. Object = file + key + metadata.**

### Key

The **key** is the full name of the object inside a bucket.

Example:

```text
users/1001/profile-image.png
```

S3 shows this like folders, but internally it is one object key. S3 is not a traditional hierarchical file system.

### Region

Buckets are created in a Region, for example:

- `us-east-1`
- `ap-south-1`
- `eu-west-1`

Choose a Region based on:

- User location and latency
- Compliance requirements
- Cost
- Integration with other AWS services

---

## S3 Is Object Storage, Not File Storage

### Object Storage

S3 stores complete objects. You upload, download, replace, or delete objects.

Best for:

- Images
- Videos
- PDFs
- Backups
- Logs
- Static files
- Data lake files

### File Storage

File storage behaves like a shared file system with folders, file locking, and mounted drives.

AWS service example:

- **Amazon EFS** for Linux shared file system
- **Amazon FSx** for Windows or high-performance file systems

### Block Storage

Block storage behaves like a hard disk attached to a server.

AWS service example:

- **Amazon EBS** attached to EC2

Interview answer:

> S3 is object storage. It is not used like a mounted disk by default. For shared file systems we use EFS or FSx, and for EC2 disk volumes we use EBS.

---

## S3 Durability and Availability

S3 Standard is designed for:

- **99.999999999% durability** also called **11 nines durability**
- **99.99% availability** for S3 Standard

Durability means:

> Probability that your object will not be lost.

Availability means:

> Probability that the service is accessible when you need it.

Important interview distinction:

| Term | Meaning | Example |
|---|---|---|
| Durability | Data is not lost | File remains safe |
| Availability | Service is reachable | You can download the file now |

Memory hook:

> **Durability protects data. Availability protects access.**

Example answer:

> S3 is designed for 11 nines durability because it stores data redundantly across multiple devices and Availability Zones, depending on storage class. Availability depends on the selected storage class.

---

## Consistency Model

Amazon S3 provides **strong read-after-write consistency** for object operations.

This means:

- After uploading a new object, a read returns the latest object.
- After overwriting an object, a read returns the latest version.
- After deleting an object, S3 reflects the delete.
- List operations also reflect changes consistently.

Interview answer:

> S3 now provides strong consistency. Once a PUT, overwrite, or DELETE succeeds, subsequent read and list operations return the latest state.

---

## S3 Storage Classes

Storage classes help optimize cost based on access pattern.

Memory hook:

> **Hot, unknown, cold, colder, coldest, one-zone, fastest.**

### 1. S3 Standard

Use for frequently accessed data.

Best for:

- Websites
- Mobile apps
- Frequently accessed files
- Big data analytics

Features:

- High durability
- High availability
- Low latency
- Higher cost than archival classes

### 2. S3 Intelligent-Tiering

Use when access pattern is unknown or changes over time.

S3 automatically moves objects between access tiers based on usage.

Best for:

- Data with unpredictable access
- Long-lived objects
- Cost optimization without manual lifecycle decisions

Interview line:

> Intelligent-Tiering is useful when we do not know whether data will be hot or cold.

### 3. S3 Standard-IA

IA means **Infrequent Access**.

Use for data accessed less often but still needs quick retrieval.

Best for:

- Backups
- Disaster recovery files
- Older documents

Tradeoff:

- Lower storage cost
- Retrieval cost applies

### 4. S3 One Zone-IA

Stores data in one Availability Zone.

Best for:

- Re-creatable data
- Secondary backups
- Data that can be regenerated

Tradeoff:

- Cheaper than Standard-IA
- Less resilient because data is in one AZ

Interview line:

> One Zone-IA is cheaper but not suitable for critical data because it stores data in a single Availability Zone.

### 5. S3 Glacier Instant Retrieval

For archive data that is rarely accessed but must be retrieved quickly.

Best for:

- Medical records
- Compliance archives
- Rarely accessed files needing milliseconds retrieval

### 6. S3 Glacier Flexible Retrieval

For archive data where retrieval can take minutes to hours.

Best for:

- Long-term backups
- Archives
- Compliance data

### 7. S3 Glacier Deep Archive

Lowest-cost archive storage.

Best for:

- Very long-term retention
- Data accessed once or twice per year
- Regulatory archives

Retrieval can take hours.

### 8. S3 Express One Zone

High-performance, single-AZ storage class for very low latency workloads.

Best for:

- Machine learning training
- Interactive analytics
- High-performance request-intensive workloads

Important:

- Uses **directory buckets**
- Stores data in a single Availability Zone
- Designed for low latency, not multi-AZ resilience

### Storage Class Summary

| Storage Class | Use Case | Cost | Retrieval |
|---|---|---:|---|
| Standard | Frequent access | High | Immediate |
| Intelligent-Tiering | Unknown/changing access | Optimized | Immediate |
| Standard-IA | Infrequent access | Lower | Immediate with retrieval fee |
| One Zone-IA | Re-creatable infrequent data | Lower | Immediate with retrieval fee |
| Glacier Instant Retrieval | Rare archive, fast access | Low | Milliseconds |
| Glacier Flexible Retrieval | Archive | Very low | Minutes to hours |
| Glacier Deep Archive | Long-term archive | Lowest | Hours |
| Express One Zone | Lowest latency workloads | Specialized | Immediate, very low latency |

---

## Versioning

S3 Versioning keeps multiple versions of the same object.

Example:

```text
report.pdf - version 1
report.pdf - version 2
report.pdf - version 3
```

Benefits:

- Recover accidentally deleted files
- Recover overwritten files
- Useful for audit and rollback
- Required for some features like Object Lock

Important behavior:

- If versioning is enabled and you delete an object, S3 adds a **delete marker**.
- Older versions still exist unless permanently deleted.
- Versioning can increase storage cost.

Interview answer:

> Versioning protects against accidental delete and overwrite by keeping multiple versions of an object. A normal delete creates a delete marker, so the object appears deleted but previous versions can be restored.

---

## Lifecycle Rules

Lifecycle rules automatically move or delete objects based on age or conditions.

Common actions:

- Move objects from Standard to Standard-IA after 30 days
- Move objects to Glacier after 90 days
- Delete objects after 7 years
- Delete incomplete multipart uploads
- Delete old versions

Example:

```text
Day 0: Upload logs to S3 Standard
Day 30: Move logs to Standard-IA
Day 90: Move logs to Glacier
Day 365: Move logs to Deep Archive
Day 2555: Delete logs
```

Interview answer:

> Lifecycle policies are used for cost optimization and retention automation. They can transition objects to cheaper storage classes or expire/delete objects after a configured time.

---

## Encryption

S3 supports encryption in transit and at rest.

### Encryption in Transit

Use HTTPS/TLS while uploading and downloading objects.

### Encryption at Rest

S3 encrypts stored objects.

Current important point:

> New objects in S3 are encrypted by default using server-side encryption with S3 managed keys, called SSE-S3.

### Server-Side Encryption Options

| Type | Meaning | Key Managed By |
|---|---|---|
| SSE-S3 | S3 managed encryption | AWS S3 |
| SSE-KMS | AWS KMS key based encryption | AWS KMS / customer |
| DSSE-KMS | Dual-layer KMS encryption | AWS KMS |
| SSE-C | Customer-provided key | Customer |

### SSE-S3

Default encryption option.

Use when:

- You want simple encryption
- You do not need detailed KMS key control

### SSE-KMS

Uses AWS Key Management Service.

Use when:

- You need key rotation control
- You need audit logs in CloudTrail
- You need customer managed keys
- You need stricter compliance

### SSE-C

Customer provides the encryption key with each request.

Use rarely, because the customer must manage the key completely.

Interview answer:

> S3 supports encryption in transit using HTTPS and encryption at rest using SSE-S3, SSE-KMS, DSSE-KMS, or SSE-C. Today SSE-S3 is enabled by default for new objects, but for compliance and audit requirements we often use SSE-KMS.

---

## Access Control and Security

S3 security is one of the most important interview areas.

Memory hook:

> **Block public access, IAM, bucket policy, encryption, logging.**

### IAM Policies

IAM policies define what users, roles, or services can do.

Example permissions:

```text
s3:GetObject
s3:PutObject
s3:DeleteObject
s3:ListBucket
```

Use IAM policies for:

- Granting access to users
- Granting access to EC2, Lambda, ECS through IAM roles
- Applying least privilege

### Bucket Policies

Bucket policies are resource-based policies attached to buckets.

Use bucket policies for:

- Cross-account access
- Public read for static website hosting
- Restricting access by IP
- Enforcing encryption
- Allowing CloudFront access

Example concept:

```text
Allow only a specific IAM role to read objects from this bucket.
```

### Block Public Access

S3 Block Public Access prevents accidental public exposure.

Best practice:

> Keep Block Public Access enabled unless there is a clear reason to allow public access.

### ACLs

ACL means Access Control List.

Older way to manage access.

Interview answer:

> ACLs are legacy. AWS recommends using IAM policies and bucket policies instead of ACLs in most cases.

### Pre-Signed URLs

A pre-signed URL gives temporary access to a private object.

Best for:

- Temporary download links
- Temporary upload links
- Sharing private files without making bucket public

Example:

```text
User can download invoice.pdf for 10 minutes using a pre-signed URL.
```

Interview answer:

> A pre-signed URL is a temporary URL generated using AWS credentials. It allows limited-time access to an S3 object without changing the object's public permissions.

---

## Object Lock

S3 Object Lock prevents objects from being deleted or overwritten for a fixed time.

Used for:

- Compliance
- Legal hold
- Write Once Read Many, also called WORM
- Ransomware protection

Modes:

| Mode | Meaning |
|---|---|
| Governance mode | Users with special permission can override retention |
| Compliance mode | No one, including root user, can delete before retention expires |

Legal hold:

- No expiry date
- Remains until explicitly removed

Important:

- Object Lock requires versioning.

Interview answer:

> Object Lock provides WORM protection. It prevents deletion or overwrite for a retention period. Governance mode can be bypassed by privileged users, but compliance mode cannot be bypassed until retention expires.

---

## Replication

S3 replication automatically copies objects from one bucket to another.

Types:

| Type | Meaning |
|---|---|
| CRR | Cross-Region Replication |
| SRR | Same-Region Replication |

Use cases:

- Disaster recovery
- Compliance
- Lower latency for users in another region
- Cross-account data copy
- Log aggregation

Important:

- Versioning must be enabled on source and destination buckets.
- Replication is asynchronous.
- Existing objects are not automatically replicated unless you use batch replication.

Interview answer:

> S3 replication is asynchronous copying of objects between buckets. CRR copies across regions for DR and compliance, while SRR copies within the same region for aggregation or account separation.

---

## Multipart Upload

Multipart upload uploads a large object in parts.

Benefits:

- Faster upload
- Retry failed parts instead of full file
- Better for large files
- Can upload parts in parallel

Interview answer:

> Multipart upload splits a large file into multiple parts, uploads them independently, and then combines them into one object. It improves reliability and performance for large uploads.

Common best practice:

> Use multipart upload for large files, commonly files larger than 100 MB.

---

## Static Website Hosting

S3 can host static websites.

Static means:

- HTML
- CSS
- JavaScript
- Images

S3 cannot run server-side code like:

- ASP.NET Core
- Node.js backend
- PHP
- Java server code

For dynamic apps use:

- EC2
- Elastic Beanstalk
- ECS/EKS
- Lambda + API Gateway
- App Runner

Common setup:

```text
User -> Route 53 -> CloudFront -> S3 static website files
```

Best practice:

> Use CloudFront in front of S3 for HTTPS, caching, performance, and security.

Interview answer:

> S3 can host static websites, but it cannot execute server-side code. For production, we normally put CloudFront in front of S3 for CDN caching, HTTPS, and controlled access.

---

## CloudFront with S3

CloudFront is AWS's CDN.

Benefits with S3:

- Faster content delivery globally
- HTTPS support
- Caching
- Reduces load on S3
- Can keep S3 private
- Supports Origin Access Control, or OAC

Interview answer:

> CloudFront caches S3 content at edge locations. We can keep the S3 bucket private and allow only CloudFront to access it using Origin Access Control.

---

## Event Notifications

S3 can trigger events when objects are created, deleted, or restored.

Targets:

- AWS Lambda
- Amazon SQS
- Amazon SNS
- Amazon EventBridge

Use cases:

- Resize image after upload
- Process CSV file
- Start video transcoding
- Send notification
- Trigger ETL pipeline

Example:

```text
User uploads image -> S3 event -> Lambda -> create thumbnail
```

Interview answer:

> S3 event notifications allow us to react to object-level events, such as object created or deleted. Common targets are Lambda, SQS, SNS, and EventBridge.

---

## S3 Performance

S3 automatically scales to high request rates.

Performance tips:

- Use multipart upload for large files.
- Use CloudFront for global read performance.
- Use S3 Transfer Acceleration for long-distance uploads.
- Use byte-range GETs to download parts of large objects.
- Use S3 Express One Zone for very low-latency single-AZ workloads.

Interview answer:

> S3 scales automatically, but performance can be improved with multipart upload, CloudFront caching, Transfer Acceleration, and choosing the right storage class such as S3 Express One Zone for latency-sensitive workloads.

---

## S3 Transfer Acceleration

Transfer Acceleration speeds up uploads and downloads over long distances by using AWS edge locations.

Use when:

- Users upload from different countries
- Large files are uploaded globally
- Network latency is high

Interview answer:

> S3 Transfer Acceleration uses AWS edge locations to speed up long-distance transfers to S3.

---

## CORS in S3

CORS means Cross-Origin Resource Sharing.

Use CORS when a browser-based app from one domain needs to access S3 objects from another domain.

Example:

```text
https://myapp.com wants to upload directly to https://my-bucket.s3.amazonaws.com
```

Interview answer:

> We configure CORS on an S3 bucket when browser clients need cross-origin access to S3 resources, such as direct uploads from a web app.

---

## S3 Select

S3 Select lets you retrieve only part of an object's data using SQL expressions.

Best for:

- CSV
- JSON
- Parquet

Example:

```sql
SELECT name, city FROM S3Object WHERE city = 'Pune'
```

Benefit:

- Less data transferred
- Faster filtering
- Lower cost for some workloads

Interview answer:

> S3 Select allows querying a subset of data from an object using SQL-like expressions, so the application does not need to download the full file.

---

## S3 Access Points

Access Points simplify managing access to shared datasets.

Instead of one complex bucket policy, create separate access points for different applications or teams.

Example:

```text
Finance app access point -> finance/ prefix
HR app access point -> hr/ prefix
Analytics access point -> read-only access
```

Interview answer:

> S3 Access Points provide separate network endpoints and policies for accessing shared buckets, making permissions easier to manage at scale.

---

## Directory Buckets

Directory buckets are a newer bucket type used mainly with S3 Express One Zone.

Use for:

- Low-latency workloads
- High-performance access
- Data stored in a specific Availability Zone

Interview answer:

> Directory buckets are optimized for high-performance workloads, especially S3 Express One Zone. They store data in a selected Availability Zone and are different from general purpose S3 buckets.

---

## Logging and Monitoring

S3 can be monitored using:

- **CloudWatch metrics**: bucket/object metrics
- **CloudTrail data events**: API-level activity, such as GetObject and PutObject
- **S3 server access logs**: detailed access request logs
- **S3 Storage Lens**: organization-wide storage visibility and recommendations

Interview answer:

> For monitoring S3, we use CloudWatch metrics, CloudTrail data events for API auditing, server access logs for request details, and S3 Storage Lens for storage analytics.

---

## Common S3 Use Cases

S3 is commonly used for:

- Static website hosting
- User file uploads
- Image and video storage
- Backup and restore
- Log storage
- Data lake storage
- Big data analytics
- Disaster recovery
- Software artifacts
- ML training data
- CDN origin with CloudFront

---

## S3 Pricing Factors

S3 cost depends on:

- Storage amount in GB
- Storage class
- Number of requests
- Data retrieval charges
- Data transfer out
- Replication
- Lifecycle transitions
- Object monitoring, depending on storage class/features

Interview answer:

> S3 pricing is mainly based on storage used, storage class, requests, data transfer, retrieval fees, and optional features like replication or analytics.

---

## Common Interview Questions and Answers

### Quick Traps Interviewers Ask

| Question | Best Short Answer |
|---|---|
| Is S3 a file system? | No, it is object storage. Folder view is based on key prefixes. |
| Is bucket name regional? | Bucket exists in a Region, but bucket name must be globally unique. |
| Can S3 run backend code? | No, S3 hosts static files only. |
| Is public bucket access recommended? | No, keep bucket private and use CloudFront or pre-signed URLs. |
| Does versioned delete immediately remove data? | No, it creates a delete marker. Old versions remain. |
| Is replication synchronous? | No, S3 replication is asynchronous. |
| Does replication require versioning? | Yes, source and destination buckets need versioning. |
| Is SSE-S3 default now? | Yes, new objects are encrypted by default with SSE-S3. |
| Which is better: IAM policy or bucket policy? | IAM policy controls principals; bucket policy controls the bucket resource. Often both are used. |
| Are ACLs preferred? | No, ACLs are legacy for most use cases. Prefer IAM and bucket policies. |
| Can Glacier data be read immediately? | Depends on class. Glacier Instant Retrieval is immediate; Flexible and Deep Archive take longer. |
| Can existing objects replicate automatically after enabling replication? | Not by normal replication; use S3 Batch Replication for existing objects. |

### What is S3?

S3 is AWS managed object storage. It stores data as objects inside buckets and is used for scalable, durable, and secure storage of files, backups, logs, media, data lakes, and static website content.

### What is the difference between bucket and object?

A bucket is a container. An object is the actual stored file/data inside the bucket. Each object has a key, data, metadata, and optionally a version ID.

### Is S3 global or regional?

S3 bucket names are globally unique, but buckets are created in a specific AWS Region.

### Can S3 store unlimited data?

Yes, S3 can store virtually unlimited data. A bucket can contain unlimited objects. Individual object size can be very large, and large objects should use multipart upload.

### What is the maximum size of an S3 object?

An individual S3 object can be up to **5 TB**. For large objects, use multipart upload.

### What is the difference between S3 and EBS?

| S3 | EBS |
|---|---|
| Object storage | Block storage |
| Stores files/objects | Acts like disk volume |
| Access over HTTP/API | Attached to EC2 |
| Highly scalable | Limited by volume size |
| Good for backups, media, logs | Good for OS/database disk |

### What is the difference between S3 and EFS?

| S3 | EFS |
|---|---|
| Object storage | File storage |
| Access by API/HTTP | Mounted file system |
| No traditional file locking | Supports file system behavior |
| Best for objects/files at scale | Best for shared Linux file system |

### How do you secure an S3 bucket?

Answer with this checklist:

1. Keep Block Public Access enabled.
2. Use IAM roles and least privilege policies.
3. Use bucket policies carefully.
4. Enable encryption, SSE-S3 or SSE-KMS.
5. Use CloudTrail and access logging.
6. Use versioning and Object Lock for protection.
7. Use pre-signed URLs for temporary access.
8. Avoid ACLs unless required.

### What is a pre-signed URL?

A pre-signed URL provides temporary access to a private S3 object. It is generated using AWS credentials and expires after a configured time.

### What happens when you delete an object in a versioned bucket?

S3 adds a delete marker. The object appears deleted, but old versions still exist and can be restored.

### What is lifecycle policy?

A lifecycle policy automatically transitions objects to cheaper storage classes or deletes them after a defined period.

### What is S3 replication?

Replication automatically and asynchronously copies objects from one bucket to another, either in the same Region or across Regions.

### What is the difference between CRR and SRR?

| CRR | SRR |
|---|---|
| Cross-Region Replication | Same-Region Replication |
| Disaster recovery, compliance, latency | Log aggregation, account separation |

### What is Object Lock?

Object Lock prevents objects from being deleted or overwritten for a retention period. It supports governance mode, compliance mode, and legal hold.

### How does S3 handle high traffic?

S3 automatically scales. Use CloudFront for caching, multipart upload for large files, Transfer Acceleration for global transfers, and S3 Express One Zone for very low-latency workloads.

### Can S3 host dynamic websites?

No. S3 only hosts static websites. For dynamic websites, use compute services like EC2, Lambda, ECS, EKS, Elastic Beanstalk, or App Runner.

### How can an application upload files directly to S3?

Common pattern:

```text
1. User requests upload permission from backend.
2. Backend generates pre-signed upload URL.
3. Browser uploads file directly to S3.
4. S3 event can trigger Lambda for processing.
```

### How do you serve private S3 content securely?

Use CloudFront with S3 as private origin, configure Origin Access Control, and allow only CloudFront to read the bucket.

### What is S3 event notification?

S3 event notification triggers another AWS service when object events happen, such as upload or delete. Targets include Lambda, SQS, SNS, and EventBridge.

### What is the best storage class for unknown access pattern?

S3 Intelligent-Tiering.

### What is the cheapest storage class?

For long-term archive, S3 Glacier Deep Archive is usually the lowest-cost storage class, but retrieval is slower.

### When would you use One Zone-IA?

Use One Zone-IA for infrequently accessed data that can be recreated and does not need multi-AZ resilience.

### When would you use S3 Express One Zone?

Use S3 Express One Zone for very low-latency, request-intensive workloads where storing data in one Availability Zone is acceptable.

---

## Scenario-Based Answers

### Scenario 1: Users upload profile pictures

Recommended design:

```text
Frontend -> Backend asks for pre-signed URL -> Frontend uploads to S3 -> S3 event triggers Lambda -> Lambda creates thumbnail
```

Mention:

- Private bucket
- Pre-signed URLs
- CORS
- Encryption
- CloudFront for serving images

### Scenario 2: Host Angular/React static website

Recommended design:

```text
Route 53 -> CloudFront -> Private S3 bucket
```

Mention:

- S3 stores static files
- CloudFront provides CDN and HTTPS
- OAC keeps bucket private
- Cache invalidation after deployment

### Scenario 3: Store application logs for 7 years

Recommended design:

```text
S3 Standard -> Glacier -> Deep Archive -> Delete after 7 years
```

Mention:

- Lifecycle policy
- Object Lock if compliance requires immutability
- SSE-KMS if audit/compliance requires KMS
- Storage Lens for visibility

### Scenario 4: Disaster recovery copy in another Region

Recommended design:

```text
Source bucket in ap-south-1 -> CRR -> Destination bucket in us-east-1
```

Mention:

- Enable versioning
- Cross-Region Replication
- Asynchronous replication
- Destination can be different account

### Scenario 5: Prevent accidental file deletion

Use:

- Versioning
- MFA Delete, if required and supported for the workflow
- Object Lock for stronger protection
- Least privilege IAM

### Scenario 6: Public file download for limited time

Use:

- Private bucket
- Pre-signed URL with expiry

Do not make the bucket public.

---

## Easy Memory Map

Remember S3 using this sentence:

> **BOSS VELCRO PMS**

Meaning:

| Letter | Topic |
|---|---|
| B | Bucket |
| O | Object |
| S | Storage classes |
| S | Security |
| V | Versioning |
| E | Encryption |
| L | Lifecycle |
| C | Consistency / CloudFront |
| R | Replication |
| O | Object Lock |
| P | Pre-signed URL / Performance |
| M | Multipart upload / Monitoring |
| S | Static website / S3 events |

If you remember this map, you can answer most S3 interview questions.

---

## 60-Second Interview Revision

Amazon S3 is AWS object storage. Data is stored as objects inside buckets. A bucket is regional and has a globally unique name. Each object has a key, data, and metadata. S3 is designed for 11 nines durability and provides strong read-after-write consistency. Security is managed using IAM policies, bucket policies, Block Public Access, encryption, and pre-signed URLs. S3 supports versioning to recover overwritten or deleted objects, lifecycle rules to move data to cheaper storage classes, replication for same-region or cross-region copying, Object Lock for WORM compliance, and event notifications to trigger Lambda, SQS, SNS, or EventBridge. Storage classes include Standard, Intelligent-Tiering, Standard-IA, One Zone-IA, Glacier classes, and S3 Express One Zone. S3 can host static websites, but for production we usually use CloudFront in front of a private bucket.

---

## Best Final Answer Template

Use this structure when answering any S3 question:

```text
1. Define the feature.
2. Explain why it is used.
3. Mention one real use case.
4. Mention security/cost/performance consideration.
```

Example:

> S3 lifecycle policy is a rule that automatically transitions or expires objects. It is used for cost optimization and retention management. For example, logs can stay in S3 Standard for 30 days, move to Glacier after 90 days, and be deleted after 7 years. We should choose transitions carefully because some storage classes have retrieval charges and minimum storage duration.

---

## Official References

- AWS S3 User Guide: https://docs.aws.amazon.com/AmazonS3/latest/userguide/Welcome.html
- S3 storage classes: https://docs.aws.amazon.com/AmazonS3/latest/userguide/storage-class-intro.html
- S3 encryption: https://docs.aws.amazon.com/AmazonS3/latest/userguide/serv-side-encryption.html
- S3 versioning: https://docs.aws.amazon.com/AmazonS3/latest/userguide/Versioning.html
- S3 replication: https://docs.aws.amazon.com/AmazonS3/latest/userguide/replication.html
- S3 security best practices: https://docs.aws.amazon.com/AmazonS3/latest/userguide/security-best-practices.html
