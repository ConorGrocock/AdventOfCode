using System.Numerics;
using SharedUtils;

namespace Day9;

public class Part1 : ISolution
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


    private static readonly int GridWidth = 101;
    private static readonly int GridHeight = 103;

    private string Answer(string[] input)
    {
        var maxSeconds = 100;
        var robots = ParseInput(input);

        foreach (var robot in robots)
        {
            var robotPositions = new List<Point> { robot.GetPosition() };
            DrawGrid(robot, robotPositions.ToArray());

            for (var i = 0; i < maxSeconds; i++)
            {
                robot.Move();
                robotPositions.Add(robot.GetPosition());

                DrawGrid(robot, robotPositions.ToArray());
            }

            DrawGrid(robot, robotPositions.ToArray());
        }

        var quadrants = GetRobotsPerQuadrant(robots);

        var product = 1;
        for (var i = 0; i < quadrants.Length; i++)
        {
            product *= quadrants[i];
        }

        return product.ToString();
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

    private int[] GetRobotsPerQuadrant(Robot[] robots)
    {
        var robotPositions = robots.Select(r => r.GetPosition()).ToArray();

        var filteredPositions = robotPositions.Where(pos => pos.X != Math.Floor(GridWidth / 2.0) && pos.Y != Math.Floor(GridHeight / 2.0)).ToArray();

        var quadrants = new int[4];

        foreach (var position in filteredPositions)
        {
            if (position.X < Math.Floor(GridWidth / 2.0) && position.Y < Math.Floor(GridHeight / 2.0))
            {
                quadrants[0]++;
            }
            else if (position.X >= Math.Ceiling(GridWidth / 2.0) && position.Y < Math.Floor(GridHeight / 2.0))
            {
                quadrants[1]++;
            }
            else if (position.X < Math.Floor(GridWidth / 2.0) && position.Y >= Math.Ceiling(GridHeight / 2.0))
            {
                quadrants[2]++;
            }
            else if (position.X >= Math.Ceiling(GridWidth / 2.0) && position.Y >= Math.Ceiling(GridHeight / 2.0))
            {
                quadrants[3]++;
            }
        }

        return quadrants;
    }

    private void DrawGrid(Robot robot, Point[] robotPositions)
    {
        // Console.Clear();
        // Console.SetCursorPosition(0,0);
        // for (var y = 0; y < gridHeight; y++)
        // {
        //     for (var x = 0; x < gridWidth; x++)
        //     {
        //         Console.Write(".");
        //     }
        //
        //     Console.WriteLine();
        // }
        //
        // for (var i = 0; i < robotPositions.Length; i++)
        // {
        //     var position = robotPositions[i];
        //     Console.SetCursorPosition(position.X, position.Y);
        //     Console.Write(i);
        // }
        //
        // Console.SetCursorPosition(0, gridHeight + 1);
        // Console.WriteLine(robot);
        // Console.ReadKey();
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