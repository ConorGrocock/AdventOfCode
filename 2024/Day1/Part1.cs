using SharedUtils;

namespace Day1;

public class Part1: ISolution
{
    public string Sample()
    {
        return Custom("sample");
    }

    public string Real()
    {
        return Custom("real");
    }

    public string Custom(string fileName)
    {
        var fileInput = Input.TestInput(fileName);
        return Answer(fileInput);
    }

    public string Answer(string[] fileContents)
    {
        var result = 0;
        var leftColumn = new List<int>();
        var rightColumn = new List<int>();

        foreach (var fileLine in fileContents)
        {
            var lineParts = fileLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var firstItem = int.Parse(lineParts[0]);
            var secondItem = int.Parse(lineParts[1]);

            leftColumn.Add(firstItem);
            rightColumn.Add(secondItem);
        }

        leftColumn.Sort();
        rightColumn.Sort();

        for (var i = 0; i < leftColumn.Count; i++)
        {
            var left = leftColumn[i];
            var right = rightColumn[i];
            var difference = Math.Abs(left - right);
            result += difference;
        }

        return result.ToString();
    }
}