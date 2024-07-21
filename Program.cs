class SpotifyWrapped
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    };

    static async Task Main(string[] args)
    {
        //await SpotifyController.RequestAccessTokenAsync(sharedClient);
        //await SpotifyController.GetTopTracksAsync(sharedClient);
        //await SpotifyController.RequestUserAccessAsync(sharedClient);
        await SpotifyController.RequestAccessToken(sharedClient);
    }
}