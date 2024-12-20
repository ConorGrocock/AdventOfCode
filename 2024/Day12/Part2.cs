using System.Numerics;
using System.Text.RegularExpressions;
using SharedUtils;

namespace Day9;

public class Part2 : ISolution
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
        var regions = SearchForRegion(input);
        var totalPrice = regions.Sum(r => r.CalculatePrice());
        foreach (var region in regions)
        {
            region.CountEdges();;
        }

        return totalPrice.ToString();
    }

    public Region[] SearchForRegion(string[] input)
    {
        var regions = new List<Region>();
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (regions.Any(r => r.IsPointInRegion(new Vec2L(x, y)))) continue;

                regions.Add(DepthSearch(input, new Vec2L(x, y)));
            }
        }


        return regions.ToArray();
    }

    public Region DepthSearch(string[] input, Vec2L startPoint)
    {
        var pointsToSearch = new Queue<Vec2L>();
        var visitedPoints = new HashSet<Vec2L>();
        var searchChar = input[(int)startPoint.Y][(int)startPoint.X];
        var region = new Region(searchChar);
        var perimeter = 0;

        pointsToSearch.Enqueue(startPoint);

        while (pointsToSearch.TryDequeue(out var point))
        {
            if (visitedPoints.Contains(point))
            {
                continue;
            }

            if (!point.IsWithinBounds(new Vec2L(input[0].Length - 1, input.Length - 1)))
            {
                perimeter++;
                continue;
            }

            if (input[(int)point.Y][(int)point.X] != searchChar)
            {
                perimeter++;
                continue;
            }

            region.AddPoint(point);
            visitedPoints.Add(point);

            pointsToSearch.Enqueue(point.Move(1, 0));
            pointsToSearch.Enqueue(point.Move(-1, 0));
            pointsToSearch.Enqueue(point.Move(0, 1));
            pointsToSearch.Enqueue(point.Move(0, -1));
            region.Area++;
        }

        region.Perimeter = perimeter;
        return region;
    }

    public class Region(char searchChar)
    {
        private readonly List<Vec2L> _points = new();
        public char SearchChar = searchChar;
        public int Perimeter;
        public int Area;

        public void AddPoint(Vec2L point)
        {
            _points.Add(point);
        }

        public bool IsPointInRegion(Vec2L point)
        {
            return _points.Contains(point);
        }

        public int CalculatePrice()
        {
            return Area * CountEdges();
        }

        private int CountCorners(Vec2L point)
        {
            var count = 0;

            if(!_points.Contains(point.Move(0, -1)) && !_points.Contains(point.Move(-1, 0)))
                count++;
            if(!_points.Contains(point.Move(0, -1)) && !_points.Contains(point.Move(1, 0)))
                count++;
            if(!_points.Contains(point.Move(0, 1)) && !_points.Contains(point.Move(-1, 0)))
                count++;
            if(!_points.Contains(point.Move(0, 1)) && !_points.Contains(point.Move(1, 0)))
                count++;

            if(_points.Contains(point.Move(0, -1)) && _points.Contains(point.Move(-1, 0)) && !_points.Contains(point.Move(-1, -1)))
                count++;
            if(_points.Contains(point.Move(0, -1)) && _points.Contains(point.Move(1, 0)) && !_points.Contains(point.Move(1, -1)))
                count++;
            if(_points.Contains(point.Move(0, 1)) && _points.Contains(point.Move(-1, 0)) && !_points.Contains(point.Move(-1, 1)))
                count++;
            if(_points.Contains(point.Move(0, 1)) && _points.Contains(point.Move(1, 0)) && !_points.Contains(point.Move(1, 1)))
                count++;

            return count;
        }

        public int CountEdges()
        {
            return _points.Select(CountCorners).Sum();
        }
    }
}