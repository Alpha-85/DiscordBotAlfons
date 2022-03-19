using System.Text.Json;
using AlfonsBot.Common.Extensions;
using AlfonsBot.Common.Files;
using AlfonsBot.Models;

namespace AlfonsBot.Services;

public class ComedyService
{
    public Random Random { get; }
    public HttpClient HttpClient { get; }
    public ComedyService(Random random, HttpClient httpClient)
    {
        Random = random ?? throw new ArgumentNullException(nameof(random));
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    private static readonly Dictionary<int, string> Dictionary = new();

    /// <summary>
    /// Returning random joke from a dictionary
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetDarkHumor()
    {
        if (Dictionary.Count is 0)
        {
            var i = 0;
            var str = Jokes.PopulateDictionary();
            var array = str.Split("#");
            array.ToList().ForEach(joke =>
            {
                i++; 
                Dictionary.Add(i,joke);

            });

        }

        Dictionary.TryGetValue(Random.Next(1, Dictionary.Count), out var value);

        return await Task.FromResult(value) ?? string.Empty;
    }

    /// <summary>
    /// Api call with application/json
    /// </summary>
    /// <returns></returns>
    public async Task<Joke> GetChuckNorrisJoke()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            "https://api.chucknorris.io/jokes/random");

        httpRequestMessage.AddDefaultHeader();

        var response = await HttpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode) return new Joke();

        var json = await response.Content.ReadAsStringAsync();

        var obj = JsonSerializer.Deserialize<Joke>(json);

        return obj ?? new Joke
        {
            IconUrl = "https://www.pngegg.com/en/png-xbgll",
            Id = "",
            Url = "",
            Value = "Internet service dies each time someone on your ISP tries to submit a dry Chuck Norris fact."
        };

    }

    /// <summary>
    /// Api call with plain/text
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetDadJoke()
    {

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            "https://icanhazdadjoke.com/");
        httpRequestMessage.Headers.Add("Accept", "text/plain");

        var response = await HttpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode) return string.Empty;

        var joke = await response.Content.ReadAsStringAsync();

        return joke;
    }

}
