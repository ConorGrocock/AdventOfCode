using System.Text.RegularExpressions;
using SharedUtils;

namespace Day3;

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

    private int Answer(string[] file)
    {
        var result = 0;

        var sortingInstructions = new List<KeyValuePair<int, int>>();
        var manualPages = new List<List<int>>();

        ParseInput(file, sortingInstructions, manualPages);

        foreach (var manual in manualPages)
        {
            var relevantInstructions = sortingInstructions.Where(kv => manual.Contains(kv.Key) && manual.Contains(kv.Value)).ToList();
            var isValidManual = true;
            foreach (var page in manual)
            {
                var instructions = relevantInstructions.Where(kv => kv.Key == page).ToArray();
                if (instructions.Length == 0)
                    continue;

                if (!isPageInCorrectSpot(manual.ToArray(), page, instructions))
                    isValidManual = false;
            }

            var middlePage = manual[(int)((decimal)manual.Count / 2)];
            if (isValidManual)
            {
                result += middlePage;
            }
        }



        return result;
    }

    private static void ParseInput(string[] file, List<KeyValuePair<int, int>> sortingInstructions, List<List<int>> manualPages)
    {
        var isSortingInstructions = true;
        foreach (var line in file)
        {
            if(line == "")
            {
                isSortingInstructions = false;
                continue;
            }

            if (isSortingInstructions)
            {
                var split = line.Split("|");
                sortingInstructions.Add(new KeyValuePair<int, int>(int.Parse(split[0]), int.Parse(split[1])));
            }
            else
            {
                manualPages.Add(line.Split(",").Select(int.Parse).ToList());
            }
        }
    }

    private bool isPageInCorrectSpot(int[] manual, int page, KeyValuePair<int, int>[] instructions)
    {
        foreach (var instruction in instructions.Where(kv => kv.Key == page))
        {
            var indexOfPage = Array.IndexOf(manual, page);
            var indexOfInstruction = Array.IndexOf(manual, instruction.Value);

            if (indexOfPage > indexOfInstruction)
            {
                return false;
            }
        }

        return true;
    }
}