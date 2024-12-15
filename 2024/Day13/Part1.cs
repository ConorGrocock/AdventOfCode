using System.Numerics;
using System.Text.RegularExpressions;
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

    private string Answer(string[] input)
    {
        var machines = ParseInput(input);

        var tokens = machines.Sum(CalculateTokens);

        return tokens.ToString();
    }

    private ArcadeMachine[] ParseInput(string[] lines)
    {
        ArcadeMachineBuilder builder = new();
        List<ArcadeMachine> machines = new();

        foreach (var line in lines)
        {
            if (line.StartsWith("Button A"))
            {
                var groups = new Regex(@"Button (A|B): X(\-|\+\d.), Y(\-|\+\d.)").Matches(line);
                var x = int.Parse(groups[0].Groups[2].Value);
                var y = int.Parse(groups[0].Groups[3].Value);
                builder.SetButtonA(new Point(x, y));
            }
            else if (line.StartsWith("Button B"))
            {
                var groups = new Regex(@"Button (A|B): X(\-|\+\d.), Y(\-|\+\d.)").Matches(line);
                var x = int.Parse(groups[0].Groups[2].Value);
                var y = int.Parse(groups[0].Groups[3].Value);
                builder.SetButtonB(new Point(x, y));
            }
            else if (line.StartsWith("Prize"))
            {
                var groups = new Regex(@"Prize: X=(\d+), Y=(\d+)").Matches(line);
                var x = int.Parse(groups[0].Groups[1].Value);
                var y = int.Parse(groups[0].Groups[2].Value);
                builder.SetDestination(new Point(x, y));
            }
            else if (line == string.Empty)
            {
                machines.Add(builder.Build());
            }
        }

        return machines.ToArray();
    }

    private int CalculateTokens(ArcadeMachine machine)
    {
        var machinePrize = machine.Prize;
        decimal buttonARequired = (machinePrize.X * machine.ButtonB.Y - machinePrize.Y * machine.ButtonB.X) /
                                  (machine.ButtonA.X * machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X);
        decimal buttonBRequired = (machinePrize.X * machine.ButtonA.Y - machinePrize.Y * machine.ButtonA.X) /
                              (machine.ButtonB.X * machine.ButtonA.Y - machine.ButtonB.Y * machine.ButtonA.X);

        if (machine.ButtonA.X * buttonARequired + machine.ButtonB.X * buttonBRequired == machinePrize.X &&
            machine.ButtonA.Y * buttonARequired + machine.ButtonB.Y * buttonBRequired == machinePrize.Y)
        {
            return (int)buttonBRequired + (int)(buttonARequired * 3);
        }

        return 0;
    }

    private class ArcadeMachine
    {
        private readonly Point _buttonA;
        private readonly Point _buttonB;

        private readonly Point _prize;

        public ArcadeMachine(Point buttonA, Point buttonB, Point prize)
        {
            _buttonA = buttonA;
            _buttonB = buttonB;
            _prize = prize;
        }

        public Point ButtonA => _buttonA;
        public Point ButtonB => _buttonB;
        public Point Prize => _prize;
    }

    private class ArcadeMachineBuilder
    {
        private Point _buttonA;
        private Point _buttonB;
        private Point _destination;

        public ArcadeMachineBuilder SetButtonA(Point buttonA)
        {
            _buttonA = buttonA;
            return this;
        }

        public ArcadeMachineBuilder SetButtonB(Point buttonB)
        {
            _buttonB = buttonB;
            return this;
        }

        public ArcadeMachineBuilder SetDestination(Point destination)
        {
            _destination = destination;
            return this;
        }

        public ArcadeMachine Build()
        {
            return new ArcadeMachine(_buttonA, _buttonB, _destination);
        }
    }
}