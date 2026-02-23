using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() =>
        {
            Thread.Sleep(2000); // Simulate delay before cancellation
            cancellationTokenSource.Cancel();
        });

        try
        {
            await LongRunningOperationAsync(cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was canceled.");
        }
    }

    static async Task LongRunningOperationAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 5; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(1000); // Simulates work
            Console.WriteLine("Working...");
        }
    }
}