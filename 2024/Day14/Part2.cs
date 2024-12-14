using System.Numerics;
using SharedUtils;

namespace Day9;

public class Part2 : ISolution
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


    private const int GridWidth = 101;
    private const int GridHeight = 103;

    private string Answer(string[] input)
    {
        const int maxSeconds = 10_000;
        var robots = ParseInput(input);

        for (var i = 0; i < maxSeconds; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
            }

            if (AreRobotsInLine(robots, 30))
            {
                DrawGrid(robots.Select(r => r.GetPosition()).ToArray(), i);
            }
        }

        return string.Empty;
    }

    private bool AreRobotsInLine(Robot[] robots, int minimumInLine)
    {
        var positions = robots.Select(r => r.GetPosition()).ToArray();
        var yPositions = positions.GroupBy(p => p.Y).Where(grouping => grouping.Count() >= minimumInLine).ToArray();

        return yPositions.Length > 0;
    }

    private Robot[] ParseInput(string[] lines)
    {
        var robots = new Robot[lines.Length];
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var parts = line.Split(" ");
            var position = parts[0].Replace("p=", "").Split(",");
            var velocity = parts[1].Replace("v=", "").Split(",");

            var x = int.Parse(position[0]);
            var y = int.Parse(position[1]);
            var vx = int.Parse(velocity[0]);
            var vy = int.Parse(velocity[1]);
            robots[i] = new Robot(new Point(x, y), new Point(vx, vy));
        }

        return robots;
    }

    private void DrawGrid(Point[] robotPositions, int index)
    {
        Console.SetCursorPosition(0,0);
        foreach (var position in robotPositions)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write("#");
        }

        Console.SetCursorPosition(0, GridHeight + 1);
        Console.WriteLine(index);

        Console.ReadLine();

        foreach (var position in robotPositions)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
        }


    }

    private class Robot
    {
        private Point _currentPosition;
        private readonly Point _currentVelocity;

        public Robot(Point position, Point velocity)
        {
            _currentPosition = position;
            _currentVelocity = velocity;
        }

        public void Move()
        {
            var newX = _currentPosition.X + _currentVelocity.X;
            var newY = _currentPosition.Y + _currentVelocity.Y;

            if (newX < 0) newX = GridWidth + newX;
            if (newX >= GridWidth) newX -= GridWidth;

            if (newY < 0) newY = GridHeight + newY;
            if (newY >= GridHeight) newY -= GridHeight;

            _currentPosition = new Point(newX, newY);
        }

        public Point GetPosition()
        {
            return _currentPosition;
        }

        public override string ToString()
        {
            return $"Robot - Position: {_currentPosition} Velocity: {_currentVelocity}";
        }
    }
}