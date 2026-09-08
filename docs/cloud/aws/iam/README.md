# AWS IAM Interview Notes

## 1. One-Line Answer

**AWS IAM**, or **Identity and Access Management**, is the AWS service used to securely control **who can access AWS resources** and **what actions they can perform**.

Simple interview answer:

> AWS IAM is a global AWS security service for managing identities and permissions. It lets us create users, groups, roles, and policies to control access to AWS services and resources using the principle of least privilege.

Memory hook:

```text
IAM = Who can do what on which resource under what condition.
```

If you remember only one line:

```text
Principal + Action + Resource + Condition = IAM decision
```

---

## 2. Why IAM Exists

Without IAM, every person or application would need broad AWS account credentials, which is dangerous.

IAM solves:

- User access to AWS Console and APIs.
- Application access to AWS services.
- Temporary access using roles.
- Cross-account access.
- Federation with corporate identity providers.
- Fine-grained permission control.
- Audit and security governance.

Interview answer:

> IAM exists to enforce secure access control in AWS. It helps us avoid sharing root credentials, assign least-privilege permissions, use temporary credentials through roles, and centrally manage access for users, applications, and AWS services.

---

## 3. IAM Is Global

IAM is a **global service**, not a regional service.

Meaning:

- IAM users, groups, roles, and policies are created globally.
- They are available across AWS Regions.
- IAM itself does not belong to one Region.

Interview line:

> IAM is global because identities and permissions apply across the AWS account, although the resources they access may be regional.

---

## 4. Root User

The **root user** is the original account owner identity created with the AWS account email address.

Root user has complete access to everything in the account.

Best practices:

1. Do not use root user for daily work.
2. Enable MFA on root user.
3. Store root credentials securely.
4. Do not create root access keys.
5. Use root only for tasks that require root.

Examples of root-only or root-preferred tasks:

- Changing account settings.
- Closing the AWS account.
- Some billing or support-level operations.
- Recovering certain access situations.

Interview answer:

> The root user has unrestricted access to the AWS account. Best practice is to secure it with MFA, avoid using it for daily tasks, and create IAM users or federated access for normal administration.

Memory:

```text
Root = break-glass account, not daily account.
```

---

## 5. IAM Core Components

Remember IAM with:

```text
U G R P
Users, Groups, Roles, Policies
```

| Component | Meaning |
|---|---|
| User | Long-term identity for a person or workload |
| Group | Collection of IAM users |
| Role | Temporary identity assumed by trusted principals |
| Policy | JSON permission document |

Simple flow:

```text
Identity gets policy -> policy allows/denies actions -> AWS evaluates request
```

---

## 6. IAM Users

An **IAM user** is an identity inside an AWS account.

IAM users can have:

- Console password.
- Access keys for programmatic access.
- MFA device.
- Permissions through policies.
- Group membership.

Use IAM users for:

- Individual human users in simple setups.
- Programmatic access when roles/federation are not possible.

Modern best practice:

> Prefer federation through IAM Identity Center or an external identity provider for human users. Prefer IAM roles for workloads.

Interview answer:

> An IAM user represents a person or application with long-term credentials. However, for production environments, AWS generally recommends using federated identities for humans and IAM roles for applications instead of long-term access keys.

---

## 7. IAM Groups

An **IAM group** is a collection of IAM users.

Use groups to assign permissions to multiple users at once.

Example:

```text
Developers group -> read-only EC2 + CloudWatch logs
Admins group     -> administrator permissions
Auditors group   -> read-only permissions
```

Important points:

- Groups can contain users.
- Groups cannot contain other groups.
- Groups are only for IAM users.
- IAM roles cannot be placed in groups.

Interview answer:

> IAM groups simplify permission management for IAM users. Instead of attaching the same policy to each user, we attach the policy to a group and add users to that group.

---

## 8. IAM Roles

An **IAM role** is an identity with permissions that can be **assumed temporarily**.

Roles do not have permanent passwords or access keys.

Roles are used by:

- AWS services like EC2, Lambda, ECS, and CodeBuild.
- Users from another AWS account.
- Federated users from corporate identity providers.
- Applications needing temporary credentials.

Interview answer:

> An IAM role is used to grant temporary permissions to a trusted principal. Unlike IAM users, roles do not have long-term credentials. A principal assumes the role and receives temporary security credentials from AWS STS.

Memory:

```text
User = long-term identity
Role = temporary hat someone or something wears
```

---

## 9. Trust Policy vs Permission Policy

This is very important for interviews.

An IAM role usually has two policy ideas:

| Policy Type | Attached To | Purpose |
|---|---|---|
| Trust policy | Role | Who can assume the role |
| Permission policy | Role | What the role can do after being assumed |

Example:

```text
Trust policy:
EC2 can assume this role.

Permission policy:
This role can read objects from S3 bucket X.
```

Interview answer:

> A trust policy controls who is allowed to assume a role. A permission policy controls what actions are allowed after the role is assumed. Both must be correct for role-based access to work.

Memory:

```text
Trust policy = who can wear the hat
Permission policy = what the hat can do
```

---

## 10. AWS STS

**AWS STS**, or **Security Token Service**, issues temporary security credentials.

Temporary credentials include:

- Access key ID.
- Secret access key.
- Session token.
- Expiration time.

Common STS APIs:

- `AssumeRole`
- `AssumeRoleWithSAML`
- `AssumeRoleWithWebIdentity`
- `GetCallerIdentity`
- `GetSessionToken`

Interview answer:

> STS provides temporary credentials in AWS. When a user, service, or federated identity assumes an IAM role, STS returns temporary credentials that expire automatically.

Why temporary credentials are safer:

- They expire.
- They reduce risk from leaked credentials.
- They support role-based access.
- They are useful for cross-account and federated access.

---

## 11. IAM Policies

An IAM policy is a JSON document that defines permissions.

Basic policy structure:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": "s3:GetObject",
      "Resource": "arn:aws:s3:::my-bucket/*"
    }
  ]
}
```

Main fields:

| Field | Meaning |
|---|---|
| `Version` | Policy language version |
| `Statement` | One or more permission rules |
| `Effect` | `Allow` or `Deny` |
| `Action` | API operation, like `s3:GetObject` |
| `Resource` | AWS resource ARN |
| `Condition` | Optional restrictions |
| `Principal` | Who the statement applies to, mainly in resource policies |

Interview answer:

> IAM policies are JSON documents that define allowed or denied actions on AWS resources. They can be attached to identities like users, groups, and roles, or to resources like S3 buckets.

---

## 12. Allow and Explicit Deny

IAM has two important effects:

```text
Allow = permits action
Deny = blocks action
```

Important rule:

> An explicit deny always overrides an allow.

Example:

```text
Policy A allows s3:DeleteObject.
Policy B explicitly denies s3:DeleteObject.
Final result = Denied.
```

Interview answer:

> In IAM, access is denied by default. An explicit allow is required for access. If any matching policy has an explicit deny, the request is denied even if another policy allows it.

Memory:

```text
Default deny -> explicit allow -> explicit deny wins
```

---

## 13. IAM Policy Evaluation Logic

When a request is made, AWS evaluates policies in this simplified order:

```text
1. Default = deny.
2. Check if any explicit deny applies.
3. Check if an allow applies.
4. Also check boundaries, SCPs, session policies, and resource policies.
5. Final decision = allow only if all required layers permit it.
```

Better memory:

```text
AWS asks:
Who are you?
What are you trying to do?
On what resource?
Under what condition?
Is there any deny?
Is there a valid allow?
```

Interview answer:

> IAM starts with default deny. A request is allowed only when a matching policy allows it and no explicit deny applies. Additional controls like permissions boundaries, SCPs, resource policies, and session policies can further limit the final permission.

---

## 14. Identity-Based Policies

An **identity-based policy** is attached to an IAM identity:

- User
- Group
- Role

It answers:

```text
What can this identity do?
```

Example:

```text
This Lambda execution role can write logs to CloudWatch.
```

Interview answer:

> Identity-based policies are attached to users, groups, or roles and define what those identities can do in AWS.

---

## 15. Resource-Based Policies

A **resource-based policy** is attached to a resource.

It answers:

```text
Who can access this resource?
```

Common examples:

- S3 bucket policy.
- SQS queue policy.
- SNS topic policy.
- KMS key policy.
- Lambda function resource policy.
- Secrets Manager resource policy.

Resource-based policies include a `Principal`.

Example concept:

```text
Allow role from Account B to read objects from bucket in Account A.
```

Interview answer:

> Resource-based policies are attached directly to resources and specify which principals can access them. They are commonly used for cross-account access and service-to-service access.

---

## 16. Identity Policy vs Resource Policy

| Identity-Based Policy | Resource-Based Policy |
|---|---|
| Attached to user/group/role | Attached to resource |
| Says what identity can do | Says who can access resource |
| No `Principal` field normally | Has `Principal` field |
| Example: role can read S3 | Example: bucket allows role to read |

Interview answer:

> Identity policies define permissions from the identity side, while resource policies define permissions from the resource side. For cross-account access, we often need permission from both sides: the caller must be allowed to call the action, and the resource must trust or allow that caller.

---

## 17. Managed Policies and Inline Policies

### AWS Managed Policy

Created and maintained by AWS.

Example:

```text
AmazonS3ReadOnlyAccess
AdministratorAccess
```

Good for:

- Quick setup.
- Standard access patterns.
- Learning and testing.

### Customer Managed Policy

Created and managed by you.

Good for:

- Reusable custom permissions.
- Company-specific least privilege.
- Version control and review.

### Inline Policy

Embedded directly in one user, group, or role.

Good for:

- Very specific one-to-one permission.
- Cases where policy should not be reused.

Interview answer:

> AWS managed policies are maintained by AWS, customer managed policies are reusable policies created by us, and inline policies are embedded directly into a single identity. For production, customer managed policies are often preferred for controlled least-privilege access.

---

## 18. IAM Policy Conditions

Conditions restrict when a policy applies.

Common condition examples:

- Allow only from certain IP range.
- Require MFA.
- Allow only in a specific Region.
- Allow only if resource has a tag.
- Allow only if request uses TLS.
- Restrict based on time.

Example concept:

```json
"Condition": {
  "Bool": {
    "aws:MultiFactorAuthPresent": "true"
  }
}
```

Common condition keys:

| Condition Key | Use |
|---|---|
| `aws:SourceIp` | Restrict by IP |
| `aws:MultiFactorAuthPresent` | Require MFA |
| `aws:RequestedRegion` | Restrict Region |
| `aws:PrincipalTag` | ABAC with principal tags |
| `aws:ResourceTag` | ABAC with resource tags |
| `aws:SecureTransport` | Require HTTPS/TLS |

Interview answer:

> IAM conditions add context-based restrictions to permissions. For example, we can allow S3 access only when MFA is present, only from a corporate IP range, or only when resource tags match the user's department.

---

## 19. ARN

ARN means **Amazon Resource Name**.

It uniquely identifies AWS resources.

Format:

```text
arn:partition:service:region:account-id:resource
```

Example:

```text
arn:aws:s3:::my-bucket/*
arn:aws:lambda:ap-south-1:123456789012:function:ProcessOrder
arn:aws:iam::123456789012:role/AppServerRole
```

Interview answer:

> An ARN uniquely identifies an AWS resource and is used in IAM policies to specify which resources a permission applies to.

---

## 20. Least Privilege

Least privilege means:

> Give only the permissions required to perform the task, nothing more.

Bad example:

```text
Action: *
Resource: *
```

Better example:

```text
Allow only s3:GetObject on arn:aws:s3:::company-reports/*
```

Interview answer:

> Least privilege is the practice of granting only the minimum permissions required. It reduces blast radius if credentials are misused or compromised.

How to implement:

1. Start with required actions.
2. Restrict resources using ARNs.
3. Add conditions where useful.
4. Avoid wildcards unless justified.
5. Review CloudTrail and IAM Access Analyzer.
6. Remove unused permissions regularly.

---

## 21. MFA

MFA means **Multi-Factor Authentication**.

It adds a second factor besides password.

Use MFA for:

- Root user.
- Admin users.
- Sensitive operations.
- Role assumption for privileged access.

Interview answer:

> MFA improves account security by requiring a second authentication factor. It should always be enabled for the root user and privileged identities.

MFA condition use case:

```text
Allow deleting production resources only when MFA is present.
```

---

## 22. Access Keys

Access keys are long-term credentials used for AWS API, CLI, and SDK access.

They include:

- Access key ID.
- Secret access key.

Best practices:

1. Avoid root access keys.
2. Prefer roles and temporary credentials.
3. Rotate access keys.
4. Remove unused access keys.
5. Never hard-code keys in code.
6. Store secrets in secure systems.

Interview answer:

> Access keys are long-term programmatic credentials for IAM users. They should be avoided where possible in favor of IAM roles and temporary credentials. If used, they must be rotated and protected carefully.

---

## 23. IAM Roles for AWS Services

AWS services use IAM roles to access other AWS services.

Examples:

```text
EC2 instance role -> read S3 objects
Lambda execution role -> write CloudWatch logs and read DynamoDB
ECS task role -> call SQS
CodeBuild role -> pull source and push artifacts
```

Interview answer:

> Instead of storing access keys on EC2 or Lambda, we attach an IAM role. AWS automatically provides temporary credentials to the service, making it safer and easier to manage.

Example flow for EC2:

```text
EC2 instance profile
   |
IAM role
   |
Temporary credentials
   |
EC2 app calls AWS APIs
```

---

## 24. Instance Profile

An **instance profile** is a container for an IAM role that can be attached to an EC2 instance.

Interview answer:

> For EC2, we attach an IAM role using an instance profile. The application on the EC2 instance can then get temporary credentials from the instance metadata service.

Memory:

```text
EC2 cannot directly wear a role.
Instance profile helps EC2 wear the role.
```

---

## 25. Cross-Account Access

Cross-account access means a principal in one AWS account accesses resources in another AWS account.

Common pattern:

```text
Account A user/role -> assumes role in Account B -> accesses Account B resources
```

Required:

1. Role in target account has trust policy allowing source account principal.
2. Role has permission policy for target resources.
3. Source identity has permission to call `sts:AssumeRole`.

Interview answer:

> Cross-account access is commonly done using IAM roles and STS AssumeRole. The target account creates a role that trusts the source account, and the source principal must be allowed to assume that role.

Memory:

```text
Target account trusts.
Source account allows AssumeRole.
Role permissions define actual access.
```

---

## 26. External ID

External ID is used mainly when granting access to a third party.

Problem it helps prevent:

- Confused deputy problem.

Example:

```text
Company gives monitoring vendor access to AWS account.
Vendor assumes a role using an ExternalId condition.
```

Interview answer:

> External ID is a value used in a role trust policy when granting access to third parties. It helps prevent the confused deputy problem by ensuring the third party assumes the role only for the correct customer.

---

## 27. Federation

Federation lets users sign in using an external identity provider instead of IAM users.

Examples:

- Microsoft Entra ID / Azure AD.
- Okta.
- Google Workspace.
- Corporate SAML provider.
- OIDC provider.

Common AWS options:

- IAM Identity Center.
- SAML federation.
- OIDC federation.
- Cognito for application users.

Interview answer:

> Federation allows users to access AWS using existing corporate identities. Instead of creating IAM users for everyone, users authenticate with an identity provider and receive temporary AWS credentials or role-based console access.

---

## 28. IAM Identity Center

IAM Identity Center was formerly called AWS SSO.

It is used to centrally manage workforce access to:

- Multiple AWS accounts.
- AWS applications.
- Permission sets.
- External identity providers.

Interview answer:

> IAM Identity Center is the recommended way to manage human access across multiple AWS accounts. It integrates with identity providers and assigns permission sets instead of creating separate IAM users in every account.

Memory:

```text
IAM users = local account identities
IAM Identity Center = centralized workforce access
```

---

## 29. Permission Sets

Permission sets are used in IAM Identity Center.

They define what access a user or group gets in an AWS account.

Example:

```text
DeveloperReadOnly permission set -> assigned to Dev team -> applied to Sandbox account
```

Interview answer:

> A permission set in IAM Identity Center is a reusable collection of permissions that can be assigned to users or groups for one or more AWS accounts.

---

## 30. Service Control Policies

**SCPs**, or **Service Control Policies**, are used with AWS Organizations.

They set maximum permissions for accounts or organizational units.

Important:

- SCPs do not grant permissions by themselves.
- SCPs only limit what identities in an account can do.
- Even account administrators are restricted by SCPs.

Example:

```text
SCP denies leaving approved AWS Regions.
Even if IAM policy allows EC2 in another Region, final result is denied.
```

Interview answer:

> SCPs are organization-level guardrails. They define the maximum permissions available in member accounts, but they do not grant access. IAM policies still need to allow the action.

Memory:

```text
IAM policy grants.
SCP limits.
```

---

## 31. Permissions Boundary

A permissions boundary limits the maximum permissions an IAM user or role can get.

Important:

- It does not grant permissions.
- It limits identity-based permissions.
- Useful for delegated administration.

Example:

```text
Developer can create roles,
but permissions boundary prevents creating admin roles.
```

Interview answer:

> A permissions boundary is an advanced IAM control that defines the maximum permissions an identity can have. It is useful when delegating IAM creation to teams while preventing privilege escalation.

Difference from SCP:

| Permissions Boundary | SCP |
|---|---|
| Applied to IAM user or role | Applied to AWS account or OU |
| Limits identity permissions | Limits account maximum permissions |
| Used for delegation | Used for organization guardrails |

---

## 32. Session Policies

Session policies are policies passed when assuming a role or federating.

They reduce permissions for that session.

Important:

- They do not grant more permissions.
- They can only restrict the role's permissions.

Interview answer:

> Session policies limit the permissions of a temporary session. They are useful when the same role is assumed in different contexts and we want to restrict each session further.

---

## 33. Attribute-Based Access Control

**ABAC** means Attribute-Based Access Control.

It uses tags and attributes to control access.

Example:

```text
User has tag Department=Finance.
Resource has tag Department=Finance.
Policy allows access only when tags match.
```

Interview answer:

> ABAC uses attributes like user tags, role tags, and resource tags to make authorization decisions. It scales well when many teams or resources follow consistent tagging.

Memory:

```text
RBAC = access by role
ABAC = access by attribute/tag
```

---

## 34. Policy Variables

Policy variables make policies dynamic.

Example idea:

```text
Allow users to access only their own S3 folder based on username.
```

Conceptual example:

```text
arn:aws:s3:::company-home/${aws:username}/*
```

Interview answer:

> IAM policy variables allow dynamic values such as username, principal tags, or session tags inside policies, which helps create reusable access rules.

---

## 35. Resource-Level Permissions

Some AWS actions support resource-level permissions.

Example:

```text
s3:GetObject can be restricted to a bucket path.
```

Some actions require `Resource: "*"`.

Example:

```text
Some list or describe actions may not support specific resource ARNs.
```

Interview answer:

> Not every AWS action supports resource-level permissions. When an action does not support a specific resource ARN, the policy may need `Resource: "*"`, often combined with conditions to reduce risk.

---

## 36. IAM Access Analyzer

IAM Access Analyzer helps identify:

- Resources shared externally.
- Public access.
- Cross-account access.
- Unused access.
- Policy validation findings.

Interview answer:

> IAM Access Analyzer helps review and validate permissions. It can detect external access to resources, help generate least-privilege policies from activity, and identify unused permissions.

Use cases:

- Find public S3 bucket access.
- Detect cross-account resource sharing.
- Validate policy syntax and security warnings.
- Refine broad policies.

---

## 37. IAM Credential Report

An IAM credential report lists credential status for IAM users in an account.

It shows:

- Password enabled.
- MFA active.
- Access key age.
- Last used dates.
- Password rotation information.

Interview answer:

> IAM credential reports help audit IAM users, passwords, MFA usage, and access keys. They are useful for security reviews and identifying stale credentials.

---

## 38. CloudTrail and IAM

CloudTrail records AWS API activity.

Use CloudTrail to answer:

- Who made this change?
- Which role was used?
- Which API was called?
- From which IP?
- At what time?
- Was the request allowed or denied?

Interview answer:

> CloudTrail is essential for IAM auditing because it records AWS API calls and helps investigate who accessed or changed resources.

Common IAM-related events:

- `AssumeRole`
- `CreateUser`
- `AttachRolePolicy`
- `PutBucketPolicy`
- `CreateAccessKey`
- `DeleteRole`

---

## 39. `iam:PassRole`

`iam:PassRole` is a very important permission.

It allows a user or service to pass an IAM role to an AWS service.

Example:

```text
Developer creates Lambda function and passes LambdaExecutionRole to Lambda.
```

Why it matters:

- If too broad, it can cause privilege escalation.
- A user could pass a powerful role to a service they control.

Interview answer:

> `iam:PassRole` allows a principal to pass an IAM role to an AWS service. It must be restricted carefully because broad PassRole permissions can allow privilege escalation.

Best practice:

```text
Allow iam:PassRole only for specific roles and specific services.
```

---

## 40. Privilege Escalation in IAM

Privilege escalation means a user gains more permissions than intended.

Risky permissions:

- `iam:CreateAccessKey`
- `iam:AttachUserPolicy`
- `iam:AttachRolePolicy`
- `iam:PutUserPolicy`
- `iam:PassRole`
- `sts:AssumeRole`
- `lambda:CreateFunction` with powerful role.
- `cloudformation:*` with admin execution role.

Interview answer:

> IAM privilege escalation can happen when users can modify policies, create credentials, pass powerful roles, or assume privileged roles. We reduce this risk using least privilege, permissions boundaries, restricted PassRole, SCPs, and monitoring.

---

## 41. KMS Key Policies and IAM

AWS KMS is a common IAM interview trap.

KMS authorization can involve:

- KMS key policy.
- IAM policy.
- Grants.

Important:

> For KMS, the key policy is very important. IAM permission alone may not be enough if the key policy does not allow it.

Interview answer:

> KMS permissions are controlled by key policies, IAM policies, and grants. A principal generally needs permission through the key policy path and IAM policy path, depending on how the key policy is configured.

---

## 42. S3 Bucket Policy and IAM

For S3 access, final permission can depend on:

- IAM identity policy.
- S3 bucket policy.
- S3 Block Public Access.
- SCP.
- Permissions boundary.
- KMS key policy if object uses SSE-KMS.

Interview answer:

> For S3, an IAM policy may allow access, but a bucket policy, explicit deny, Block Public Access setting, SCP, or KMS key policy can still prevent access.

---

## 43. Service-Linked Roles

A service-linked role is a predefined IAM role linked to an AWS service.

Examples:

- Auto Scaling service-linked role.
- Elastic Load Balancing service-linked role.
- AWS Config service-linked role.

Interview answer:

> Service-linked roles are IAM roles created for AWS services to perform actions on your behalf. They have predefined trust and permission policies managed for that service.

---

## 44. IAM for Lambda

Lambda commonly uses two IAM concepts:

### Execution Role

The role Lambda assumes while running.

Example permissions:

- Write logs to CloudWatch.
- Read from DynamoDB.
- Get object from S3.

### Resource-Based Policy

Allows another service to invoke the Lambda function.

Example:

```text
API Gateway can invoke this Lambda function.
S3 can invoke this Lambda function on object upload.
```

Interview answer:

> Lambda's execution role controls what the function can do. Lambda's resource policy controls who can invoke the function.

---

## 45. IAM for EC2

Best practice:

> Do not store access keys on EC2. Attach an IAM role through an instance profile.

Flow:

```text
EC2 app -> instance metadata -> temporary credentials -> AWS API
```

Interview answer:

> EC2 should access AWS services through an IAM role attached using an instance profile. This avoids hard-coded credentials and gives the instance temporary credentials.

Security note:

> Use Instance Metadata Service Version 2, or IMDSv2, to reduce metadata credential theft risk.

---

## 46. IAM for ECS

ECS has two common roles:

| Role | Purpose |
|---|---|
| Task execution role | Used by ECS agent to pull images and write logs |
| Task role | Used by application code inside the container |

Interview answer:

> In ECS, the task execution role is for ECS platform actions like pulling images and sending logs, while the task role is what the application uses to call AWS APIs.

---

## 47. IAM for EKS

For EKS, important IAM concepts include:

- IAM roles for service accounts, also called IRSA.
- EKS Pod Identity.
- Node instance roles.

Interview answer:

> In EKS, workloads should use pod-level IAM access through IRSA or EKS Pod Identity instead of sharing broad node permissions. This gives each Kubernetes service account or pod only the AWS permissions it needs.

---

## 48. IAM for Applications

Best practice for applications:

```text
Use IAM roles and temporary credentials.
Avoid hard-coded access keys.
Use least privilege.
Separate environments.
Rotate secrets.
Monitor with CloudTrail.
```

Interview answer:

> Applications should use roles wherever possible. For example, Lambda uses an execution role, EC2 uses an instance profile, ECS tasks use task roles, and cross-account workloads use AssumeRole.

---

## 49. Authentication vs Authorization

Authentication:

```text
Who are you?
```

Authorization:

```text
What are you allowed to do?
```

IAM handles both identity and authorization, but interviewers often expect this distinction.

Interview answer:

> Authentication verifies identity, while authorization determines allowed actions. In IAM, users, roles, and federation identify the principal, while policies define authorization.

---

## 50. IAM vs Cognito

| IAM | Cognito |
|---|---|
| Workforce, admins, services, AWS resource access | Application end-user identity |
| Controls AWS API permissions | User sign-up, sign-in, tokens for apps |
| Used by developers, services, workloads | Used by customers or app users |
| Policies grant AWS actions | User pools manage app users |

Interview answer:

> IAM is mainly for controlling access to AWS resources by employees, workloads, and services. Cognito is for managing application users, sign-up, sign-in, and user tokens.

---

## 51. IAM vs IAM Identity Center

| IAM Users | IAM Identity Center |
|---|---|
| Created inside one AWS account | Central workforce access |
| Long-term credentials | Federated login and temporary access |
| Harder across many accounts | Designed for multi-account access |
| Older pattern for human users | Recommended for workforce access |

Interview answer:

> IAM users are account-level identities, while IAM Identity Center centrally manages workforce access across multiple AWS accounts and applications using permission sets.

---

## 52. IAM vs Resource Policies

This question often appears as:

> Why do I need a bucket policy if I already have IAM?

Answer:

> IAM policies control what the principal can do. Resource policies control who can access the resource. For same-account access, an IAM allow may be enough in many cases. For cross-account access, the resource usually needs to allow the external principal as well.

---

## 53. Common IAM Security Best Practices

Memorize this checklist:

1. Do not use root for daily tasks.
2. Enable MFA on root and privileged users.
3. Prefer IAM Identity Center for human access.
4. Prefer IAM roles over access keys.
5. Apply least privilege.
6. Use customer managed policies for reusable permissions.
7. Avoid wildcards where possible.
8. Use conditions for MFA, IP, tags, Region, or TLS.
9. Rotate and remove unused credentials.
10. Restrict `iam:PassRole`.
11. Use permissions boundaries for delegated IAM administration.
12. Use SCPs for organization guardrails.
13. Monitor with CloudTrail.
14. Review access with IAM Access Analyzer.
15. Remove unused users, roles, policies, and access keys.

Interview answer:

> IAM security is based on least privilege, MFA, temporary credentials, careful policy design, restricted privilege-escalation paths, and continuous monitoring.

---

## 54. Common IAM Request Flow

Example:

```text
Application running on Lambda wants to read DynamoDB.
```

Flow:

```text
1. Lambda assumes execution role.
2. STS provides temporary credentials.
3. Application calls DynamoDB GetItem.
4. AWS checks identity policy on Lambda role.
5. AWS checks resource policy if applicable.
6. AWS checks SCP, boundaries, session policies, and explicit denies.
7. If allowed, DynamoDB returns data.
```

Interview sentence:

> Every AWS API request is evaluated against the principal, action, resource, conditions, and all applicable policy layers.

---

## 55. Policy Evaluation Example

Scenario:

```text
User wants to delete an S3 object.
```

Policies:

```text
IAM policy allows s3:* on bucket.
Bucket policy denies s3:DeleteObject.
```

Result:

```text
Denied
```

Reason:

```text
Explicit deny overrides allow.
```

Another scenario:

```text
IAM policy allows ec2:RunInstances.
SCP denies ec2:* in us-east-1.
User tries to launch EC2 in us-east-1.
```

Result:

```text
Denied
```

Reason:

```text
SCP limits maximum permissions.
```

---

## 56. Common Interview Questions and Answers

### Q1. What is IAM?

IAM is AWS Identity and Access Management. It controls who can access AWS resources and what actions they can perform using users, groups, roles, and policies.

### Q2. Is IAM regional or global?

IAM is a global AWS service. IAM identities and policies are available across Regions.

### Q3. What is the root user?

The root user is the original AWS account identity with full access. It should be protected with MFA and not used for daily work.

### Q4. What is an IAM user?

An IAM user is a long-term identity in an AWS account. It can have console access, access keys, MFA, and permissions.

### Q5. What is an IAM group?

An IAM group is a collection of IAM users. Policies attached to the group apply to all users in that group.

### Q6. What is an IAM role?

An IAM role is an identity that can be assumed temporarily by users, AWS services, or external identities. It provides temporary credentials through STS.

### Q7. Difference between IAM user and IAM role?

An IAM user has long-term credentials. An IAM role is assumed temporarily and does not have permanent credentials. Roles are preferred for AWS services and cross-account access.

### Q8. What is a policy?

An IAM policy is a JSON document that defines allowed or denied actions on AWS resources.

### Q9. What is the difference between identity-based and resource-based policies?

Identity-based policies are attached to users, groups, or roles and define what they can do. Resource-based policies are attached to resources and define who can access them.

### Q10. What is explicit deny?

Explicit deny is a policy statement that blocks an action. It always overrides any allow.

### Q11. What is least privilege?

Least privilege means giving only the permissions required for a task and nothing extra.

### Q12. What is STS?

STS is AWS Security Token Service. It issues temporary credentials when assuming roles or using federation.

### Q13. What is AssumeRole?

`AssumeRole` is an STS API that lets a trusted principal assume an IAM role and receive temporary credentials.

### Q14. What is a trust policy?

A trust policy is attached to an IAM role and defines who is allowed to assume that role.

### Q15. What is a permissions policy?

A permissions policy defines what actions the identity or role can perform after authentication or role assumption.

### Q16. What is cross-account access?

Cross-account access allows a principal in one AWS account to access resources in another account, usually by assuming a role in the target account.

### Q17. What is External ID?

External ID is used in role trust policies for third-party access to prevent confused deputy problems.

### Q18. What is IAM Identity Center?

IAM Identity Center centrally manages workforce access to multiple AWS accounts and applications using users, groups, identity providers, and permission sets.

### Q19. What is an SCP?

An SCP is a Service Control Policy in AWS Organizations. It sets maximum permissions for accounts or OUs but does not grant permissions by itself.

### Q20. What is a permissions boundary?

A permissions boundary defines the maximum permissions an IAM user or role can have. It limits permissions but does not grant them.

### Q21. What is ABAC?

ABAC is Attribute-Based Access Control. It uses tags or attributes on users, roles, sessions, and resources to make access decisions.

### Q22. What is `iam:PassRole`?

`iam:PassRole` allows a principal to pass an IAM role to an AWS service. It must be restricted carefully to avoid privilege escalation.

### Q23. How do you secure IAM access keys?

Avoid them when possible, prefer roles, never hard-code keys, rotate them, monitor usage, and delete unused keys.

### Q24. Why should EC2 use IAM roles?

IAM roles give EC2 temporary credentials automatically, avoiding hard-coded access keys and simplifying permission management.

### Q25. What is an instance profile?

An instance profile is a container that allows an IAM role to be attached to an EC2 instance.

### Q26. What is the difference between task role and task execution role in ECS?

The task execution role is used by ECS to pull images and write logs. The task role is used by the application container to call AWS APIs.

### Q27. What is IAM Access Analyzer?

IAM Access Analyzer helps find external access, validate policies, detect unused permissions, and generate least-privilege policy recommendations.

### Q28. What is a credential report?

An IAM credential report is an account-level report showing password, MFA, access key, and credential usage details for IAM users.

### Q29. What happens if IAM allows but SCP denies?

The request is denied. SCPs define maximum permissions and explicit deny or lack of allow at the SCP layer can block actions.

### Q30. What happens if one policy allows and another explicitly denies?

The request is denied because explicit deny always wins.

---

## 57. Scenario-Based Answers

### Scenario 1: Give Lambda access to DynamoDB

Recommended:

```text
Create Lambda execution role.
Attach policy allowing only required DynamoDB actions on required table.
Lambda assumes role automatically.
```

Answer:

> I would create an IAM role for Lambda with a trust policy allowing Lambda to assume it. Then I would attach a least-privilege policy allowing only required actions like `dynamodb:GetItem` or `dynamodb:PutItem` on the specific table.

### Scenario 2: Give EC2 access to S3

Recommended:

```text
Create IAM role -> attach S3 least-privilege policy -> attach role to EC2 instance profile.
```

Do not:

```text
Store access keys on EC2.
```

Answer:

> I would attach an IAM role to the EC2 instance through an instance profile. The role would allow only the required S3 actions on the required bucket or prefix.

### Scenario 3: Third-party monitoring tool needs access

Recommended:

```text
Create cross-account role.
Trust vendor account.
Require ExternalId.
Attach read-only monitoring permissions.
```

Answer:

> I would create a role in our account that trusts the third-party AWS account and requires an External ID. Then I would attach least-privilege read-only permissions needed by the monitoring tool.

### Scenario 4: Developers need access to multiple AWS accounts

Recommended:

```text
Use IAM Identity Center.
Connect identity provider.
Create permission sets.
Assign groups to accounts.
```

Answer:

> I would use IAM Identity Center instead of creating IAM users in each account. Developers authenticate with the corporate identity provider and receive access through permission sets.

### Scenario 5: Prevent developers from creating admin roles

Recommended:

```text
Use permissions boundaries.
Restrict iam:PassRole.
Use SCPs if organization-wide control is needed.
```

Answer:

> I would use permissions boundaries to limit the maximum permissions of roles developers can create, restrict `iam:PassRole`, and optionally use SCPs as organization-level guardrails.

### Scenario 6: Allow users to access only resources tagged with their department

Recommended:

```text
Use ABAC with principal tags and resource tags.
```

Answer:

> I would use ABAC. For example, users or roles have a `Department` tag, resources have a `Department` tag, and the IAM policy allows access only when the tags match.

---

## 58. Common Traps Interviewers Ask

| Trap Question | Best Answer |
|---|---|
| Does IAM policy allow access by default? | No, access is denied by default. |
| Does explicit allow override explicit deny? | No, explicit deny always wins. |
| Does an SCP grant permissions? | No, SCPs only set maximum permissions. |
| Does a permissions boundary grant permissions? | No, it only limits maximum permissions. |
| Should root user be used daily? | No, root should be secured and rarely used. |
| Should access keys be stored on EC2? | No, use IAM roles and instance profiles. |
| Can roles have long-term credentials? | No, roles use temporary credentials. |
| Can groups contain roles? | No, groups contain IAM users only. |
| Is IAM regional? | No, IAM is global. |
| Is `iam:PassRole` harmless? | No, it can lead to privilege escalation if broad. |
| Is IAM enough for KMS access? | Not always; KMS key policy matters. |
| Should human users always be IAM users? | Prefer IAM Identity Center or federation for workforce users. |

---

## 59. Two-Minute Interview Explanation

Use this when the interviewer says: "Explain AWS IAM."

> AWS IAM, or Identity and Access Management, is the AWS service used to control access to AWS resources. It answers who can do what on which resource under what condition. IAM is global and is built around users, groups, roles, and policies. Users are long-term identities, groups help manage permissions for users, roles provide temporary credentials, and policies are JSON documents that allow or deny actions.
>
> The most important IAM principle is least privilege. By default, access is denied. A request is allowed only if a policy allows it and no explicit deny applies. Explicit deny always wins. IAM permissions can come from identity-based policies, resource-based policies, permissions boundaries, session policies, and organization-level SCPs.
>
> For applications and AWS services, we should prefer IAM roles instead of long-term access keys. For example, EC2 uses an instance profile, Lambda uses an execution role, and cross-account access usually uses STS AssumeRole. For human users, IAM Identity Center or federation is preferred in multi-account environments. Security best practices include MFA, avoiding root usage, least privilege, restricted `iam:PassRole`, temporary credentials, CloudTrail monitoring, Access Analyzer reviews, and regular cleanup of unused permissions.

---

## 60. Memory Map

Remember IAM using:

```text
P A R C
Principal, Action, Resource, Condition
```

Policy decision:

```text
Default Deny
Explicit Allow
Explicit Deny Wins
```

Core components:

```text
U G R P
User, Group, Role, Policy
```

Role memory:

```text
Trust policy = who can assume
Permission policy = what it can do
STS = gives temporary credentials
```

Security memory:

```text
M L T A C
MFA, Least privilege, Temporary credentials, Analyzer, CloudTrail
```

---

## 61. Quick Revision Sheet

Memorize these lines:

- IAM controls access to AWS resources.
- IAM is global.
- Root user has full account access and should not be used daily.
- IAM user has long-term credentials.
- IAM group is a collection of users.
- IAM role is assumed temporarily.
- STS issues temporary credentials.
- Policy is a JSON permission document.
- Default access is denied.
- Explicit deny always wins.
- Identity policy says what an identity can do.
- Resource policy says who can access a resource.
- Trust policy says who can assume a role.
- Permission policy says what the role can do.
- Least privilege is the most important IAM design principle.
- Prefer IAM roles over access keys.
- Prefer IAM Identity Center for workforce access.
- SCPs limit account permissions but do not grant access.
- Permissions boundaries limit identity permissions but do not grant access.
- `iam:PassRole` must be restricted carefully.
- Cross-account access commonly uses STS AssumeRole.
- External ID helps protect third-party cross-account access.
- ABAC uses tags for access control.
- CloudTrail audits API activity.
- IAM Access Analyzer helps review and validate access.

---

## 62. Best Final Answer Template

For any IAM interview question, answer in this format:

```text
1. Define the IAM concept.
2. Say where it is attached or used.
3. Explain how it affects access decisions.
4. Mention least privilege or security risk.
5. Give one real example.
```

Example:

> An IAM role is a temporary identity that can be assumed by trusted principals like AWS services, users, or external accounts. The role has a trust policy that defines who can assume it and permission policies that define what it can do. For example, a Lambda function can assume an execution role to write logs to CloudWatch and read from DynamoDB. Roles are safer than long-term access keys because AWS provides temporary credentials through STS.

---

## 63. Official References

- AWS IAM User Guide: https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html
- IAM identities: https://docs.aws.amazon.com/IAM/latest/UserGuide/id.html
- IAM policies: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html
- IAM policy evaluation logic: https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_policies_evaluation-logic.html
- IAM roles: https://docs.aws.amazon.com/IAM/latest/UserGuide/id_roles.html
- Temporary security credentials: https://docs.aws.amazon.com/IAM/latest/UserGuide/id_credentials_temp.html
- IAM best practices: https://docs.aws.amazon.com/IAM/latest/UserGuide/best-practices.html
- IAM Access Analyzer: https://docs.aws.amazon.com/IAM/latest/UserGuide/what-is-access-analyzer.html
- IAM Identity Center: https://docs.aws.amazon.com/singlesignon/latest/userguide/what-is.html
- AWS Organizations SCPs: https://docs.aws.amazon.com/organizations/latest/userguide/orgs_manage_policies_scps.html
