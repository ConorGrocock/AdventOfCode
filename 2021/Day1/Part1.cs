using SharedUtils;

namespace Day1;

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
        var fileInput = Input.TestInput(fileName);
        return Answer(fileInput);
    }

    public int Answer(string[] fileContents)
    {
        var result = 0;
        var depths = fileContents.Select(int.Parse).ToArray();

        for (var i = 1; i < depths.Length; i++)
        {
            if (depths[i] > depths[i - 1]) result++;
        }

        return result;
    }
}