using SharedUtils;

namespace Day2;

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
        var reports = fileContents.Select(line => line.Split(' ').Select(int.Parse).ToArray()).ToArray();
        return reports.Count(IsReportValidEnough);
    }

    private static bool IsReportValidEnough(int[] report)
    {
        return Enumerable.Range(0, report.Length)
            .Any(unsafeIndex =>
                IsReportValid(report.Where((_, index) => index != unsafeIndex).ToArray())
            );
    }
    private static bool IsReportValid(int[] report)
    {
        var deltas = report.Zip(report.Skip(1), (a, b) => a - b);
        var enumerable = deltas.ToList();
        return enumerable.All(delta => delta is >= 1 and <= 3) || enumerable.All(delta => delta is <= -1 and >= -3);
    }
}