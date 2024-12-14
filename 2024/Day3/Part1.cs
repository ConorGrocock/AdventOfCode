using System.Text.RegularExpressions;
using SharedUtils;

namespace Day3;

public class Part1: ISolution
{
    public int Sample()
    {
        return Custom("sample");
    }

    public int Real()
    {
        return Custom("real");
    }

    public int Custom(string fileName)
    {
        var fileInput = Input.ReadSingleLine(fileName);
        return Answer(fileInput);
    }

    public int Answer(string fileContents)
    {
        var regexMatcher = @"mul\((\d+),(\d+)\)";
        RegexOptions options = RegexOptions.Multiline;
        var result = 0;

        foreach (Match m in Regex.Matches(fileContents, regexMatcher, options))
        {
            result += int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
        }

        return result;
    }
}