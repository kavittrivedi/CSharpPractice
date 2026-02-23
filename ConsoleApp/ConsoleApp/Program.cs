using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var tasks = new[]
        {
            GetDataAsync("https://jsonplaceholder.typicode.com/posts/1"),
            GetDataAsync("https://jsonplaceholder.typicode.com/users/1")
        };

        var results = await Task.WhenAll(tasks);
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }

    static async Task<string> GetDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }
}