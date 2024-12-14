using System.Text;

namespace SharedUtils;

public static class StringExtensions
{
    public static string[] SplitAndTrim(this string input, char seperator)
    {
        return input.Split(seperator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
    public static char[] SplitAndTrim(this string input)
    {
        return input.ToCharArray();
    }

    public static Dictionary<char, int> FrequencyMap(this string input)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (var c in input.AsSpan())
        {
            if (dictionary.TryGetValue(c, out var value))
            {
                dictionary[c] = value + 1;
            }
            else
            {
                dictionary.Add(c, 1);
            }
        }

        return dictionary;
    }

    public static string ReplaceAt(this string input, int index, char newChar)
    {
        return new StringBuilder(input)
        {
            [index] = newChar
        }.ToString();
    }

    public static string Repeat(this string input, int count, string? separator = null)
    {
        return string.Join(separator ?? string.Empty, Enumerable.Repeat(input, count).ToList());
    }
}