using SharedUtils;

namespace Day7;

public class Part1 : ISolution
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
        var testValues = input.Select(x => new Calibration(x)).ToArray();

        var answers = testValues.AsParallel().Where(x => x.IsValid()).ToArray();
        var answer = answers.Sum(x => x.Answer());

        Console.WriteLine(answer);
        return (int)answer;
    }

    class Calibration
    {
        private long answer;
        private long[] values;
        private bool? isPossible;

        public Calibration(string input)
        {
            var split = input.Split(":");
            answer = long.Parse(split[0]);
            values = split[1]
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();
        }

        public bool IsValid() =>
            isPossible ??= CalculateValidity();


        private bool CalculateValidity()
        {
            if (values.Length == 1)
                return values[0] == answer;

            var combinations = GenerateAllCombinations();
            foreach (var ops in combinations)
            {
                try
                {
                    if (EvaluateCombination(ops) == answer)
                        return true;
                }
                catch (OverflowException)
                {
                }
            }

            return false;
        }

        private IEnumerable<List<Operation>> GenerateAllCombinations()
        {
            var operatorCount = values.Length - 1;
            const int states = 2;
            var combinationCount = (int)Math.Pow(states, operatorCount);

            for (var i = 0; i < combinationCount; i++)
            {
                var combination = new List<Operation>();
                var value = i;

                for (var pos = 0; pos < operatorCount; pos++)
                {
                    var op = (value % 2) switch
                    {
                        0 => Operation.Add,
                        1 => Operation.Multiply,
                        _ => throw new ArgumentException("Invalid operator state")
                    };
                    combination.Add(op);
                    value /= states;
                }

                yield return combination;
            }
        }


        private long EvaluateCombination(List<Operation> operators)
        {
            var result = values[0];
            for (var i = 0; i < operators.Count; i++)
            {
                result = operators[i] switch
                {
                    Operation.Add => result + values[i + 1],
                    Operation.Multiply => result * values[i + 1],
                    _ => throw new ArgumentException($"Unknown operator: {operators[i]}")
                };
            }

            return result;
        }

        public long Answer() => answer;
    }

    public enum Operation
    {
        Add,
        Multiply
    }
}