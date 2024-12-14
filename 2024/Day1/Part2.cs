﻿using SharedUtils;

namespace Day1;

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

    public int Answer(string[] fileContents)
    {
        var result = 0;
        var leftColumn = new List<int>();
        var rightColumn = new Dictionary<int, int>();

        foreach (var line in fileContents)
        {
            var lineParts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var firstItem = int.Parse(lineParts[0]);
            var secondItem = int.Parse(lineParts[1]);

            leftColumn.Add(firstItem);
            if(!rightColumn.TryAdd(secondItem, 1))
            {
                rightColumn[secondItem]++;
            }
        }

        foreach (var leftValue in leftColumn)
        {
            if(rightColumn.TryGetValue(leftValue, out var right))
            {
                result += leftValue * right;
            }
        }

        return result;
    }

}