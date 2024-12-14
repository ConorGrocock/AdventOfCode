using SharedUtils;

namespace Day2;

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
        foreach (var line in fileContents)
        {
            var reportSteps = line.Split(' ').Select(int.Parse).ToArray();
            var isValidReport = true;
            var isIncreasing = true;


            var direction = reportSteps[1] - reportSteps[0];
            if (direction < 0)
            {
                isIncreasing = false;
            }

            for (var i = 1; i < reportSteps.Length; i++)
            {
                var difference = reportSteps[i] - reportSteps[i - 1];
                switch (isIncreasing)
                {
                    case true when difference is < 1 or > 3:
                    case false when difference is > -1 or < -3:
                        isValidReport = false;
                        break;
                }
            }

            if (isValidReport)
            {
                result++;
            }
        }

        return result;
    }
}