using System.Numerics;
using System.Text.RegularExpressions;
using SharedUtils;

namespace Day9;

public class Part2 : ISolution
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
        var offset = new Vec2L(10000000000000,10000000000000);

        var tokens = machines.Sum(machine => CalculateTokens(machine, offset));

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
                builder.SetButtonA(new Vec2L(x, y));
            }
            else if (line.StartsWith("Button B"))
            {
                var groups = new Regex(@"Button (A|B): X(\-|\+\d.), Y(\-|\+\d.)").Matches(line);
                var x = int.Parse(groups[0].Groups[2].Value);
                var y = int.Parse(groups[0].Groups[3].Value);
                builder.SetButtonB(new Vec2L(x, y));
            }
            else if (line.StartsWith("Prize"))
            {
                var groups = new Regex(@"Prize: X=(\d+), Y=(\d+)").Matches(line);
                var x = int.Parse(groups[0].Groups[1].Value);
                var y = int.Parse(groups[0].Groups[2].Value);
                builder.SetDestination(new Vec2L(x, y));
            }
            else if (line == string.Empty)
            {
                machines.Add(builder.Build());
            }
        }
        machines.Add(builder.Build());

        return machines.ToArray();
    }

    private long CalculateTokens(ArcadeMachine machine, Vec2L offset)
    {
        var machinePrize = machine.Prize.Move(offset);
        var buttonARequired = (machinePrize.X * machine.ButtonB.Y - machinePrize.Y * machine.ButtonB.X) /
                                  (machine.ButtonA.X * machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X);
        var buttonBRequired = (machinePrize.X * machine.ButtonA.Y - machinePrize.Y * machine.ButtonA.X) /
                              (machine.ButtonB.X * machine.ButtonA.Y - machine.ButtonB.Y * machine.ButtonA.X);

        if (machine.ButtonA.X * buttonARequired + machine.ButtonB.X * buttonBRequired == machinePrize.X &&
            machine.ButtonA.Y * buttonARequired + machine.ButtonB.Y * buttonBRequired == machinePrize.Y)
        {
            return buttonBRequired + buttonARequired * 3;
        }

        return 0;
    }

    private class ArcadeMachine
    {
        private readonly Vec2L _buttonA;
        private readonly Vec2L _buttonB;

        private readonly Vec2L _prize;

        public ArcadeMachine(Vec2L buttonA, Vec2L buttonB, Vec2L prize)
        {
            _buttonA = buttonA;
            _buttonB = buttonB;
            _prize = prize;
        }

        public Vec2L ButtonA => _buttonA;
        public Vec2L ButtonB => _buttonB;
        public Vec2L Prize => _prize;
    }

    private class ArcadeMachineBuilder
    {
        private Vec2L _buttonA;
        private Vec2L _buttonB;
        private Vec2L _destination;

        public ArcadeMachineBuilder SetButtonA(Vec2L buttonA)
        {
            _buttonA = buttonA;
            return this;
        }

        public ArcadeMachineBuilder SetButtonB(Vec2L buttonB)
        {
            _buttonB = buttonB;
            return this;
        }

        public ArcadeMachineBuilder SetDestination(Vec2L destination)
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