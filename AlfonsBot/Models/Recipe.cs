using System.Text.Json.Serialization;

namespace AlfonsBot.Models;

public class Recipe
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("sourceUrl")]
    public string SourceUrl { get; set; }
    [JsonPropertyName("image")]
    public string Image { get; set; }

}
