using System.Net.Http.Headers;

namespace AlfonsBot.Common.Extensions;

public static class HttpRequestExtensions
{
    public static void AddAuthenticationBearer(this HttpRequestMessage httpRequestMessage, string accessToken)
    {
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
    }

    public static void AddDefaultHeader(this HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
