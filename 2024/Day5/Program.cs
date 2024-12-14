using SharedUtils;

namespace Day3;

class Program
{
    static void Main(string[] args)
    {
        ISolution? program = null;

        var part = args[0];

        if (part == "part1")
        {
            program = new Part1();
        } else if (part == "part2")
        {
            program = new Part2();
        }

        if (program == null)
        {
            Console.WriteLine($"{ConsoleColor.Red}No part specified!");
        }

        var mode = args[1];

        Console.WriteLine($"Starting {part} in {mode}");

        var startTime = DateTime.Now.Millisecond;
        var score = mode switch
        {
            "sample" => program!.Sample(),
            "real" => program!.Real(),
            _ => program!.Custom(mode)
        };
        var endTime = DateTime.Now.Millisecond;
        var duration = TimeSpan.FromMilliseconds(endTime)- TimeSpan.FromMilliseconds(startTime);

        Console.WriteLine($"Complete {part} in {mode}. Performed in {duration.Duration()}");
        Console.WriteLine($"Result is {score}");
    }
}