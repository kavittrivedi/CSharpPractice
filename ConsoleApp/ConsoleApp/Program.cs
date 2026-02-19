using System;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var sum = MathHelpers.Add(2, 3);
var pi = MathHelpers.Pi;

Console.WriteLine($"sum={sum}, pi={pi}");

Console.WriteLine(Person.GetName());
Person.name = "kavit";
Console.WriteLine(Person.GetName());
Console.WriteLine(Person.GetName());

// OOP demo
IAccount account = new SavingsAccount(openingBalance: 100m, interestRate: 0.05m);
account.Deposit(50m);
account.Withdraw(20m);

Console.WriteLine($"Account type: {account.GetType().Name}");
Console.WriteLine($"Balance (encapsulated read-only): {account.Balance}");

account = new CheckingAccount(openingBalance: 100m, overdraftLimit: 25m);
account.Withdraw(120m);

Console.WriteLine($"Account type: {account.GetType().Name}");
Console.WriteLine($"Balance: {account.Balance}");