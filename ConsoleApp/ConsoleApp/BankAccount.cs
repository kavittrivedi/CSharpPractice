using System;

public interface IAccount
{
    decimal Balance { get; }
    void Deposit(decimal amount);
    void Withdraw(decimal amount);
}

public class BankAccount : IAccount
{
    protected decimal _balance;

    public decimal Balance => _balance;

    public BankAccount(decimal openingBalance = 0m)
    {
        if (openingBalance < 0m) throw new ArgumentOutOfRangeException(nameof(openingBalance));
        _balance = openingBalance;
    }

    public virtual void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (amount > _balance) throw new InvalidOperationException("Insufficient funds.");
        _balance -= amount;
    }
}

public sealed class SavingsAccount : BankAccount
{
    public decimal InterestRate { get; }

    public SavingsAccount(decimal openingBalance, decimal interestRate) : base(openingBalance)
    {
        if (interestRate < 0m) throw new ArgumentOutOfRangeException(nameof(interestRate));
        InterestRate = interestRate;
    }

    public void ApplyMonthlyInterest()
    {
        var interest = _balance * (InterestRate / 12m);
        if (interest > 0m)
        {
            _balance += interest;
        }
    }
}

public sealed class CheckingAccount : BankAccount
{
    public decimal OverdraftLimit { get; }

    public CheckingAccount(decimal openingBalance, decimal overdraftLimit) : base(openingBalance)
    {
        if (overdraftLimit < 0m) throw new ArgumentOutOfRangeException(nameof(overdraftLimit));
        OverdraftLimit = overdraftLimit;
    }

    public override void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        // Polymorphic behavior: checking can go negative up to the overdraft limit.
        if (amount > _balance + OverdraftLimit)
            throw new InvalidOperationException("Overdraft limit exceeded.");

        _balance -= amount;
    }
}