using SharedUtils;

namespace Day8;

public class Part2 : ISolution
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

    private int Answer(string[] input)
    {
        var signals = new Dictionary<char, List<Point>>();

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var signalFreq = input[y][x];
                if(signalFreq == '.') continue;

                if(!signals.ContainsKey(signalFreq))
                {
                    signals[signalFreq] = [];
                }
                signals[signalFreq].Add(new Point(x, y));
            }
        }

        var antinodes = new List<Point>();
        foreach (var (_, signalPoints) in signals)
        {
            foreach (var currentPoint in signalPoints)
            {
                antinodes.Add(currentPoint);
                var distanceToOtherPoints = signalPoints.Where(p => p != currentPoint).Select(p => p.Difference(currentPoint));

                foreach (var distanceToOtherPoint in distanceToOtherPoints)
                {
                    var potentialAntinode = currentPoint.Move(distanceToOtherPoint.X, distanceToOtherPoint.Y);
                    while (potentialAntinode.IsWithinBounds(input[0].Length - 1, input.Length - 1))
                    {
                        antinodes.Add(potentialAntinode);
                        potentialAntinode = potentialAntinode.Move(distanceToOtherPoint.X, distanceToOtherPoint.Y);
                    }
                }
            }
        }

        DrawMap(input, signals, antinodes.Where(p => p.IsWithinBounds(input[0].Length, input.Length)).ToArray());

        return antinodes
            .Where(p => p.IsWithinBounds(input[0].Length, input.Length))
            .DistinctBy(p => p.ToString())
            .Count();;
    }



    private void DrawMap(string[] input, Dictionary<char, List<Point>> signals, Point[] antiNodes)
    {
        Console.Clear();
        Console.SetCursorPosition(0,0);
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var line in input)
        {
            Console.WriteLine(line);
        }

        foreach (var signal in signals)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var point in signal.Value)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(signal.Key);
            }
        }

        foreach (var antiNode in antiNodes)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(antiNode.X, antiNode.Y);
            Console.Write("X");
            Console.ResetColor();
        }

        Console.SetCursorPosition(0, input.Length);
    }
}