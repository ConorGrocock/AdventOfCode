using System.Text;
using System.Text.RegularExpressions;
using SharedUtils;

namespace Day6;

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

    private int Answer(string[] input)
    {
        var direction = 0;
        var directionPoint = new Point[]{ new(0, -1), new(1, 0), new(0, 1), new(-1, 0) };

        var startingPositionLine = input.First(line => line.Contains('^'));

        var startingPositionX = startingPositionLine.IndexOf('^');
        var startingPositionY = Array.IndexOf(input, startingPositionLine);

        var currentPosition = new Point(startingPositionX, startingPositionY);
        var nextPosition = currentPosition.Move(directionPoint[direction % directionPoint.Length]);
        while (nextPosition.IsWithinBounds(startingPositionLine.Length - 1, input.Length - 1))
        {
            // Set the current position to an X
            var currentLine = new StringBuilder(input[currentPosition.Y])
            {
                [currentPosition.X] = 'X'
            };
            input[currentPosition.Y] = currentLine.ToString();

            if (input[nextPosition.Y][nextPosition.X] == '#')
            {
                direction++;
                nextPosition = currentPosition.Move(directionPoint[direction % directionPoint.Length]);
            }

            currentPosition = nextPosition;

            input = SetPositionToChar(input, nextPosition, '^');

            nextPosition = currentPosition.Move(directionPoint[direction % directionPoint.Length]);
            DrawMap(input);
        }

        return input.Sum(line => line.Count(c => c == 'X')) + 1;
    }

    private void DrawMap(string[] input)
    {
        Console.SetCursorPosition(0,0);
        foreach (var line in input)
        {
            Console.WriteLine(line);
        }
        Thread.Sleep(10);
        // Console.ReadLine(); // Uncomment this line to step through the map
    }

    private string[] SetPositionToChar(string[] input, Point position, char c)
    {
        var line = new StringBuilder(input[position.Y])
        {
            [position.X] = c
        };
        input[position.Y] = line.ToString();
        return input;
    }
}