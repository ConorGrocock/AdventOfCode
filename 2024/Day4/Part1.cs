using System.Text.RegularExpressions;
using SharedUtils;

namespace Day3;

public partial class Part1: ISolution
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

    public int Answer(string[] fileContents)
    {
        var result = 0;

        // Search for the string XMAS, vertically, horizontally, and diagonally
        for (var i = 0; i < fileContents.Length; i++)
        {
            for (var j = 0; j < fileContents[i].Length; j++)
            {
                // Origin point
                var c0 = CharAt(fileContents, i, j);

                // Horizontal chars
                var ch1 = CharAt(fileContents, i, j + 1);
                var ch2 = CharAt(fileContents, i, j + 2);
                var ch3 = CharAt(fileContents, i, j + 3);

                // Vertical chars
                var cv1 = CharAt(fileContents, i + 1, j);
                var cv2 = CharAt(fileContents, i + 2, j);
                var cv3 = CharAt(fileContents, i + 3, j);

                // Diagonal right chars
                var cdr1 = CharAt(fileContents, i + 1, j + 1);
                var cdr2 = CharAt(fileContents, i + 2, j + 2);
                var cdr3 = CharAt(fileContents, i + 3, j + 3);

                // Diagonal left chars
                var cdl1 = CharAt(fileContents, i + 1, j - 1);
                var cdl2 = CharAt(fileContents, i + 2, j - 2);
                var cdl3 = CharAt(fileContents, i + 3, j - 3);

                if (SequenceEquals("XMAS", c0, ch1, ch2, ch3) || SequenceEquals("SAMX", c0, ch1, ch2, ch3))
                {
                    result++;
                }

                if (SequenceEquals("XMAS", c0, cv1, cv2, cv3) || SequenceEquals("SAMX", c0, cv1, cv2, cv3))
                {
                    result++;
                }

                if (SequenceEquals("XMAS", c0, cdr1, cdr2, cdr3) || SequenceEquals("SAMX", c0, cdr1, cdr2, cdr3))
                {
                    result++;
                }

                if (SequenceEquals("XMAS", c0, cdl1, cdl2, cdl3) || SequenceEquals("SAMX", c0, cdl1, cdl2, cdl3))
                {
                    result++;
                }
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