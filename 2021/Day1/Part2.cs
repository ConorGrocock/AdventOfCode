using SharedUtils;

namespace Day1;

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

    public int Answer(string[] fileContents)
    {
        var result = 0;
        var depths = fileContents.Select(int.Parse).ToArray();

        var slidingWindow = new List<int>();

        for (var i = 2; i < depths.Length; i++)
        {
            slidingWindow.Add(depths[i - 2] + depths[i - 1] + depths[i]);
        }

        for (var i = 1; i < slidingWindow.Count; i++)
        {
            if(slidingWindow[i] > slidingWindow[i - 1]) result++;
        }

        return result;
    }

}