using System;
using System.Text;

public static class ApiKeyGenerator
{
    public static string GenerateApiKey(int length = 32)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var apiKey = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            apiKey.Append(chars[random.Next(chars.Length)]);
        }

        return apiKey.ToString();
    }
}
