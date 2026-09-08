The **Observer Pattern** is a design pattern where **one object automatically notifies other objects when something changes**.

Think of it like a **YouTube subscription**:

* **YouTube Channel** = Subject/Publisher
* **Subscribers** = Observers
* Channel uploads a new video → all subscribers are notified automatically.

### Simple software example

Imagine your **Support Ticket System**. When a ticket status changes to `Resolved`, several things may need to happen:

```text
Ticket Status Changed
        |
        ↓
   Notify Observers
     /    |     \
    ↓     ↓      ↓
 Email   SMS    Audit Log
```

Instead of the Ticket service directly knowing how to send emails, SMS, and write audit logs, these components **subscribe** to ticket-status changes.

For example:

```csharp
public interface IObserver
{
    void Update(string status);
}

public class EmailNotifier : IObserver
{
    public void Update(string status)
    {
        Console.WriteLine($"Email: Ticket status changed to {status}");
    }
}

public class SmsNotifier : IObserver
{
    public void Update(string status)
    {
        Console.WriteLine($"SMS: Ticket status changed to {status}");
    }
}
```

The publisher maintains its subscribers:

```csharp
public class Ticket
{
    private readonly List<IObserver> _observers = [];

    public void Subscribe(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void ChangeStatus(string status)
    {
        // Change ticket status...

        foreach (var observer in _observers)
        {
            observer.Update(status);
        }
    }
}
```

Now:

```csharp
var ticket = new Ticket();

ticket.Subscribe(new EmailNotifier());
ticket.Subscribe(new SmsNotifier());

ticket.ChangeStatus("Resolved");
```

Both observers automatically receive the update.

### Why use Observer Pattern?

The main advantage is **loose coupling**. The `Ticket` doesn't need to know the details of sending emails, SMS, etc. You can add another observer later, such as `TeamsNotifier`, without significantly changing the Ticket logic.

### Interview answer

> **Observer Pattern is a behavioral design pattern where multiple objects subscribe to another object and are automatically notified when its state changes. A common example is a notification system where email, SMS, and other services subscribe to an event and react when that event occurs.**

**Easy way to remember:**
**Publisher → Event happens → Notify all Subscribers.**


Yes. A **workspace booking system** gives you a very natural Observer Pattern scenario. Just present it as a representative implementation only if it matches what your system actually did.

For example, you can use **booking cancellation → notify interested components**.

Yes, I used the Observer Pattern in a workspace booking system.

For example, when a user cancelled a workspace booking, multiple actions needed to happen. We needed to notify the user, update workspace availability, and potentially notify other interested components.

Instead of putting all this logic directly inside the booking component, we raised a booking cancellation event. Different handlers subscribed to that event and performed their respective actions.

This kept the booking logic loosely coupled from notification and other downstream functionality, and it also made it easier to add new handlers later without changing the core booking logic.

If the interviewer asks **“What were the Subject and Observers?”**, you can explain:

```text
Booking Service / Booking
        │
        │ BookingCancelled Event
        ↓
 ┌──────────────┬──────────────────┬─────────────────┐
 ↓              ↓                  ↓
Email Handler   Availability       Audit Handler
                Handler
```

So:

**Subject/Publisher:** Booking component
**Event:** `BookingCancelled`
**Observers:** Email notification, availability update, audit logging, etc.

One thing I'd avoid saying is **“we used Observer Pattern”** if the actual implementation was purely Azure Service Bus messaging. In that case, say **“we used a publish/subscribe approach based on the same observer-style principle.”** That distinction can matter in a design-pattern interview.


Extra Content -> For an interview,  **realistic project scenario** 

Based on your .NET experience, you can answer like this:

> **“Yes, I have used the Observer Pattern in event-driven and notification scenarios. For example, when a business event occurs, such as a ticket status change or an order update, multiple components may need to react to it—such as sending an email, updating an audit log, or triggering another process. Instead of tightly coupling all these operations, we publish an event and different handlers subscribe to it. In .NET, I have used similar concepts through events, event handlers, and message-based architectures.”**

If the interviewer asks **“Where exactly is Observer Pattern in .NET?”**, you can say:

> **“The most common example is C# events and delegates. An object publishes an event, and multiple event handlers subscribe to that event and get notified when it occurs.”**

For example:

```csharp
public event EventHandler? TicketResolved;
```

Subscribers:

```csharp
ticket.TicketResolved += SendEmail;
ticket.TicketResolved += WriteAuditLog;
```

When raised:

```csharp
TicketResolved?.Invoke(this, EventArgs.Empty);
```

So the interview connection is:

**Observer Pattern → Publisher/Subscriber → C# events & delegates → multiple handlers react to a change.**

One important distinction: if you mention **Azure Service Bus**, say it uses a similar **publish/subscribe idea**, but don't say Service Bus itself *is* the GoF Observer Pattern. Observer is typically an in-process object design pattern, while Service Bus provides distributed messaging.


