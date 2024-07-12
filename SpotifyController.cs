using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

class SpotifyController
{

    private static string ?_clientId;
    private static string ?_clientSecret;
    private static string ?_accessToken;
    
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
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponseString)!;

            _accessToken = responseObject.access_token;

            Console.WriteLine($"Access Token: {responseObject.access_token}");

        }

    }
}