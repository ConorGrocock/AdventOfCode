using System.Text.RegularExpressions;
using SharedUtils;

namespace Day3;

public class Part2: ISolution
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
        const string regexMatcher = @"do\(\)|don't\(\)|mul\((\d+),(\d+)\)";
        const RegexOptions options = RegexOptions.Multiline;
        var result = 0;

        var shouldActionNextMatch = true;

        foreach (Match m in Regex.Matches(fileContents, regexMatcher, options))
        {
            if(m.Groups[0].Value == "do()")
            {
                shouldActionNextMatch = true;
                continue;
            }
            if(m.Groups[0].Value == "don't()")
            {
                shouldActionNextMatch = false;
                continue;
            }

            if(!shouldActionNextMatch)
            {
                continue;
            }

            result += int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
        }

        return result;
    }
}