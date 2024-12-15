using System.Numerics;
using System.Text.RegularExpressions;
using SharedUtils;

namespace Day9;

public class Part1 : ISolution
{
    private static RenderMode _renderMode = RenderMode.DELAY;

    public string Sample()
    {
        return Custom("sample");
    }

    public string Real()
    {
        _renderMode = RenderMode.DISABLED;
        return Custom("real");
    }

    public string Custom(string fileName)
    {
        var fileInput = Input.TestInput(fileName);
        return Answer(fileInput);
    }

    private string Answer(string[] input)
    {
        var (grid, directions) = ParseInput(input);

        var cursor = new Point(0, 1);
        grid.Render(_renderMode, cursor, directions.ToArray());

        while (directions.TryDequeue(out var direction))
        {
            grid.MakeMove(direction);
            grid.Render(_renderMode, cursor, directions.ToArray());
        }



        return grid.CalculateGps().ToString();
    }

    private static (Grid, Queue<Direction>) ParseInput(string[] input)
    {
        var gridInput = input.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var grid = new Grid(gridInput);

        var instructions = input.Skip(gridInput.Length + 1).ToArray();
        var directionStack = ParseDirection(instructions);

        return (grid, directionStack);
    }

    private static Queue<Direction> ParseDirection(string[] input)
    {
        var directions = new List<Direction>();
        foreach (var line in input)
        {
            directions.AddRange(line.Select(DirectionExtensions.FromAngledChar));
        }

        return new Queue<Direction>(directions);
    }


    private class Grid
    {
        private readonly int _width;
        private readonly int _height;

        private readonly Robot _robot;
        private readonly List<Package> _packages;
        private readonly List<Vec2L> _walls;

        public Grid(string[] input)
        {
            _height = input.Length;
            _width = input[0].Length;

            _packages = [];
            _walls = [];

            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[0].Length; x++)
                {
                    var gridChar = input[y][x];
                    switch (gridChar)
                    {
                        case '#':
                            _walls.Add(new Vec2L(x, y));
                            break;
                        case '@':
                            _robot = new Robot(this, new Vec2L(x, y));
                            break;
                        case 'O':
                            _packages.Add(new Package(new Vec2L(x, y)));
                            break;
                    }
                }
            }

            if(_robot == null)
                throw new Exception("No robot found");
            if(_walls.Count == 0)
                throw new Exception("No walls found");
            if(_packages.Count == 0)
                throw new Exception("No packages found");
        }

        public bool CanRobotMoveTo(Vec2L position, Direction direction)
        {
            if(_walls.Any(p => p.X == position.X && p.Y == position.Y))
                return false;
            if (_packages.Any(p => p.Position.X == position.X && p.Position.Y == position.Y))
                return MovePackageTo(position, direction);

            return true;
        }

        private bool MovePackageTo(Vec2L position, Direction direction)
        {
            var package = _packages.FirstOrDefault(p => p.Position.X == position.X && p.Position.Y == position.Y);
            if(package == null)
                return true;

            var newPosition = position.Move(direction.ToVec2L());
            if(_walls.Any(p => Equals(p, newPosition)))
                return false;

            // Check if there is a package in the new position
            // If there is, move that package
            if(!MovePackageTo(newPosition, direction))
                return false;

            package.Move(direction);
            return true;
        }

        public void MakeMove(Direction direction)
        {
            _robot.Move(direction);
        }

        public long CalculateGps()
        {
            return _packages.Sum(p => p.CalculateGps());
        }

        public void Render(RenderMode mode, Point cursor, Direction[] directions)
        {
            if(mode == RenderMode.DISABLED)
                return;

            Console.SetCursorPosition(cursor.X, cursor.Y);
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    var position = new Vec2L(x, y);
                    if(Equals(_robot.Position, position))
                        Console.Write('@');
                    else if(_packages.Any(p => Equals(p.Position, position)))
                        Console.Write('O');
                    else if(_walls.Any(p => Equals(p, position)))
                        Console.Write('#');
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }

            var directionString = string.Join("", directions.Select(DirectionExtensions.ToDirectionString).ToArray());
            Console.WriteLine(directionString + " ");


            if(mode == RenderMode.DELAY)
                Thread.Sleep(100);
            else if(mode == RenderMode.STEP_THROUGH)
                Console.ReadKey();
        }
    }

    private class Robot(Grid grid, Vec2L position)
    {
        public void Move(Direction direction)
        {
            var moveVec = direction.ToVec2L();
            var newPosition = Position.Move(moveVec);

            if(!grid.CanRobotMoveTo(newPosition, direction))
                return;

            Position = newPosition;
        }

        public Vec2L Position { get; private set; } = position;
    }

    private class Package(Vec2L position)
    {
        public void Move(Direction direction)
        {
            Position = Position.Move(direction.ToVec2L());
        }

        public long CalculateGps()
        {
            return 100 * Position.Y + Position.X;
        }

        public Vec2L Position { get; private set; } = position;
    }
}