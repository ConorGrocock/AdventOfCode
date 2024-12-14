using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using SharedUtils;
using Point = SharedUtils.Point;

namespace Day6;

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

    private int Answer(string[] input)
    {
        var direction = 0;
        var directionPoint = new Point[]{ new(0, -1), new(1, 0), new(0, 1), new(-1, 0) };

        var startingPositionLine = input.First(line => line.Contains('^'));

        var startingPositionX = startingPositionLine.IndexOf('^');
        var startingPositionY = Array.IndexOf(input, startingPositionLine);

        var currentPosition = new Point(startingPositionX, startingPositionY);
        var nextPosition = currentPosition.Move(directionPoint[direction % 4]);
        while (nextPosition.IsWithinBounds(startingPositionLine.Length - 1, input.Length - 1))
        {
            while (input[nextPosition.Y][nextPosition.X] == '#')
            {
                direction++;
                nextPosition = currentPosition.Move(directionPoint[direction % directionPoint.Length]);
                SetPositionToChar(input, currentPosition, '+');
            }

            currentPosition = nextPosition;
            nextPosition = currentPosition.Move(directionPoint[direction % directionPoint.Length]);
            // DrawMap(input);
        }

        input = SetPositionToChar(input, new Point(startingPositionX, startingPositionY), '+');

        return FindRectanglesInGrid(input);
    }

    private void DrawMap(string[] input)
    {
        Console.SetCursorPosition(0,1);
        foreach (var line in input)
        {
            Console.WriteLine(line);
        }
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

    private int FindRectanglesInGrid(string[] input)
    {
        var foundRectangles = new List<KeyValuePair<Point, Point>>();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '+') continue;

                var rectangle = FindRectangle(input, x, y);
                if (rectangle.Length != 4) continue;

                var minX = rectangle.Min(p => p.X);
                var minY = rectangle.Min(p => p.Y);

                var maxX = rectangle.Max(p => p.X);
                var maxY = rectangle.Max(p => p.Y);

                var isAbleToWalk = AbleToWalkPath(input, new Point(minX, minY), new Point(maxX, maxY));
                if (isAbleToWalk)
                {
                    foundRectangles.Add(new KeyValuePair<Point, Point>(new Point(minX, minY), new Point(maxX, maxY)));
                }
            }
        }

        var uniqueRectangles = new List<KeyValuePair<Point, Point>>();
        foreach (var foundRectangle in foundRectangles)
        {
            var found = false;

            foreach (var uniqueRectangle in uniqueRectangles)
            {
                if (foundRectangle.Key.X == uniqueRectangle.Key.X &&
                    foundRectangle.Key.Y == uniqueRectangle.Key.Y &&
                    foundRectangle.Value.X == uniqueRectangle.Value.X &&
                    foundRectangle.Value.Y == uniqueRectangle.Value.Y) found = true;
            }

            if(!found) uniqueRectangles.Add(foundRectangle);
        }

        return uniqueRectangles.Count;
    }

    private Point[] FindRectangle(string[] input, int x, int y)
    {
        var rectangle = new List<Point>();
        var checkedX = 0;
        var checkedY = 0;

        rectangle.Add(new Point(x, y));

        for(checkedX = 0; checkedX < input[y].Length; checkedX++)
        {
            if (checkedX == x) continue;
            if (input[y][checkedX] != '+') continue;

            rectangle.Add(new Point(checkedX, y));
            break;
        }

        if(checkedX == input[y].Length)
        {
            return [];
        }

        for(checkedY = 0; checkedY < input.Length; checkedY++)
        {
            if (checkedY == y) continue;
            if (input[checkedY][x] != '+') continue;

            rectangle.Add(new Point(x, checkedY));
            break;
        }

        if(checkedY == input.Length)
        {
            return [];
        }

        if(input[checkedY][checkedX] == '#')
        {
            return [];
        }



        rectangle.Add(new Point(checkedX, checkedY));
        return rectangle.ToArray();
    }

    private bool AbleToWalkPath(string[] input, Point startPosition, Point endPosition)
    {
        for (int i = startPosition.X; i < endPosition.X; i++)
        {
            if(input[startPosition.Y][i] == '#') return false;
        }
        for (int i = startPosition.X; i < endPosition.X; i++)
        {
            if(input[endPosition.Y][i] == '#') return false;
        }

        for (int i = startPosition.Y; i < endPosition.Y; i++)
        {
            if(input[i][startPosition.X] == '#') return false;
        }
        for (int i = startPosition.Y; i < endPosition.Y; i++)
        {
            if(input[i][endPosition.X] == '#') return false;
        }

        return true;
    }
}