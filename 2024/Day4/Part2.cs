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
        var fileInput = Input.TestInput(fileName);
        return Answer(fileInput);
    }

    public int Answer(string[] input)
    {
        var result = 0;

        var dx = new[] { 1, 1, -1, -1 };
        var dy = new[] { 1, -1, 1, -1 };

        for (var y = 1; y <= input.Length - 2; y++)
        {
            for (var x = 1; x <= input[0].Length - 2; x++)
            {
                if (input[y][x] != 'A')
                    continue;

                var corners = Enumerable.Range(0, 4).Select(i => input[y + dy[i]][x + dx[i]]).ToArray();

                if (corners.All(n => n is 'M' or 'S') && corners[0] != corners[3] && corners[1] != corners[2])
                    result += 1;
            }
        }

        return result;
    }

    int CharAt(string[] input, int x, int y)
    {
        if (x < input.Length && x >= 0 && y < input[x].Length && y >= 0)
        {
            return input[x][y];
        }
        return -1;
    }

    bool SequenceEquals(string expected, params int[] chars)
    {
        if (expected.Length != chars.Length)
        {
            return false;
        }

        return !expected.Where((t, i) => t != chars[i]).Any();
    }
}