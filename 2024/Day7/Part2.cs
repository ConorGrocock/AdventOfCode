using SharedUtils;

namespace Day7;

public class Part2 : ISolution
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
        private readonly long _answer;
        private readonly long[] _values;
        private bool? _isPossible;

        public Calibration(string input)
        {
            var split = input.Split(":");
            _answer = long.Parse(split[0]);
            _values = split[1]
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();
        }

        public bool IsValid() =>
            _isPossible ??= CalculateValidity();


        private bool CalculateValidity()
        {
            if (_values.Length == 1)
                return _values[0] == _answer;

            var combinations = GenerateAllCombinations();
            return combinations.Any(ops => EvaluateCombination(ops) == _answer);
        }

        private IEnumerable<List<Operation>> GenerateAllCombinations()
        {
            var operatorCount = _values.Length - 1;
            const int states = 3;
            var combinationCount = (int)Math.Pow(states, operatorCount);

            for (var value = 0; value < combinationCount; value++)
            {
                var combination = new List<Operation>();

                for (var pos = 0; pos < operatorCount; pos++)
                {
                    var op = (value % states) switch
                    {
                        0 => Operation.Add,
                        1 => Operation.Multiply,
                        2 => Operation.Concatenate,
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
            var result = _values[0];
            for (var i = 0; i < operators.Count; i++)
            {
                result = operators[i] switch
                {
                    Operation.Add => result + _values[i + 1],
                    Operation.Multiply => result * _values[i + 1],
                    Operation.Concatenate => ConcatenateNumbers(result, _values[i + 1]),
                    _ => throw new ArgumentException($"Unknown operator: {operators[i]}")
                };
            }

            return result;
        }


        private static long ConcatenateNumbers(long left, long right) =>
            long.Parse(left.ToString() + right.ToString());

        public long Answer() => _answer;
    }

    public enum Operation
    {
        Add,
        Multiply,
        Concatenate
    }
}