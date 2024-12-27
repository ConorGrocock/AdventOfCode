using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using SharedUtils;

namespace Day9;

public partial class Part1 : ISolution
{
    private static RenderMode _renderMode = RenderMode.STEP_THROUGH;

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
        Console.Clear();
        var fileInput = Input.TestInput(fileName);
        return Answer(fileInput);
    }

    private string Answer(string[] input)
    {
        ParseInput(out var registers, out var instructions, input);

        var instructionPointer = 0;
        var output = new StringBuilder();
        while (instructionPointer < instructions.Count)
        {
            var instruction = (Operation) instructions[instructionPointer];
            var literalOperand = instructions[instructionPointer + 1];
            BigInteger comboOperand = 0;

            RenderExecution(instruction, comboOperand, registers, instructionPointer, instructions, output);

            switch (instruction)
            {
                case Operation.ADV:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    registers[0] /= (BigInteger)Math.Pow(2, (double) comboOperand);
                    break;
                case Operation.BXL:
                    registers[1] ^= literalOperand;
                    break;
                case Operation.BST:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    registers[1] = comboOperand % 8;
                    break;
                case Operation.JNZ:
                    if (registers[0] != 0)
                    {
                        instructionPointer = literalOperand;
                        continue;
                    }
                    break;
                case Operation.BXC:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    registers[1] ^= registers[2];
                    break;
                case Operation.OUT:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    output = AddOutput(output, comboOperand % 8);
                    break;
                case Operation.BDV:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    registers[1] = registers[0] / (BigInteger)Math.Pow(2,(double) comboOperand);
                    break;
                case Operation.CDV:
                    comboOperand = GetOperationInput(instruction, instructions, instructionPointer, registers);
                    registers[2] = registers[0] / (BigInteger)Math.Pow(2,(double) comboOperand);
                    break;
                default:
                    return output.ToString();
            }

            instructionPointer += 2;
        }

        RenderExecution(Operation.ADV, 0, registers, instructionPointer, instructions, output);

        return output.ToString();
    }

    private static void RenderExecution(Operation instruction, BigInteger comboOperand, BigInteger[] registers, int instructionPointer, List<short> instructions, StringBuilder output)
    {
        if(_renderMode == RenderMode.DISABLED)
            return;

        Console.Clear();
        Console.WriteLine($"Instruction: {instruction}");
        Console.WriteLine($"Combo Operand: {comboOperand}");
        Console.WriteLine();

        Console.WriteLine($"Register A: {registers[0]}");
        Console.WriteLine($"Register B: {registers[1]}");
        Console.WriteLine($"Register C: {registers[2]}");
        Console.WriteLine();

        Console.WriteLine($"Instruction Pointer: {instructionPointer}");
        Console.WriteLine($"Instructions: {string.Join(',', instructions)}");
        Console.WriteLine($"Output: {output}");

        if (_renderMode == RenderMode.STEP_THROUGH)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        } else if (_renderMode == RenderMode.DELAY)
        {
            Thread.Sleep(500);
        }
    }

    private static StringBuilder AddOutput(StringBuilder output, BigInteger value)
    {
        if(output.Length > 0)
            output.Append(',');
        output.Append(value);
        return output;
    }

    private static BigInteger GetOperationInput(Operation operation, List<short> instructions, int instructionPointer, BigInteger[] registers)
    {
        var operationInput = 7;
        if(instructionPointer + 1 < instructions.Count)
            operationInput = instructions[instructionPointer + 1];

        switch (operationInput)
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return registers[0];
            case 5:
                return registers[1];
            case 6:
                return registers[2];
            case 7:
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }
    }

    private static void ParseInput(out BigInteger[] registers, out List<short> instructions, string[] input)
    {
        registers = new BigInteger[3];
        var registerPattern = RegisterRegex();
        foreach (var inputLine in input.TakeWhile(line => !string.IsNullOrEmpty(line)))
        {
            var match = registerPattern.Match(inputLine);
            var register = match.Groups[1].Value;
            var value = BigInteger.Parse(match.Groups[2].Value);
            registers[register[0] - 'A'] = value;
        }

        instructions = [];
        var instructionLine = input[^1];
        var programPattern = ProgramRegex();
        var m = programPattern.Match(instructionLine);
        instructions.AddRange(m.Value.Split(',').Select(short.Parse));
    }

    private enum Operation
    {
        ADV = 0, // Divide value in register a by the square of the input. The result is stored in register a.
        BXL = 1, // XOR the value in register b with the value of 1. The result is stored in register b.
        BST = 2, // Take in the input mod 8 and store it in register b.
        JNZ = 3, // if register a is not zero, jump to the instruction at the offset of the input.
        BXC = 4, // XOR the value in register b with the value of register c. The result is stored in register b.
        OUT = 5, // Take the input mod 8 and output it.
        BDV = 6, // Divide value in register a by the square of the input. The result is stored in register b.
        CDV = 7, // Divide value in register a by the square of the input. The result is stored in register c.
    }


    [GeneratedRegex(@"Register (\w): (\d+)")]
    private static partial Regex RegisterRegex();
    [GeneratedRegex(@"(\d\,?)+")]
    private static partial Regex ProgramRegex();
}