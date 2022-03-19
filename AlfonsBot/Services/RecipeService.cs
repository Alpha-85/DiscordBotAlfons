using AlfonsBot.Common.Extensions;
using AlfonsBot.Common.Settings;
using AlfonsBot.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AlfonsBot.Services;

public class RecipeService
{
    public HttpClient HttpClient { get; }
    private readonly UserSettings _userSettings;

    public RecipeService(HttpClient httpClient, IOptions<UserSettings> userSettings)
    {
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _userSettings = userSettings.Value ?? throw new ArgumentNullException(nameof(userSettings));
    }

    /// <summary>
    ///  Authenticating to a api to get a jwtToken
    /// </summary>
    /// <returns></returns>

    public async Task<AuthenticateResponse> Authenticate()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"https://recipehg.azurewebsites.net/api/v1/users/authenticate?username={_userSettings.UserName}&password={_userSettings.Password}");

        httpRequestMessage.AddDefaultHeader();

        var response = await HttpClient.SendAsync(httpRequestMessage);

        var json = await response.Content.ReadAsStringAsync();

        var obj = JsonSerializer.Deserialize<AuthenticateResponse>(json);

        return obj ?? new AuthenticateResponse();

    }

    /// <summary>
    /// Asking the random endpoint for recipes with token in header, returns a list of 3 recipes.
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="mealType"></param>
    /// <returns></returns>

    public async Task<Recipe> GetRandomRecipe(string accessToken, string mealType,string preference)
    {
        var queryString = GetQueryString(mealType,preference);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"https://recipehg.azurewebsites.net/api/v1/recipe/{queryString}");

        httpRequestMessage.AddAuthenticationBearer(accessToken);

        var response = await HttpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode) return new Recipe();

        var json = await response.Content.ReadAsStringAsync();

        var obj = JsonSerializer.Deserialize<List<Recipe>>(json);

        var recipe = new Recipe
        {
            Image = obj.FirstOrDefault().Image,
            Title = obj.FirstOrDefault().Title,
            SourceUrl = obj.FirstOrDefault().SourceUrl
        };

        return recipe;
    }

    public Task<string> PreferenceFinder(string mealType)
    {
        if (string.IsNullOrEmpty(mealType)) return Task.FromResult(string.Empty);

        return mealType switch
        {
            "beef" => Task.FromResult("Beef"),
            "pork" => Task.FromResult("Pork"),
            "chicken" => Task.FromResult("Chicken"),
            "vegetarian" => Task.FromResult("Vegetarian"),
            "dessert" => Task.FromResult("Dessert"),
            "breakfast" => Task.FromResult("Breakfast"),
            _ => Task.FromResult(string.Empty)
        };
    }

    private static string GetQueryString(string mealType,string preference)
    {
        var sb = new StringBuilder();
        sb.Append($"?MealType={mealType}");
        sb.Append($"&Preference={preference}");
        sb.Append("&Allergies.IsMilk=false");
        sb.Append("&Allergies.IsGluten=false");
        sb.Append("&Allergies.IsNuts=false");
        sb.Append("&Allergies.IsEgg=false");
        sb.Append("&Allergies.IsShellfish=false");

        return sb.ToString();
    }

}
