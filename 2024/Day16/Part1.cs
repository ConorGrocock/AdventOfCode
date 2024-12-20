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
        Console.Clear();
        var grid = new Grid(input);

        return grid.AStarSearch().ToString();
    }

    private class Grid
    {
        private int Width { get; }
        private int Height { get; }
        private char[,] Data { get; }
        private Vec2L Start { get; }
        private Vec2L End { get; }

        public Grid(string[] input)
        {
            Width = input[0].Length;
            Height = input.Length;
            Data = new char[Width, Height];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Data[x, y] = input[y][x];
                    if (Data[x, y] == 'S')
                    {
                        Start = new Vec2L(x, y);
                    }

                    if (Data[x, y] == 'E')
                    {
                        End = new Vec2L(x, y);
                    }
                }
            }

            if(Start == null || End == null)
            {
                throw new Exception("Start or End not found");
            }
        }

        private void Render(GridSearchable current)
        {
            if (_renderMode == RenderMode.DISABLED)
            {
                return;
            }

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (current.Position.X == x && current.Position.Y == y)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('O');
                        Console.ResetColor();
                    }
                    // } else if (current.HasVisited(new Vec2L(x, y)))
                    // {
                    //     Console.SetCursorPosition(x,y);
                    //     Console.ForegroundColor = ConsoleColor.DarkBlue;
                    //     Console.Write('+');
                    //     Console.ResetColor();
                    // }
                }
            }

            Console.SetCursorPosition(0, Height + 1);
            Console.WriteLine($"Search Cost: {current.Cost}");
            Console.WriteLine($"Direction: {current.Direction}");
            Console.WriteLine();

            if (_renderMode == RenderMode.DELAY)
            {
                // Thread.Sleep(10);
            }
            if(_renderMode == RenderMode.STEP_THROUGH)
            {
                Console.ReadKey();
            }
        }

        public long AStarSearch()
        {
            // Perform an A* search from the start to the end
            var openSet = new PriorityQueue<GridSearchable, long>();
            var closedSet = new HashSet<Vec2L>();
            var minimumCost = long.MaxValue;

            var compassDirection = DirectionExtensions.FromCompassString("EAST");
            var start = new GridSearchable(Start, 0, null, compassDirection);
            openSet.Enqueue(start, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                closedSet.Add(current.Position);

                Render(current);

                if (Equals(current.Position, End))
                {
                    Console.WriteLine("Found the end");
                    Console.WriteLine("Current Cost: " + current.Cost);
                    Console.WriteLine("Visited: " + current.Visited.Count);
                    return current.Cost;
                }


                foreach (var neighbor in current.Position.GetNeighbors())
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // If the neighbor is a wall, skip it
                    if (Data[neighbor.X, neighbor.Y] == '#')
                    {
                        continue;
                    }
                    var currentDirection = current.Direction;
                    var directionToNeighbor = current.Position.GetDirectionTo(neighbor);
                    var directionDiff = currentDirection.RotationTo(directionToNeighbor);

                    var cost = current.Cost + 1;
                    if (directionDiff != 0)
                    {
                        cost += 1000;
                    }

                    var neighborSearchable = new GridSearchable(neighbor, cost, current, directionToNeighbor);
                    openSet.Enqueue(neighborSearchable, current.Cost);
                }
            }

            return minimumCost;
        }

        public long DepthSearch()
        {
            // Perform a depth first search from the start to the end
            var openSet = new Stack<GridSearchable>();
            var closedSet = new HashSet<Vec2L>();
            var minimumCost = long.MaxValue;

            var compassDirection = DirectionExtensions.FromCompassString("EAST");
            var start = new GridSearchable(Start, 0, null, compassDirection);
            openSet.Push(start);

            while (openSet.TryPop(out var current))
            {
                closedSet.Add(current.Position);
                Render(current);

                if (Equals(current.Position, End))
                {
                    Console.WriteLine("Found the end");
                    Console.WriteLine("Current Cost: " + current.Cost);
                    Console.WriteLine("Visited: " + current.Visited.Count);
                    minimumCost = Math.Min(minimumCost, current.Cost);
                }

                foreach (var neighbor in current.Position.GetNeighbors())
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // If the neighbor is a wall, skip it
                    if (Data[neighbor.X, neighbor.Y] == '#')
                    {
                        continue;
                    }

                    var neighborSearchable = new GridSearchable(neighbor, current.Cost + 1, current, current.Position.GetDirectionTo(neighbor));
                    openSet.Push(neighborSearchable);
                }
            }

            return minimumCost;
        }
    }

    private class GridSearchable
    {
        private readonly HashSet<Vec2L> _visited;

        public GridSearchable(Vec2L position, int cost, GridSearchable? parent, Direction direction)
        {
            Position = position;
            Cost = cost;
            Direction = direction;

            _visited = parent?._visited ?? [];
            _visited.Add(position);
        }

        public Vec2L Position { get; }
        public int Cost { get; }
        public Direction Direction { get; }
        public HashSet<Vec2L> Visited => _visited;
    }
}