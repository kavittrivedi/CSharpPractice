using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting data fetch...");

        var postData = await GetPostDataAsync(1);
        Console.WriteLine($"Post Data: {postData}");

        var userData = await GetUserDataAsync(1);
        Console.WriteLine($"User Data: {userData}");

        Console.WriteLine("All data fetched.");
    }

    static async Task<string> GetPostDataAsync(int postId)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/posts/{postId}");
            return response;
        }
    }

    static async Task<string> GetUserDataAsync(int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
            return response;
        }
    }
}