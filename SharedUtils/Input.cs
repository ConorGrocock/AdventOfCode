namespace SharedUtils;

public class Input
{
    public static string[] ReadAsLines()
    {
        var lines = new List<string>();
        using (var sr = new StreamReader("inputFile.txt"))
        {
            while (sr.ReadLine() is { } line)
            {
                lines.Add(line);
            }
        }

        return lines.ToArray();
    }
    
    
    public static string[] TestInput(string inputFileName = "testFile")
    {
        var lines = new List<string>();
        using (var sr = new StreamReader($"input-{inputFileName}.txt"))
        {
            while (sr.ReadLine() is { } line)
            {
                lines.Add(line);
            }
        }

        return lines.ToArray();
    }

    public static string ReadSingleLine(string inputFileName = "sample")
    {
        string fileValue;

        using var sr = new StreamReader($"input-{inputFileName}.txt");
        fileValue = sr.ReadToEnd();

        return fileValue;
    }
}