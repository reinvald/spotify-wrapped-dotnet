using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

class SpotifyController
{

    private static string ?_clientId;
    private static string ?_clientSecret;
    private static string ?_accessToken;
    private static string ?_authorizationCode;
    
    public static async Task RequestUserAccessAsync(HttpClient client) {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<SpotifyController>()
            .Build();

        _clientId = configuration["spotifyClientId"];

        if (_clientId is not null) {

            HttpResponseMessage response = await client.GetAsync($"https://accounts.spotify.com/authorize?client_Id={_clientId}&response_type=code&redirect_uri=http://localhost:9090");
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponseString);
        }
    }

    public static async Task RequestAccessToken(HttpClient client) {

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<SpotifyController>()
            .Build();

        _clientId = configuration["spotifyClientId"];
        _clientSecret = configuration["spotifyClientSecret"];
        _authorizationCode = configuration["spotifyAuthorizationCode"];

        if (_clientId is not null && _clientSecret is not null) {

            var data = new StringContent($"grant_type=authorization_code&redirect_uri=http://localhost:9090&code={_authorizationCode}" + _authorizationCode, Encoding.UTF8, "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_clientId + ":" + _clientSecret)));
            HttpResponseMessage response = await client.PostAsync("https://accounts.spotify.com/api/token", data);

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponseString)!;

            _accessToken = responseObject.access_token;
        }
    }

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
        }
    }

    public static async Task GetTopTracksAsync(HttpClient client)
    {
        // client.BaseAddress = new Uri("https://api.spotify.com/v1/me/top/tracks");
        Console.WriteLine(_accessToken);
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  _accessToken);
        HttpResponseMessage response = await client.GetAsync("https://api.spotify.com/v1/me/top/tracks");
        response.EnsureSuccessStatusCode();

        var jsonResponseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponseString);
        var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponseString)!;
    }
}