class SpotifyWrapped
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    };

    static async Task Main(string[] args)
    {
        await SpotifyController.RequestAccessTokenAsync(sharedClient);
    }
}