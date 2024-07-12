using System;
using Microsoft.Extensions.Configuration;

class SpotifyController
{

    private static string ?_clientId;
    private static string ?_clientSecret;
    
    public static async Task RequestAccessTokenAsync(HttpClient client)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<SpotifyController>()
            .Build();

        _clientId = configuration["spotifyClientId"];
        _clientSecret = configuration["spotifyClientSecret"];

        if (_clientId is not null && _clientSecret is not null) {
            var contentMap = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "client_credentials" }
            };

            var content = new FormUrlEncodedContent(contentMap);
            HttpResponseMessage response = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.Write(responseString);
        }

    }
}